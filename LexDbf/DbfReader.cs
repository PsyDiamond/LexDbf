using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using LexTalionis.LexDbf.Common;
using LexTalionis.LexDbf.Enums;
using LexTalionis.LexDbf.Exceptions;
using LexTalionis.StreamTools;

namespace LexTalionis.LexDbf
{
    
    /// <summary>
    /// Читатель DBF файлов
    /// </summary>
    public class DbfReader : DbfFile, IDisposable
    {
        private BinaryReader _reader;
        private byte[] _buffer;
        private string _tablename;
        private static string _tmpfile;

        private DbfReader()
        {
        }

        /// <summary>
        /// Открыть поток 
        /// </summary>
        /// <param name="stream">поток</param>
        /// <param name="encoding">кодировка</param>
        /// <returns>Читатель DBF</returns>
        private static DbfReader Open(Stream stream, Encoding encoding)
        {
            _tmpfile = Path.GetTempFileName();
            using (var file = File.Create(_tmpfile))
            {
                stream.CopyTo(file);
            }
            return Open(_tmpfile, encoding);
        }

        /// <summary>
        /// Открыть поток 
        /// </summary>
        /// <param name="stream">поток</param>
        /// <returns>Читатель DBF</returns>
        public static DbfReader Open(Stream stream)
        {
            return Open(stream, null);
        }
        /// <summary>
        /// Открыть файл 
        /// </summary>
        /// <param name="file">путь к файлу</param>
        /// <returns>Читатель DBF</returns>
        public static DbfReader Open(string file)
        {
            return Open(file, null);
        }

        /// <summary>
        /// Открыть файл
        /// </summary>
        /// <param name="file">путь к файлу</param>
        /// <param name="encoding">кодировка</param>
        /// <returns>Читатель DBF</returns>
        private static DbfReader Open(string file, Encoding encoding)
        {
            var dbf = new DbfReader {Encoding = encoding ?? Encoding.Default, _tablename = Path.GetFileNameWithoutExtension(file)+"DBF"};
            using (var f = File.OpenRead(file))
            {
                var buffer = new byte[f.Length];
                f.Read(buffer, 0, (int) f.Length);
                var mem = new MemoryStream(buffer);
                dbf._reader = new BinaryReader(mem);
            }
            ReadHeader(dbf);
            return dbf;
        }
        
        private static void ReadHeader(DbfReader dbf)
        {
            dbf.Header = new DbfHeader
                {
                    VersionNumber = (Signature) dbf._reader.ReadByte()
                };
            var year = dbf._reader.ReadByte();
            var month = dbf._reader.ReadByte();
            var day = dbf._reader.ReadByte();
            dbf.Header.LastUpdate = new DateTime(1900 + year, month, day);
            dbf.Header.NumberOfRecords = dbf._reader.ReadInt32();
            dbf.Header.LengthOfHeader = dbf._reader.ReadInt16();
            dbf.Header.LengthOfEachRecord = dbf._reader.ReadInt16();
                //};
            // Резерв
            dbf._reader.ReadBytes(2);
            dbf.Header.Transaction = (Transaction)dbf._reader.ReadByte();
            dbf.Header.Encripted = (Encripted)dbf._reader.ReadByte();
            // Free record thread 
            dbf._reader.ReadBytes(4);
            // Reserved for multi-user dBASE
            dbf._reader.ReadBytes(8);
            dbf.Header.MDXFlag = dbf._reader.ReadByte();
            dbf.Header.LanguageDriver = (CodePage)dbf._reader.ReadByte();

            switch (dbf.Header.LanguageDriver)
            {
                case CodePage.СodePage1251:
                case CodePage.Unknow:
                    dbf.Encoding = Encoding.GetEncoding(1251); 
                    break;
                case CodePage.СodePage866:
                    dbf.Encoding = Encoding.GetEncoding(866);
                    break;
            }
            // Reserved
            dbf._reader.ReadBytes(2);

            var list = new List<ColumnInfo>();
            int flag;
            var offset = 1;
            var errors = new StringBuilder();

            var types = new List<char>();
            var fieldsEnum = typeof (DbfColumnType).GetFields();
// ReSharper disable LoopCanBeConvertedToQuery
// ReSharper disable ForCanBeConvertedToForeach
            for (var index = 0; index < fieldsEnum.Length; index++)
// ReSharper restore ForCanBeConvertedToForeach
// ReSharper restore LoopCanBeConvertedToQuery
            {
                var type = fieldsEnum[index];
                if (type.Name.Equals("value__"))
                    continue;
                types.Add((char) (int) type.GetRawConstantValue());
            }
            do
            {
                var item = new ColumnInfo();
                var name = dbf.Encoding.GetString(dbf._reader.ReadBytes(11)).TrimEnd(' ', (char) 0x0);
                
                var type = dbf._reader.ReadChar();
                
                item.Name = name;
                
                if (types.Any(x => x == type))
                {
                    item.Type = (DbfColumnType)type;
                }
                else
                {
                    errors.AppendFormat("Что за тип {0}? Как его обрабатывать? Поле {1}", type, name).AppendLine();
                }
                    
                item.FieldDataAddress = dbf._reader.ReadInt32();
                item.FieldLength = dbf._reader.ReadByte();
                item.DecimalCount = dbf._reader.ReadByte();
                // Reserved for multi-user dBASE
                dbf._reader.ReadBytes(2);
                item.WorkAreaID = dbf._reader.ReadByte();
                // Reserved for multi-user dBASE
                dbf._reader.ReadBytes(2);
                item.FlagForSETFIELDS = dbf._reader.ReadByte();
                // Reserved
                dbf._reader.ReadBytes(7);
                item.IndexFieldFlag = dbf._reader.ReadByte();
                item.Offset = offset;
                offset += item.FieldLength;
                list.Add(item);
                flag = dbf._reader.PeekChar();
            } while (flag != 0x0D);
            if (errors.Length > 0)
                throw new DbfHeadException(errors.ToString());
            dbf._reader.ReadByte();
            dbf.Header.Columns = list;
            dbf._reader.BaseStream.Position = dbf.Header.LengthOfHeader;
            //Console.WriteLine(dbf.Header);
        }

        /// <summary>
        /// Завершить работу
        /// </summary>
        public void Dispose()
        {
            _reader.Close();
            if (_tmpfile != null)
                File.Delete(_tmpfile);
        }

        /// <summary>
        /// Получить список колонок
        /// </summary>
        public List<ColumnInfo> GetColumns
        {
            get
            {
                return Header.Columns;
            }
        }
        /// <summary>
        /// Прочитать одну строку
        /// </summary>
        /// <returns>Удалось ли прочитать</returns>
        [Obsolete("Следует использвать GetBody<T>()")]
        public bool Read()
        {
            _buffer = _reader.ReadBytes(Header.LengthOfEachRecord);
            //Console.WriteLine("Original position " + _reader.BaseStream.Position);
            if (_buffer.Count() == 0)
                return false;
            return _buffer[0] != 0x1A;
        }

        /// <summary>
        /// Получить ячейку строки
        /// </summary>
        /// <param name="index">порядковый №</param>
        /// <exception cref="DbfIndexException">Если не верный индекс</exception>
        [Obsolete("Следует использвать GetBody<T>()")]
        private object this[int index] {
            get
            {
                if (index <= Header.Columns.Count)
                {
                    var column = GetColumns[index];
                    return Encoding.GetString(_buffer, column.Offset, column.FieldLength);    
                }
                throw new DbfIndexException(index);
            }
        }

        /// <summary>
        /// Получить ячейку по имени
        /// </summary>
        /// <param name="index">название ячейки</param>
        /// <exception cref="DbfIndexException">если название не верно</exception>
        [Obsolete("Следует использвать GetBody<T>()")]
        public object this[string index]
        {
            get
            {
                var column = GetColumns.FirstOrDefault(x => x.Name == index);
                if(column != null)
                    return Encoding.GetString(_buffer, column.Offset, column.FieldLength);
                throw new DbfIndexException(index);
            }
            
        }

        /// <summary>
        /// Заполнить таблицу
        /// </summary>
        /// <param name="dt">Таблица</param>
        [Obsolete("Следует использвать GetBody<T>()")]
        public void Fill(DataTable dt)
        {
            foreach (var columnInfo in GetColumns)
            {
                var type = ColumnInfo.GetTypeOfColumnT(columnInfo.Type);
                if (type.IsGenericType)
                {
                    var subtyptype = Nullable.GetUnderlyingType(type);
                    dt.Columns.Add(columnInfo.Name, subtyptype);
                }
                else
                {
                    dt.Columns.Add(columnInfo.Name, type);    
                }
                
            }
            while (Read())
            {
                var row = dt.Rows.Add();
                for (var i = 0; i < GetColumns.Count; i++)
                {
                    var str = this[i].ToString().Trim();
                    switch (GetColumns[i].Type)
                    {
                        case DbfColumnType.Date:
                            if (str.Length > 0)
                            {
                                var item = str;
                                var year = int.Parse(item.Substring(0, 4));
                                var month = int.Parse(item.Substring(4, 2));
                                var day = int.Parse(item.Substring(6, 2));
                                row[i] = new DateTime(year, month, day);
                            }
                            break;
                        case DbfColumnType.Number:
                            if (str.Length == 0)
                                row[i] = 0m;
                            else
                            {
                                row[i] = decimal.Parse(str.Replace('.', ','));
                            }
                            break;
                        default:
                            row[i] = str;
                            break;
                    }
                    
                }
            }    
        }
        /// <summary>
        /// Получить содержимое
        /// </summary>
        /// <typeparam name="T">Хранилище</typeparam>
        /// <param name="skipCheking">пропустить необязательные проверки</param>
        /// <returns>Набор элементов</returns>
        /// <exception cref="DbfMappingException">Ошибка маппинга</exception>
        public List<T> GetBody<T>(bool skipCheking)
        {
            return GetBodyCore<T>(skipCheking);
        }
        /// <summary>
        /// Получить содержимое
        /// </summary>
        /// <typeparam name="T">Хранилище</typeparam>
        /// <returns>Набор элементов</returns>
        /// <exception cref="DbfMappingException">Ошибка маппинга</exception>
        public List<T> GetBody<T>()
        {
            return GetBodyCore<T>(false);
        }
        
        private List<T> GetBodyCore<T>(bool skipCheking)
        {
            var type = typeof (T);
            if (!skipCheking)
            {
                if (_tablename != type.Name)
                    throw new DbfMappingException("Должен быть класс " + _tablename + " " + type.Name);    
            }
            

            var fields = type.GetFields();
            
            var errors = new StringBuilder();
            foreach (var column in Header.Columns)
            {
                var item = fields.FirstOrDefault(x => x.Name == column.Name);
                if (item == null)
                {
                    errors.AppendFormat("Поля {0} в классе нет", column.Name).AppendLine();
                }
                else
                {
                    var columntype = ColumnInfo.GetTypeOfColumnT(column.Type);
                    if (item.FieldType != columntype)
                    {
                        errors.AppendFormat("Поле {0} имеет не верный тип. Ожидается {1}", column.Name, columntype)
                              .AppendLine();
                    }    
                }
                
            }

            if (errors.Length > 0)
            {
                throw new DbfMappingException(errors.ToString());
            }
            var list = new List<T>(Header.NumberOfRecords);

#pragma warning disable 612,618
            while(Read())
#pragma warning restore 612,618
            {   
                var box = Activator.CreateInstance(type);

                foreach (var column in Header.Columns)
                {
                    object value;

                    switch (column.Type)
                    {
                        case DbfColumnType.Date:
                            value = GetDate(_buffer, column);
                            break;
                        case DbfColumnType.DateTime:
                            value = GetDateTime(_buffer, column);
                            break;
                        case DbfColumnType.Logical:
                            {
                                var tmp = Encoding.GetString(_buffer, column.Offset, 1);
                                if (tmp == "T" || tmp == "t" || tmp == "Y" || tmp == "y" || tmp == "1")
                                    value = true;
                                else
                                    value = false;
                            }
                            break;
                        default:
                            {
                                var str = Encoding.GetString(_buffer,
                                                             column.Offset, column.FieldLength).Trim();
                                if (column.Type == DbfColumnType.Number)
                                {
                                    if (str.Trim().Length == 0)
                                    {
                                        value = null;
                                    }
                                    else
                                    {
                                        value = decimal.Parse(str.Replace('.', ','));
                                    }

                                }
                                else
                                {
                                    value = str;
                                }

                            }
                            break;
                    }
                    type.GetField(column.Name).SetValue(box, value);
                }
                list.Add((T)box);
            }
            return list;
        }

        private object GetDateTime(byte[] buffer, ColumnInfo column)
        {
            var str = Encoding.GetString(buffer, column.Offset, 4);
            if (str.Trim().Length == 0)
                return null;

            var year = int.Parse(str);
            if (year < 2000)
                year += 2000;
            var month = int.Parse(Encoding.GetString(buffer,
                                                     column.Offset + 4, 2));
            var day = int.Parse(Encoding.GetString(buffer,
                                                   column.Offset + 6, 2));
            var hour = int.Parse(Encoding.GetString(buffer,
                                                   column.Offset + 8, 2));
            var minute = int.Parse(Encoding.GetString(buffer,
                                                   column.Offset + 10, 2));
            var second = int.Parse(Encoding.GetString(buffer,
                                                   column.Offset + 12, 2));
            return new DateTime(year, month, day, hour, minute, second);
        }

        private DateTime? GetDate(byte[] buffer, ColumnInfo column)
        {
            var str = Encoding.GetString(buffer, column.Offset, 4);
            if (str.Trim().Length == 0)
                return null;

            var year = int.Parse(str);
            if (year < 2000)
                year += 2000;
            var month = int.Parse(Encoding.GetString(buffer,
                                                     column.Offset + 4, 2));
            var day = int.Parse(Encoding.GetString(buffer,
                                                   column.Offset + 6, 2));
            return new DateTime(year, month, day);
        }
#if DEBUG
        /// <summary>
        /// Получить текущую строку (сырую)
        /// </summary>
        /// <returns>сырая строка</returns>
        public byte[] Row()
        {
            return _buffer;
        }
#endif
        /// <summary>
        /// Количество запсисей
        /// </summary>
        public int Count
        {
            get { return Header.NumberOfRecords; }
        }
    }
}

﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using LexTalionis.LexDbf.Common;
using LexTalionis.LexDbf.Enums;
using LexTalionis.LexDbf.Exceptions;

namespace LexTalionis.LexDbf
{
    /// <summary>
    /// Писатель Dbf файлов
    /// </summary>
    public class DbfWriter: DbfFile
    {
        private static byte[] Empty(int len)
        {
            return Enumerable.Repeat((byte) 0x20, len).ToArray();
        }

        /// <summary>
        /// Сохранить
        /// </summary>
        /// <param name="path">путь к файлу</param>
        /// <param name="val">хранилище данных</param>
        /// <typeparam name="T">тип данных</typeparam>
        /// <exception cref="DbfMappingException">Ошибка маппинга</exception>
        public static void Save<T>(string path, List<T> val)
        {
            using (var mem = new MemoryStream())
            {
                // Тип генерика
                var type = typeof(T);
                // Поля
                var fields = type.GetFields();

                var dbf = Map(type, val.Count());

                var headbuffer = dbf.GenerateHead();
                mem.Write(headbuffer, 0, headbuffer.Length);
                
                foreach (var v in val)
                {
                    mem.WriteByte(0x20);
                    foreach (var f in fields)
                    {
                        var item = f.GetValue(v);
                        var content = dbf.Header.Columns.First(x => x.Name == f.Name);
                        var fieldlength = content.FieldLength;           
                        byte[] buffer = null;
                        if (item == null)
                            buffer = Empty(fieldlength);
                        else
                        {
                            var attribute = f.GetCustomAttributes(typeof(FieldAttribute), true).FirstOrDefault() as
                                                            FieldAttribute;
                            if (attribute == null)
                               throw new DbfMappingException("Не найден аттрибут [FieldAttribute] у поля " + f.Name);

                            if (f.FieldType == typeof(string))
                            {
                                buffer = dbf.Encoding.GetBytes(item.ToString().PadRight(attribute.Length));
                            }
                            else if (attribute.Type == 'D' && f.FieldType == typeof(DateTime?))
                            {
                                var dt = (DateTime?)item;
                                var bytelist = new List<byte>();
                                if (dt == DateTime.MinValue)
                                {
                                    buffer = Empty(fieldlength);
                                }
                                else
                                {
                                    var dtval = dt.Value;
                                    bytelist.AddRange(dbf.Encoding.GetBytes(dtval.Year.ToString(CultureInfo.InvariantCulture)));
                                    bytelist.AddRange(dbf.Encoding.GetBytes(dtval.Month.ToString("D2")));
                                    bytelist.AddRange(dbf.Encoding.GetBytes(dtval.Day.ToString("D2")));

                                    buffer = bytelist.ToArray();
                                }
                            }
                            else if (attribute.Type == 'T' && f.FieldType == typeof(DateTime?))
                            {
                                var dt = (DateTime?)item;
                                var bytelist = new List<byte>();
                                if (dt == DateTime.MinValue)
                                {
                                    buffer = Empty(fieldlength);
                                }
                                else
                                {
                                    var dtval = dt.Value;
                                    bytelist.AddRange(dbf.Encoding.GetBytes(dtval.Year.ToString(CultureInfo.InvariantCulture)));
                                    bytelist.AddRange(dbf.Encoding.GetBytes(dtval.Month.ToString("D2")));
                                    bytelist.AddRange(dbf.Encoding.GetBytes(dtval.Day.ToString("D2")));
                                    bytelist.AddRange(dbf.Encoding.GetBytes(dtval.Hour.ToString("D2")));
                                    bytelist.AddRange(dbf.Encoding.GetBytes(dtval.Minute.ToString("D2")));
                                    bytelist.AddRange(dbf.Encoding.GetBytes(dtval.Second.ToString("D2")));

                                    buffer = bytelist.ToArray();
                                }
                            }
                            else if (f.FieldType == typeof(decimal?))
                            {
                                buffer = dbf.Encoding.GetBytes(item.ToString().Replace(',', '.').PadRight(fieldlength));
                            }
                            else if (f.FieldType == typeof (bool))
                                buffer = dbf.Encoding.GetBytes((bool)item ? "T" : "F");
                        }
                        
                        if (buffer == null)
                            throw new DbfMappingException("Используется не определённый тип данных");
// ReSharper disable AssignNullToNotNullAttribute
// ReSharper disable PossibleNullReferenceException
                        mem.Write(buffer, 0, buffer.Length);
                        
// ReSharper restore PossibleNullReferenceException
// ReSharper restore AssignNullToNotNullAttribute
                    }           
                }
                mem.WriteByte(0x1A);

                using (var file = File.Create(path))
                {
                    mem.WriteTo(file);
                }    
            }
        }

        private static DbfWritterInternal Map(Type type, int count)
        {
            // Поля
            var fields = type.GetFields();
            // Агрегатор сообщений об ошибках
            var errors = new StringBuilder();
            // Перечень колонок
            var list = new List<ColumnInfo>();
            // Длинна записи целиком
            short len = 0;
            // Смещение от начала записи
            var offset = 1;

            foreach (var fieldInfo in fields)
            {
                // Берём важный атрибут, который описывает мапинг
                var attribute =
                    fieldInfo.GetCustomAttributes(typeof(FieldAttribute), true).FirstOrDefault() as FieldAttribute;

                // Если такого нет у поля - ругаемся
                if (attribute == null)
                    errors.AppendFormat("Не найден аттрибут [FieldAttribute] у поля {0}", fieldInfo.Name).AppendLine();
                else
                {
                    // Заполняем информацию о колонке таблицы
                    var item = new ColumnInfo
                    {
                        Name = fieldInfo.Name,
                        Type = (DbfColumnType)attribute.Type,
                        FieldLength = attribute.Length,
                        DecimalCount = attribute.DecimalCount,
                        FieldDataAddress = offset
                    };

                    offset += attribute.Length;
                    list.Add(item);
                    len += attribute.Length;
                }
            }

            if (errors.Length != 0)
                throw new DbfMappingException(errors.ToString());
            var dbf = new DbfWritterInternal
                {
                    Header = new DbfHeader
                        {
                            Columns = list, 
                            LengthOfEachRecord = (short)(len + 1)
                        }
                };
            dbf.Header.NumberOfRecords = count;
            dbf.Header.LengthOfHeader = (short)(31 + (32 * fields.Count()) + 1 + 1);
            return dbf;
        }
    }
}
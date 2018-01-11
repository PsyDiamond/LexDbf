using System;
using System.Text;
using LexTalionis.LexDbf.Enums;
using LexTalionis.LexDbf.Exceptions;

namespace LexTalionis.LexDbf.Common
{
    /// <summary>
    /// Информация о колонке таблицы
    /// </summary>
    public class ColumnInfo
    {
        /// <summary>
        /// Название
        /// </summary>
        public string Name;
        /// <summary>
        /// Тип
        /// </summary>
        public DbfColumnType Type;
        /// <summary>
        /// Смещение от начала записи
        /// </summary>
        public int FieldDataAddress = 1;

        /// <summary>
        /// Длинна записи
        /// </summary>
        public byte FieldLength;

        /// <summary>
        /// Количество десятичных знаков
        /// </summary>
        public byte DecimalCount;

        /// <summary>
        /// Рабочая область
        /// </summary>
        public byte WorkAreaID;

        /// <summary>
        /// Флаг
        /// </summary>
        public byte FlagForSETFIELDS;

        /// <summary>
        /// Индекс
        /// </summary>
        public byte IndexFieldFlag;

        /// <summary>
        /// Смещениие
        /// </summary>
        public int Offset;

        /// <summary>
        /// Возвращает объект <see cref="T:System.String"/>, который представляет текущий объект <see cref="T:System.Object"/>.
        /// </summary>
        /// <returns>
        /// Объект <see cref="T:System.String"/>, представляющий текущий объект <see cref="T:System.Object"/>.
        /// </returns>
        /// <filterpriority>2</filterpriority>
        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.AppendFormat("Name: {0}\n", Name);
            sb.AppendFormat("Type: {0}\n", Enum.GetName(typeof (DbfColumnType), Type));
            sb.AppendFormat("Field data address: {0}\n", FieldDataAddress);
            sb.AppendFormat("Field length: {0}\n", FieldLength);
            sb.AppendFormat("Decimal count: {0}\n", DecimalCount);
            sb.AppendFormat("Work area ID: {0}\n", WorkAreaID);
            sb.AppendFormat("Flag for SET FIELDS: {0}\n", FlagForSETFIELDS);
            sb.AppendFormat("Index field flag: {0}\n", IndexFieldFlag);

            return sb.ToString();
        }

        /// <summary>
        /// Получить тип данных подходящий для колонки таблицы
        /// </summary>
        /// <param name="type">тип данных колонки таблицы</param>
        /// <returns>системный тип</returns>
        public static Type GetTypeOfColumnT(DbfColumnType type)
        {
            string s;
            return GetTypeOfColumn(type, out s);
        }

        /// <summary>
        /// Получить строкое представление типа подходящего для колонки таблицы
        /// </summary>
        /// <param name="type">тип данных колонки таблицы</param>
        /// <returns>строкое представление</returns>
        public static string GetTypeOfColumnS(DbfColumnType type)
        {
            string s;
            GetTypeOfColumn(type, out s);
            return s;
        }
        private static Type GetTypeOfColumn(DbfColumnType type, out string text)
        {
            Type columntype;
            string stringtype;
            switch (type)
            {
                case DbfColumnType.Date:
                    columntype = typeof(DateTime?);
                    stringtype = "DateTime?";
                    break;
                case DbfColumnType.Number:
                    columntype = typeof(decimal?);
                    stringtype = "decimal?";
                    break;
                case DbfColumnType.Character:
                    columntype = typeof(string);
                    stringtype = columntype.ToString();
                    break;
                case DbfColumnType.DateTime:
                    stringtype = "DateTime?";
                    columntype = typeof(DateTime?);
                    break;
                default:
                    throw new DbfMappingException("Нет сопоставлеия для типа " + type);
            }
            text = stringtype;
            return columntype;
        }
    }
}
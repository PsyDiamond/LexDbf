using System;

namespace LexTalionis.LexDbf.Common
{
    /// <summary>
    /// Атрибут обозначающий поле таблицы
    /// </summary>
    public class FieldAttribute : Attribute
    {
        /// <summary>
        /// Тип колонки
        /// </summary>
        public char Type;
        /// <summary>
        /// Длинна
        /// </summary>
        public byte Length;
        /// <summary>
        /// Количество знаков после запятой
        /// </summary>
        public byte DecimalCount;
    }
}
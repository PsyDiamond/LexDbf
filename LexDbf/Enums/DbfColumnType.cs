namespace LexTalionis.LexDbf.Enums
{
    /// <summary>
    /// Типы данных колонок
    /// </summary>
    public enum DbfColumnType
    {
        /// <summary>
        /// Строка
        /// </summary>
        Character = 'C',
        /// <summary>
        /// Число
        /// </summary>
        Number = 'N',
        /// <summary>
        /// Дата (YYYYMMDD)
        /// </summary>
        Date = 'D',
        /// <summary>
        /// ДатаВремя (YYYYMMDDHHMMSS)
        /// </summary>
        DateTime = 'T',
        /// <summary>
        /// Логический
        /// </summary>
        Logical = 'L',
        /*FloatingPoint = 'F',
        Currency = 'Y',
        
        Integer = 'I',
        Binary = 'B'*/
    }
}
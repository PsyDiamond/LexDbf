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
        Date = 'D'
        /*Logical = 'L',
        FloatingPoint = 'F',
        Currency = 'Y',
        DateTime = 'T',
        Integer = 'I',
        Binary = 'B'*/
    }
}
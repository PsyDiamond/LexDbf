using System;

namespace LexTalionis.LexDbf.Exceptions
{
    /// <summary>
    /// Ошибка индексации
    /// </summary>
    public class DbfIndexException : Exception
    {
        /// <summary>
        /// Формирование исключениия
        /// </summary>
        /// <param name="index">индекс</param>
        public DbfIndexException(int index) 
            : base("Ошибка индексации DBF: " + index)
        {
            
        }

        /// <summary>
        /// Формирование исключениия
        /// </summary>
        /// <param name="index">индекс</param>
        public DbfIndexException(string index)
            : base("Ошибка индексации DBF: " + index)
        {
            
        }
    }
}
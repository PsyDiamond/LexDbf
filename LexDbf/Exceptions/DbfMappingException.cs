using System;

namespace LexTalionis.LexDbf.Exceptions
{
    /// <summary>
    /// Ошибка мапинга данных
    /// </summary>
    public class DbfMappingException : Exception
    {
        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="s">суть ошибки</param>
        public DbfMappingException(string s) : base("Ошибка маппинга данных: " + s)
        {
            
        }
    }
}
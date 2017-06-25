using System;

namespace LexTalionis.Dbf
{
    public class DbfMappingException : Exception
    {
        public DbfMappingException(string s) : base("Ошибка маппинга данных: " + s)
        {
            
        }
    }
}
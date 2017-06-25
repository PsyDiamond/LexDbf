using System;

namespace LexTalionis.LexDbf.Exceptions
{
    internal class DbfHeadException : Exception
    {
        public DbfHeadException(string toString)
            :base("Ошибки разабора шапки DBF\n: " + toString)
        {
            
        }
    }
}
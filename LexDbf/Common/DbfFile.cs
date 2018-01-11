using System.Text;

namespace LexTalionis.LexDbf.Common
{
    /// <summary>
    /// Ядро Dbf файла
    /// </summary>
    public class DbfFile
    {
        /// <summary>
        /// Шапка
        /// </summary>
        internal protected DbfHeader Header;
        /// <summary>
        /// Кодировка
        /// </summary>
        internal protected Encoding Encoding = Encoding.Default;
    }
}

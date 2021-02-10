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
        internal DbfHeader Header;
        /// <summary>
        /// Кодировка
        /// </summary>
        internal Encoding Encoding = Encoding.Default;
    }
}

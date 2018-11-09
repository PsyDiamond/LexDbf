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
        protected DbfHeader Header;
        /// <summary>
        /// Кодировка
        /// </summary>
        protected Encoding Encoding = Encoding.Default;
    }
}

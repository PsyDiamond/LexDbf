using System.Text;

namespace LexTalionis.LexDbf.Common
{
    /// <summary>
    /// Ядро Dbf файла
    /// </summary>
    public class DbfFile
    {
        internal protected DbfHeader Header;
        internal protected Encoding Encoding = Encoding.Default;
    }
}

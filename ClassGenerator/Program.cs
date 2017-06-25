using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using LexTalionis.LexDbf;
using LexTalionis.LexDbf.Common;

namespace ClassGenerator
{
    class Program
    {
        static void Main(string[] args)
        {

            if (args.Length != 2)
                throw new Exception("Следует запускать в виде: ClassGenerator.exe pathtodbf pathtoclass");

            var pathtodbf = args[0];
            if (!File.Exists(pathtodbf))
                throw new FileNotFoundException(pathtodbf);

            var pathtoclass = args[1];

            using (var dbf = DbfReader.Open(pathtodbf))
            {
                var filename = Path.GetFileNameWithoutExtension(pathtodbf);
                GenerateClass(dbf.GetColumns, filename, pathtoclass);
            }
            Console.WriteLine("Final");
            Console.ReadLine();
        }

        private static void GenerateClass(IEnumerable<ColumnInfo> columns, string filename, string pathtoclass)
        {
            var sb = new StringBuilder();
            var classname = string.Format("{0}DBF", filename);
            sb.AppendLine("using System;");
            sb.AppendLine("using LexTalionis.LexDbf.Common;");
            sb.AppendFormat("public class {0}", classname).AppendLine();
            sb.AppendLine("{");

            foreach (var col in columns)
            {
                var type = ColumnInfo.GetTypeOfColumnS(col.Type);
                sb.AppendFormat("\t[Field(Type = '{0}', Length = {1}, DecimalCount = {2})]",
                                (char)col.Type, col.FieldLength,
                                col.DecimalCount).AppendLine();
                sb.AppendFormat("\tpublic {0} {1};", type, col.Name).AppendLine();
            }
            sb.AppendLine("}");
            var fullpath = pathtoclass + @"\" + classname + ".cs";
            
            using (var file = File.CreateText(fullpath))
            {
                file.Write(sb.ToString());
            }
        }
    }
}

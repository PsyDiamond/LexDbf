using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using LexTalionis.LexDbf;
using LexTalionis.LexDbf.Common;

namespace ClassGenerator
{
    static class Program
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
            sb.AppendLine();

            sb.AppendLine("// ReSharper disable CheckNamespace");
            sb.AppendLine("// ReSharper disable CSharpWarnings::CS1591");
            sb.AppendLine("// ReSharper disable InconsistentNaming");
            sb.AppendLine("// ReSharper disable ClassNeverInstantiated.Global");

            sb.AppendLine("/// <summary>");
            sb.AppendFormat("/// {0}", classname).AppendLine();
            sb.AppendLine("/// </summary>");

            sb.AppendFormat("public class {0}", classname).AppendLine();

            sb.AppendLine("// ReSharper restore ClassNeverInstantiated.Global");
            sb.AppendLine("// ReSharper restore InconsistentNaming");
            sb.AppendLine("// ReSharper restore CSharpWarnings::CS1591");
            sb.AppendLine("// ReSharper restore CheckNamespace");

            sb.AppendLine("{");

            foreach (var col in columns)
            {
                var type = ColumnInfo.GetTypeOfColumnS(col.Type);
                
                sb.AppendLine("\t/// <summary>");
                sb.AppendFormat("\t/// {0}", col.Name).AppendLine();
                sb.AppendLine("\t/// </summary>");

                sb.AppendFormat("\t[Field(Type = '{0}', Length = {1}, DecimalCount = {2})]",
                                (char)col.Type, col.FieldLength,
                                col.DecimalCount).AppendLine();

                sb.AppendLine("\t// ReSharper disable CSharpWarnings::CS1591");
                sb.AppendLine("\t// ReSharper disable InconsistentNaming");
                sb.AppendLine("\t// ReSharper disable RedundantNameQualifier");
                sb.AppendLine("\t// ReSharper disable UnassignedField.Global");

                sb.AppendFormat("\tpublic {0} {1};", type, col.Name).AppendLine();

                sb.AppendLine("\t// ReSharper restore UnassignedField.Global");
                sb.AppendLine("\t// ReSharper restore RedundantNameQualifier");
                sb.AppendLine("\t// ReSharper restore InconsistentNaming");
                sb.AppendLine("\t// ReSharper restore CSharpWarnings::CS1591");
                
                sb.AppendLine();
            }

            sb.AppendLine("}");
            var fullpath = pathtoclass + @"\" + classname + ".cs";
            
            using (var file = File.CreateText(fullpath))
                file.Write(sb.ToString());
        }
    }
}

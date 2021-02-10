using System;
using System.Collections.Generic;
using LexTalionis.LexDbf.Common;

namespace LexTalionis.LexDbf
{
    internal class DbfWritterInternal : DbfFile
    {
        internal byte[] GenerateHead()
        {
            var list = new List<byte>
                {
                    (byte) Header.VersionNumber,
                    (byte) (Header.LastUpdate.Year - 1900),
                    (byte) Header.LastUpdate.Month,
                    (byte) Header.LastUpdate.Day
                };
            list.AddRange(BitConverter.GetBytes(Header.NumberOfRecords));
            list.AddRange(BitConverter.GetBytes(Header.LengthOfHeader));
            list.AddRange(BitConverter.GetBytes(Header.LengthOfEachRecord));
            list.AddRange(new byte[2]);
            list.Add((byte)Header.Transaction);
            list.Add((byte)Header.Encripted);
            list.AddRange(new byte[4]);
            list.AddRange(new byte[8]);
            list.Add(Header.MDXFlag);
            list.Add((byte)Header.LanguageDriver);
            list.AddRange(new byte[2]);

            foreach (var c in Header.Columns)
            {
                var tmp = Encoding.GetBytes(c.Name.PadRight(11, (char)0x0));
                list.AddRange(tmp);

                list.Add((byte)c.Type);
                list.AddRange(BitConverter.GetBytes(c.FieldDataAddress));
                list.Add(c.FieldLength);
                list.Add(c.DecimalCount);
                list.AddRange(new byte[2]);
                list.Add(c.WorkAreaID);
                list.AddRange(new byte[2]);
                list.Add(c.FlagForSETFIELDS);
                list.AddRange(new byte[7]);
                list.Add(c.IndexFieldFlag);
            }
            list.Add(0x0D);
            return list.ToArray();
        }
    }

}

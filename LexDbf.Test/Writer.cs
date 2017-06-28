using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using LexTalionis.LexDbf;
using NUnit.Framework;

namespace LexDbf.Test
{
    [TestFixture]
    class Writer
    {
        private const string Tmp = "tmp.dbf";

        [Test]
        public void WriteHeadOnly()
        {
            int count;
            var item = new List<KLF_NAMESDBF>();
            DbfWriter.Save(Tmp, item);
            using (var dbf = DbfReader.Open(Tmp))
            {
                count = dbf.Count;
            }
            Assert.AreEqual(0,count);
        }

        [Test]
        public void WriteNRecords()
        {
            var rand = new Random();
            var n = rand.Next(Int16.MaxValue);
            var list = new List<KLF_NAMESDBF>(Enumerable.Repeat(new KLF_NAMESDBF{FULL_NAME = "1"}, n));
            DbfWriter.Save(Tmp,list);
            int readN;
            using (var dbf = DbfReader.Open(Tmp))
            {
                readN = dbf.Count;
            }
            Assert.AreEqual(n,readN);
        }
    }
}

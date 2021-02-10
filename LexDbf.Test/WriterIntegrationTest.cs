using System;
using System.Collections.Generic;
using System.Linq;
using LexDbf.Test.StubDbf;
using LexTalionis.LexDbf;
using LexTalionis.LexDbf.Exceptions;
using Moq;
using NUnit.Framework;

namespace LexDbf.Test
{
    [TestFixture]
    class WriterIntegrationTest
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
            var list = new List<KLF_NAMESDBF>(Enumerable.Repeat(new KLF_NAMESDBF
                {
                    FULL_NAME = "1",
                    DATEF = DateTime.Now,
                    DATEEmpty = DateTime.MinValue,
                    TimeEmpty = DateTime.MinValue,
                    TimeFull = DateTime.Now,
                    Bool = false,
                    Dec = 0m
                }, n));
            DbfWriter.Save(Tmp,list);
            int readN;
            using (var dbf = DbfReader.Open(Tmp))
            {
                readN = dbf.Count;
            }
            Assert.AreEqual(n,readN);
        }

        [Test]
        public void WriteRecord_DbfMappingException()
        {
            var list = new List<ErrorDbf> {new ErrorDbf {NAME_ID = "1"}};

            Assert.Throws<DbfMappingException>(() => DbfWriter.Save(It.IsAny<string>(), list));

        }
    }
}

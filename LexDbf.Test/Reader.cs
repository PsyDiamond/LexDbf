using System.Data;
using NUnit.Framework;
using LexTalionis.LexDbf;
namespace LexDbf.Test
{
    /// <summary>
    /// Тесты читателя
    /// </summary>
    [TestFixture]
    class Reader
    {
        private const string PathToDbf = @"..\..\Dbf\KLF_NAMES.dbf";
        private const int Count = 606;
        private const string FirstColumn = "NAME_ID";

        /// <summary>
        /// Количество прочитанных записей
        /// </summary>
        [Test]
        public void CountRecordByRead()
        {
            var items = 0;
            using (var dbf = DbfReader.Open(PathToDbf))
            {
#pragma warning disable 612,618
                while (dbf.Read())
#pragma warning restore 612,618
                {
                    items++;
                }
            }
           Assert.AreEqual(Count, items,"Количество записей");
        }
        /// <summary>
        /// Наименование первой колонки
        /// </summary>
        [Test]
        public void NameFirstColumnByNumber()
        {
            string name;
            using (var dbf = DbfReader.Open(PathToDbf))
            {
                name = dbf.GetColumns[0].Name;
            }
            Assert.AreEqual(name, FirstColumn);
        }

        /// <summary>
        /// Верно ли, что с данным названием колонка стоит на первом месте
        /// </summary>
        [Test]
        public void NumberOfColumnByName()
        {
            var index = -1;
            using (var dbf = DbfReader.Open(PathToDbf))
            {
                for (var i = 0; i < dbf.GetColumns.Count; i++)
                {
                    if (dbf.GetColumns[i].Name == FirstColumn)
                        index = i;
                }

            }
            Assert.AreEqual(index, 0);
        }
        
        [Test]
        public void ReadToDataTable()
        {
            var dt = new DataTable();
            int columns;
            using (var dbf = DbfReader.Open(PathToDbf))
            {
                columns = dbf.GetColumns.Count;
#pragma warning disable 612,618
                dbf.Fill(dt);
#pragma warning restore 612,618
            }
            Assert.AreEqual(columns, dt.Columns.Count, "Колонок");
            Assert.AreEqual(Count,dt.Rows.Count, "Строк");
        }
    }
}

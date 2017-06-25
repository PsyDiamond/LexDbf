using System;
using System.Collections.Generic;
using System.Text;
using Dbf;

namespace LexTalionis.LexDbf.Common
{
    /// <summary>
    /// Заголовок файла
    /// </summary>
    public class DbfHeader
    {
        /// <summary>
        /// Сигнатура - тип файла
        /// </summary>
        protected internal Signature VersionNumber = Signature.FileWithoutDBT;
        
        /// <summary>
        /// Дата последнего изменения
        /// </summary>
        protected internal DateTime LastUpdate = new DateTime(1979, 3, 11);
        
        /// <summary>
        /// Количество записей
        /// </summary>
        protected internal int NumberOfRecords;

        /// <summary>
        /// Длинна заголовка
        /// </summary>
        protected internal short LengthOfHeader;

        /// <summary>
        /// Длинна каждой записи
        /// </summary>
        protected internal short LengthOfEachRecord;

        /// <summary>
        /// Информация о незаконченных транзакциях
        /// </summary>
        protected internal Transaction Transaction = Transaction.Ended;
        
        /// <summary>
        /// Есть ли шифрование
        /// </summary>
        protected internal Encripted Encripted = Encripted.NotEncrypted;
        /// <summary>
        /// MDX
        /// </summary>
        protected internal byte MDXFlag;
        /// <summary>
        /// Кодировка
        /// </summary>
        protected internal CodePage LanguageDriver;

        /// <summary>
        /// Колонки таблицы
        /// </summary>
        protected internal  List<ColumnInfo> Columns;

        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.AppendFormat("Signature: {0}", Enum.GetName(typeof(Signature), VersionNumber)).AppendLine();
            sb.AppendFormat("Last update: {0}", LastUpdate.ToShortDateString()).AppendLine();
            sb.AppendFormat("Number of records: {0}", NumberOfRecords).AppendLine();
            sb.AppendFormat("Length of header: {0}", LengthOfHeader).AppendLine();
            sb.AppendFormat("Length of each record: {0}", LengthOfEachRecord).AppendLine();
            sb.AppendFormat("Transaction: {0}", Enum.GetName(typeof (Transaction), Transaction)).AppendLine();
            sb.AppendFormat("Encryption flag: {0}", Enum.GetName(typeof (Encripted), Encripted)).AppendLine();
            sb.AppendFormat("MDX Flag: {0}", MDXFlag).AppendLine();
            sb.AppendFormat("Language driver: {0}", Enum.GetName(typeof (CodePage), LanguageDriver)).AppendLine();
            /*foreach (var columnInfo in Columns)
            {
                sb.AppendLine(columnInfo.ToString());
            }*/
            return sb.ToString();
        }
        
    }
}
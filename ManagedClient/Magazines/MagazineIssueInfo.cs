/* MagazineIssueInfo.cs
 */

#region Using directives

using System;
using System.Linq;

using ManagedClient.Fields;

#endregion

namespace ManagedClient.Magazines
{
    [Serializable]
    public sealed class MagazineIssueInfo
    {
        #region Properties

        public int Mfn { get; set; }

        /// <summary>
        /// Шифр документа в базе. Поле 903.
        /// </summary>
        public string DocumentCode { get; set; }

        /// <summary>
        /// Шифр журнала. Поле 933.
        /// </summary>
        public string MagazineCode { get; set; }

        /// <summary>
        /// Год. Поле 934.
        /// </summary>
        public string Year { get; set; }

        /// <summary>
        /// Том. Поле 935.
        /// </summary>
        public string Volume { get; set; }

        /// <summary>
        /// Номер, часть. Поле 936.
        /// </summary>
        public string Number { get; set; }

        /// <summary>
        /// Дополнение к номеру. Поле 931^c.
        /// </summary>
        public string Supplement { get; set; }

        /// <summary>
        /// Рабочий лист. Поле 920.
        /// (чтобы отличать подшивки от выпусков журналов)
        /// </summary>
        public string Worksheet { get; set; }

        /// <summary>
        /// Расписанное оглавление. Поле 922.
        /// </summary>
        public MagazineArticleInfo[] Articles { get; set; }

        /// <summary>
        /// Экземпляры. Поле 910.
        /// </summary>
        public ExemplarInfo[] Exemplars { get; set; }

        #endregion

        #region Construction

        #endregion

        #region Private members

        #endregion

        #region Public methods

        public static MagazineIssueInfo Parse
            (
                IrbisRecord record
            )
        {
            if (ReferenceEquals(record, null))
            {
                throw new ArgumentNullException("record");
            }

            MagazineIssueInfo result = new MagazineIssueInfo
            {
                Mfn = record.Mfn,
                DocumentCode = record.FM("903"),
                MagazineCode = record.FM("933"),
                Year = record.FM("934"),
                Volume = record.FM("935"),
                Number = record.FM("936"),
                Supplement = record.FM("931", 'c'),
                Worksheet = record.FM("920"),
                Articles = record.Fields
                    .GetField("922")
                    .Select(field => MagazineArticleInfo.Parse(field))
                    .ToArray(),
                Exemplars = record.Fields
                    .GetField("910")
                    .Select(field => ExemplarInfo.Parse(field))
                    .ToArray()
            };

            if (string.IsNullOrEmpty(result.Number))
            {
                return null;
            }

            return result;
        }

        public static int CompareNumbers
            (
                MagazineIssueInfo first,
                MagazineIssueInfo second
            )
        {
            return NumberText.Compare(first.Number, second.Number);
        }

        #endregion

        #region Object info

        public override string ToString()
        {
            //return string.Format
            //    (
            //        "MagazineCode: {0}, Year: {1}, Volume: {2}, Number: {3}", 
            //        MagazineCode, 
            //        Year, 
            //        Volume, 
            //        Number
            //    );
            if (!string.IsNullOrEmpty(Supplement))
            {
                return string
                    .Format("{0} ({1})", Number, Supplement)
                    .Trim();
            }
            return Number.Trim();
        }

        #endregion
    }
}

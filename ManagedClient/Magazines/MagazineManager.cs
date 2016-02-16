/* MagazineManager.cs
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.Linq;

#endregion

namespace ManagedClient.Magazines
{
    /// <summary>
    /// Работа с периодикой.
    /// </summary>
    public sealed class MagazineManager
    {
        #region Constants

        /// <summary>
        /// Вид документа – сводное описание газеты.
        /// </summary>
        public const string Newspaper = "V=01";

        /// <summary>
        /// Вид документа – сводное описание журнала.
        /// </summary>
        public const string Magazine = "V=02";

        #endregion

        #region Properties

        public ManagedClient64 Client{get { return _client; }}

        #endregion

        #region Construction

        public MagazineManager 
            ( 
                ManagedClient64 client 
            )
        {
            if ( ReferenceEquals ( client, null ) )
            {
                throw new ArgumentNullException("client");
            }

            _client = client;
        }

        #endregion

        #region Private members

        private readonly ManagedClient64 _client;

        #endregion

        #region Public methods

        public MagazineInfo[] GetAllMagazines ()
        {
            List<MagazineInfo> result = new List<MagazineInfo>();

            BatchRecordReader batch = new BatchRecordReader
                (
                    Client,
                    String.Concat ( Newspaper, " + ", Magazine )
                );
            foreach (IrbisRecord record in batch)
            {
                if (!ReferenceEquals(record, null))
                {
                    MagazineInfo magazine = MagazineInfo.Parse(record);
                    if (!ReferenceEquals(magazine, null))
                    {
                        result.Add(magazine);
                    }
                }
            }

            return result.ToArray();
        }

        public MagazineInfo GetMagazine
            (
                MagazineIssueInfo issue
            )
        {
            if (ReferenceEquals(issue, null))
            {
                throw new ArgumentNullException("issue");
            }
            return null;
        }

        public MagazineIssueInfo GetIssue
            (
                MagazineArticleInfo article
            )
        {
            if (ReferenceEquals(article, null))
            {
                throw new ArgumentNullException("article");
            }

            return null;
        }

        public MagazineIssueInfo[] GetIssues
            (
                MagazineInfo magazine
            )
        {
            if (ReferenceEquals(magazine, null))
            {
                throw new ArgumentNullException("magazine");
            }

            IrbisRecord[] records = Client.SearchRead
                (
                    "\"I={0}/$\"",
                    magazine.Index
                );

            MagazineIssueInfo[] result = records
                .NonNullItems()
                .Select(record => MagazineIssueInfo.Parse(record))
                .NonNullItems()
                .ToArray();

            return result;
        }

        public MagazineArticleInfo[] GetArticles
            (
                MagazineIssueInfo issue
            )
        {
            if (ReferenceEquals(issue, null))
            {
                throw new ArgumentNullException("issue");
            }
            return null;
        }

        public MagazineManager CreateMagazine
            (
                MagazineInfo magazine
            )
        {
            return this;
        }

        #endregion
    }
}

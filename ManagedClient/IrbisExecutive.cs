/* IrbisExecutive.cs
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ManagedClient.Requests;

#endregion

namespace ManagedClient
{
    using Magazines;
    using Readers;
    using Transactions;
    using Query;

    /// <summary>
    /// 
    /// </summary>
    public sealed class IrbisExecutive
        : IDisposable
    {
        #region Properties

        public ManagedClient64 Client { get; private set; }

        public QueryManager Queries { get; private set; }

        public MagazineManager Magazines { get; private set; }

        public IrbisTransactionManager Transactions { get; private set; }

        public ReaderManager Readers { get; private set; }

        //public RequestManager Requests { get; private set; }

        #endregion

        #region Construction

        internal IrbisExecutive
            (
                ManagedClient64 client
            )
        {
            if (ReferenceEquals(client, null))
            {
                throw new ArgumentNullException("client");
            }

            Client = client;
            Queries = new QueryManager();
            Magazines = new MagazineManager(Client);
            Transactions = new IrbisTransactionManager(Client);
            Readers = new ReaderManager(Client);
            //Requests = new RequestManager();
        }

        #endregion

        #region Private members

        private bool _clientNotOwned;

        #endregion

        #region Public methods

        public static IrbisExecutive FromClientSettings
            (
                string fileName
            )
        {
            return new IrbisExecutive(null);
        }

        public static IrbisExecutive FromConnectionString
            (
                string connectionString
            )
        {
            ManagedClient64 client = new ManagedClient64();
            client.ParseConnectionString(connectionString);
            return new IrbisExecutive(client);
        }

        public static IrbisExecutive FromManagedClient
            (
                ManagedClient64 client
            )
        {
            IrbisExecutive result = new IrbisExecutive(client)
            {
                _clientNotOwned = true
            };

            return result;
        }

        #endregion

        #region IDisposable members

        public void Dispose()
        {
            if (!_clientNotOwned 
                && !ReferenceEquals(Client, null))
            {
                Client.Dispose();
            }
        }

        #endregion
    }
}

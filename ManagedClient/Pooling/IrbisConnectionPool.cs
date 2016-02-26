/* IrbisConnectionPool.cs -- пул соединений с сервером.
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


#endregion

namespace ManagedClient.Pooling
{
    /// <summary>
    /// Пул соединений с сервером.
    /// </summary>
    public class IrbisConnectionPool
    {
        #region Properties

        public static string DefaultConnectionString { get; set; }

        public static int DefaultCapacity
        {
            get { return _defaultCapacity; }
            set { _defaultCapacity = value; }
        }

        public int Capacity { get; set; }

        public string ConnectionString { get; set; }

        #endregion

        #region Construction

        /// <summary>
        /// Конструктор по умолчанию.
        /// </summary>
        public IrbisConnectionPool()
        {
            if (string.IsNullOrEmpty(DefaultConnectionString))
            {
                DefaultConnectionString
                    = IrbisUtilities.GetConnectionString();
            }
            ConnectionString = DefaultConnectionString;
        }

        public IrbisConnectionPool
            (
                int capacity
            )
            : this()
        {
            Capacity = capacity;
        }

        /// <summary>
        /// Конструктор с конкретной строкой соединения.
        /// </summary>
        /// <param name="connectionString"></param>
        public IrbisConnectionPool
            (
                string connectionString
            )
        {
            ConnectionString = connectionString;
        }

        public IrbisConnectionPool
            (
                int capacity,
                string connectionString
            )
        {
            Capacity = capacity;
            ConnectionString = connectionString;
        }

        #endregion

        #region Private members

        private static int _defaultCapacity = 5;

        private readonly List<ManagedClient64> _connections
            = new List<ManagedClient64>();

        private readonly object _lockRoot = new object();

        private ManagedClient64 _GetClient()
        {
            ManagedClient64 result = new ManagedClient64();
            result.ParseConnectionString(ConnectionString);
            result.Connect();

            return result;
        }

        #endregion

        #region Public methods

        public ManagedClient64 AcquireConnection()
        {
            lock (_lockRoot)
            {
                return _GetClient();
            }
        }

        public void ReleaseConnection
            (
                ManagedClient64 client
            )
        {
            lock (_lockRoot)
            {
                if (client != null)
                {
                    client.Disconnect();
                }
            }
        }

        #endregion
    }
}

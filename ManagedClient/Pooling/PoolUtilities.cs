/* PoolUtilities.cs
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
    /// Утилиты для работы пулом соединений.
    /// </summary>
    public static class PoolUtilities
    {
        #region Private members

        #endregion

        #region Public methods

        public static IrbisRecord ReadRecord
            (
                this IrbisConnectionPool pool,
                int mfn
            )
        {
            ManagedClient64 client = pool.AcquireConnection();
            IrbisRecord result = client.ReadRecord(mfn);
            pool.ReleaseConnection(client);
            return result;
        }

        public static int[] Search
            (
                this IrbisConnectionPool pool,
                string format,
                params object[] args
            )
        {
            ManagedClient64 client = pool.AcquireConnection();
            int[] result = client.Search(format, args);
            pool.ReleaseConnection(client);
            return result;
        }

        public static void WriteRecord
            (
                this IrbisConnectionPool pool,
                IrbisRecord record
            )
        {
            ManagedClient64 client = pool.AcquireConnection();
            client.WriteRecord(record, false, true);
            pool.ReleaseConnection(client);
        }

        #endregion
    }
}

﻿/* PoolUtilities.cs
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using JetBrains.Annotations;

#endregion

namespace ManagedClient.Pooling
{
    /// <summary>
    /// Утилиты для работы пулом соединений.
    /// </summary>
    [PublicAPI]
    public static class PoolUtilities
    {
        #region Private members

        #endregion

        #region Public methods

        /// <summary>
        /// Чтение записи с помощью пула.
        /// </summary>
        [NotNull]
        public static IrbisRecord ReadRecord
            (
                [NotNull] this IrbisConnectionPool pool,
                int mfn
            )
        {
            ManagedClient64 client = pool.AcquireConnection();
            IrbisRecord result = client.ReadRecord(mfn);
            pool.ReleaseConnection(client);
            return result;
        }

        /// <summary>
        /// Поиск в каталоге с помощью пула.
        /// </summary>
        [NotNull]
        public static int[] Search
            (
                [NotNull] this IrbisConnectionPool pool,
                string format,
                params object[] args
            )
        {
            ManagedClient64 client = pool.AcquireConnection();
            int[] result = client.Search(format, args);
            pool.ReleaseConnection(client);
            return result;
        }

        /// <summary>
        /// Сохранение записей с помощью пула.
        /// </summary>
        public static void WriteRecord
            (
                [NotNull] this IrbisConnectionPool pool,
                [NotNull] IrbisRecord record
            )
        {
            ManagedClient64 client = pool.AcquireConnection();
            client.WriteRecord(record, false, true);
            pool.ReleaseConnection(client);
        }

        #endregion
    }
}

// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* BeforeQueryEventArgs.cs
 */

#region Using directives

using System;

using JetBrains.Annotations;

#endregion

namespace ManagedClient
{
    using Query;

    /// <summary>
    /// Трансформация поискового запроса перед отправкой на сервер.
    /// </summary>
    public sealed class BeforeQueryEventArgs
        : EventArgs
    {
        #region Properties

        /// <summary>
        /// Client.
        /// </summary>
        [NotNull]
        public ManagedClient64 Client { get; internal set; }

        /// <summary>
        /// Query manager.
        /// </summary>
        [NotNull]
        public QueryManager QueryManager { get; internal set; }

        /// <summary>
        /// Original query.
        /// </summary>
        [NotNull]
        public string OriginalQuery { get; internal set; }

        /// <summary>
        /// Processed query.
        /// </summary>
        [NotNull]
        public string ProcessedQuery { get; set; }

        /// <summary>
        /// Cancel the query?
        /// </summary>
        public bool CancelQuery { get; set; }

        /// <summary>
        /// Arbitrary user data.
        /// </summary>
        public object AuxiliaryData { get; set; }

        #endregion
    }
}

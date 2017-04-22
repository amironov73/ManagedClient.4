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
    [Serializable]
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

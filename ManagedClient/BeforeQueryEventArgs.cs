/* BeforeQueryEventArgs.cs
 */

#region Using directives

using System;

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

        public ManagedClient64 Client { get; internal set; }

        public QueryManager QueryManager { get; internal set; }

        public string OriginalQuery { get; internal set; }

        public string ProcessedQuery { get; set; }

        public bool CancelQuery { get; set; }

        public object AuxiliaryData { get; set; }

        #endregion
    }
}

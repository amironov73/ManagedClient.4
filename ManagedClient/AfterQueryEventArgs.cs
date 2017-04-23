// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* AfterQueryEventArgs.cs
 */

#region Using directives

using System;
using System.Collections.Generic;

#endregion

namespace ManagedClient
{
    using Query;

    /// <summary>
    /// Трансформация результатов поискового запроса, полученных от сервера.
    /// </summary>
    public sealed class AfterQueryEventArgs
        : EventArgs
    {
        #region Properties

        /// <summary>
        /// Connection.
        /// </summary>
        public ManagedClient64 Client { get; internal set; }

        /// <summary>
        /// Query manager.
        /// </summary>
        public QueryManager QueryManager { get; internal set; }

        /// <summary>
        /// Original query text.
        /// </summary>
        public string OriginalQuery { get; internal set; }

        /// <summary>
        /// Rewritten query text.
        /// </summary>
        public string ProcessedQuery { get; internal set; }

        /// <summary>
        /// Server answer.
        /// </summary>
        public List<IrbisAnswerItem> Answer { get { return _answer; } }

        /// <summary>
        /// Arbitrary user data.
        /// </summary>
        public object AuxiliaryData { get; set; }

        #endregion

        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        public AfterQueryEventArgs()
        {
            _answer = new List<IrbisAnswerItem>();
        }

        #endregion

        #region Private members

        private readonly List<IrbisAnswerItem> _answer;

        #endregion
    }
}

﻿// This is an open source non-commercial project. Dear PVS-Studio, please check it.
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

        public ManagedClient64 Client { get; internal set; }

        public QueryManager QueryManager { get; internal set; }

        public string OriginalQuery { get; internal set; }

        public string ProcessedQuery { get; internal set; }

        public List<IrbisAnswerItem> Answer { get { return _answer; } }

        public object AuxiliaryData { get; set; }

        #endregion

        #region Construction

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

﻿// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* GblEventArgs.cs
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

#endregion

namespace ManagedClient.Gbl
{
    [Serializable]
    public sealed class GblEventArgs
        : EventArgs
    {
        #region Properties

        public DateTime TimeStarted { get; set; }

        public TimeSpan TimeElapsed { get; set; }

        public int RecordsProcessed { get; set; }

        public int RecordsSucceeded { get; set; }

        public int RecordsFailed { get; set; }

        public bool Cancel { get; set; }

        #endregion
    }
}

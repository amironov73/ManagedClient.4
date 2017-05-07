// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* GblEventArgs.cs
 */

#region Using directives

using System;

using JetBrains.Annotations;

using MoonSharp.Interpreter;

#endregion

namespace ManagedClient.Gbl
{
    /// <summary>
    /// 
    /// </summary>
    [PublicAPI]
    [Serializable]
    [MoonSharpUserData]
    public sealed class GblEventArgs
        : EventArgs
    {
        #region Properties

        /// <summary>
        /// When GBL started.
        /// </summary>
        public DateTime TimeStarted { get; set; }

        /// <summary>
        /// Elapsed time.
        /// </summary>
        public TimeSpan TimeElapsed { get; set; }

        /// <summary>
        /// Number of processed records.
        /// </summary>
        public int RecordsProcessed { get; set; }

        /// <summary>
        /// Number of succeeded records.
        /// </summary>
        public int RecordsSucceeded { get; set; }

        /// <summary>
        /// Number of failed records.
        /// </summary>
        public int RecordsFailed { get; set; }

        /// <summary>
        /// Whether the GBL was failed?
        /// </summary>
        public bool Cancel { get; set; }

        #endregion
    }
}

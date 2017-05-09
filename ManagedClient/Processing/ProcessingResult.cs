// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* ProcessingResult.cs
 */

#region Using directives

using System;

using JetBrains.Annotations;

using MoonSharp.Interpreter;

#endregion

namespace ManagedClient.Processing
{
    /// <summary>
    /// Result of record processing.
    /// </summary>
    [PublicAPI]
    [Serializable]
    [MoonSharpUserData]
    public sealed class ProcessingResult
    {
        #region Properties

        /// <summary>
        /// Whether the processing is cancelled?
        /// </summary>
        public bool Cancel { get; set; }

        /// <summary>
        /// Accumulated text.
        /// </summary>
        public string Result { get; set; }

        #endregion
    }
}

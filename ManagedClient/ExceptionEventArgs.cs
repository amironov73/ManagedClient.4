// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* ExceptionEventArgs.cs
 */

#region Using directives

using System;
using System.Runtime.InteropServices;

using JetBrains.Annotations;

#endregion

namespace ManagedClient
{
    /// <summary>
    /// Arguments for exception event.
    /// </summary>
    [Serializable]
    [ComVisible(false)]
    public sealed class ExceptionEventArgs<T>
        : EventArgs
        where T: Exception
    {
        #region Properties

        /// <summary>
        /// Exception itself.
        /// </summary>
        public T Exception { get; set; }

        #endregion

        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        public ExceptionEventArgs
            (
                [NotNull] T exception
            )
        {
            Exception = exception;
        }

        #endregion
    }
}

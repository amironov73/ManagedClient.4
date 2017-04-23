// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* ExceptionEventArgs.cs
 */

#region Using directives

using System;
using System.Runtime.InteropServices;

#endregion

namespace ManagedClient
{
    [Serializable]
    [ComVisible(false)]
    public sealed class ExceptionEventArgs<T>
        : EventArgs
        where T: Exception
    {
        #region Properties

        public T Exception { get; set; }

        #endregion

        #region Construction

        public ExceptionEventArgs
            (
                T exception
            )
        {
            Exception = exception;
        }

        #endregion
    }
}

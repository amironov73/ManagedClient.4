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

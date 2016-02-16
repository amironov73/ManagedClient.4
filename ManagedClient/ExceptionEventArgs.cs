/* ExceptionEventArgs.cs
 */

#region Using directives

using System;

#endregion

namespace ManagedClient
{
    [Serializable]
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

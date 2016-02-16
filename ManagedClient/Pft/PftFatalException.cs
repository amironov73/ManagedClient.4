/* PftFatalException.cs
 */

#region Using directives

using System;

#endregion

namespace ManagedClient.Pft
{
    /// <summary>
    /// Выбрасывается функцией <code>fatal ('text')</code>.
    /// </summary>
    [Serializable]
    public sealed class PftFatalException
        : ApplicationException
    {
        #region Properties

        #endregion

        #region Construction

        public PftFatalException()
        {
        }

        public PftFatalException
            (
                string message
            )
            : base(message)
        {
        }

        public PftFatalException
            (
                string message, 
                Exception innerException
            )
            : base
            (
                message, 
                innerException
            )
        {
        }

        #endregion

        #region Private members

        #endregion

        #region Public methods

        #endregion
    }
}

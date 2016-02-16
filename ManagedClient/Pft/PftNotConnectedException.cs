/* PftNotConnectedException.cs
 */

#region Using directives

using System;

#endregion

namespace ManagedClient.Pft
{
    /// <summary>
    /// Возникает, когда необходимо обращение к серверу,
    /// а подключение отсутствует.
    /// </summary>
    [Serializable]
    public sealed class PftNotConnectedException
        : PftException
    {
        #region Properties

        #endregion

        #region Construciton

        public PftNotConnectedException()
        {
        }

        public PftNotConnectedException(string message) 
            : base(message)
        {
        }

        public PftNotConnectedException
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

        #region Public methods

        #endregion
    }
}

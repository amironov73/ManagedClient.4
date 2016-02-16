/* PftException.cs
 */

#region Using directives

using System;

#endregion

namespace ManagedClient.Pft
{
    /// <summary>
    /// Базовый класс для исключений, происходящих при
    /// разборе и исполнении PFT-скриптов.
    /// </summary>
    [Serializable]
    public class PftException
        : ApplicationException
    {
        #region Properties

        #endregion

        #region Construciton

        public PftException()
        {
        }

        public PftException(string message) 
            : base(message)
        {
        }

        public PftException
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

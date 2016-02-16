/* PftAbstractionLayer.cs
 */

#region Using directives

using System;

#endregion

namespace ManagedClient.Pft.Abstraction
{
    [Serializable]
    public class PftAbstractionLayer
    {
        #region Events

        #endregion

        #region Properties

        #endregion

        #region Construction

        #endregion

        #region Private members

        #endregion

        #region Public methods

        public virtual string Ask
            (
                string prompt
            )
        {
            return null;
        }
        
        public virtual void Beep()
        {            
        }

        #endregion
    }
}

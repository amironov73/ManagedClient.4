/* IrbisSocket.cs
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

#endregion

namespace ManagedClient.Sockets
{
    /// <summary>
    /// 
    /// </summary>
    public class IrbisSocket
        : IDisposable
    {
        #region Properties

        public virtual bool Debug { get; set; }

        #endregion

        #region Construction

        #endregion

        #region Public methods

        public virtual void Cancel()
        {
        }

        public virtual void Receive()
        {
        }

        public virtual void Send()
        {
        }

        #endregion

        #region IDisposable members

        public void Dispose()
        {
        }

        #endregion

        #region Object members

        /// <summary>
        /// Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        /// <returns>A <see cref="System.String" /> that represents this instance.</returns>
        public override string ToString()
        {
            return "IrbisSocket";
        }

        #endregion
    }
}

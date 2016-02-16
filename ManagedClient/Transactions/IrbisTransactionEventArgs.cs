/* IrbisTransactionEventArgs.cs
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

#endregion

namespace ManagedClient.Transactions
{
    [Serializable]
    public sealed class IrbisTransactionEventArgs
        : EventArgs
    {
        #region Properties

        public ManagedClient64 Client { get; internal set; }

        public IrbisTransactionContext Context { get; internal set; }

        public IrbisTransactionItem Item { get; internal set; }

        #endregion
    }
}

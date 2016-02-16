/* IrbisTransactionManager.cs
 */

#region Using directives

using System;

#endregion

namespace ManagedClient.Transactions
{
    /// <summary>
    /// Менеджер транзакций
    /// </summary>
    public sealed class IrbisTransactionManager
        : IDisposable
    {
        #region Properties

        public IrbisTransactionContext Context { get; private set; }

        public ManagedClient64 Client { get; private set; }

        public bool InTransactionNow
        {
            get
            {
                return (Context.Items.Count != 0);
            }
        }

        #endregion

        #region Construction

        public IrbisTransactionManager
            (
                ManagedClient64 client
            )
        {
            if (ReferenceEquals(client, null))
            {
                throw new ArgumentNullException("client");
            }

            Context = new IrbisTransactionContext();

            Client = client;
            Client.Transaction += _EventHandler;
        }

        #endregion

        #region Private members

        private void _EventHandler
            (
                object sender, 
                IrbisTransactionEventArgs eventArgs
            )
        {
            Context.Items.Add
                (
                    eventArgs.Item
                );
        }

        #endregion

        #region Public methods

        public void BeginTransaction
            (
                string name
            )
        {
            Context = new IrbisTransactionContext
                (
                    name,
                    Context
                );
        }

        public void CommitTransaction()
        {
            Context = Context.ParentContext;
            if (ReferenceEquals(Context, null))
            {
                Context = new IrbisTransactionContext();
            }
        }

        public void RollbackTransaction()
        {
            throw new NotImplementedException("Rollback transaction");
        }

        public void CommitAllTransactions()
        {
            throw new NotImplementedException("CommitAllTransactions");
        }

        public void RollbackAllTransactions()
        {
            throw new NotImplementedException("RollbackAllTransactions");
        }

        #endregion

        #region IDisposable members

        /// <summary>
        /// Performs application-defined tasks associated with freeing, 
        /// releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            if (Client != null)
            {
                Client.Transaction -= _EventHandler;
                Client = null;
            }
        }

        #endregion
    }
}

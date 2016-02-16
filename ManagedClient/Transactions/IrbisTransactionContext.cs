﻿/* IrbisTransactionContext.cs
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
    public sealed class IrbisTransactionContext
    {
        #region Properties

        public List<IrbisTransactionItem> Items
        {
            get { return _items; }
        }

        public string Name { get; private set; }

        public IrbisTransactionContext ParentContext
        {
            get; private set; 
        }

        #endregion

        #region Construction

        public IrbisTransactionContext()
        {
            _items = new List<IrbisTransactionItem>();
        }

        public IrbisTransactionContext
            (
                string name
            )
            : this ()
        {
            Name = name;
        }

        public IrbisTransactionContext
            (
                IrbisTransactionContext parentContext
            )
            : this ()
        {
            ParentContext = parentContext;
        }

        public IrbisTransactionContext
            (
                string name, 
                IrbisTransactionContext parentContext
            )
            : this ()
        {
            Name = name;
            ParentContext = parentContext;
        }

        #endregion

        #region Private members

        private readonly List<IrbisTransactionItem> _items;

        #endregion

        #region Public methods

        public void Clear()
        {
            Items.Clear();
        }

        #endregion
    }
}

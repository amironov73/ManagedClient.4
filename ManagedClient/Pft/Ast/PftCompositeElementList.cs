/* PftCompositeElementList.cs
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

#endregion

namespace ManagedClient.Pft.Ast
{
    [Serializable]
    public sealed class PftCompositeElementList
        : Collection<PftCompositeElement>
    {
        #region Properties

        public List<PftAst> List
        {
            get { return _list; }
            set { _list = value; }
        } 

        #endregion

        #region Construction

        public PftCompositeElementList()
        {
        }

        public PftCompositeElementList
            (
                List<PftAst> list
            )
        {
            _list = list;
        }

        #endregion

        #region Private members

        private List<PftAst> _list;

        #endregion

        #region Public methods

        #endregion

        #region Collection<T> members

        protected override void ClearItems()
        {
            foreach (PftCompositeElement element in this)
            {
                List.Remove(element);
            }

            base.ClearItems();
        }

        protected override void InsertItem
            (
                int index, 
                PftCompositeElement item
            )
        {
            if (ReferenceEquals(item, null))
            {
                throw new ArgumentNullException("item");
            }

            if (!List.Contains(item))
            {
                List.Add(item);
            }

            base.InsertItem(index, item);
        }

        protected override void RemoveItem
            (
                int index
            )
        {
            PftCompositeElement element = this[index];
            if (List.Contains(element))
            {
                List.Remove(element);
            }

            base.RemoveItem(index);
        }

        protected override void SetItem
            (
                int index, 
                PftCompositeElement item
            )
        {
            if (ReferenceEquals(item, null))
            {
                throw new ArgumentNullException("item");    
            }

            PftCompositeElement element = this[index];
            if (List.Contains(element))
            {
                List.Remove(element);
            }
            if (!List.Contains(item))
            {
                List.Add(item);
            }
            base.SetItem(index, item);
        }

        #endregion
    }
}

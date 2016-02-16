/* AstTextCollection.cs
 */

#region Using directives

using System;
using System.Collections.ObjectModel;
using System.Text;

#endregion

namespace ManagedClient.Pft
{
    [Serializable]
    public sealed class AstTextCollection
        : Collection<AstText>
    {
        #region Collection<T> members

        protected override void InsertItem
            (
                int index, 
                AstText item
            )
        {
            if (ReferenceEquals(item, null)
                || ReferenceEquals(item.Node, null)
                || ReferenceEquals(item.Text, null))
            {
                throw new ArgumentNullException("item");
            }

            base.InsertItem(index, item);
        }

        protected override void SetItem
            (
                int index, 
                AstText item
            )
        {
            if (ReferenceEquals(item, null)
                || ReferenceEquals(item.Node, null)
                || ReferenceEquals(item.Text, null))
            {
                throw new ArgumentNullException("item");
            }

            base.SetItem(index, item);
        }

        #endregion

        #region Public methods

        public AstTextCollection Compress()
        {
            // TODO: implement compression
            return this;
        }

        public AstTextCollection Write
            (
                PftAst node,
                string format,
                params object[] args
            )
        {
            if (ReferenceEquals(format, null))
            {
                throw new ArgumentNullException("format");
            }

            AstText item = new AstText
                (
                    node,
                    string.Format(format, args)
                );
            Add(item);

            return this;
        }

        #endregion

        #region Object members

        public override string ToString()
        {
            StringBuilder result = new StringBuilder();
            foreach (AstText node in this)
            {
                result.Append(node.Text);
            }
            return result.ToString();
        }

        #endregion
    }
}

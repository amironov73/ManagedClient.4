/* PftGroupItem.cs
 */

#region Using directives

using System;

using Antlr4.Runtime.Tree;

#endregion

namespace ManagedClient.Pft
{
    /// <summary>
    /// Элемент формата, могущий воходить в повторяющуюся группу.
    /// (А может и не входить, вот в чём фокус!)
    /// </summary>
    [Serializable]
    public abstract class PftGroupItem
        : PftAst
    {
        #region Properties

        /// <summary>
        /// Если не <c>null</c>, то элемент входит в повторяющуюся группу
        /// и должен обрабатываться соответственно.
        /// </summary>
        public PftGroupStatement Group { get; set; }
        
        #endregion

        #region Construction

        protected PftGroupItem()
        {
        }

        internal PftGroupItem(IParseTree node)
            : base(node)
        {
        }

        #endregion
    }
}

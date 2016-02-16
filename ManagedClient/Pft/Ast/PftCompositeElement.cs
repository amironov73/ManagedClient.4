/* PftCompositeElement.cs
 */

#region Using directives

using System;

#endregion

namespace ManagedClient.Pft.Ast
{
    /// <summary>
    /// Составной элемент.
    /// </summary>
    [Serializable]
    public sealed class PftCompositeElement
        : PftAst
    {
        #region Properties

        #endregion

        #region Construction

        public PftCompositeElement()
        {
        }

        public PftCompositeElement
            (
                PftParser.CompositeElementContext context
            )
        {
            DiscoverChildren(context);
        }

        #endregion

        #region Private members

        #endregion

        #region Public methods

        #endregion
    }
}

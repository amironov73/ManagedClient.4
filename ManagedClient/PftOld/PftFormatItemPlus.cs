/* PftFormatItemPlus.cs
 */

#region Using directives

using System;
using System.Collections.Generic;

#endregion

namespace ManagedClient.Pft
{
    [Serializable]
    public sealed class PftFormatItemPlus
        : PftAst
    {
        #region Properties

        #endregion

        #region Construction

        public PftFormatItemPlus()
        {
        }

        public PftFormatItemPlus(PftParser.FormatItemPlusContext node) 
            : base(node)
        {
            foreach (PftParser.FormatItemContext context in node.formatItem())
            {
                PftAst item = PftNonGrouped.DispatchContext(context);
                Children.Add(item);
            }
        }

        #endregion

        #region Private members

        #endregion

        #region PftAst members

        public override void Execute
            (
                PftContext context
            )
        {
            foreach (PftAst item in Children)
            {
                item.Execute(context);
            }
        }

        #endregion
    }
}

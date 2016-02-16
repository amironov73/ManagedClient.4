/* PftProgram.cs
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Antlr4.Runtime;

#endregion

namespace ManagedClient.Pft.Ast
{
    /// <summary>
    /// Верхний элемент дерева разбора – собственно программа.
    /// </summary>
    [Serializable]
    public sealed class PftProgram
        : PftAst
    {
        #region Properties

        public PftInfoBlock Info { get; set; }

        #endregion

        #region Construction

        public PftProgram()
        {
        }

        public PftProgram
            (
                PftParser.ProgramContext context
            )
        {
            if (context.pftInfoBlock() != null)
            {
                Info = new PftInfoBlock(context.pftInfoBlock());
                Children.Add(Info);
            }

            foreach (PftParser.CompositeElementContext subContext 
                in context.compositeElement())
            {
                PftAst child = PftDispatcher.DispatchFormat(subContext);
                if (!ReferenceEquals(child, null))
                {
                    Children.Add(child);
                }
            }
        }

        #endregion
    }
}

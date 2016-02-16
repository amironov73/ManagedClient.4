/* PftPercentOperator.cs
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

#endregion

namespace ManagedClient.Pft.Ast
{
    /// <summary>
    /// Удаляет ранее созданные пустые строки, если они есть.
    /// </summary>
    [Serializable]
    public sealed class PftPercentOperator
        : PftAst
    {
        #region Properties

        #endregion

        #region Construction

        public PftPercentOperator()
        {
        }

        public PftPercentOperator
            (
                PftParser.PercentOperatorContext context
            )
        {
            // Nothing to do actually
        }

        #endregion

        #region Private members

        #endregion

        #region Public methods

        #endregion

        #region PftAst members

        public override void Execute
            (
                PftContext context
            )
        {
            context.Output.RemoveEmptyLine();
        }

        public override void Write
            (
                StreamWriter writer
            )
        {
            // Обрамляем пробелами
            writer.Write(" % ");
        }

        #endregion
    }
}

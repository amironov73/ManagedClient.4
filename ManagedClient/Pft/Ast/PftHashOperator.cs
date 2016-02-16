/* PftHashOperator.cs
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
    /// Команда вертикального позиционирования.
    /// Безусловный переход на новую строку.
    /// </summary>
    [Serializable]
    public sealed class PftHashOperator
        : PftAst
    {
        #region Properties

        #endregion

        #region Construction

        public PftHashOperator()
        {
        }

        public PftHashOperator
            (
                PftParser.HashOperatorContext context
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
            OnBeforeExecution(context);

            context.WriteLine(this);

            OnAfterExecution(context);
        }

        public override void Write
            (
                StreamWriter writer
            )
        {
            // Обрамляем пробелами
            writer.Write(" # ");
        }

        #endregion
    }
}

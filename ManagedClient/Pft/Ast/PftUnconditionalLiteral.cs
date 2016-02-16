/* PftUnconditionalLiteral.cs
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

#endregion

namespace ManagedClient.Pft.Ast
{
    /// <summary>
    /// Безусловный литерал (ограниченный одинарными кавычками).
    /// </summary>
    [Serializable]
    public sealed class PftUnconditionalLiteral
        : PftAst
    {
        #region Properties

        #endregion

        #region Construction

        public PftUnconditionalLiteral()
        {
        }

        public PftUnconditionalLiteral
            (
                string text
            )
        {
            if (ReferenceEquals(text, null))
            {
                throw new ArgumentNullException("text");
            }
            Text = text;
        }

        public PftUnconditionalLiteral
            (
                PftParser.UnconditionalLiteralContext context
            )
        {
            string text = context.GetText();
            Text = PftUtility.PrepareLiteral(text);
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
            context.Write
                (
                    this,
                    Text
                );
        }

        #endregion
    }
}

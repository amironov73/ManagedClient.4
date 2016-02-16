/* PftConditionalLiteral.cs
 */

#region Using directives

using System;

#endregion

namespace ManagedClient.Pft.Ast
{
    /// <summary>
    /// Условный литерал.
    /// </summary>
    [Serializable]
    public sealed class PftConditionalLiteral
        : PftAst
    {
        #region Properties

        #endregion

        #region Construction

        public PftConditionalLiteral()
        {
        }

        public PftConditionalLiteral
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

        public PftConditionalLiteral
            (
                PftParser.ConditionalLiteralContext context
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

        #endregion
    }
}

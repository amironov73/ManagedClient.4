/* PftRepeatableLiteral.cs
 */

#region Using directives

using System;

#endregion

namespace ManagedClient.Pft.Ast
{
    /// <summary>
    /// Повторяющийся литерал
    /// </summary>
    [Serializable]
    public sealed class PftRepeatableLiteral
        : PftAst
    {
        #region Properties

        #endregion

        #region Construction

        public PftRepeatableLiteral()
        {
        }

        public PftRepeatableLiteral
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

        public PftRepeatableLiteral
            (
                PftParser.RepeatableLiteralContext context
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

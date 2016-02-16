/* PftFatalStatement.cs
 */

#region Using directives

using System;

#endregion

namespace ManagedClient.Pft.Ast
{
    /// <summary>
    /// Фатальная ошибка.
    /// </summary>
    [Serializable]
    public sealed class PftFatalStatement
        : PftAst
    {
        #region Properties

        #endregion

        #region Construction

        public PftFatalStatement()
        {
        }

        public PftFatalStatement
            (
                PftParser.FatalStatementContext context
            )
        {
            DiscoverChildren(context);
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

            string text = context.Evaluate (Children);
            context.Output.Error.WriteLine(text);

            OnAfterExecution(context);

            throw new PftFatalException
                (
                    text
                );
        }

        #endregion
    }
}

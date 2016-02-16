/* PftErrorStatement.cs
 */

#region Using directives

using System;

#endregion

namespace ManagedClient.Pft.Ast
{
    /// <summary>
    /// Сообщение об ошибке.
    /// Выводится в специальный поток
    /// контекста форматирования.
    /// </summary>
    [Serializable]
    public sealed class PftErrorStatement
        : PftAst
    {
        #region Properties

        #endregion

        #region Construction

        public PftErrorStatement()
        {
        }

        public PftErrorStatement
            (
                PftParser.ErrorStatementContext context
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

            string text = context.Evaluate(Children);
            context.Output.Error.WriteLine(text);

            OnAfterExecution(context);
        }

        #endregion
    }
}

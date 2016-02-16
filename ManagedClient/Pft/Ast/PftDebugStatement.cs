/* PftDebugStatement.cs
 */

#region Using directives

using System;
using System.Diagnostics;

#endregion

namespace ManagedClient.Pft.Ast
{
    /// <summary>
    /// Отладочная печать (видна в VS).
    /// </summary>
    [Serializable]
    public sealed class PftDebugStatement
        : PftAst
    {
        #region Properties

        #endregion

        #region Construction

        public PftDebugStatement()
        {
        }

        public PftDebugStatement
            (
                PftParser.DebugStatementContext context
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
            Debug.WriteLine(text);

            OnAfterExecution(context);
        }

        #endregion
    }
}

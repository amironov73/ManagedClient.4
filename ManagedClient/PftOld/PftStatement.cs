/* PftStatement.cs
 */

#region Using directives

using System;

#endregion

namespace ManagedClient.Pft
{
    /// <summary>
    /// Оператор. Может состоять из группы элементов 
    /// или из цепочки несгруппированных элементов.
    /// </summary>
    [Serializable]
    public sealed class PftStatement
        : PftAst
    {
        #region Construction

        public PftStatement()
        {
        }

        public PftStatement(PftParser.StatementContext node)
            : base(node)
        {
            PftParser.GroupStatementContext groupStatementContext 
                = node.groupStatement();
            if (groupStatementContext == null)
            {
                Children.Add(new PftNonGrouped(node.nonGrouped()));
            }
            else
            {
                Children.Add(new PftGroupStatement(groupStatementContext));
            }
        }

        #endregion

        #region PftAst members

        #endregion
    }
}
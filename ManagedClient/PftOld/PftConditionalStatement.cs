/* PftConditionalStatement.
 */

#region Using directives

using System;

#endregion

namespace ManagedClient.Pft
{
    /// <summary>
    /// Условный оператор: if then else fi.
    /// </summary>
    [Serializable]
    public sealed class PftConditionalStatement
        : PftAst
    {
        #region Properties

        /// <summary>
        /// Собственно условие.
        /// </summary>
        public PftCondition Condition { get; set; }

        /// <summary>
        /// Ветка ТОГДА.
        /// </summary>
        public PftStatement ThenStatement { get; set; }

        /// <summary>
        /// Ветка ИНАЧЕ.
        /// </summary>
        public PftStatement ElseStatement { get; set; }

        #endregion

        #region Construction

        public PftConditionalStatement()
        {
        }

        public PftConditionalStatement(PftParser.ConditionalStatementContext node)
            : base(node)
        {
            PftParser.ConditionContext context = node.condition();
            Condition = PftCondition.DispatchContext(context);
            Children.Add(Condition);
            if (node.thenBranch != null)
            {
                ThenStatement = new PftStatement(node.thenBranch);
                Children.Add(ThenStatement);
            }
            if (node.elseBranch != null)
            {
                ElseStatement = new PftStatement(node.elseBranch);
                Children.Add(ElseStatement);
            }
        }

        #endregion

        #region PftAst members

        public override void Execute
            (
                PftContext context
            )
        {
            if (Condition.Evaluate(context))
            {
                if (ThenStatement != null)
                {
                    ThenStatement.Execute(context);
                }
            }
            else
            {
                if (ElseStatement != null)
                {
                    ElseStatement.Execute(context);
                }
            }
        }

        #endregion
    }
}
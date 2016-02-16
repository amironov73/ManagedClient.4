/* PftConditionParen.cs
 */

#region Using directives

using System;

#endregion

namespace ManagedClient.Pft
{
    /// <summary>
    /// Условие в скобках
    /// </summary>
    [Serializable]
    public sealed class PftConditionParen
        : PftCondition
    {
        #region Properties

        public PftCondition Inner { get; set; }

        #endregion

        #region Construction

        public PftConditionParen()
        {
        }

        public PftConditionParen(PftParser.ConditionParenContext node)
            : base(node)
        {
            Inner = DispatchContext(node.condition());
            Children.Add(Inner);
        }

        #endregion

        #region PftCondition members

        public override bool Evaluate
            (
                PftContext context
            )
        {
            return Inner.Evaluate(context);
        }

        #endregion
    }
}
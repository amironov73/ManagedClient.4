/* PftConditionAndOr.cs
 */

#region Using directives

using System;

#endregion

namespace ManagedClient.Pft
{
    [Serializable]
    public sealed class PftConditionAndOr
        : PftCondition
    {
        #region Properties

        public PftCondition Left { get; set; }

        public PftCondition Right { get; set; }

        public bool And { get; set; }

        #endregion

        #region Construction

        public PftConditionAndOr()
        {
        }

        public PftConditionAndOr(PftParser.ConditionAndOrContext node)
            : base(node)
        {
            Left = DispatchContext(node.condition(0));
            Children.Add(Left);
            Right = DispatchContext(node.condition(1));
            Children.Add(Right);
            And = node.AND() != null;
        }

        #endregion

        #region PftCondition members

        public override bool Evaluate
            (
                PftContext context
            )
        {
            if (And)
            {
                return Left.Evaluate(context) && Right.Evaluate(context);
            }
            return Left.Evaluate(context) || Right.Evaluate(context);
        }

        #endregion
    }
}
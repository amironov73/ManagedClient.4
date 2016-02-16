/* PftArithCondition.cs
 */

#region Using directives

using System;

#endregion

namespace ManagedClient.Pft
{
    /// <summary>
    /// 
    /// </summary>
    [Serializable]
    public sealed class PftArithCondition
        : PftCondition
    {
        #region Properties

        public PftArithExpression Left { get; set; }

        public string Op { get; set; }

        public PftArithExpression Right { get; set; }

        #endregion

        #region Construction

        public PftArithCondition()
        {
        }

        public PftArithCondition(PftParser.ConditionArithContext node) 
            : base(node)
        {
            var context = node.arithCondition();
            Left = new PftArithExpression(context.left);
            Children.Add(Left);
            Op = context.op.Text;
            Right = new PftArithExpression(context.right);
            Children.Add(Right);
        }

        #endregion

        #region PftCondition members

        public override bool Evaluate
            (
                PftContext context
            )
        {
            Left.Execute(context);
            Right.Execute(context);
            // ReSharper disable CompareOfFloatsByEqualityOperator
            switch (Op)
            {
                case "=":
                    return Left.Value == Right.Value;
                case "<>":
                    return Left.Value != Right.Value;
                case ">":
                    return Left.Value > Right.Value;
                case ">=":
                    return Left.Value >= Right.Value;
                case "<":
                    return Left.Value < Right.Value;
                case "<=":
                    return Left.Value <= Right.Value;
            }
            // ReSharper restore CompareOfFloatsByEqualityOperator

            throw new ArgumentException();
        }

        #endregion
    }
}

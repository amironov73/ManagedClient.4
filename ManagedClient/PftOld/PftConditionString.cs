/* PftConditionString.cs
 */

#region Using directives

using System;

#endregion

namespace ManagedClient.Pft
{
    /// <summary>
    /// Сравнение строк в условии
    /// </summary>
    [Serializable]
    public sealed class PftConditionString
        : PftCondition
    {
        #region Properties

        public PftAst Left { get; set; }

        public string Op { get; set; }

        public PftAst Right { get; set; }

        #endregion

        #region Construction

        public PftConditionString()
        {
        }

        public PftConditionString(PftParser.ConditionStringContext node)
            : base(node)
        {
            var context = node.stringTest();
            Left = PftNonGrouped.DispatchContext(context.left);
            Children.Add(Left);
            Op = context.op.Text;
            Right = PftNonGrouped.DispatchContext(context.right);
            Children.Add(Right);
       }

        #endregion

        #region Private members

        private static int Compare
            (
                string left,
                string right
            )
        {
            return string.Compare
                (
                    left,
                    right,
                    StringComparison.InvariantCultureIgnoreCase
                );
        }

        #endregion

        #region PftCondition members

        public override bool Evaluate
            (
                PftContext context
            )
        {
            string left = context.Evaluate(Left);
            string right = context.Evaluate(Right);
            
            bool result = false;

            switch (Op)
            {
                case ":":                    
                    result = left.Contains(right);
                    break;
                case "=":
                    result = Compare(left,right) == 0;
                    break;
                case "<>":
                    result = Compare(left,right) != 0;
                    break;
                case ">":
                    result = Compare(left, right) > 0;
                    break;
                case ">=":
                    result = Compare(left, right) >= 0;
                    break;
                case "<":
                    result = Compare(left, right) < 0;
                    break;
                case "<=":
                    result = Compare(left, right) <= 0;
                    break;
            }

            return result;
        }

        #endregion
    }
}
/* PftArithExpression.cs
 */

#region Using directives

using System;
using System.Security.Cryptography;

#endregion

namespace ManagedClient.Pft
{
    [Serializable]
    public sealed class PftArithExpression
        : PftNumber
    {
        #region Properties

        public PftArithExpression Left { get; set; }

        public string Op { get; set; }

        public PftArithExpression Right { get; set; }

        public PftValue SimpleValue { get; set; }

        #endregion

        #region Construciton

        public PftArithExpression()
        {
        }

        public PftArithExpression(PftParser.ArithExprContext node) 
            : base(node)
        {
            if (node.value() != null)
            {
                SimpleValue = new PftValue(node.value());
            }
            else
            {
                Left = new PftArithExpression(node.left);
                Children.Add(Left);
                Op = node.op.Text;
                Right = new PftArithExpression(node.right);
                Children.Add(Right);
            }
        }

        #endregion

        #region PftAst members

        public override void Execute
            (
                PftContext context
            )
        {
            if (SimpleValue != null)
            {
                SimpleValue.Execute(context);
                Value = SimpleValue.Value;
            }
            else
            {
                Left.Execute(context);
                Right.Execute(context);
                switch (Op)
                {
                    case "+":
                        Value = Left.Value + Right.Value;
                        break;
                    case "-":
                        Value = Left.Value - Right.Value;
                        break;
                    case "*":
                        Value = Left.Value * Right.Value;
                        break;
                    case "/":
                        Value = Left.Value / Right.Value;
                        break;
                }
            }
        }

        #endregion
    }
}

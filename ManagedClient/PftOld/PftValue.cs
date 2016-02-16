/* PftValue.cs
 */

#region Using directives

using System;

#endregion

namespace ManagedClient.Pft
{
    /// <summary>
    /// Литерал-число или любое другое плавающее/целочисленное значение,
    /// например, MFN.
    /// </summary>
    [Serializable]
    public sealed class PftValue
        : PftNumber
    {
        #region Properties

        public PftNumber Expression { get; set; }

        public bool Minus { get; set; }

        #endregion

        #region Construction

        public PftValue()
        {
        }

        public PftValue(double value) 
            : base(value)
        {
        }

        public PftValue(PftParser.ValueContext node)
            : base(node)
        {
            // ReSharper disable CanBeReplacedWithTryCastAndCheckForNull

            string text;

            if (node is PftParser.FloatValueContext)
            {
                text = ((PftParser.FloatValueContext) node).FLOAT().GetText();
                Value = ExtractNumber(text);
            }
            else if (node is PftParser.UnsignedValueContext)
            {
                text = ((PftParser.UnsignedValueContext) node).UNSIGNED().GetText();
                Value = ExtractNumber(text);
            }
            else if (node is PftParser.SignedValueContext)
            {
                text = ((PftParser.SignedValueContext) node).SIGNED().GetText();
                Value = ExtractNumber(text);
            }
            else if (node is PftParser.MfnValueContext)
            {
                Expression = new PftMfnValue((PftParser.MfnValueContext)node);
            }
            else if (node is PftParser.MinusExpressionContext)
            {
                Minus = true;
                Expression = new PftArithExpression(((PftParser.MinusExpressionContext)node).arithExpr());
            }
            else if (node is PftParser.ParenthesisExpressionContext)
            {
                Expression = new PftArithExpression(((PftParser.ParenthesisExpressionContext)node).arithExpr());
            }
            else if (node is PftParser.ArithFunctionOuterContext)
            {
                PftParser.ArithFunctionContext arithFunction 
                    = ((PftParser.ArithFunctionOuterContext) node).arithFunction();
                if (arithFunction is PftParser.RsumFunctionContext)
                {
                    Expression = new PftRsumFunction((PftParser.RsumFunctionContext)arithFunction);
                }
                else if (arithFunction is PftParser.RmaxFunctionContext)
                {
                    Expression = new PftRmaxFunction((PftParser.RmaxFunctionContext)arithFunction);
                }
                else if (arithFunction is PftParser.RminFunctionContext)
                {
                    Expression = new PftRminFunction((PftParser.RminFunctionContext)arithFunction);
                }
                else if (arithFunction is PftParser.RavrFunctionContext)
                {
                    Expression = new PftRavrFunction((PftParser.RavrFunctionContext)arithFunction);
                }
                else if (arithFunction is PftParser.ValFunctionContext)
                {
                    Expression = new PftValFunction((PftParser.ValFunctionContext)arithFunction);
                }
                else if (arithFunction is PftParser.LFunctionContext)
                {
                    Expression = new PftLFunction((PftParser.LFunctionContext)arithFunction);
                }
                else
                {
                    throw new ApplicationException();
                }
            }
            else
            {
                throw new ApplicationException();
            }

            // ReSharper restore CanBeReplacedWithTryCastAndCheckForNull
        }

        #endregion

        #region PftAst members

        public override void Execute
            (
                PftContext context
            )
        {
            if (Expression != null)
            {
                Expression.Execute(context);
                double value = Expression.Value;
                Value = Minus
                    ? -value
                    : value;
            }
        }

        #endregion
    }
}

/* PftDispatcher.cs
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Antlr4.Runtime;

#endregion

namespace ManagedClient.Pft
{
    using Ast;

    public static class PftDispatcher
    {
        #region Private members

        //private static PftAst S<T1, T2>
        //    (
        //        ParserRuleContext context
        //    )
        //    where T1 : ParserRuleContext
        //    where T2 : PftAst
        //{
        //    T1 ctx = context as T1;
        //    if (!ReferenceEquals(ctx, null))
        //    {
        //        T2 result = (T2) Activator.CreateInstance
        //            (
        //                typeof(T2),
        //                new [] { ctx }
        //            );
        //        return result;
        //    }
        //    return null;
        //}

        #endregion

        #region Public methods

        public static PftAst DispatchFormat
            (
                ParserRuleContext context
            )
        {
            PftParser.CommaOperatorContext ctx1 = context as PftParser.CommaOperatorContext;
            if (ctx1 != null)
                return new PftCommaOperator(ctx1);
            PftParser.UnconditionalLiteralContext ctx2 = context as PftParser.UnconditionalLiteralContext;
            if (ctx2 != null)
                return new PftUnconditionalLiteral(ctx2);
            PftParser.EscapedLiteralContext ctx3 = context as PftParser.EscapedLiteralContext;
            if (ctx3 != null)
                return new PftEscapedLiteral(ctx3);
            PftParser.CompositeElementContext ctx4 = context as PftParser.CompositeElementContext;
            if (ctx4 != null)
                return new PftCompositeElement(ctx4);
            PftParser.SimpleFormatContext ctx5 = context as PftParser.SimpleFormatContext;
            if (ctx5 != null)
                return new PftSimpleFormat(ctx5);
            PftParser.SlashOperatorContext ctx6 = context as PftParser.SlashOperatorContext;
            if (ctx6 != null)
                return new PftSlashOperator(ctx6);
            PftParser.HashOperatorContext ctx7 = context as PftParser.HashOperatorContext;
            if (ctx7 != null)
                return new PftHashOperator(ctx7);
            PftParser.PercentOperatorContext ctx8 = context as PftParser.PercentOperatorContext;
            if (ctx8 != null)
                return new PftPercentOperator(ctx8);
            PftParser.FieldReferenceContext ctx9 = context as PftParser.FieldReferenceContext;
            if (ctx9 != null)
                return new PftFieldReference(ctx9);
            PftParser.GlobalReferenceContext ctx10 = context as PftParser.GlobalReferenceContext;
            if (ctx10 != null)
                return new PftGlobalReference(ctx10);
            PftParser.LeftHandContext ctx11 = context as PftParser.LeftHandContext;
            if (ctx11 != null)
                return new PftLeftHand(ctx11);
            PftParser.RightHandContext ctx12 = context as PftParser.RightHandContext;
            if (ctx12 != null)
                return new PftRightHand(ctx12);
            PftParser.CommandCContext ctx13 = context as PftParser.CommandCContext;
            if (ctx13 != null)
                return new PftCommandC(ctx13);
            PftParser.CommandXContext ctx14 = context as PftParser.CommandXContext;
            if (ctx14 != null)
                return new PftCommandX(ctx14);
            PftParser.ConditionalLiteralContext ctx15 = context as PftParser.ConditionalLiteralContext;
            if (ctx15 != null)
                return new PftConditionalLiteral(ctx15);
            PftParser.RepeatableLiteralContext ctx16 = context as PftParser.RepeatableLiteralContext;
            if (ctx16 != null)
                return new PftRepeatableLiteral(ctx16);
            PftParser.ModeSwitchContext ctx17 = context as PftParser.ModeSwitchContext;
            if (ctx17 != null)
                return new PftModeSwitch(ctx17);
            PftParser.ConditionalStatementContext ctx18 = context as PftParser.ConditionalStatementContext;
            if (ctx18 != null)
                return new PftConditionalStatement(ctx18);
            PftParser.LeftHandContext ctx19 = context as PftParser.LeftHandContext;
            if (ctx19 != null)
                return new PftLeftHand(ctx19);
            PftParser.RightHandContext ctx20 = context as PftParser.RightHandContext;
            if (ctx20 != null)
                return new PftRightHand(ctx20);
            PftParser.EatFunctionContext ctx21 = context as PftParser.EatFunctionContext;
            if (ctx21 != null)
                return new PftEatFunction(ctx21);
            PftParser.DebugStatementContext ctx22 = context as PftParser.DebugStatementContext;
            if (ctx22 != null)
                return new PftDebugStatement(ctx22);
            PftParser.DebugBreakContext ctx23 = context as PftParser.DebugBreakContext;
            if (ctx23 != null)
                return new PftDebugBreak(ctx23);
            PftParser.ErrorStatementContext ctx24 = context as PftParser.ErrorStatementContext;
            if (ctx24 != null)
                return new PftErrorStatement(ctx24);
            PftParser.FatalStatementContext ctx25 = context as PftParser.FatalStatementContext;
            if (ctx25 != null)
                return new PftFatalStatement(ctx25);
            PftParser.IffFunctionContext ctx26 = context as PftParser.IffFunctionContext;
            if (ctx26 != null)
                return new PftIffFunction(ctx26);
            PftParser.IncrementFunctionContext ctx27 = context as PftParser.IncrementFunctionContext;
            if (ctx27 != null)
                return new PftIncrementFunction(ctx27);
            PftParser.NlOperatorContext ctx28 = context as PftParser.NlOperatorContext;
            if (ctx28 != null)
                return new PftNlOperator(ctx28);

            throw new ApplicationException();
        }

        public static PftCondition DispatchCondition
            (
                PftParser.ConditionContext context
            )
        {
            return null;
        }

        #endregion
    }
}

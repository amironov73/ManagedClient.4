/* PftNonGrouped.cs
 */

#region Using directives

using System;
using System.Diagnostics;
using Antlr4.Runtime.Tree;

#endregion

namespace ManagedClient.Pft
{
    /// <summary>
    /// Ёлемент форматировани€
    /// </summary>
    [Serializable]
    public sealed class PftNonGrouped
        : PftAst
    {
        #region Construction

        public PftNonGrouped()
        {
        }

        public PftNonGrouped(PftParser.NonGroupedContext node)
            : base(node)
        {
            PftParser.FormatItemPlusContext plusContext = node as PftParser.FormatItemPlusContext;
            if (plusContext != null)
            {
                foreach (PftParser.FormatItemContext context in plusContext.formatItem())
                {
                    PftAst item = DispatchContext(context);
                    Children.Add(item);
                }
            }
            //else
            //{
            //    throw new ApplicationException();
            //}
        }

        #endregion

        #region Public methods

        public static PftAst DispatchContext
            (
                IParseTree context
            )
        {
            // ReSharper disable CanBeReplacedWithTryCastAndCheckForNull

            if (context is PftParser.ConditionalStatementOuterContext)
            {
                return new PftConditionalStatement(((PftParser.ConditionalStatementOuterContext)context)
                    .conditionalStatement());
            }
            if (context is PftParser.FieldReferenceOuterContext)
            {
                return new PftFieldReference(((PftParser.FieldReferenceOuterContext)context)
                    .fieldReference());
            }
            if (context is PftParser.GlobalReferenceContext)
            {
                return new PftGlobalReference(((PftParser.GlobalReferenceContext)context));
            }
            if (context is PftParser.UnconditionalLiteralContext)
            {
                return new PftUnconditionalLiteral((PftParser.UnconditionalLiteralContext)context);
            }
            if (context is PftParser.SimpleMfnContext)
            {
                return new PftSimpleMfn((PftParser.SimpleMfnContext)context);
            }
            if (context is PftParser.MfnWithLengthContext)
            {
                return new PftMfnWithLength((PftParser.MfnWithLengthContext)context);
            }
            if (context is PftParser.CommaContext)
            {
                return new PftComma((PftParser.CommaContext)context);
            }
            if (context is PftParser.SlashNewLineContext)
            {
                return new PftSlash((PftParser.SlashNewLineContext)context);
            }
            if (context is PftParser.HashNewLineContext)
            {
                return new PftHash((PftParser.HashNewLineContext)context);
            }
            if (context is PftParser.PercentNewLineContext)
            {
                return new PftPercent((PftParser.PercentNewLineContext)context);
            }
            if (context is PftParser.ModeSwitchContext)
            {
                return new PftModeSwitch((PftParser.ModeSwitchContext)context);
            }
            if (context is PftParser.FormatExitOuterContext)
            {
                return new PftFormatExit(((PftParser.FormatExitOuterContext)context).formatExit());
            }
            if (context is PftParser.ErrorOuterContext)
            {
                return new PftError(((PftParser.ErrorOuterContext)context).error());
            }
            if (context is PftParser.WarningOuterContext)
            {
                return new PftWarning(((PftParser.WarningOuterContext)context).warning());
            }
            if (context is PftParser.FatalOuterContext)
            {
                return new PftFatal(((PftParser.FatalOuterContext)context).fatal());
            }
            if (context is PftParser.TraceOuterContext)
            {
                return new PftTrace(((PftParser.TraceOuterContext)context).trace());
            }
            if (context is PftParser.DebugOuterContext)
            {
                return new PftDebug(((PftParser.DebugOuterContext)context).debug());
            }
            if (context is PftParser.CommandCContext)
            {
                return new PftCommandC((PftParser.CommandCContext)context);
            }
            if (context is PftParser.CommandXContext)
            {
                return new PftCommandX((PftParser.CommandXContext) context);
            }
            if (context is PftParser.SFunctionOuterContext)
            {
                return new PftSFunction(((PftParser.SFunctionOuterContext)context).sFunction());
            }
            if (context is PftParser.TrimFunctionOuterContext)
            {
                return new PftTrimFunction(((PftParser.TrimFunctionOuterContext)context).trimFunction());
            }
            if (context is PftParser.IoccOuterContext)
            {
                return new PftIocc(((PftParser.IoccOuterContext)context).iocc());
            }
            if (context is PftParser.NoccOuterContext)
            {
                return new PftNocc(((PftParser.NoccOuterContext)context).nocc());
            }
            if (context is PftParser.BreakContext)
            {
                return new PftBreakStatement((PftParser.BreakContext)context);
            }
            if (context is PftParser.FFunctionOuterContext)
            {
                return new PftFFunction(((PftParser.FFunctionOuterContext)context).fFunction());
            }
            if (context is PftParser.RefFunctionOuterContext)
            {
                return new PftRefFunction(((PftParser.RefFunctionOuterContext)context).refFunction());
            }
            if (context is PftParser.FormatItemPlusContext)
            {
                return new PftFormatItemPlus((PftParser.FormatItemPlusContext)context);
            }

            throw new ArgumentException();

            // ReSharper restore CanBeReplacedWithTryCastAndCheckForNull
        }

        #endregion
    }
}
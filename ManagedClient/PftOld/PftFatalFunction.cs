/* PftFatalFunction.cs
 */

#region Using directives

using System;

#endregion

namespace ManagedClient.Pft
{
    /// <summary>
    /// Фатальная ошибка.
    /// </summary>
    [Serializable]
    public sealed class PftFatalFunction
        : PftAst
    {
        #region Properties

        public PftAst Argument { get; set; }

        #endregion

        #region Construction

        public PftFatalFunction()
        {
        }

        public PftFatalFunction(PftParser.FatalFunctionContext node)
            : base(node)
        {
            Argument = DispatchContext(node.nonGrouped());
            Children.Add(Argument);
        }

        #endregion

        #region PftAst members

        public override bool Execute
            (
                PftContext context
            )
        {
            OnBeforeExecution(context);

            bool happen;
            string text = context.Evaluate
                (
                    Argument,
                    out happen
                );
            context.Output.Error.WriteLine(text);

            OnAfterExecution(context);

            throw new PftFatalException
                (
                    text
                );
        }

        #endregion
    }
}
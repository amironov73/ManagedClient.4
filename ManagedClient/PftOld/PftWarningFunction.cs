/* PftWarningFunction.cs
 */

#region Using directives

using System;

#endregion

namespace ManagedClient.Pft
{
    /// <summary>
    /// Предупреждение.
    /// Выводится в особый поток контекста.
    /// </summary>
    [Serializable]
    public sealed class PftWarningFunction
        : PftAst
    {
        #region Properties

        public PftAst Argument { get; set; }

        #endregion

        #region Construction

        public PftWarningFunction()
        {
        }

        public PftWarningFunction(PftParser.WarningFunctionContext node)
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
            context.Output.Warning.WriteLine(text);

            OnAfterExecution(context);

            return happen;
        }

        #endregion
    }
}
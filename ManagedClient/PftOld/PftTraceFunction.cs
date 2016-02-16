/* PftTraceFunction.cs
 */

#region Using directives

using System;
using System.Diagnostics;

#endregion

namespace ManagedClient.Pft
{
    /// <summary>
    /// Трассировочное сообщение.
    /// Выводится в консоли отладчика
    /// и в зарегистрированных листенерах.
    /// </summary>
    [Serializable]
    public sealed class PftTraceFunction
        : PftAst
    {
        #region Properties

        public PftAst Argument { get; set; }

        #endregion

        #region Construction

        public PftTraceFunction()
        {
        }

        public PftTraceFunction(PftParser.TraceFunctionContext node)
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
            Trace.WriteLine(text);

            OnAfterExecution(context);

            return happen;
        }

        #endregion
    }
}
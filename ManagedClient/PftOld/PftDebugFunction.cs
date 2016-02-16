/* PftDebugFunction.cs
 */

#region Using directives

using System;
using System.Diagnostics;

#endregion

namespace ManagedClient.Pft
{
    /// <summary>
    /// Отладочное сообщение (выводится в консоли отладчика).
    /// </summary>
    [Serializable]
    public sealed class PftDebugFunction
        : PftAst
    {
        #region Properties

        public PftAst Argument { get; set; }

        #endregion

        #region Construction

        public PftDebugFunction()
        {
        }

        public PftDebugFunction(PftParser.DebugFunctionContext node)
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
            Debug.WriteLine(text);

            OnAfterExecution(context);

            return happen;
        }

        #endregion
    }
}
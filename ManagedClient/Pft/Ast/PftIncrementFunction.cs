/* PftIncrementFunction.cs
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

#endregion

namespace ManagedClient.Pft.Ast
{
    [Serializable]
    public sealed class PftIncrementFunction
        : PftAst
    {
        #region Properties

        #endregion

        #region Construciton

        public PftIncrementFunction()
        {
        }

        public PftIncrementFunction
            (
                PftParser.IncrementFunctionContext context
            )
        {
            DiscoverChildren(context);
        }

        #endregion

        #region Private members

        #endregion

        #region Public methods

        #endregion

        #region PftAst members

        public override void Execute
            (
                PftContext context
            )
        {
            OnBeforeExecution(context);

            string argument = context.Evaluate(Children);
            NumberText text = new NumberText(argument);
            string result = text.Increment().ToString();
            context.Write
                (
                    this,
                    result
                );

            OnAfterExecution(context);
        }

        #endregion
    }
}

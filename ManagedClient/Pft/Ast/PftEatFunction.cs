/* PftEatFunction.cs
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

#endregion

namespace ManagedClient.Pft.Ast
{
    /// <summary>
    /// Съедает весь переданный ей текст.
    /// </summary>
    [Serializable]
    public sealed class PftEatFunction
        : PftAst
    {
        #region Properties

        #endregion

        #region Construction

        public PftEatFunction()
        {
        }

        public PftEatFunction
            (
                PftParser.EatFunctionContext context
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
            context = context.Push();
            base.Execute(context);
        }

        #endregion
    }
}

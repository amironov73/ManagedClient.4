/* PftIffFunction.cs
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
    /// Функция, возвращающая значение,
    /// в зависимости от условия.
    /// </summary>
    [Serializable]
    public sealed class PftIffFunction
        : PftAst
    {
        #region Properties

        #endregion

        #region Construction

        public PftIffFunction()
        {
        }

        public PftIffFunction
            (
                PftParser.IffFunctionContext context
            )
        {
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

            base.Execute(context);

            OnAfterExecution(context);
        }

        #endregion
    }
}

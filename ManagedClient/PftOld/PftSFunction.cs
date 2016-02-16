/* PftSFunction.cs
 */

#region Using directives

using System;

#endregion

namespace ManagedClient.Pft
{
    [Serializable]
    public sealed class PftSFunction
        : PftAst
    {
        #region Properties

        public PftAst Arguments { get; set; }

        #endregion

        #region Construction

        public PftSFunction()
        {
        }

        public PftSFunction(PftParser.SFunctionContext node)
            : base(node)
        {
            Arguments = DispatchContext(node.nonGrouped());
            Children.Add(Arguments);
        }

        #endregion

        #region PftAst members

        public override bool Execute
            (
                PftContext context
            )
        {
            return Arguments.Execute
                (
                    context
                );
        }

        #endregion

        #region Public methods

        public string Evaluate
            (
                PftContext context,
                out bool happen
            )
        {
            string result = context.Evaluate
                (
                    this,
                    out happen
                );
            return result;
        }

        #endregion
    }
}

/* PftTrimFunction.cs
 */

#region Using directives

using System;

#endregion

namespace ManagedClient.Pft
{
    [Serializable]
    public sealed class PftTrimFunction
        : PftAst
    {
        #region Properties

        public PftNonGrouped Arguments { get; set; }

        #endregion

        #region Construction

        public PftTrimFunction()
        {
        }

        public PftTrimFunction(PftParser.TrimFunctionContext node)
            : base (node)
        {
            Arguments = new PftNonGrouped(node.nonGrouped());
        }

        #endregion

        #region PftAst members

        public override void Execute
            (
                PftContext context
            )
        {
            string result = context.Evaluate(Arguments);
            result = result.Trim();
            context.Write(result);
        }

        #endregion

        #region Public methods

        public string Evaluate
            (
                PftContext context
            )
        {
            return context.Evaluate(this);
        }

        #endregion
    }
}

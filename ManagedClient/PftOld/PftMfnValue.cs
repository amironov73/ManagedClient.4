/* PftMfnValue.cs
 */

#region Using directives

using System;

#endregion

namespace ManagedClient.Pft
{
    [Serializable]
    public sealed class PftMfnValue
        : PftNumber
    {
        #region Properties

        #endregion

        #region Construction

        public PftMfnValue()
        {
        }

        public PftMfnValue(int value) 
            : base(value)
        {
        }

        public PftMfnValue(PftParser.MfnValueContext node) 
            : base(node)
        {
        }

        #endregion

        #region PftAst members

        public override void Execute
            (
                PftContext context
            )
        {
            Value = (context.Record == null)
                ? 0
                : context.Record.Mfn;
        }

        #endregion
    }
}

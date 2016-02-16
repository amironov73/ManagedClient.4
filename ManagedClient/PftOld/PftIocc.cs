/* PftIocc.cs
 */

#region Using directives

using System;

#endregion

namespace ManagedClient.Pft
{
    [Serializable]
    public sealed class PftIocc
        : PftFieldOrGlobal
    {
        #region Properties

        #endregion

        #region Construction

        public PftIocc()
        {
        }

        public PftIocc(PftParser.IoccContext node)
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
            string result = Group == null
                ? "0"
                : Group.GroupIndex.ToInvariantString();

            context.Write
                (
                    result
                );
        }

        #endregion
    }
}

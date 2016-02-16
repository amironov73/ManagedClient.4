/* PftNocc.cs
 */

#region Using directives

using System;

#endregion

namespace ManagedClient.Pft
{
    [Serializable]
    public sealed class PftNocc
        : PftFieldOrGlobal
    {
        #region Properties
        #endregion

        #region Construction

        public PftNocc()
        {
        }

        public PftNocc(PftParser.NoccContext node)
            : base (node)
        {
        }

        #endregion

        #region PftAst members

        public override void Execute
            (
                PftContext context
            )
        {
            string result = ((Group == null) || (Group.GroupItems == null))
                ? "0"
                : Group.GroupItems.Length.ToInvariantString();
            context.Write
                (
                    result
                );
        }

        #endregion
    }
}

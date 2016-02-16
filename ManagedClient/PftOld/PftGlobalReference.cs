/* PftGlobalReference.cs
 */

#region Using directives

using System;

#endregion

namespace ManagedClient.Pft
{
    /// <summary>
    /// —сылка на глобальную переменную.
    /// </summary>
    [Serializable]
    public sealed class PftGlobalReference
        : PftFieldOrGlobal
    {
        #region Properties

        public FieldReference Reference { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public int Index { get; set; }

        #endregion

        #region Construction

        public PftGlobalReference()
        {
        }

        public PftGlobalReference(PftParser.GlobalReferenceContext node)
            : base(node)
        {
            Reference = new FieldReference(node.GetText());
            Index = int.Parse(Reference.Field);
        }

        #endregion

        #region PftAst members

        public override void Execute
            (
                PftContext context
            )
        {
            string text = Reference.FormatSingle(context.Globals.Get(Index));
            context.Write(text);
        }

        #endregion
    }
}
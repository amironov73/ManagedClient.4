/* PftMfnWithLength.cs
 */

#region Using directives

using System;

#endregion

namespace ManagedClient.Pft
{
    /// <summary>
    /// Ссылка на MFN записи (с указанием длины).
    /// </summary>
    [Serializable]
    public sealed class PftMfnWithLength
        : PftAst
    {
        #region Properties

        /// <summary>
        /// Необходимая длина (добивается нулями).
        /// </summary>
        public int Length;

        #endregion

        #region Construction

        public PftMfnWithLength()
        {
        }

        public PftMfnWithLength(PftParser.MfnWithLengthContext node)
            : base(node)
        {
            string text = node.GetText();
            Length = (int)uint.Parse(text.Substring(4,text.Length-5));
        }

        #endregion

        #region PftAst members

        public override void Execute
            (
                PftContext context
            )
        {
            string format = new string('0', Length);
            string text = context.Record.Mfn.ToString(format);
            context.Write(text);
        }

        #endregion
    }
}
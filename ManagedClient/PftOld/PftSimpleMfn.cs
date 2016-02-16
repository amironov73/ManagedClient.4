/* PftSimpleMfn.cs
 */

#region Using directives

using System;
using System.IO;

#endregion

namespace ManagedClient.Pft
{
    /// <summary>
    /// Ссылка на MFN записи (без указания длины).
    /// </summary>
    [Serializable]
    public sealed class PftSimpleMfn
        : PftAst
    {
        #region Construction

        public PftSimpleMfn()
        {
        }

        public PftSimpleMfn(PftParser.SimpleMfnContext node)
            : base(node)
        {
        }

        #endregion

        #region PftAst members

        public override bool Execute
            (
                PftContext context
            )
        {
            OnBeforeExecution(context);

            string format = new string('0', 10);
            string text = context.Record.Mfn.ToString(format);
            context.Write
                (
                    this,
                    text
                );

            OnAfterExecution(context);

            return false;
        }

        public override void Write
            (
                StreamWriter writer
            )
        {
            // Всегда в нижнем регистре
            writer.Write("mfn");
        }

        #endregion
    }
}
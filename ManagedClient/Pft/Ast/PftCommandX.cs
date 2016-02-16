/* PftCommandX.cs
 */

#region Using directives

using System;
using System.IO;

#endregion

namespace ManagedClient.Pft.Ast
{
    /// <summary>
    /// Команда горизонтального позиционирования.
    /// Вставляет n пробелов.
    /// </summary>
    [Serializable]
    public sealed class PftCommandX
        : PftAst
    {
        #region Properties

        /// <summary>
        /// Количество добавляемых пробелов.
        /// </summary>
        public int Shift { get; set; }
        
        #endregion

        #region Construciton

        public PftCommandX()
        {
        }

        public PftCommandX
            (
                PftParser.CommandXContext node
            )
        {
            Shift = int.Parse(node.COMMANDX().GetText().Substring(1));
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

            if (Shift > 0)
            {
                context.Write
                    (
                        this,
                        new string(' ', Shift)
                    );
            }

            OnAfterExecution(context);
        }

        public override void Write
            (
                StreamWriter writer
            )
        {
            writer.Write
                (
                    "x{0}", // Всегда в нижнем регистре
                    Shift.ToInvariantString()
                );
        }

        #endregion
    }
}

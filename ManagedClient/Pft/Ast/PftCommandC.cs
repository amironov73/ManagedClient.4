/* PftCommandC.cs
 */

#region Using directives

using System;
using System.IO;

#endregion

namespace ManagedClient.Pft.Ast
{
    /// <summary>
    /// Команда горизонтального позиционирования.
    /// Перемещает виртуальный курсор в n-ю позицию строки
    /// (табуляция в указанную позицию строки).
    /// </summary>
    [Serializable]
    public sealed class PftCommandC
        : PftAst
    {
        #region Properties

        /// <summary>
        /// Новая позиция курсора.
        /// </summary>
        public int NewPosition { get; set; }

        #endregion

        #region Construction

        public PftCommandC()
        {
        }

        public PftCommandC
            (
                PftParser.CommandCContext context
            )
        {
            NewPosition = int.Parse(context.COMMANDC().GetText().Substring(1));
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

            int desired = NewPosition * 8;
            int current = context.Output.GetCaretPosition();
            int delta = desired - current;
            if (delta > 0)
            {
                context.Write
                    (
                        this,
                        new string(' ', delta)
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
                    "c{0}", // Всегда в нижнем регистре
                    NewPosition.ToInvariantString()
                );
        }
        #endregion
    }
}

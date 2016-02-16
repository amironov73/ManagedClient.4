/* PftSlashOperator.cs
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

#endregion

namespace ManagedClient.Pft.Ast
{
    /// <summary>
    /// Команда вертикального позиционирования.
    /// Переход на новую строку, если текущая строка не была пустой.
    /// приводит к размещению последующих данных с начала следующей строки.
    /// Однако подряд расположенные команды /, хотя и являются синтаксически 
    /// правильными, но имеют тот же смысл, что и одна команда /, 
    /// т.е. команда / никогда не создает пустых строк.
    /// </summary>
    [Serializable]
    public sealed class PftSlashOperator
        : PftAst
    {
        #region Properties

        #endregion

        #region Construction

        public PftSlashOperator()
        {
        }

        public PftSlashOperator
            (
                PftParser.SlashOperatorContext context
            )
        {
            // Nothing to do actually
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

            if (!context.Output.HaveEmptyLine())
            {
                context.WriteLine(this);
            }

            OnAfterExecution(context);
        }

        public override void Write
            (
                StreamWriter writer
            )
        {
            // Обрамляем пробелами
            writer.Write(" / ");
        }

        #endregion
    }
}

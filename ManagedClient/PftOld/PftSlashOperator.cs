/* PftSlashOperator.cs
 */

#region Using directives

using System;
using System.IO;

#endregion

namespace ManagedClient.Pft
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
        #region Construction

        public PftSlashOperator()
        {
        }

        public PftSlashOperator(PftParser.SlashOperatorContext node)
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

            if (!context.Output.HaveEmptyLine())
            {
                context.WriteLine(this);
            }

            OnAfterExecution(context);

            return false;
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
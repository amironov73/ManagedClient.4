// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* ConsoleOutput.cs -- консольный вывод.
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

#endregion

namespace ManagedClient.Output
{
    /// <summary>
    /// Консольный вывод.
    /// </summary>
    public sealed class ConsoleOutput
        : AbstractOutput
    {
        #region AbstractOutput members

        /// <summary>
        /// Флаг: был ли вывод с помощью WriteError.
        /// </summary>
        public override bool HaveError { get; set; }

        /// <summary>
        /// Очищает вывод, например, окно.
        /// Надо переопределить в потомке.
        /// </summary>
        public override AbstractOutput Clear()
        {
            HaveError = false;
            Console.Clear();
            return this;
        }

        /// <summary>
        /// Конфигурирование объекта.
        /// Надо переопределить в потомке.
        /// </summary>
        public override AbstractOutput Configure
            (
                string configuration
            )
        {
            // TODO: implement properly
            return this;
        }

        /// <summary>
        /// Метод, который нужно переопределить
        /// в потомке.
        /// </summary>
        /// <param name="text">The text.</param>
        /// <returns>Возвращает сам объект
        /// вывода.</returns>
        public override AbstractOutput Write
            (
                string text
            )
        {
            System.Console.Write(text);
            return this;
        }

        /// <summary>
        /// Выводит ошибку. Например, красным цветом.
        /// Надо переопределить в потомке.
        /// </summary>
        public override AbstractOutput WriteError
            (
                string text
            )
        {
            HaveError = true;
            System.Console.Error.Write(text);
            return this;
        }

        #endregion
    }
}

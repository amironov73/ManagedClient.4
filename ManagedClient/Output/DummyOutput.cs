﻿// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* DummyOutput.cs
 */

#region Using directives

using System;

using JetBrains.Annotations;

#endregion

namespace ManagedClient.Output
{
    /// <summary>
    /// Выходной поток, который не даёт закрыться
    /// другому потоку.
    /// </summary>
    [PublicAPI]
    public sealed class DummyOutput
        : AbstractOutput
    {
        #region Properties

        /// <summary>
        /// Внутренний поток.
        /// </summary>
        [NotNull]
        public AbstractOutput Inner { get { return _inner; } }

        #endregion

        #region Construction

        /// <summary>
        /// Конструктор.
        /// </summary>
        public DummyOutput
            (
                [NotNull] AbstractOutput inner
            )
        {
            if (ReferenceEquals(inner, null))
            {
                throw new ArgumentNullException("inner");
            }
            _inner = inner;
        }

        #endregion

        #region Private members

        private readonly AbstractOutput _inner;

        #endregion

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
            Inner.Clear();
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
            Inner.Configure(configuration);
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
            Inner.Write(text);
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
            Inner.WriteError(text);
            return this;
        }

        #endregion
    }
}

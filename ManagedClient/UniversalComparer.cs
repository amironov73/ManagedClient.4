// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* UniversalComparer.cs -- универсальный компаратор
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Collections.Generic;

using JetBrains.Annotations;

using MoonSharp.Interpreter;

#endregion

namespace ManagedClient
{
    /// <summary>
    /// Универсальный компаратор.
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    public sealed class UniversalComparer<T>
        : IComparer<T>
    {
        #region Properties

        /// <summary>
        /// Используемый для сравнения делегат.
        /// </summary>
        [NotNull]
        public Func<T, T, int> Function { get { return _function; } }

        #endregion

        #region Construction

        /// <summary>
        /// Конструктор.
        /// </summary>
        public UniversalComparer
            (
                [NotNull] Func<T, T, int> function
            )
        {
            _function = function;
        }

        #endregion

        #region Private members

        private Func<T, T, int> _function;

        #endregion

        #region IComparer<T> members

        /// <summary>
        /// Compares the specified values.
        /// </summary>
        public int Compare(T left, T right)
        {
            return _function(left, right);
        }

        #endregion
    }
}

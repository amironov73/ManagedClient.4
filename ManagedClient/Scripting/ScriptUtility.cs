// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* ScriptUtility.cs
 */

#region Using directives

using System;
using System.Linq;

using JetBrains.Annotations;

using MoonSharp.Interpreter;

#endregion

namespace ManagedClient.Scripting
{
    /// <summary>
    /// Полезные методы расширения для скриптов.
    /// </summary>
    public static class ScriptUtility
    {
        #region Public methods

        /// <summary>
        /// Получение значения из таблицы.
        /// </summary>
        [CanBeNull]
        public static object GetProperty
            (
                [CanBeNull]this DynValue dynValue,
                params object[] keys
            )
        {
            if (dynValue == null)
            {
                return null;
            }
            if (dynValue.IsNil()
                || dynValue.IsVoid())
            {
                return null;
            }
            if (keys.Length == 0)
            {
                throw new ArgumentOutOfRangeException("keys");
            }

            Table table = dynValue.Table;
            return GetProperty
                (
                    table,
                    keys
                );
        }

        /// <summary>
        /// Получение значения из таблицы.
        /// </summary>
        [CanBeNull]
        public static object GetProperty
        (
            [CanBeNull]this Table table,
            params object[] keys
        )
        {
            if (table == null)
            {
                return null;
            }
            if (keys.Length == 0)
            {
                throw new ArgumentOutOfRangeException("keys");
            }

            object[] prefix = keys.Take(keys.Length - 1).ToArray();
            foreach (object key in prefix)
            {
                table = (Table)table[key];
                if (table == null)
                {
                    return null;
                }
            }
            object result = table[keys[keys.Length - 1]];
            return result;
        }

        /// <summary>
        /// Регистрация функции.
        /// </summary>
        public static void RegisterFunction<TResult>
            (
                this Script script,
                string name,
                Func<TResult> func
            )
        {
            if (string.IsNullOrEmpty(name))
            {
                throw new ArgumentNullException("name");
            }

            script.Globals[name] = func;
        }

        /// <summary>
        /// Регистрация функции.
        /// </summary>
        public static void RegisterFunction<TArg, TResult>
            (
                this Script script,
                string name,
                Func<TArg, TResult> func
            )
        {
            if (string.IsNullOrEmpty(name))
            {
                throw new ArgumentNullException("name");
            }

            script.Globals[name] = func;
        }

        /// <summary>
        /// Регистрация функции.
        /// </summary>
        public static void RegisterFunction<TArg1, TArg2, TResult>
            (
                this Script script,
                string name,
                Func<TArg1, TArg2, TResult> func
            )
        {
            if (string.IsNullOrEmpty(name))
            {
                throw new ArgumentNullException("name");
            }

            script.Globals[name] = func;
        }

        /// <summary>
        /// Регистрация функции.
        /// </summary>
        public static void RegisterFunction<TArg1, TArg2, TArg3, TResult>
            (
                this Script script,
                string name,
                Func<TArg1, TArg2, TArg3, TResult> func
            )
        {
            if (string.IsNullOrEmpty(name))
            {
                throw new ArgumentNullException("name");
            }

            script.Globals[name] = func;
        }


        #endregion
    }
}

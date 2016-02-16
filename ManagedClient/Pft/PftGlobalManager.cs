/* PftGlobalManager.cs
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.Linq;

#endregion

namespace ManagedClient.Pft
{
    /// <summary>
    /// Менеджер глобальных переменных для контекста форматирования.
    /// </summary>
    [Serializable]
    public sealed class PftGlobalManager
    {
        #region Properties

        /// <summary>
        /// Словарь, в котором хранятся глобальные переменные.
        /// </summary>
        public Dictionary<int, PftGlobalVariable> Dictionary
                { get { return _dictionary; } }

        /// <summary>
        /// Получение значения глобальной переменной по её индексу
        /// в строковом представлении. Если такой переменной нет,
        /// возвращается пустая строка.
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public string this[int index]
        {
            get
            {
                PftGlobalVariable result;
                return Dictionary.TryGetValue(index, out result)
                    ? result.ToString()
                    : string.Empty;
            }
            set
            {
                PftGlobalVariable variable = new PftGlobalVariable
                {
                    Number = index
                };
                variable.Parse(value);
                Dictionary[index] = variable;
            }
        }

        #endregion

        #region Construction

        public PftGlobalManager()
        {
            _dictionary = new Dictionary<int, PftGlobalVariable>();
        }

        #endregion

        #region Private members

        private readonly Dictionary<int,PftGlobalVariable> _dictionary;

        #endregion

        #region Public methods

        public PftGlobalManager Add
            (
                int index,
                string text
            )
        {
            this[index] = text;
            return this;
        }

        public PftGlobalManager Delete
            (
                int index
            )
        {
            Dictionary.Remove(index);
            return this;
        }

        public RecordField[] Get
            (
                int index
            )
        {
            PftGlobalVariable variable;
            if (Dictionary.TryGetValue(index, out variable))
            {
                return variable.Fields.Select(f => f.Clone()).ToArray();
            }
            return new RecordField[0];
        }

        public bool HaveVariable
            (
                int index
            )
        {
            return Dictionary.ContainsKey(index);
        }

        #endregion
    }
}

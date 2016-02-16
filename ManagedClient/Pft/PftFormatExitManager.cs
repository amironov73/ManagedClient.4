/* PftFormatExitManager.cs
 */

#region Using directives

using System;
using System.Collections.Generic;

#endregion

namespace ManagedClient.Pft
{
    using Ast;

    /// <summary>
    /// Менеджер форматных выходов. Позволяет
    /// зарегистрировать свои собственные форматные выходы.
    /// </summary>
    [Serializable]
    public sealed class PftFormatExitManager
    {
        #region Properties

        #endregion

        #region Construction

        public PftFormatExitManager()
        {
            _dictionary = new Dictionary<string, PftFormatExit>();
        }

        #endregion

        #region Private members

        private readonly Dictionary<string, PftFormatExit> _dictionary;

        #endregion

        #region Public methods

        public void Clear()
        {
            _dictionary.Clear();
        }

        public void Create
            (
                PftFormatExit formatExit
            )
        {
            _dictionary[formatExit.Name] = formatExit;
        }

        public void Delete
            (
                string name
            )
        {
            _dictionary.Remove(name);
        }

        public bool Have
            (
                string name
            )
        {
            return _dictionary.ContainsKey(name);
        }

        #endregion
    }
}

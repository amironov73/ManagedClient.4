// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* IrbisCache.cs
 */

#region Using directives

using System;
using System.Collections.Generic;

#endregion

namespace ManagedClient
{
    /// <summary>
    /// Простой кэш для форматов, меню и т. д.
    /// </summary>
    [Serializable]
    public sealed class IrbisCache
    {
        #region Events

        #endregion

        #region Properties

        public ManagedClient64 Client { get; set; }

        #endregion

        #region Construction

        public IrbisCache()
        {
            _dictionary = new Dictionary<string, string>();
        }

        #endregion

        #region Private members

        private readonly Dictionary<string, string> _dictionary;

        #endregion

        #region Public methods

        public void Clear()
        {
            _dictionary.Clear();
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

        public string Get
            (
                string name
            )
        {
            return Get(name, Client);
        }

        public string Get
            (
                string name,
                ManagedClient64 client
            )
        {
            string result;
            if (!_dictionary.TryGetValue(name, out result))
            {
                if (client != null)
                {
                    result = client.ReadTextFile(name);
                }
            }

            if (string.IsNullOrEmpty(result))
            {
                result = null;
            }

            if (result == null)
            {
                _dictionary.Remove(name);
            }
            else
            {
                _dictionary[name] = result;
            }

            return result;
        }

        public void Set
            (
                string name,
                string value
            )
        {
            if (string.IsNullOrEmpty(value))
            {
                _dictionary.Remove(name);
            }
            else
            {
                _dictionary[name] = value;
            }
        }

        #endregion
    }
}

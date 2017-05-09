// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* IrbisCache.cs
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
    /// Простой кэш для форматов, меню и т. д.
    /// </summary>
    [PublicAPI]
    [Serializable]
    [MoonSharpUserData]
    public sealed class IrbisCache
    {
        #region Events

        #endregion

        #region Properties

        /// <summary>
        /// Client connection.
        /// </summary>
        [CanBeNull]
        public ManagedClient64 Client { get; set; }

        #endregion

        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        public IrbisCache()
        {
            _dictionary = new Dictionary<string, string>();
            _lockRoot = new object();
        }

        #endregion

        #region Private members

        private readonly object _lockRoot;

        private readonly Dictionary<string, string> _dictionary;

        #endregion

        #region Public methods

        /// <summary>
        /// Clear the cache.
        /// </summary>
        public void Clear()
        {
            lock (_lockRoot)
            {
                _dictionary.Clear();
            }
        }

        /// <summary>
        /// Delete specified item.
        /// </summary>
        public void Delete
            (
                [NotNull] string name
            )
        {
            lock (_lockRoot)
            {
                _dictionary.Remove(name);
            }
        }

        /// <summary>
        /// Whether the cache have specified item?
        /// </summary>
        public bool Have
            (
                [NotNull] string name
            )
        {
            bool result;

            lock (_lockRoot)
            {
                result = _dictionary.ContainsKey(name);
            }

            return result;
        }

        /// <summary>
        /// Get specified item (may use client connection).
        /// </summary>
        public string Get
            (
                string name
            )
        {
            return Get(name, Client);
        }

        /// <summary>
        /// Get specified item (may use client connection);
        /// </summary>
        [CanBeNull]
        public string Get
            (
                [NotNull] string name,
                [CanBeNull] ManagedClient64 client
            )
        {
            string result;

            lock (_lockRoot)
            {
                if (!_dictionary.TryGetValue(name, out result))
                {
                    if (!ReferenceEquals(client, null))
                    {
                        result = client.ReadTextFile(name);
                    }
                }

                if (string.IsNullOrEmpty(result))
                {
                    result = null;
                }

                if (ReferenceEquals(result, null))
                {
                    _dictionary.Remove(name);
                }
                else
                {
                    _dictionary[name] = result;
                }
            }

            return result;
        }

        /// <summary>
        /// Set specified item.
        /// </summary>
        public void Set
            (
                [NotNull] string name,
                [NotNull] string value
            )
        {
            lock (_lockRoot)
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
        }

        #endregion
    }
}

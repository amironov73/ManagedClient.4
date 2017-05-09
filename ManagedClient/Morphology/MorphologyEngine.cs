// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* MorphologyEngine.cs
 */

#region Using directives

using System;
using JetBrains.Annotations;

using MoonSharp.Interpreter;

#endregion

namespace ManagedClient.Morphology
{
    /// <summary>
    /// Morphology engine.
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    public sealed class MorphologyEngine
    {
        #region Properties

        /// <summary>
        /// Client connection.
        /// </summary>
        [NotNull]
        public ManagedClient64 Client
        {
            get { return _client; }
        }

        /// <summary>
        /// Morphology provider.
        /// </summary>
        [NotNull]
        public MorphologyProvider Provider
        {
            get { return _provider; }
        }

        #endregion

        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        public MorphologyEngine
            (
                [NotNull] ManagedClient64 client
            )
        {
            if (ReferenceEquals(client, null))
            {
                throw new ArgumentNullException("client");
            }

            _client = client;
            _provider = new IrbisMorphologyProvider
                (
                    client
                );
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        public MorphologyEngine
            (
                [NotNull] ManagedClient64 client,
                [NotNull] MorphologyProvider provider
            )
        {
            if (ReferenceEquals(client, null))
            {
                throw new ArgumentNullException("client");
            }
            if (ReferenceEquals(provider, null))
            {
                throw new ArgumentNullException("provider");
            }

            _client = client;
            _provider = provider;
        }

        /// <summary>
        /// Contructor.
        /// </summary>
        public MorphologyEngine
            (
                [NotNull] ManagedClient64 client,
                [NotNull] string prefix,
                [NotNull] string database
            )
        {
            if (ReferenceEquals(client, null))
            {
                throw new ArgumentNullException("client");
            }
            if (string.IsNullOrEmpty(prefix))
            {
                throw new ArgumentNullException("prefix");
            }
            if (string.IsNullOrEmpty(database))
            {
                throw new ArgumentNullException("database");
            }

            _client = client;
            _provider = new IrbisMorphologyProvider
                (
                    prefix,
                    database,
                    client
                );
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        public MorphologyEngine
            (
                [NotNull] MorphologyProvider provider
            )
        {
            if (ReferenceEquals(provider, null))
            {
                throw new ArgumentNullException("provider");
            }

            _provider = provider;
        }

        #endregion

        #region Private members

        private readonly MorphologyProvider _provider;

        private readonly ManagedClient64 _client;

        #endregion

        #region Public methods

        /// <summary>
        /// Rewrite the query.
        /// </summary>
        [NotNull]
        public string RewriteQuery
            (
                [NotNull] string queryText
            )
        {
            if (string.IsNullOrEmpty(queryText))
            {
                throw new ArgumentNullException("queryText");
            }

            MorphologyProvider provider = Provider.ThrowIfNull("Provider");

            return provider.RewriteQuery(queryText);
        }

        /// <summary>
        /// Search with query rewritting.
        /// </summary>
        [NotNull]
        public int[] Search
            (
                [NotNull] string format,
                params object[] args
            )
        {
            if (string.IsNullOrEmpty(format))
            {
                throw new ArgumentNullException("format");
            }

            string original = string.Format(format, args);
            string rewritten = RewriteQuery(original);

            ManagedClient64 client = Client.ThrowIfNull("Client");

            return client.Search(rewritten);
        }

        /// <summary>
        /// Search and read records with query rewritting.
        /// </summary>
        [NotNull]
        public IrbisRecord[] SearchRead
            (
                [NotNull] string format,
                params object[] args
            )
        {
            if (string.IsNullOrEmpty(format))
            {
                throw new ArgumentNullException("format");
            }

            string original = string.Format(format, args);
            string rewritten = RewriteQuery(original);

            ManagedClient64 client = Client.ThrowIfNull("Client");

            return client.SearchRead(rewritten);
        }

        /// <summary>
        /// Search and read first found record using query rewritting.
        /// </summary>
        [CanBeNull]
        public IrbisRecord SearchReadOneRecord
            (
                [NotNull] string format,
                params object[] args
            )
        {
            if (string.IsNullOrEmpty(format))
            {
                throw new ArgumentNullException("format");
            }

            string original = string.Format(format, args);
            string rewritten = RewriteQuery(original);

            ManagedClient64 client = Client.ThrowIfNull("Client");

            return client.SearchReadOneRecord(rewritten);
        }

        /// <summary>
        /// Search and format found records using query rewritting.
        /// </summary>
        [NotNull]
        public string[] SearchFormat
            (
                [NotNull] string expression,
                [NotNull] string format
            )
        {
            if (string.IsNullOrEmpty(expression))
            {
                throw new ArgumentNullException("expression");
            }
            if (string.IsNullOrEmpty(format))
            {
                throw new ArgumentNullException("format");
            }

            string rewritten = RewriteQuery(expression);

            ManagedClient64 client = Client.ThrowIfNull("Client");

            return client.SearchFormat(rewritten, format);
        }

        #endregion
    }
}

// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* IrbisCommandEventArgs.cs
 */

#region Using directives

using System;

using JetBrains.Annotations;

using MoonSharp.Interpreter;

#endregion

namespace ManagedClient
{
    /// <summary>
    /// 
    /// </summary>
    [PublicAPI]
    [Serializable]
    [MoonSharpUserData]
    public sealed class IrbisCommadEventArgs
        : EventArgs
    {
        #region Properties

        /// <summary>
        /// Client connection.
        /// </summary>
        [CanBeNull]
        public ManagedClient64 Client { get; set; }

        /// <summary>
        /// Exception (if any).
        /// </summary>
        [CanBeNull]
        public IrbisException Exception { get; set; }

        /// <summary>
        /// Client query header.
        /// </summary>
        [CanBeNull]
        public QueryHeader QueryHeader { get; set; }

        /// <summary>
        /// Response header.
        /// </summary>
        [CanBeNull]
        public ResponseHeader Response { get; set; }

        /// <summary>
        /// Retry count.
        /// </summary>
        public int RetryCount { get; set; }

        /// <summary>
        /// Stop flag.
        /// </summary>
        public bool StopExecution { get; set; }

        /// <summary>
        /// Empty args.
        /// </summary>
        [NotNull]
        public static readonly IrbisCommadEventArgs EmptyArgs
            = new IrbisCommadEventArgs();

        #endregion

        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        public IrbisCommadEventArgs()
        {
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        public IrbisCommadEventArgs
            (
                [NotNull] ManagedClient64 client
            )
        {
            if (ReferenceEquals(client, null))
            {
                throw new ArgumentNullException("client");
            }

            Client = client;
            RetryCount = client.RetryCount;
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        public IrbisCommadEventArgs
            (
                [NotNull] ManagedClient64 client,
                [NotNull] IrbisException exception
            )
        {
            if (ReferenceEquals(client, null))
            {
                throw new ArgumentNullException("client");
            }
            if (ReferenceEquals(exception, null))
            {
                throw new ArgumentNullException("exception");
            }

            Client = client;
            RetryCount = client.RetryCount;
            Exception = exception;
        }

        #endregion
    }
}

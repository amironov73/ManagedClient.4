// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* ProcessingContext.cs
 */

#region Using directives

using System;
using System.IO;
using System.Text;

using JetBrains.Annotations;

using MoonSharp.Interpreter;

#endregion

namespace ManagedClient.Processing
{
    /// <summary>
    /// 
    /// </summary>
    [PublicAPI]
    [Serializable]
    [MoonSharpUserData]
    public sealed class ProcessingContext
    {
        #region Propeties

        /// <summary>
        /// Client connection.
        /// </summary>
        [NotNull]
        public ManagedClient64 Client { get; private set; }

        /// <summary>
        /// Current processing record.
        /// </summary>
        public IrbisRecord Record { get; set; }

        /// <summary>
        /// Accumulated text.
        /// </summary>
        [NotNull]
        public StringBuilder Accumulated { get { return _accumulated; } }

        /// <summary>
        /// Protocol.
        /// </summary>
        public TextWriter Protocol
        {
            get { return _protocol; }
        }

        /// <summary>
        /// Arbitrary user data.
        /// </summary>
        public object UserData
        {
            get { return _userData; }
            set { _userData = value; }
        }

        #endregion

        #region Construciton

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="client"></param>
        public ProcessingContext
            (
                [NotNull] ManagedClient64 client
            )
        {
            if (ReferenceEquals(client, null))
            {
                throw new ArgumentNullException("client");
            }

            Client = client;
            _protocol = TextWriter.Null;
            _accumulated = new StringBuilder();
        }

        #endregion

        #region Private members

        [NonSerialized]
        private object _userData;

        [NonSerialized]
        internal TextWriter _protocol;

        internal StringBuilder _accumulated;

        #endregion

        #region Public methods

        #endregion
    }
}

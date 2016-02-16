/* ProcessingContext.cs
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;

#endregion

namespace ManagedClient.Processing
{
    [Serializable]
    public sealed class ProcessingContext
    {
        #region Propeties

        public ManagedClient64 Client { get; private set; }

        public IrbisRecord Record { get; set; }

        public StringBuilder Accumulated { get { return _accumulated; } }

        public TextWriter Protocol
        {
            get { return _protocol; }
        }

        public object UserData
        {
            get { return _userData; }
            set { _userData = value; }
        }

        #endregion

        #region Construciton

        public ProcessingContext
            (
                ManagedClient64 client
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

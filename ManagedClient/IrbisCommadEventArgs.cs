/* IrbisCommandEventArgs.cs
 */

#region Using directives

using System;

#endregion

namespace ManagedClient
{
    [Serializable]
    public sealed class IrbisCommadEventArgs
        : EventArgs
    {
        #region Properties

        public ManagedClient64 Client { get; set; }

        public IrbisException Exception { get; set; }

        public QueryHeader QueryHeader { get; set; }

        public ResponseHeader Response { get; set; }

        public int RetryCount { get; set; }

        public bool StopExecution { get; set; }

        public static IrbisCommadEventArgs EmptyArgs 
            = new IrbisCommadEventArgs();

        #endregion

        #region Construction

        /// <summary>
        /// Default constructor.
        /// </summary>
        public IrbisCommadEventArgs ()
        {
        }

        /// <summary>
        /// Initializes a new instance of the 
        /// <see cref="IrbisCommadEventArgs" /> class.
        /// </summary>
        /// <param name="client">The client.</param>
        public IrbisCommadEventArgs 
            ( 
                ManagedClient64 client 
            )
        {
            Client = client;
            RetryCount = client.RetryCount;
        }

        /// <summary>
        /// Initializes a new instance of the 
        /// <see cref="IrbisCommadEventArgs" /> class.
        /// </summary>
        /// <param name="client">The client.</param>
        /// <param name="exception">The exception.</param>
        public IrbisCommadEventArgs 
            ( 
                ManagedClient64 client, 
                IrbisException exception 
            )
        {
            Client = client;
            RetryCount = client.RetryCount;
            Exception = exception;
        }

        #endregion
    }
}

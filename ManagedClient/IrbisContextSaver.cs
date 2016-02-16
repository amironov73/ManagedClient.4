﻿/* IrbisContextSaver.cs
 */

#region Using directives

using System;

#endregion

namespace ManagedClient
{
    /// <summary>
    /// Простая сохранялка текущего контекста для клиента.
    /// </summary>
    [Serializable]
    public class IrbisContextSaver
        : IDisposable
    {
        #region Properties

        /// <summary>
        /// Адрес сервера.
        /// </summary>
        public string Host { get; set; }

        /// <summary>
        /// Порт сервера.
        /// </summary>
        public int Port { get; set; }

        /// <summary>
        /// Имя пользователя.
        /// </summary>
        public string Username { get; set; }

        /// <summary>
        /// Пароль пользователя.
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// Имя базы данных.
        /// </summary>
        public string Database { get; set; }

        /// <summary>
        /// Тип АРМ.
        /// </summary>
        public IrbisWorkstation Workstation { get; set; }

        /// <summary>
        /// Клиент, для которого сохранён контекст.
        /// </summary>
        public ManagedClient64 Client
        {
            get { return _client; }
        }

        #endregion

        #region Construction

        /// <summary>
        /// Сохраняет контекст для указанного клиента.
        /// </summary>
        public IrbisContextSaver 
            ( 
                ManagedClient64 client 
            )
        {
            if ( ReferenceEquals ( client, null ) )
            {
                throw new ArgumentNullException("client");
            }
            _client = client;

            SaveContext ();
        }

        #endregion

        #region Private members

        [NonSerialized] 
        private readonly ManagedClient64 _client;

        #endregion

        #region Public methods

        /// <summary>
        /// Сохраняет контекст.
        /// </summary>
        /// <returns>Клиента.</returns>
        public ManagedClient64 SaveContext ()
        {
            Host = Client.Host;
            Port = Client.Port;
            Username = Client.Username;
            Password = Client.Password;
            Database = Client.Database;
            Workstation = Client.Workstation;

            return Client;
        }

        /// <summary>
        /// Восстанавливает контекст.
        /// </summary>
        /// <returns>Клиента.</returns>
        public ManagedClient64 RestoreContext ()
        {
            Client.Host = Host;
            Client.Port = Port;
            Client.Username = Username;
            Client.Password = Password;
            Client.Database = Database;
            Client.Workstation = Workstation;

            return Client;
        }

        #endregion

        #region IDisposable members

        /// <summary>
        /// Performs application-defined tasks associated 
        /// with freeing, releasing, or resetting 
        /// unmanaged resources.
        /// </summary>
        public void Dispose ()
        {
            RestoreContext ();
        }

        #endregion
    }
}

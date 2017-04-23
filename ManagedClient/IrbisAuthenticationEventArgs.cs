// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* IrbisAuthenticationEventArgs.cs
 */

#region Using directives

using System;

#endregion

namespace ManagedClient
{
    /// <summary>
    /// Аргументы для запроса логина/пароля пользователя.
    /// </summary>
    [Serializable]
    public sealed class IrbisAuthenticationEventArgs
        : EventArgs
    {
        #region Properties

        /// <summary>
        /// Код возврата ИРБИС (для показа читателю).
        /// </summary>
        public int ReturnCode { get; set; }

        /// <summary>
        /// Логин пользователя.
        /// </summary>
        public string Username { get; set; }

        /// <summary>
        /// Пароль пользователя.
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// Можно продолжать.
        /// </summary>
        public bool OkToConnect { get; set; }

        #endregion
    }
}

// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* IrbisClientSettings.cs
 */

#region Using directives

using System;
using System.Runtime.InteropServices;

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
    [ClassInterface(ClassInterfaceType.None)]
    public sealed class IrbisClientSettings
        : IniFile
    {
        #region Constants

        /// <summary>
        /// Main section.
        /// </summary>
        public const string Main = "MAIN";

        /// <summary>
        /// Desktop section.
        /// </summary>
        public const string Desktop = "DESKTOP";

        /// <summary>
        /// Context section.
        /// </summary>
        public const string Context = "CONTEXT";

        /// <summary>
        /// Private section.
        /// </summary>
        public const string Private = "PRIVATE";

        #endregion

        #region Properties

        /// <summary>
        /// User name.
        /// </summary>
        [CanBeNull]
        public string User
        {
            get { return GetString(Main, "User", string.Empty); }
        }

        /// <summary>
        /// Server IP address.
        /// </summary>
        [NotNull]
        public string ServerIP
        {
            get { return GetString(Main, "ServerIP", "127.0.0.1"); }
        }

        /// <summary>
        /// Server IP port number.
        /// </summary>
        public int ServerPort
        {
            get { return Get(Main, "ServerPort", 6666); }
        }

        /// <summary>
        /// UI font name.
        /// </summary>
        [NotNull]
        public string FontName
        {
            get { return GetString(Main, "FontName", "Arial"); }
        }

        /// <summary>
        /// UI font charset.
        /// </summary>
        public int FontCharSet
        {
            get { return Get(Main, "FontCharSet", 204); }
        }

        /// <summary>
        /// UI font size.
        /// </summary>
        public int FontSize
        {
            get { return Get(Main, "FontSize", 0); }
        }

        /// <summary>
        /// Whether the UI is scalable?
        /// </summary>
        public bool Scalable
        {
            get { return Get(Main, "Scalable", true); }
        }

        #endregion
    }
}

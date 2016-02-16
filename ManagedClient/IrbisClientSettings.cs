/* IrbisClientSettings.cs
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

#endregion

namespace ManagedClient
{
    public sealed class IrbisClientSettings
        : IniFile
    {
        #region Constants

        public const string Main = "MAIN";

        public const string Desktop = "DESKTOP";

        public const string Context = "CONTEXT";

        public const string Private = "PRIVATE";

        #endregion

        #region Properties

        public string User
        {
            get { return GetString(Main, "User", string.Empty); }
        }

        public string ServerIP
        {
            get { return GetString(Main, "ServerIP", "127.0.0.1"); }
        }

        public int ServerPort
        {
            get { return Get(Main, "ServerPort", 6666); }
        }

        public string FontName
        {
            get { return GetString(Main, "FontName", "Arial"); }
        }

        public int FontCharSet
        {
            get { return Get(Main, "FontCharSet", 204); }
        }

        public int FontSize
        {
            get { return Get(Main, "FontSize", 0); }
        }

        public bool Scalable
        {
            get { return Get(Main, "Scalable", true); }
        }

        #endregion
    }
}

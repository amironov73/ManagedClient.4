/* IrbisLoginCenter.cs -- вход в ИРБИС
 */

#region Using directives

using System;
using System.Globalization;
using System.Windows.Forms;

using ManagedClient;

#endregion

namespace IrbisUI
{
    /// <summary>
    /// Вход в ИРБИС.
    /// </summary>
    public static class IrbisLoginCenter
    {
        #region Public methods

        /// <summary>
        /// Простой вход в сервер.
        /// </summary>
        /// <param name="client"></param>
        /// <param name="owner"></param>
        /// <returns></returns>
        public static bool Login
            (
                this ManagedClient64 client,
                IWin32Window owner
            )
        {
            if (ReferenceEquals( client, null ))
            {
                throw new ArgumentNullException("client");
            }

            using (IrbisLoginForm form = new IrbisLoginForm())
            {
                form.UserName = client.Username;
                form.UserPassword = client.Password;
                form.Text = string.Format
                    (
                        "{0} - {1}",
                        form.Text,
                        client.Host
                    );

                DialogResult result = form.ShowDialog
                    (
                        owner
                    );
                if (result == DialogResult.OK)
                {
                    client.Username = form.UserName;
                    client.Password = form.UserPassword;
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Вход в сервер с указанием адреса.
        /// </summary>
        /// <param name="client"></param>
        /// <param name="owner"></param>
        /// <returns></returns>
        public static bool Login2
            (
                this ManagedClient64 client,
                IWin32Window owner
            )
        {
            if (ReferenceEquals(client, null))
            {
                throw new ArgumentNullException("client");
            }

            using (IrbisLoginForm2 form = new IrbisLoginForm2())
            {
                form.Host = client.Host;
                form.Port = client.Port.ToString(CultureInfo.InvariantCulture);
                form.UserName = client.Username;
                form.UserPassword = client.Password;
                form.Text = string.Format
                    (
                        "{0} - {1}",
                        form.Text,
                        client.Host
                    );

                DialogResult result = form.ShowDialog
                    (
                        owner
                    );
                if (result == DialogResult.OK)
                {
                    client.Host = form.Host;
                    client.Port = int.Parse(form.Port);
                    client.Username = form.UserName;
                    client.Password = form.UserPassword;
                    return true;
                }
            }

            return false;
        }

        public static bool TryLogin
            (
                this ManagedClient64 client,
                IWin32Window owner
            )
        {
            while (Login(client, owner))
            {
                try
                {
                    client.Connect();
                    if (client.Connected)
                    {
                        return true;
                    }
                }
                // ReSharper disable once EmptyGeneralCatchClause
                catch
                {
                    return false;
                }
            }

            return false;
        }

        public static bool TryLogin2
            (
                this ManagedClient64 client,
                IWin32Window owner
            )
        {
            while (Login2(client, owner))
            {
                try
                {
                    client.Connect();
                    if (client.Connected)
                    {
                        return true;
                    }
                }
                // ReSharper disable once EmptyGeneralCatchClause
                catch
                {
                    return false;
                }
            }

            return false;
        }

        #endregion
    }
}

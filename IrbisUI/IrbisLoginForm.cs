// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* IrbisLoginForm.cs -- логин и пароль для входа в ИРБИС
 */

#region Using directives

using System;
using System.Windows.Forms;

#endregion

namespace IrbisUI
{
    /// <summary>
    /// Окно с вводом логина и пароля для входа в ИРБИС.
    /// </summary>
    public partial class IrbisLoginForm 
        : Form
    {
        #region Properties

        /// <summary>
        /// Логин.
        /// </summary>
        public string UserName
        {
            get { return _nameBox.Text; }
            set { _nameBox.Text = value; }
        }

        /// <summary>
        /// Пароль.
        /// </summary>
        public string UserPassword
        {
            get { return _passwordBox.Text; }
            set { _passwordBox.Text = value; }
        }

        #endregion

        #region Construction

        /// <summary>
        /// Конструктор по умолчанию.
        /// </summary>
        public IrbisLoginForm()
        {
            InitializeComponent();
        }

        #endregion

        #region Private members

        private void _okButton_Click
            (
                object sender, 
                EventArgs e
            )
        {
            if (string.IsNullOrEmpty(_nameBox.Text))
            {
                _nameBox.Focus();
                DialogResult = DialogResult.None;
            }
            else if (string.IsNullOrEmpty(_passwordBox.Text))
            {
                _passwordBox.Focus();
                DialogResult = DialogResult.None;
            }
            else
            {
                DialogResult = DialogResult.OK;
            }
        }

        #endregion
    }
}

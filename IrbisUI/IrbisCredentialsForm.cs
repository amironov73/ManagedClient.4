// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace IrbisUI
{
    public partial class IrbisCredentialsForm
        : Form
    {
        public IrbisCredentialsForm()
        {
            InitializeComponent();
        }

        public string Server
        {
            get { return _serverBox.Text; } 
            set { _serverBox.Text = value; }
        }

        public string Port
        {
            get { return _portBox.Text; } 
            set { _portBox.Text = value; }
        }

        public string User
        {
            get { return _loginBox.Text; } 
            set { _loginBox.Text = value; }
        }

        public string Password
        {
            get { return _passwordBox.Text; } 
            set { _passwordBox.Text = value; }
        }
    }
}

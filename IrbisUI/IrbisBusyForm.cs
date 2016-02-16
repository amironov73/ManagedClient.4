/* IrbisBusyForm.cs
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;

#endregion

namespace IrbisUI
{
    public partial class IrbisBusyForm 
        : Form
    {
        #region Events

        public event EventHandler BreakPressed;

        #endregion

        #region Construction

        public IrbisBusyForm()
        {
            Debug.WriteLine("ENTER IrbisBusyForm..ctor");
            Debug.WriteLine("THREAD=" + Thread.CurrentThread.Name);

            InitializeComponent();

            Debug.WriteLine("LEAVE IrbisBusyForm..ctor");
        }

        #endregion

        #region Private members

        private void _breakButton_Click
            (
                object sender, 
                EventArgs e
            )
        {
            EventHandler handler = BreakPressed;
            if (!ReferenceEquals(handler, null))
            {
                handler(sender, e);
            }
        }

        #endregion

        #region Public methods

        public void SetTitle
            (
                string title
            )
        {
            Text = title;
        }

        public void SetMessage
            (
                string message
            )
        {
            _messageLabel.Text = message;
        }

        #endregion
    }
}

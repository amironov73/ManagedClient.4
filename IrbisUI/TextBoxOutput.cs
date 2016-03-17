/* TextBoxOutput.cs -- вывод в текстовое поле
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ManagedClient.Output;

#endregion

namespace IrbisUI
{
    /// <summary>
    /// Вывод в текстовое поле.
    /// </summary>
    public sealed class TextBoxOutput
        : AbstractOutput
    {
        #region Properties

        public TextBox TextBox { get; set; }

        #endregion

        #region Construction

        public TextBoxOutput()
        {
        }

        public TextBoxOutput
            (
                TextBox textBox
            )
        {
            if (ReferenceEquals(textBox, null))
            {
                throw new ArgumentNullException("textBox");
            }
            TextBox = textBox;
        }

        #endregion

        #region Private members

        #endregion

        #region Public methods

        public void AppendText
            (
                string text
            )
        {
            if (!string.IsNullOrEmpty(text))
            {
                if (TextBox != null)
                {
                    TextBox.AppendText(text);
                }
            }
            if (TextBox != null)
            {
                TextBox.SelectionStart = TextBox.TextLength;
            }
        }

        #endregion

        #region AbstractOutput members

        public override bool HaveError { get; set; }

        public override AbstractOutput Clear()
        {
            HaveError = false;
            if (TextBox != null)
            {
                TextBox.Clear();
            }
            return this;
        }

        public override AbstractOutput Configure
            (
                string configuration
            )
        {
            // TODO: implement
            return this;
        }

        public override AbstractOutput Write
            (
                string text
            )
        {
            AppendText(text);
            return this;
        }

        public override AbstractOutput WriteError(string text)
        {
            HaveError = true;
            AppendText(text);
            return this;
        }

        #endregion
    }
}

// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* LogTextBox.cs -- текстовый контрол для логирования.
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

#endregion

namespace IrbisUI
{
    /// <summary>
    /// Текстовый контрол для логирования
    /// </summary>
    public partial class LogTextBox 
        : TextBox
    {
        #region Properties

        public TextBoxOutput Output
        {
            get { return _output; }
        }

        #endregion

        #region Construction

        public LogTextBox()
        {
            InitializeComponent();
            _Setup();
        }

        public LogTextBox(IContainer container)
        {
            container.Add(this);
            InitializeComponent();
            _Setup();
        }

        #endregion

        #region Private members

        private TextBoxOutput _output;

        private void _Setup
            (
            )
        {
            _output = new TextBoxOutput(this);
            ReadOnly = true;
            Multiline = true;
            BackColor = SystemColors.Window;
            ScrollBars = ScrollBars.Vertical;
            WordWrap = true;

            ContextMenu = _CreateContextMenu();
        }

        private ContextMenu _CreateContextMenu()
        {
            ContextMenu result = new ContextMenu();
            result.MenuItems.Add
                (
                    "Очистить",
                    _clearClick
                );
            result.MenuItems.Add
                (
                    "Выделить всё",
                    _selectAllClick
                );
            result.MenuItems.Add
                (
                    "Копировать",
                    _copyClick
                );
            result.MenuItems.Add
                (
                    "Сохранить в файл",
                    _saveClick
                );

            return result;
        }

        private void _selectAllClick
            (
                object sender, 
                EventArgs eventArgs
            )
        {
            SelectAll();
        }

        private void _copyClick
            (
                object sender, 
                EventArgs eventArgs
            )
        {
            Copy();
        }

        private void _saveClick
            (
                object sender, 
                EventArgs eventArgs
            )
        {
            SaveFileDialog dialog = new SaveFileDialog
            {
                AddExtension = true,
                CheckFileExists = true,
                CheckPathExists = true,
                FileName = "log.txt",
                Filter = "Текстовые файлы|*.txt|Все файлы|*.*"
            };
            if (dialog.ShowDialog(FindForm()) == DialogResult.OK)
            {
                File.WriteAllText
                    (
                        dialog.FileName,
                        Text
                    );
            }
        }

        private void _clearClick
            (
                object sender, 
                EventArgs eventArgs
            )
        {
            Output.Clear();
        }

        #endregion

        #region Public methods

        public void Write
            (
                string text
            )
        {
            Output.Write(text);
        }

        public void WriteLine
            (
                string text
            )
        {
            Output.WriteLine(text);
        }

        #endregion
    }
}

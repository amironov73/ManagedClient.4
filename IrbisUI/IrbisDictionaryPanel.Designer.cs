// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

namespace IrbisUI
{
    partial class IrbisDictionaryPanel
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this._termBox = new System.Windows.Forms.ComboBox();
            this._listBox = new System.Windows.Forms.ListView();
            this._keyBox = new System.Windows.Forms.TextBox();
            this._countColumn = new System.Windows.Forms.ColumnHeader();
            this._termColumn = new System.Windows.Forms.ColumnHeader();
            this.SuspendLayout();
            // 
            // _termBox
            // 
            this._termBox.Dock = System.Windows.Forms.DockStyle.Top;
            this._termBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this._termBox.FormattingEnabled = true;
            this._termBox.Location = new System.Drawing.Point(0, 0);
            this._termBox.Name = "_termBox";
            this._termBox.Size = new System.Drawing.Size(299, 21);
            this._termBox.TabIndex = 0;
            // 
            // _listBox
            // 
            this._listBox.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this._countColumn,
            this._termColumn});
            this._listBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this._listBox.Location = new System.Drawing.Point(0, 21);
            this._listBox.Name = "_listBox";
            this._listBox.Size = new System.Drawing.Size(299, 474);
            this._listBox.TabIndex = 3;
            this._listBox.UseCompatibleStateImageBehavior = false;
            this._listBox.View = System.Windows.Forms.View.Details;
            // 
            // _keyBox
            // 
            this._keyBox.Dock = System.Windows.Forms.DockStyle.Bottom;
            this._keyBox.Location = new System.Drawing.Point(0, 495);
            this._keyBox.Name = "_keyBox";
            this._keyBox.Size = new System.Drawing.Size(299, 20);
            this._keyBox.TabIndex = 4;
            // 
            // _countColumn
            // 
            this._countColumn.Text = "Ссылок";
            // 
            // _termColumn
            // 
            this._termColumn.Text = "Термины";
            this._termColumn.Width = 260;
            // 
            // IrbisDictionaryPanel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this._listBox);
            this.Controls.Add(this._keyBox);
            this.Controls.Add(this._termBox);
            this.Name = "IrbisDictionaryPanel";
            this.Size = new System.Drawing.Size(299, 515);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox _termBox;
        private System.Windows.Forms.ListView _listBox;
        private System.Windows.Forms.TextBox _keyBox;
        private System.Windows.Forms.ColumnHeader _countColumn;
        private System.Windows.Forms.ColumnHeader _termColumn;
    }
}

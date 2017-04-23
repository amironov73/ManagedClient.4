// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

namespace IrbisUI
{
    partial class WssForm
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

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(WssForm));
            this.panel1 = new System.Windows.Forms.Panel();
            this._hintBox = new System.Windows.Forms.TextBox();
            this._gridView = new System.Windows.Forms.DataGridView();
            this._okButton = new System.Windows.Forms.Button();
            this._cancelButton = new System.Windows.Forms.Button();
            this._nameColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this._valueColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this._gridView)).BeginInit();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this._cancelButton);
            this.panel1.Controls.Add(this._okButton);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel1.Location = new System.Drawing.Point(0, 220);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(498, 49);
            this.panel1.TabIndex = 0;
            // 
            // _hintBox
            // 
            this._hintBox.Dock = System.Windows.Forms.DockStyle.Bottom;
            this._hintBox.Location = new System.Drawing.Point(0, 200);
            this._hintBox.Name = "_hintBox";
            this._hintBox.ReadOnly = true;
            this._hintBox.Size = new System.Drawing.Size(498, 20);
            this._hintBox.TabIndex = 1;
            this._hintBox.WordWrap = false;
            // 
            // _gridView
            // 
            this._gridView.AllowUserToAddRows = false;
            this._gridView.AllowUserToDeleteRows = false;
            this._gridView.AllowUserToResizeRows = false;
            this._gridView.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this._gridView.BackgroundColor = System.Drawing.SystemColors.Control;
            this._gridView.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this._gridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this._gridView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this._nameColumn,
            this._valueColumn});
            this._gridView.Dock = System.Windows.Forms.DockStyle.Fill;
            this._gridView.Location = new System.Drawing.Point(0, 0);
            this._gridView.MultiSelect = false;
            this._gridView.Name = "_gridView";
            this._gridView.RowHeadersVisible = false;
            this._gridView.ShowEditingIcon = false;
            this._gridView.ShowRowErrors = false;
            this._gridView.Size = new System.Drawing.Size(498, 200);
            this._gridView.TabIndex = 2;
            // 
            // _okButton
            // 
            this._okButton.Location = new System.Drawing.Point(330, 14);
            this._okButton.Name = "_okButton";
            this._okButton.Size = new System.Drawing.Size(75, 23);
            this._okButton.TabIndex = 0;
            this._okButton.Text = "Ввод";
            this._okButton.UseVisualStyleBackColor = true;
            // 
            // _cancelButton
            // 
            this._cancelButton.Location = new System.Drawing.Point(411, 14);
            this._cancelButton.Name = "_cancelButton";
            this._cancelButton.Size = new System.Drawing.Size(75, 23);
            this._cancelButton.TabIndex = 1;
            this._cancelButton.Text = "Отказ";
            this._cancelButton.UseVisualStyleBackColor = true;
            // 
            // _nameColumn
            // 
            this._nameColumn.HeaderText = "Подполе";
            this._nameColumn.Name = "_nameColumn";
            this._nameColumn.ReadOnly = true;
            // 
            // _valueColumn
            // 
            this._valueColumn.HeaderText = "Значение";
            this._valueColumn.Name = "_valueColumn";
            // 
            // WssForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(498, 269);
            this.Controls.Add(this._gridView);
            this.Controls.Add(this._hintBox);
            this.Controls.Add(this.panel1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MinimizeBox = false;
            this.Name = "WssForm";
            this.ShowInTaskbar = false;
            this.Text = "Редактирование поля";
            this.panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this._gridView)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.TextBox _hintBox;
        private System.Windows.Forms.DataGridView _gridView;
        private System.Windows.Forms.Button _cancelButton;
        private System.Windows.Forms.Button _okButton;
        private System.Windows.Forms.DataGridViewTextBoxColumn _nameColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn _valueColumn;
    }
}
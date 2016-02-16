namespace IrbisUI
{
    partial class IrbisMenuForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(IrbisMenuForm));
            this._gridView = new System.Windows.Forms.DataGridView();
            this._okButton = new System.Windows.Forms.Button();
            this._cancelButton = new System.Windows.Forms.Button();
            this._valueColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this._commentColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this._gridView)).BeginInit();
            this.SuspendLayout();
            // 
            // _gridView
            // 
            this._gridView.AllowUserToAddRows = false;
            this._gridView.AllowUserToDeleteRows = false;
            this._gridView.AllowUserToResizeRows = false;
            this._gridView.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this._gridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this._gridView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this._valueColumn,
            this._commentColumn});
            this._gridView.Dock = System.Windows.Forms.DockStyle.Top;
            this._gridView.Location = new System.Drawing.Point(0, 0);
            this._gridView.MultiSelect = false;
            this._gridView.Name = "_gridView";
            this._gridView.ReadOnly = true;
            this._gridView.RowHeadersVisible = false;
            this._gridView.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this._gridView.ShowEditingIcon = false;
            this._gridView.Size = new System.Drawing.Size(502, 222);
            this._gridView.TabIndex = 0;
            // 
            // _okButton
            // 
            this._okButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this._okButton.Location = new System.Drawing.Point(278, 235);
            this._okButton.Name = "_okButton";
            this._okButton.Size = new System.Drawing.Size(95, 23);
            this._okButton.TabIndex = 1;
            this._okButton.Text = "Ввод";
            this._okButton.UseVisualStyleBackColor = true;
            // 
            // _cancelButton
            // 
            this._cancelButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this._cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this._cancelButton.Location = new System.Drawing.Point(390, 235);
            this._cancelButton.Name = "_cancelButton";
            this._cancelButton.Size = new System.Drawing.Size(100, 23);
            this._cancelButton.TabIndex = 2;
            this._cancelButton.Text = "Отказ";
            this._cancelButton.UseVisualStyleBackColor = true;
            // 
            // _valueColumn
            // 
            this._valueColumn.HeaderText = "Значение";
            this._valueColumn.Name = "_valueColumn";
            this._valueColumn.ReadOnly = true;
            // 
            // _commentColumn
            // 
            this._commentColumn.HeaderText = "Пояснения";
            this._commentColumn.Name = "_commentColumn";
            this._commentColumn.ReadOnly = true;
            // 
            // IrbisMenuForm
            // 
            this.AcceptButton = this._okButton;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this._cancelButton;
            this.ClientSize = new System.Drawing.Size(502, 270);
            this.Controls.Add(this._cancelButton);
            this.Controls.Add(this._okButton);
            this.Controls.Add(this._gridView);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MinimizeBox = false;
            this.Name = "IrbisMenuForm";
            this.Text = "Меню";
            ((System.ComponentModel.ISupportInitialize)(this._gridView)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView _gridView;
        private System.Windows.Forms.Button _okButton;
        private System.Windows.Forms.Button _cancelButton;
        private System.Windows.Forms.DataGridViewTextBoxColumn _valueColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn _commentColumn;
    }
}
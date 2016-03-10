namespace IrbisUI
{
    partial class SelectionPanel
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
            this.label1 = new System.Windows.Forms.Label();
            this._databaseBox = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this._criteriaBox = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this._statementBox = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Dock = System.Windows.Forms.DockStyle.Top;
            this.label1.Location = new System.Drawing.Point(0, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(72, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "База данных";
            // 
            // _databaseBox
            // 
            this._databaseBox.Dock = System.Windows.Forms.DockStyle.Top;
            this._databaseBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this._databaseBox.FormattingEnabled = true;
            this._databaseBox.Location = new System.Drawing.Point(0, 13);
            this._databaseBox.Name = "_databaseBox";
            this._databaseBox.Size = new System.Drawing.Size(287, 21);
            this._databaseBox.TabIndex = 1;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Dock = System.Windows.Forms.DockStyle.Top;
            this.label2.Location = new System.Drawing.Point(0, 34);
            this.label2.Name = "label2";
            this.label2.Padding = new System.Windows.Forms.Padding(0, 5, 0, 0);
            this.label2.Size = new System.Drawing.Size(93, 18);
            this.label2.TabIndex = 2;
            this.label2.Text = "Критерий отбора";
            // 
            // _criteriaBox
            // 
            this._criteriaBox.Dock = System.Windows.Forms.DockStyle.Top;
            this._criteriaBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this._criteriaBox.FormattingEnabled = true;
            this._criteriaBox.Location = new System.Drawing.Point(0, 52);
            this._criteriaBox.Name = "_criteriaBox";
            this._criteriaBox.Size = new System.Drawing.Size(287, 21);
            this._criteriaBox.TabIndex = 3;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Dock = System.Windows.Forms.DockStyle.Top;
            this.label3.Location = new System.Drawing.Point(0, 73);
            this.label3.Name = "label3";
            this.label3.Padding = new System.Windows.Forms.Padding(0, 5, 0, 0);
            this.label3.Size = new System.Drawing.Size(124, 18);
            this.label3.TabIndex = 4;
            this.label3.Text = "Поисковое выражение";
            // 
            // _statementBox
            // 
            this._statementBox.Dock = System.Windows.Forms.DockStyle.Top;
            this._statementBox.Location = new System.Drawing.Point(0, 91);
            this._statementBox.Name = "_statementBox";
            this._statementBox.Size = new System.Drawing.Size(287, 20);
            this._statementBox.TabIndex = 5;
            // 
            // SelectionPanel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.Controls.Add(this._statementBox);
            this.Controls.Add(this.label3);
            this.Controls.Add(this._criteriaBox);
            this.Controls.Add(this.label2);
            this.Controls.Add(this._databaseBox);
            this.Controls.Add(this.label1);
            this.Name = "SelectionPanel";
            this.Size = new System.Drawing.Size(287, 116);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox _databaseBox;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox _criteriaBox;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox _statementBox;
    }
}

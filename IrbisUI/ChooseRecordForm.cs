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
    public partial class ChooseRecordForm 
        : Form
    {
        public ChooseRecordForm()
        {
            InitializeComponent();
        }

        public ChooseInfo Choosed
        {
            get { return _listBox.SelectedItem as ChooseInfo; }
        }

        public void SetVariants
            (
                List<ChooseInfo> variants
            )
        {
            _listBox.Items.Clear();
            _listBox.Items.AddRange(variants.ToArray());
            _listBox.SelectedIndex = 0;
        }

        private void _listBox_MeasureItem
            (
                object sender, 
                MeasureItemEventArgs e
            )
        {
            int index = e.Index;
            if ((index >= 0) || (index < _listBox.Items.Count))
            {
                ChooseInfo chooseInfo = (ChooseInfo) _listBox.Items[index];
                string text = chooseInfo.ToString();
                SizeF size = e.Graphics.MeasureString(text, _listBox.Font, 
                    _listBox.ClientSize.Width);
                e.ItemWidth = (int) size.Width;
                e.ItemHeight = (int) size.Height + 4;
            }
        }

        private void _listBox_DrawItem
            (
                object sender, 
                DrawItemEventArgs e
            )
        {
            int index = e.Index;
            e.DrawBackground();
            if ((index >= 0) || (index < _listBox.Items.Count))
            {
                ChooseInfo chooseInfo = (ChooseInfo)_listBox.Items[index];
                string text = chooseInfo.ToString();
                //SizeF size = e.Graphics.MeasureString(text, _listBox.Font,
                //    _listBox.ClientSize.Width);
                using (Brush brush = new SolidBrush(e.ForeColor))
                {
                    Rectangle bounds = e.Bounds;
                    bounds.Inflate(-1,-2);
                    e.Graphics.DrawString(text,e.Font,brush,bounds);
                }
            }
            e.DrawFocusRectangle();
        }
    }
}

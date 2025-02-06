#if false
using System.Drawing;
using System.Windows.Forms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Diagnostics;

namespace net_yuri_installer.ExtraWindows
{
    public partial class DropDownList : Form
    {
        private ListBox listBox1;
        private ScrollPBPanel listPanel;
        public string SelectedItem;

        public DropDownList()
        {
            Visible = false;
            InitializeComponent();
            CustomPanelsAndTextBoxes();
        }

        public void Show(List<string> list)
        {
            Show();
            listBox1.Items.Clear();
            listBox1.DataSource = list;
        }

        private void CustomPanelsAndTextBoxes()
        {
            listBox1 = new ListBox();
            listPanel = new ScrollPBPanel(listBox1);
            listPanel.SuspendLayout();
            // 
            // contentPanel
            // 
            listPanel.Location = new Point(0, 0);
            listPanel.Name = "contentPanel";
            listPanel.Size = Size;
            listPanel.Visible = true;
            listPanel.BackColor = BackColor;
            // 
            // listBox1
            // 
            listBox1.FormattingEnabled = true;
            listBox1.ItemHeight = 12;
            listBox1.Name = "listBox1";
            listBox1.TabIndex = 0;
            listBox1.SelectedValueChanged += ListBox1_SelectedValueChanged;
            listBox1.BackColor = BackColor;
            listBox1.ForeColor = _StartWindow.lightTextColor;
            listBox1.BorderStyle = BorderStyle.None;
            listPanel.ResumeLayout(false);
            Controls.Add(listPanel);
        }

        private void ListBox1_SelectedValueChanged(object sender, EventArgs e)
        {
            SelectedItem = (string)listBox1.SelectedItem;
            Visible = false;
        }

        private void DropDownList_Leave(object sender, EventArgs e)
        {
            Debug.WriteLine("梁如萱我想杀了你");
            Visible = false;
        }

        public void SetFont(Font font)
        {
            Font = font;
            listBox1.Font = font;
        }

        public string SetItems(List<string> list)
        {
            listBox1.DataSource = list;
            return list == null || list.Count < 1  ? "null" : list[0];
        }

        public List<string> GetItems() => listBox1.Items.Cast<string>().ToList();

        private void DropDownList_SizeChanged(object sender, EventArgs e)
        {
            listPanel.Size = Size;
            if (listPanel.Height != listBox1.Height + 4) 
            {
                Height = listPanel.Height = MizukiTools.GetValueInADuring(listBox1.Height + 4, 0, 500);
            }
        }
    }
}
#endif
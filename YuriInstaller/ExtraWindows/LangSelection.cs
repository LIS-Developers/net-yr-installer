using System;
using System.Collections.Generic;
using System.Globalization;
using System.Windows.Forms;
using YuriInstaller.MizukiTools;
using YuriInstaller.Properties;

namespace YuriInstaller.ExtraWindows
{
    public partial class LangSelection : DoubleBufferedForm
    {
        private readonly static List<string> LangList = new List<string>();
        private readonly static List<string> langNames = new List<string>();
        internal Form SetLanguageToForm;

        private bool firstuse = true;
        private int _lastSel;

        public LangSelection()
        {
            InitializeComponent();
            pictureBox1.BackgroundImage = Resources.icon.ToBitmap();
            lrxLabel1.Font = btnOK.Font =
            btnCancel.Font = dropDownPanel.Font = Program.defaultFont;

            langNames.Add(Program.L10N[Program.Lang].L10n.AutoSelect);
            foreach (var i in Program.L10N.Keys)
            {
                LangList.Add(i);
                string j = new CultureInfo(i).NativeName;
                langNames.Add(j);
            }
            dropDownPanel.DataSource = langNames;

            ShowDialog();
        }

        private void OKButton_Click(object sender, EventArgs e)
        {
            var index = dropDownPanel.SelectedIndex;
            var selected = index <= 0 ? Program.PCLang : LangList[index - 1];

            // 如果语言没改就跳过
            if (_lastSel == index || Program.Lang == selected)
            {
                goto end;
            }
            _lastSel = index;

            if (SetLanguageToForm == null)
            {
                Program.Lang = Program.GetLanguage(selected);
            }
            else
            {
                (SetLanguageToForm as _StartWindow)?.L10n(selected);
            }

        end:
            firstuse = false;
            Close();
        }

        private void CancelButton_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void LangSelection_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (firstuse)
            {
                Environment.Exit(0);
            }
        }

        private void LangSelection_HelpButtonClicked(object sender, System.ComponentModel.CancelEventArgs e)
        {
            MessageBox.Show(Program.L10N[Program.Lang].L10n.SelectLanguageHelp);
        }

        private void LangSelection_VisibleChanged(object sender, EventArgs e)
        {
            if (Visible)
            {
                Text = Program.L10N[Program.Lang].L10n.SelectLanguageTitle;
                lrxLabel1.Text = Program.L10N[Program.Lang].L10n.SelectLanguageLabel;
                btnOK.Text = Program.L10N[Program.Lang].L10n.Button_OK;
                btnCancel.Text = Program.L10N[Program.Lang].L10n.Button_Cancel;
            }
        }
    }
}

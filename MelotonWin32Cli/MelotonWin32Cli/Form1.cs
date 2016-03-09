using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace MelotonWin32Cli
{
    public partial class Form1 : Form
    {
        string mark = "yhfs://";

        public Form1()
        {
            InitializeComponent();
            Win32API.AddClipboardFormatListener(this.Handle); 
        }

        private void UpdateClipValueList()
        {
            if (Clipboard.ContainsText())
            {
                var text = Clipboard.GetText().Trim();

                if (text.StartsWith("yhfs://"))
                {
                    FrmProgress p = new FrmProgress(text.Substring(mark.Length - 1));
                    p.Show(); 
                }
            }
        }

        protected override void SetVisibleCore(bool value)
        {
            base.SetVisibleCore(false);
        }

        protected override void DefWndProc(ref System.Windows.Forms.Message m)
        {
            if (m.Msg == Win32API.WM_CLIPBOARDUPDATE)
            {
                UpdateClipValueList();
            }
            else
            {
                base.DefWndProc(ref m);
            }
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            Win32API.RemoveClipboardFormatListener(this.Handle);
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            notifyIcon1.ShowBalloonTip(1000);
        }
    }
}

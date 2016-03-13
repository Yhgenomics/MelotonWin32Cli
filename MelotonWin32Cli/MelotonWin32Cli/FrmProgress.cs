using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace MelotonWin32Cli
{
    public partial class FrmProgress : Form
    {
        melotoncsharp.Meloton meloton = new melotoncsharp.Meloton("10.0.0.20", 101);
        string uri = "";
        Thread thr;

        Stream fileStream = null;

        public FrmProgress(string uri)
        {
            InitializeComponent();
            this.textBox1.Text = uri;
            this.uri = uri;

            meloton.GetProgressCallback((int p, string s) => {

                this.Invoke(new Action(() => {

                    if (s != null)
                    {
                        this.textBox1.Text = s;
                        return;
                    }

                    this.progressBar1.Value = p;

                    if(  p == 100 )
                    {
                        this.Close();
                    }

                }));

            });
        }

        private void FrmProgress_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            FileDialog fd = new SaveFileDialog();
            fd.FileName = Path.GetFileName(uri);
            if(fd.ShowDialog() == DialogResult.OK )
            {
                this.button1.Hide();

                thr = new Thread(() =>
                {
                    fileStream = System.IO.File.Open(fd.FileName, System.IO.FileMode.OpenOrCreate);
                    int ret = meloton.GetFile(uri, fileStream);
                    if (ret > 0)
                    {

                    }
                });
                thr.Start();

            }

        }

        private void FrmProgress_FormClosing(object sender, FormClosingEventArgs e)
        {
            if(meloton!=null)
                meloton.GetProgressCallback(null);
            if (thr != null)
                thr.Abort();
            if(fileStream!=null)
                fileStream.Close();
        }
    }
}

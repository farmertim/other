using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net;

namespace testCamera
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            HttpWebRequest req;
            HttpWebResponse resp = null;
            Uri url = new Uri("https://www.google.com/search?q=rtsp%3A%2F%2Fmp-tw-rtsp-2.auto.mydlink.com%3A443%2Falexa%2Fprofile.0%3Fcid%3DpttQoKWuGcMF%26mac%3DB0C55458FE0A&rlz=1C1CHBF_zh-TWTW933TW933&oq=rtsp%3A%2F%2Fmp-tw-rtsp-2.auto.mydlink.com%3A443%2Falexa%2Fprofile.0%3Fcid%3DpttQoKWuGcMF%26mac%3DB0C55458FE0A&aqs=chrome.0.69i59j69i58.1416j0j9&sourceid=chrome&ie=UTF-8"); //*** 是為隱藏 IP 位置
            req = (HttpWebRequest)HttpWebRequest.Create(url); //Create HttpWebRequest
            try
            {
                resp = (HttpWebResponse)req.GetResponse();
            }
            catch (Exception wex)
            {
                textBox1.Text = wex.ToString();
            }
            finally
            {
            }
        }
    }
}

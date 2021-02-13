using System;
using System.Drawing;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Windows.Forms;
using System.IO.MemoryMappedFiles;
using System.Threading;
using System.IO.Pipes;
using System.Text;

namespace form1
{
    public partial class Form1 : Form
    {
        int numBytes;
        int numChars;
        string message = "";
        StreamReader reader;

        StreamWriter writer;
        NamedPipeClientStream client,client1;
        MemoryStream ms;
        byte[] b;
        int checkfirst = 0;
        Decoder decoder = Encoding.UTF8.GetDecoder();
        byte[] bytes = new byte[10];
        char[] chars = new char[10];
        public Form1()
        {
            InitializeComponent();

        }
        public void pipClient()
        {

            client = new NamedPipeClientStream("PipeC#"); //pipe初始化
            client1 = new NamedPipeClientStream("Yang");
            client.Connect();  //等待連接
            client1.Connect();
            reader = new StreamReader(client1); //讀取資料
            //writer = new StreamWriter(client);
        }
       
        private void timer1_Tick(object sender, EventArgs e) //接收影像畫面
        {
            if (checkfirst == 0) {
                pipClient();
                checkfirst = 1;  //連接成功時更改為1
            }
            if (checkfirst==1)
            {
                b = new byte[2000000];
                int num;
                num = client.Read(b, 0, b.Length);
                if (num > 0)
                {
                    ms = new MemoryStream(b);
                    pictureBox1.Image = Image.FromStream(ms);
                    ms.Dispose();
                }
            }
        }

        private void label1_Click(object sender, EventArgs e)
        {
            label1.Width = 100;
            label1.Height = 100;
        }
        private void TabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch ((sender as TabControl).SelectedIndex)
            {
                case 0:
                    //MessageBox.Show("第一頁");
                    break;
                case 1:
                    //MessageBox.Show("2");
                    break;
                case 2:
                    //MessageBox.Show("3");
                    break;
            }
        }

        private void tabPage1_Click(object sender, EventArgs e)
        {

        }

        private void Timer2_Tick(object sender, EventArgs e) //模擬發生跌倒的事件
        {
            pictureBox2.Visible = true;
            if (checkfirst == 1)  //checkfirst = 1表示已連接
            {
                numBytes = client1.Read(bytes, 0, bytes.Length);
                numChars = decoder.GetChars(bytes, 0, numBytes, chars, 0);
                message = new string(chars, 0, numChars);
                label3.Text = message;
                if (message == "aa")
                {
                    pictureBox2.Visible = false;
                }
            }
        }

    }
}

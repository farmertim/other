using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.IO.Pipes;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Emgu.CV;
using Emgu.CV.Structure;
namespace pipserver
{
    public partial class Form1 : Form
    {
        Capture capture;
        Bitmap bitmap=new Bitmap(1080,720);
        Image<Bgr, Byte> fram;
        MemoryStream ms;
        NamedPipeServerStream server;
        bool check = false;
        int count = 0;
        StreamReader reader;
        StreamWriter writer;
        Stream stream;
        byte[] b;
        public Form1()
        {
            InitializeComponent();
            capture = new Capture(0);
            Application.Idle += new EventHandler(Application_idle);
            
        }
        public void Application_idle(object sender, EventArgs e)
        {
            if (check)
            {
                fram = capture.QueryFrame();
                System.Diagnostics.Stopwatch sw = new System.Diagnostics.Stopwatch();//引用stopwatch物件

                sw.Reset();//碼表歸零

                sw.Start();//碼表開始計時
                pictureBox1.Image = fram.ToBitmap();

                ms = new MemoryStream();
                fram.ToBitmap().Save(ms, System.Drawing.Imaging.ImageFormat.Png);
                b = ms.ToArray();

                server.Write(b, 0, b.Length);
                ms.Dispose();
                sw.Stop();
                label2.Text = Convert.ToString(sw.Elapsed.TotalMilliseconds.ToString());
            }
            //label1.Text = Convert.ToString(fram.Width);
            //label4.Text = Convert.ToString(fram.Height);
            //bitmap = fram.ToBitmap();
        }
        public void pipServer() {
            
        }
        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            /*if (check)
            {
                byte[] b;
                ms = new MemoryStream();
                bitmap.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
                b = ms.ToArray();
                label2.Text = Convert.ToString(b.Length);
                server.Write(b, 0, b.Length);
                ms.Dispose();
            }*/

        }

        private void button1_Click(object sender, EventArgs e)
        {
            StartServer();
            Task.Delay(1000).Wait();
            
        }
        public void StartServer()
        {
            Task.Factory.StartNew(() =>
            {
                server = new NamedPipeServerStream("PipesOfPiece");
                server.WaitForConnection();
                reader = new StreamReader(server);
                writer = new StreamWriter(server);
                check = true;
            });
        }

        private void button2_Click(object sender, EventArgs e)
        {
            var line = textBox1.Text;
            writer.WriteLine(line);
            writer.Flush();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            byte[] b;
            ms = new MemoryStream();
            bitmap.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
            b = ms.ToArray();
            label2.Text = Convert.ToString(b.Length);
            server.Write(b, 0,b.Length);
            
        }
    }
}

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
        Bitmap bitmap = new Bitmap(1080, 720);
        Image<Bgr, Byte> fram;
        MemoryStream ms;
        NamedPipeServerStream serverC,serverP;
        bool checkC = false, checkP = false;
        int count = 0;
        StreamReader reader;
        StreamWriter writer;
        BinaryReader br;
        BinaryWriter bw;
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
            System.Diagnostics.Stopwatch sw = new System.Diagnostics.Stopwatch();//引用stopwatch物件

            sw.Reset();//碼表歸零

            sw.Start();//碼表開始計時
            fram = capture.QueryFrame();

            pictureBox1.Image =fram.ToBitmap();

            ms = new MemoryStream();
            fram.ToBitmap().Save(ms, System.Drawing.Imaging.ImageFormat.Png);
            b = ms.ToArray();
            label3.Text = Convert.ToString(b.Length);
            ms.Dispose();
            sw.Stop();
            label2.Text = Convert.ToString(sw.Elapsed.TotalMilliseconds.ToString());

        }
        public void pipServer()
        {

        }
        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void timer1_Tick(object sender, EventArgs e)
        {

            if (checkC)
            {
                serverC.Write(b, 0, b.Length);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Task.Factory.StartNew(() =>
            {
                serverC = new NamedPipeServerStream("PipeC#", PipeDirection.InOut, 10);
                serverC.WaitForConnection();
                reader = new StreamReader(serverC);
                writer = new StreamWriter(serverC);
                checkC = true;
            });
            Task.Delay(1000).Wait();

        }
     

        private void timer2_Tick(object sender, EventArgs e)
        {

            if (checkP)
            {

                bw.Write((uint)b.Length);
                bw.Write(b);
                //bw.Flush();
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
           
            Task.Factory.StartNew(() =>
            {
                serverP = new NamedPipeServerStream("PipePython", PipeDirection.InOut, 10);
                serverP.WaitForConnection();
                br = new BinaryReader(serverP);
                bw = new BinaryWriter(serverP);
                checkP = true;
            });
            Task.Delay(1000).Wait();
        }
    }
}

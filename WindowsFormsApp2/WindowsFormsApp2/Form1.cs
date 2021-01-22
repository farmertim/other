using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.IO.Pipes;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Emgu.CV;
using Emgu.CV.Structure;


using Emgu.CV.ML;
using Emgu.CV.Util;
using Microsoft.Azure.Amqp;
using System.IO.MemoryMappedFiles;

namespace WindowsFormsApp2
{
    public partial class Form1 : Form
    {
        private static byte[] result = new byte[1024 * 10000];
        private static int myProt = 8888;   //埠
        Capture cap = null;
        int Height = Screen.PrimaryScreen.Bounds.Height;
        int Width = Screen.PrimaryScreen.Bounds.Width;
        byte[] b;
        Socket listener;
        int count = 0, temp = 0, count1 = 0;
        bool check = false;
        string result1 = "";
        MemoryStream ms;
        Socket serverSokcet;
        bool mutexCreat;
        MemoryMappedFile picture_memory = MemoryMappedFile.CreateOrOpen("picture", 10000000);
        Mutex mutex;
        int memory_location = 0, memoryCount = 0, check_first_memory = 0;
        public Form1()
        {
            InitializeComponent();
            //label5.Text = Width + ":" +Height+"--"+width+":"+height;
            mutex = new Mutex(true, "test", out mutexCreat);
        }

        public void ServerCode()
        {

            IPAddress ip = IPAddress.Parse("127.0.0.1");
            listener = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            listener.Bind(new IPEndPoint(ip, myProt));  //繫結IP地址：埠 
            listener.Listen(10);    //設定最多10個排隊連線請求
            label3.Text = "啟動監聽{0}g" + listener.LocalEndPoint.ToString();
            check = true;

        }
        public void sendMSG()
        {
            serverSokcet = listener.Accept();
            //byte[] msg = Encoding.ASCII.GetBytes(Convert.ToString(b));
            serverSokcet.Send(b);
            count++;
            serverSokcet.Shutdown(SocketShutdown.Both);
            serverSokcet.Close();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }


        public Bitmap getCreen()
        {
            Bitmap myImage = new Bitmap(Width, Height);
            Graphics g = Graphics.FromImage(myImage);
            g.CopyFromScreen(new Point(0, 0), new Point(0, 0), new Size(Width, Height));
            return myImage;

        }
        private void timer1_Tick(object sender, EventArgs e)
        {
            label5.Text = Convert.ToString(count);
            if (check)
            {

                System.Diagnostics.Stopwatch sw = new System.Diagnostics.Stopwatch();//引用stopwatch物件

                sw.Reset();//碼表歸零

                sw.Start();//碼表開始計時
                //Graphics g = Graphics.FromImage(myImage);
                MemoryStream ms = new MemoryStream();
                //ms.Position = 100;
                getCreen().Save(ms, System.Drawing.Imaging.ImageFormat.Png);   //Here I use the BMP format

                b = ms.ToArray();
                //ms.Write(b, 0, b.Length);
                //ms.Write(result, 100, Convert.ToInt32(ms.Length));
                ms.Flush();
                ms.Close();
                sendMSG();
                sw.Stop();
                result1 = sw.Elapsed.TotalMilliseconds.ToString();
                label2.Text = result1;
                label4.Text = "go";
            }
            else
            {
                label4.Text = "no ready";
            }
        }

        private void timer2_Tick(object sender, EventArgs e)
        {


            int ttt = 0;
            if (check_first_memory == 0)
            {
                mutex.WaitOne();
                writeMemory();
                mutex.ReleaseMutex();
                if (count1 == 30)
                {
                    check_first_memory = 1;
                    memoryCount = 0;
                }
                count1++;
            }
            else
            {
                mutex.WaitOne();
                int tempmemory = 0, ch = 0;
                for (int i = 0; i < 30; i++)
                {
                    MemoryMappedFile mmf = MemoryMappedFile.OpenExisting("picture");
                    MemoryMappedViewAccessor accessor = mmf.CreateViewAccessor();

                    byte[] Buffer = new byte[4];
                    accessor.ReadArray(tempmemory, Buffer, 0, 4);//memoryCount  
                    
                    if (BitConverter.ToInt32(Buffer, 0) == 1)
                    {
                        memoryCount = tempmemory;
                        count = i;
                        ch = 1;
                        break;
                    }
                    tempmemory += 150008;
                    accessor.Dispose();
                }
                
                if (ch == 1)
                {
                    mutex.ReleaseMutex();
                    mutex.WaitOne();
                    writeMemory();
                    count++;
                    mutex.ReleaseMutex();

                    if (count == 30)
                    {
                        count = 0;
                        memoryCount = 0;
                    }
                    temp++;
                }
            }





            //label2.Text = Convert.ToString(bytes1.Length);


        }
        public void writeMemory()
        {
            //mutex.WaitOne();
            System.Diagnostics.Stopwatch sw = new System.Diagnostics.Stopwatch();//引用stopwatch物件

            sw.Reset();//碼表歸零

            sw.Start();//碼表開始計時
            label3.Text = Convert.ToString(temp);
            ms = new MemoryStream();
            getCreen().Save(ms, System.Drawing.Imaging.ImageFormat.Png);
            b = ms.ToArray();
            label5.Text = Convert.ToString(b.Length);

            //picture_memory = MemoryMappedFile.CreateOrOpen("picture", 5000000);  // 建立指定大小的記憶體檔案，會在應用程式退出時自動釋放
            MemoryMappedViewAccessor accessor1 = picture_memory.CreateViewAccessor();
            byte[] header = BitConverter.GetBytes(2);
            byte[] pictureSize = BitConverter.GetBytes(b.Length);
            accessor1.WriteArray(memoryCount, header, 0, 4);
            memoryCount += 4;
            accessor1.WriteArray(memoryCount, pictureSize, 0, 4);
            memoryCount += 4;
            accessor1.WriteArray(memoryCount, b, 0, b.Length);//memoryCount
            memoryCount += 150000;
            accessor1.Dispose();
            sw.Stop();
            result1 = sw.Elapsed.TotalMilliseconds.ToString();
            label2.Text = result1;
            //mutex.ReleaseMutex();

        }
        private void button1_Click(object sender, EventArgs e)
        {
            ServerCode();



        }
    }
}

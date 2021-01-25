using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.IO.MemoryMappedFiles;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Emgu.CV;
using Emgu.CV.Structure;

namespace openCamera
{
    public partial class Form1 : Form
    {
        int count = 0, temp = 0, count1 = 0;
        bool check = false;
        string result1 = "";
        MemoryStream ms;
        MemoryMappedFile picture_memory = MemoryMappedFile.CreateOrOpen("picture", 150000000);
        int memoryCount = 0, check_first_memory = 0;
        Mutex mutex;
        Capture capture = null;
        byte[] b;
        Image<Bgr, Byte> fram;
        Bitmap bitmap = new Bitmap(1080, 720);
        bool check_camera = false;
        MemoryMappedViewAccessor accessor1;
        private bool mutexCreat=true;

        public Form1()
        {
            InitializeComponent();
            capture = new Capture(0);
            Application.Idle += new EventHandler(Application_idle);
            check_camera = true;
            mutex = new Mutex(true, "test", out mutexCreat);
            mutex.ReleaseMutex();

        }
        public void Application_idle(object sender, EventArgs e)
        {
            fram = capture.QueryFrame();
            System.Diagnostics.Stopwatch sw = new System.Diagnostics.Stopwatch();//引用stopwatch物件

            sw.Reset();//碼表歸零

            sw.Start();//碼表開始計時
            pictureBox1.Image = fram.ToBitmap();

            sw.Stop();
            result1 = sw.Elapsed.TotalMilliseconds.ToString();
            label2.Text = result1;
            //label1.Text = Convert.ToString(fram.Width);
            //label4.Text = Convert.ToString(fram.Height);
            bitmap = fram.ToBitmap();
        }
        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void timer2_Tick(object sender, EventArgs e)
        {

        }

        private void timer1_Tick(object sender, EventArgs e)
        {

            if (check_camera)
            {
                int ttt = 0;
                if (check_first_memory == 0)
                {
                    mutex.WaitOne();
                    firstWriteMemory();
                    mutex.ReleaseMutex();
                    if (count1 == 20)
                    {
                        check_first_memory = 1;
                        memoryCount = 0;
                        count = 0;

                    }
                    count1++;
                }
                else
                {
                    mutex.WaitOne();
                    writeMemory();

                    mutex.ReleaseMutex();

                    if (count == 20)
                    {
                        count = 0;
                        memoryCount = 0;
                    }
                    count++;
                    temp++;
                }





                //label2.Text = Convert.ToString(bytes1.Length);

            }
        }
        public void firstWriteMemory()
        {
            label3.Text = Convert.ToString(temp);
            ms = new MemoryStream();

            bitmap.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
            b = ms.ToArray();
            label5.Text = Convert.ToString(b.Length);

            //picture_memory = MemoryMappedFile.CreateOrOpen("picture", 5000000);  // 建立指定大小的記憶體檔案，會在應用程式退出時自動釋放
            accessor1 = picture_memory.CreateViewAccessor();
            byte[] header = BitConverter.GetBytes(2);
            byte[] pictureSize = BitConverter.GetBytes(b.Length);

            label1.Text = Convert.ToString(header.Length);
            accessor1.WriteArray(memoryCount, b, 0, b.Length);//memoryCount
            //label5.Text = "picture_size" + b.Length;
            memoryCount += 2000000;
            accessor1.WriteArray(memoryCount, pictureSize, 0, pictureSize.Length);


            memoryCount += 40;
            accessor1.WriteArray(memoryCount, header, 0, header.Length);
            //label4.Text = Convert.ToString(pictureSize.Length);
            memoryCount += 40;

            accessor1.Dispose();

        }
        public void writeMemory()
        {
            //bitmap.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
            accessor1 = picture_memory.CreateViewAccessor();
            byte[] Buffer = new byte[4];
            mutex.WaitOne();
            accessor1.ReadArray(memoryCount + 2000040, Buffer, 0, 4);//memoryCount  
            mutex.ReleaseMutex();
            label1.Text = Convert.ToString(BitConverter.ToInt32(Buffer, 0));
            if (BitConverter.ToInt32(Buffer, 0) == 1)
            {
                label3.Text = Convert.ToString(temp);
                ms = new MemoryStream();

                bitmap.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
                b = ms.ToArray();
                label5.Text = Convert.ToString(b.Length);

                //picture_memory = MemoryMappedFile.CreateOrOpen("picture", 5000000);  // 建立指定大小的記憶體檔案，會在應用程式退出時自動釋放
                //accessor1 = picture_memory.CreateViewAccessor();
                byte[] header = BitConverter.GetBytes(2);
                byte[] pictureSize = BitConverter.GetBytes(b.Length);

                label1.Text = Convert.ToString(count);
                mutex.WaitOne();
                accessor1.WriteArray(memoryCount, b, 0, b.Length);//memoryCount
                mutex.ReleaseMutex();
                memoryCount += 2000000;
                mutex.WaitOne();
                accessor1.WriteArray(memoryCount, pictureSize, 0, pictureSize.Length);
                mutex.ReleaseMutex();

                memoryCount += 40;
                mutex.WaitOne();
                accessor1.WriteArray(memoryCount, header, 0, header.Length);
                mutex.ReleaseMutex();
                //label4.Text = Convert.ToString(pictureSize.Length);
                memoryCount += 40;

               

            }
            accessor1.Dispose();

        }
    }
}

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

namespace WindowsFormsApp2
{
    public partial class Form1 : Form
    {
        private static byte[] result = new byte[1024 * 10000];
        private static int myProt = 8888;   //埠

        //int Width = Convert.ToInt32(System.Windows.Forms.SystemInformation.WorkingArea.Width);
        //int Height = Convert.ToInt32(System.Windows.Forms.SystemInformation.WorkingArea.Height);
        int Height = Screen.PrimaryScreen.Bounds.Height;
        int Width = Screen.PrimaryScreen.Bounds.Width;
        //static int Width =Convert.ToInt32(Screen.PrimaryScreen.WorkingArea.Width);
        //static int Height = Convert.ToInt32(Screen.PrimaryScreen.WorkingArea.Height);
        int count = 0;
        
        Socket listener;
        bool check = false, check_save_image = false;

        public Form1()
        {
            InitializeComponent();
            //label5.Text = Width + ":" +Height+"--"+width+":"+height;

        }

        public void ServerCode()
        {

            IPAddress ip = IPAddress.Parse("127.0.0.1");
            listener = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            listener.Bind(new IPEndPoint(ip, myProt));  //繫結IP地址：埠  
            listener.Listen(1);    //設定最多10個排隊連線請求
            label3.Text = "啟動監聽{0}g" + listener.LocalEndPoint.ToString();
            check = true;







        }
        public void sendMSG()
        {
            Socket serverSokcet = listener.Accept();
            byte[] msg = Encoding.ASCII.GetBytes(Convert.ToString(count));
            serverSokcet.Send(msg);
            serverSokcet.Shutdown(SocketShutdown.Both);
            serverSokcet.Close();
        }
        private void ReceiveMessage(object clientSocket)
        {
            Socket myClientSocket = (Socket)clientSocket;
            while (true)
            {
                try
                {
                    //通過clientSocket接收資料
                    int receiveNumber = myClientSocket.Receive(result);
                    label4.Text = "接收客戶端" + myClientSocket.RemoteEndPoint.ToString() + "訊息 " + Encoding.ASCII.GetString(result, 0, receiveNumber);

                }
                catch (Exception ex)
                {
                    //label4.Text ="987";
                    myClientSocket.Shutdown(SocketShutdown.Both);
                    myClientSocket.Close();
                    break;
                }
            }
        }
        private void Form1_Load(object sender, EventArgs e)
        {

        }



        private void timer1_Tick(object sender, EventArgs e)
        {

            if (check)
            {
                Bitmap myImage = new Bitmap(Width, Height);
                Graphics g = Graphics.FromImage(myImage);
                label4.Text = Convert.ToString(count);
                string result1 = "";
                System.Diagnostics.Stopwatch sw = new System.Diagnostics.Stopwatch();//引用stopwatch物件

                sw.Reset();//碼表歸零

                sw.Start();//碼表開始計時
                g.CopyFromScreen(new Point(0, 0), new Point(0, 0), new Size(Width, Height));
                IntPtr dc1 = g.GetHdc();
                g.ReleaseHdc(dc1);
                myImage.Save("C:\\picture\\"+ count + ".jpg");
                
                sendMSG();
                sw.Stop();
                result1 = sw.Elapsed.TotalMilliseconds.ToString();
                label2.Text = result1;
                count++;
                
                myImage.Dispose();
                g.Dispose();
                if (count == 100)
                {
                    count = 0;
                }
            }
            else
            {
                label4.Text = "no ready";
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            ServerCode();
            
            

        }
    }
}

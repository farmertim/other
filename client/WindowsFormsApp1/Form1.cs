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

namespace WindowsFormsApp1
{

    public partial class Form1 : Form
    {
        private byte[] result = new byte[1024 * 10000];
        public static string receivedPath;
        public static string curMsg = "Stopped";
        Bitmap srcBitmap;
        int count = 0;
        string s;
        int receiveLength;
        byte[] bytes;
        bool check_connect = false;
        Socket clientSocket;
        public Form1()
        {
            InitializeComponent();
        }

        public void getPixel(string s, int number)
        {
            Color srcColor;
            srcBitmap = new Bitmap(s);
            int wide = srcBitmap.Width;

            int height = srcBitmap.Height;

            for (int y = 0; y < height - 1; y++)

                for (int x = 0; x < wide - 1; x++)

                {

                    //獲取像素的ＲＧＢ顏色值

                    srcColor = srcBitmap.GetPixel(x, y);

                    //a[number][x][y] = Convert.ToString(srcColor.R) + ":" + Convert.ToString(srcColor.G) + ":" + Convert.ToString(srcColor.B);
                    label1.Text = Convert.ToString(srcColor.R);
                    byte temp = (byte)(srcColor.R * .299 + srcColor.G * .587 + srcColor.B * .114);

                    //設置像素的ＲＧＢ顏色值

                    srcBitmap.SetPixel(x, y, Color.FromArgb(temp, temp, temp));

                }

        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }
        public void clientServer()
        {
            bytes = new byte[1024 * 10000];
            IPAddress ip = IPAddress.Parse("127.0.0.1");
            IPEndPoint remoteEP = new IPEndPoint(ip, 8888);
            clientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            try
            {
                clientSocket.Connect(remoteEP); //配置伺服器IP與埠
                label1.Text = "連線伺服器成功";
                check_connect = true;
                //byte[] msg = Encoding.ASCII.GetBytes("Hellow");

                // Send the data through the socket.    
                //int bytesSent = clientSocket.Send(msg);

                // Receive the response from the remote device.    
                int bytesRec = clientSocket.Receive(bytes);
                count = Convert.ToInt32(Encoding.ASCII.GetString(bytes, 0, bytesRec));
                label3.Text = Convert.ToString(count);
                clientSocket.Shutdown(SocketShutdown.Both);
                clientSocket.Close();
            }
            catch (Exception ex)
            {
                label1.Text = "連線伺服器失敗，請按回車鍵退出！" + ex.ToString();
                return;
            }
        }
        private void button1_Click(object sender, EventArgs e)
        {
            clientServer();
        }
        private void timer1_Tick(object sender, EventArgs e)
        {
            clientServer();
            s = "C:\\picture\\" + count + ".jpg";
            FileStream fs = File.OpenRead(s);

            pictureBox1.Image = Image.FromStream(fs);
            fs.Close();

        }

        private void label1_Click(object sender, EventArgs e)
        {
            label1.Width = 100;
            label1.Height = 100;
        }

        private void button2_Click(object sender, EventArgs e)
        {


        }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.IO.MemoryMappedFiles;
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
        private byte[] result = new byte[1024 * 20000];
        public static string receivedPath;
        public static string curMsg = "Stopped";
        Bitmap srcBitmap;
        int count = 0;
        byte[] bytes;
        Socket clientSocket;
        Image oImage = null;
        MemoryStream ms;
        int memoryCount = 0;
        int memorySize = 0;
        int Data;
        MemoryMappedFile picture_memory;
        public Form1()
        {
            InitializeComponent();
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
                //byte[] msg = Encoding.ASCII.GetBytes("Hellow");

                // Send the data through the socket.    
                //int bytesSent = clientSocket.Send(msg);

                // Receive the response from the remote device.    
                int bytesRec = clientSocket.Receive(bytes);

                if (bytesRec > 0)
                {
                    ms= new MemoryStream(bytes);
                    //stringData = Encoding.UTF8.GetString(bytes);
                    //label3.Text = stringData;
                    //ms.Position = 100;
                    //Image img = Image.FromStream(ms);
                    //oImage = Image.FromStream(ms);
                    //pictureBox1.Image = oImage;
                    
                    MemoryMappedFile mmf1 = MemoryMappedFile.OpenExisting("picture");
                    MemoryMappedViewAccessor accessor1 = mmf1.CreateViewAccessor();
                    //int data = accessor1.ReadInt32(4 * Convert.ToInt32(stringData));
                    //label4.Text = Convert.ToString(data);
                    
                    MemoryMappedFile mmf = MemoryMappedFile.OpenExisting("picture");
                    MemoryMappedViewAccessor accessor = mmf.CreateViewAccessor();

                    byte[] Buffer = new byte[5000000];

                    accessor.ReadArray(memoryCount, Buffer, 0,1);//memoryCount    
                    MemoryStream ms1 = new MemoryStream(Buffer);
                    pictureBox1.Image = Image.FromStream(ms1);
                    accessor.Dispose();




                    
                    ms.Close();
                }
                count++;
                //count = Convert.ToInt32(Encoding.ASCII.GetString(bytes, 0, bytesRec));
                //label3.Text = Convert.ToString(count);
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
            //clientServer();
            label4.Text = Convert.ToString(count);
            //s = "C:\\picture\\" + count + ".jpg";
            //FileStream fs = File.OpenRead(s);

            //pictureBox1.Image = Image.FromStream(fs);
            //fs.Close();

        }

        private void label1_Click(object sender, EventArgs e)
        {
            label1.Width = 100;
            label1.Height = 100;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            
        }

        private void timer2_Tick(object sender, EventArgs e)
        {
            if (checkMemory() == 2)
            {
                writeMemory();
                if (count == 30)
                {
                    memoryCount = 0;
                    count = 0;
                }
                MemoryMappedFile mmf = MemoryMappedFile.OpenExisting("picture");
                MemoryMappedViewAccessor accessor = mmf.CreateViewAccessor();

                byte[] Buffer = new byte[5000000];
                byte[] number = new byte[4];
                memoryCount += 4;
                accessor.ReadArray(memoryCount, number, 0, 4);//memoryCount    
                memoryCount += 4;
                Data = BitConverter.ToInt32(number, 0);
                accessor.ReadArray(memoryCount, Buffer, 0, Data);//memoryCount   
                memoryCount += 150000;
                
                MemoryStream ms1 = new MemoryStream(Buffer);
                pictureBox1.Image = Image.FromStream(ms1);
                accessor.Dispose();
                count++;
            }
        }
        public void writeMemory()
        {
            
           picture_memory = MemoryMappedFile.OpenExisting("picture");  // 建立指定大小的記憶體檔案，會在應用程式退出時自動釋放
            MemoryMappedViewAccessor accessor1 = picture_memory.CreateViewAccessor();
            byte[] header = BitConverter.GetBytes(1);
            accessor1.WriteArray(memoryCount, header, 0, 4);
            accessor1.Dispose();

        }
        public int checkMemory()
        {
            MemoryMappedFile mmf = MemoryMappedFile.OpenExisting("picture");
            MemoryMappedViewAccessor accessor = mmf.CreateViewAccessor();

            byte[] Buffer = new byte[4];

            accessor.ReadArray(memoryCount, Buffer, 0, 4);//memoryCount  
            return BitConverter.ToInt32(Buffer, 0);
        }
    }
}

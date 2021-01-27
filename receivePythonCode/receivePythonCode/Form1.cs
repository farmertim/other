using NetMQ;
using NetMQ.Sockets;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace receivePythonCode
{
    public partial class Form1 : Form
    {
        RequestSocket client;
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            var address = "tcp://127.0.0.1:5555";

            // client 端要使用 RequestSocket 類別
            using (var client = new RequestSocket(address))
            {
                var message = "hello";

                // client 傳送訊息給 server 端
                client.SendFrame(message);
                
                // 最多等待 10 秒，若超過 10 秒仍不回應則略過
                var timeout = TimeSpan.FromSeconds(10);
                client.TryReceiveFrameString(timeout, out string result);
                label1.Text = result;
                //Debug.WriteLine(result);
            }
        }
    }
}

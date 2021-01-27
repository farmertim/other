﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.IO.Pipes;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace pipclient
{
    public partial class Form1 : Form
    {
        StreamReader reader;
        StreamWriter writer;
        bool check = false;
        NamedPipeClientStream client;
        MemoryStream ms;
        byte[] b;
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }
        public void pipClient()
        {

            client = new NamedPipeClientStream("PipeC#");
            client.Connect();
            reader = new StreamReader(client);
            writer = new StreamWriter(client);
           
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (check)
            {
                /*string input = Console.ReadLine();
                writer.WriteLine(input);
                writer.Flush();*/
                //label1.Text = reader.ReadLine();
                //Console.WriteLine(reader.ReadLine());
                b=new byte[2000000];
                int num;
                num= client.Read(b, 0, b.Length);
                if (num > 0)
                {
                    ms = new MemoryStream(b);
                    pictureBox1.Image = Image.FromStream(ms);
                }
                ms.Dispose();

            }
        }   

        private void button1_Click(object sender, EventArgs e)
        {
            pipClient();
            check = true;
        }
    }
}

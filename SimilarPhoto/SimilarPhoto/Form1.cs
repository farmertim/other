using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SimilarPhoto
{
    public partial class Form1 : Form
    {
        int width = 192;
        int height = 128;
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }
        
        private void button1_Click(object sender, EventArgs e)
        {
            aaaa();
        }
        public void GetRGB()
        {
            int count = 0;
            int[] number = new int[1000];
            int[] number1 = new int[1000];
            for (int i = 0; i < number.Length; i++)
            {
                number[i] = 0;
                number1[i] = 0;
            }
            Random r = new Random();
            int a = r.Next(1, 1001);
            label1.Text = Convert.ToString(a);
            int R = 0, G = 0, B = 0, ttt = 0;
            int k = a, range = 300, black = 0, white = 0, tblack = 0, twhite = 0, gray = 0;
            Bitmap bitmap1 = new Bitmap(@"C:\Users\mount\Downloads\image_data 2\image_data\" + k + ".jpg");
            //Bitmap bimap1 = new Bitmap(Thumb(bitmap));
            //bitmap1.MakeTransparent(Color.White);

            pictureBox1.Image = bitmap1;
            for (int i = 0; i < bitmap1.Width; i++)
            {
                for (int j = 0; j < bitmap1.Height; j++)
                {
                    R = bitmap1.GetPixel(i, j).R;
                    G = bitmap1.GetPixel(i, j).G;
                    B = bitmap1.GetPixel(i, j).B;
                    if ((R + G + B) / 3 >= 127)
                    {
                        twhite++;
                    }
                    else
                    {
                        tblack++;
                    }
                }
            }
            string s = "";

            for (k = 1; k <= 1000; k++)
            {
                white = 0; black = 0;
                bitmap1 = new Bitmap(@"C:\Users\mount\Downloads\image_data 2\image_data\" + k + ".jpg");
                //bitmap1 = new Bitmap(Thumb(bitmap));
                bitmap1.MakeTransparent(Color.White);
                for (int i = 0; i < bitmap1.Width; i++)
                {
                    for (int j = 0; j < bitmap1.Height; j++)
                    {

                        R = bitmap1.GetPixel(i, j).R;
                        G = bitmap1.GetPixel(i, j).G;
                        B = bitmap1.GetPixel(i, j).B;
                        if ((R + G + B) / 3 >= 127)
                        {
                            white++;
                        }
                        else
                        {
                            black++;
                        }
                    }
                }
                if (white <= twhite + range && white >= twhite - range && black <= tblack + range && black >= tblack - range)
                {

                    number[k - 1] = Math.Abs(white - range);
                    number1[k - 1] = Math.Abs(black - range);
                    s += k + ":";
                }

            }
            label2.Text = s;
            for (int m = 0; m < number.Length - 1; m++)//排大到小，泡沫排序法
            {
                for (int n = 0; n < number.Length - m - 1; n++)
                {
                    if (number[n] > number[n++])
                    {
                        int temp = number[n];
                        number[n] = number[n++];
                        number[n++] = temp;

                    }
                }
            }
            for (int m = 0; m < number1.Length - 1; m++)//排大到小，泡沫排序法
            {
                for (int n = 0; n < number1.Length - m - 1; n++)
                {
                    if (number[n] > number[n++])
                    {
                        int temp = number1[n];
                        number1[n] = number1[n++];
                        number1[n++] = temp;

                    }
                }
            }
            s = "";
            int bb = 0;
            string s1 = "";
            for (int i = 0; i < 1000; i++)
            {
                if (number[i] != 0)
                {
                    s += i + " : ";
                    s1 += number[i] + ":";
                    bb++;
                }
                if (bb == 10)
                {
                    break;
                }

            }
            label7.Text = s;
            label4.Text = s;
            bb = 0;
            s = "";
            for (int i = 0; i < 1000; i++)
            {
                if (number1[i] != 0)
                {
                    s += i + " : ";
                    bb++;
                }
                if (bb == 10)
                {
                    break;
                }

            }
            label5.Text = s;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            GetRGB();
        }
        public void aaaa()
        {
            int count = 0;
            float[] number = new float[1000];
            float[] number1 = new float[1000];
            int[] gray = new int[256];
            int[] tgray = new int[256];
            for (int i = 0; i < number.Length; i++)
            {
                number[i] = 0;
                number1[i] = 0;

            }
            for (int j = 0; j < 256; j++)
            {
                gray[j] = 0;
            }

            Random r = new Random();
            int a = r.Next(1, 1001);
            label1.Text = Convert.ToString(a);
            int R = 0, G = 0, B = 0, ttt = 0;
            int kk = a, range = 300, black = 0, white = 0, tblack = 0, twhite = 0;
            Bitmap bitmap1 = new Bitmap(@"C:\Users\mount\Downloads\image_data 2\image_data\" + kk + ".jpg");
            //Bitmap bimap1 = new Bitmap(Thumb(bitmap));
            //bitmap1.MakeTransparent(Color.White);

            pictureBox1.Image = bitmap1;
            for (int i = 0; i < bitmap1.Width; i++)
            {
                for (int j = 0; j < bitmap1.Height; j++)
                {
                    R = bitmap1.GetPixel(i, j).R;
                    G = bitmap1.GetPixel(i, j).G;
                    B = bitmap1.GetPixel(i, j).B;
                    tgray[(R + G + B) / 3]++;
                }
            }
            string s = "";

            for (int k = 1; k <= 1000; k++)
            {
                for (int u = 0; u < 256; u++) {
                    gray[u] = 0;
                }
                white = 0; black = 0;
                bitmap1 = new Bitmap(@"C:\Users\mount\Downloads\image_data 2\image_data\" + k + ".jpg");
                //bitmap1 = new Bitmap(Thumb(bitmap));
                bitmap1.MakeTransparent(Color.White);
                for (int i = 0; i < bitmap1.Width; i++)
                {
                    for (int j = 0; j < bitmap1.Height; j++)
                    {

                        R = bitmap1.GetPixel(i, j).R;
                        G = bitmap1.GetPixel(i, j).G;
                        B = bitmap1.GetPixel(i, j).B;
                        gray[(R + G + B) / 3]++;
                    }
                }
                number[k-1]=compare(tgray, gray);
                number1[k - 1] = compare(tgray, gray);

            }
            label2.Text =Convert.ToString(kk) ;
            for (int m = 0; m < number.Length - 1; m++)//排大到小，泡沫排序法
            {
                for (int n = 0; n < number.Length -1; n++)
                {
                    if (number[n] < number[n+1])
                    {
                        float temp = number[n];
                        number[n] = number[n+1];
                        number[n+1] = temp;

                    }
                }
            }
            s = "";
            string ss = "";
            int[] pp = new int[20];
            for (int i = 0; i < 1000; i++) {
                s += number[i] + ":";
                ss += number1[i] + ":";
            }
            label6.Text = s;
            label7.Text = ss;
            s = "";
            for (int i = 0; i < 10; i++) {
                for (int j = 0; j < 1000; j++) {
                    if (number[i] == number1[j]) {
                        s += j+":";
                        break;
                    }
                }
            }
            label6.Text = s;
            

        }
        public float compare(int[] s, int[] t)
        {
            try
            {
                float result = 0F;
                for (int i = 0; i < 256; i++)
                {
                    int abs = Math.Abs(s[i] - t[i]);
                    int max = Math.Max(s[i], t[i]);
                    result += (1 - ((float)abs / (max == 0 ? 1 : max)));
                }
                return (result / 256) * 100;
            }
            catch (Exception exception)
            {
                return 0;
            }
        }




    }
}

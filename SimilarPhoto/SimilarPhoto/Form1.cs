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
        public string GetImageHashCode(Stream stream)
        {
            return GetImageHashCode((Bitmap)Image.FromStream(stream));
        }

        /// <summary> 
        /// 获取图片的Hashcode 
        /// </summary> 
        /// <param name="imageName"></param> 
        /// <returns></returns> 
        public string GetImageHashCode(Bitmap image)
        {


            //  第一步 
            //  将图片缩小到8x8的尺寸，总共64个像素。这一步的作用是去除图片的细节， 
            //  只保留结构、明暗等基本信息，摒弃不同尺寸、比例带来的图片差异。 
            Bitmap bmp = new Bitmap(Thumb(image));
            int[] pixels = new int[width * height];

            //  第二步 
            //  将缩小后的图片，转为64级灰度。也就是说，所有像素点总共只有64种颜色。 
            for (int i = 0; i < width; i++)
            {
                for (int j = 0; j < height; j++)
                {
                    Color color = bmp.GetPixel(i, j);
                    pixels[i * height + j] = RGBToGray(color.ToArgb());
                }
            }

            //  第三步 
            //  计算所有64个像素的灰度平均值。 
            int avgPixel = Average(pixels);

            //  第四步 
            //  将每个像素的灰度，与平均值进行比较。大于或等于平均值，记为1；小于平均值，记为0。 
            int[] comps = new int[width * height];
            for (int i = 0; i < comps.Length; i++)
            {
                if (pixels[i] >= avgPixel)
                {
                    comps[i] = 1;
                }
                else
                {
                    comps[i] = 0;
                }
            }

            //  第五步 
            //  将上一步的比较结果，组合在一起，就构成了一个64位的整数，这就是这张图片的指纹。组合的次序并不重要，只要保证所有图片都采用同样次序就行了。 
            StringBuilder hashCode = new StringBuilder();
            for (int i = 0; i < comps.Length; i += 4)
            {
                int result = comps[i] * (int)Math.Pow(2, 3) + comps[i + 1] * (int)Math.Pow(2, 2) + comps[i + 2] * (int)Math.Pow(2, 1) + comps[i + 2];
                hashCode.Append(BinaryToHex(result));
            }
            bmp.Dispose();
            return hashCode.ToString();
        }
        public int HammingDistance(String sourceHashCode, String hashCode)
        {
            int difference = 0;
            int len = sourceHashCode.Length;

            for (int i = 0; i < len; i++)
            {
                if (sourceHashCode != hashCode)
                {
                    difference++;
                }
            }
            return difference;
        }

        /// <summary> 
        /// 缩放图片 
        /// </summary> 
        /// <param name="imageName"></param> 
        /// <returns></returns> 
        private Image Thumb(Bitmap image)
        {
            return image.GetThumbnailImage(width, height, () => { return false; }, IntPtr.Zero);
        }

        /// <summary> 
        /// 转为64级灰度 
        /// </summary> 
        /// <param name="pixels"></param> 
        /// <returns></returns> 
        private static int RGBToGray(int pixels)
        {
            int _red = (pixels >> 16) & 0xFF;
            int _green = (pixels >> 8) & 0xFF;
            int _blue = (pixels) & 0xFF;
            return (int)(0.5 * _red + 0.5 * _green + 0.5 * _blue);
        }

        /// <summary> 
        /// 计算平均值 
        /// </summary> 
        /// <param name="pixels"></param> 
        /// <returns></returns> 
        private static int Average(int[] pixels)
        {
            float m = 0;
            for (int i = 0; i < pixels.Length; ++i)
            {
                m += pixels[i];
            }
            m = m / pixels.Length;
            return (int)m;
        }

        /// <summary>
        /// 生成16位指纹
        /// </summary>
        /// <param name="binary"></param>
        /// <returns></returns>
        private static char BinaryToHex(int binary)
        {
            char ch = ' ';
            char[] c = { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9', 'a', 'b', 'c', 'd', 'e', 'f' };
            ch = c[binary];
            return ch;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            int check = 1;
            string str = "", str1 = "";
            int count = 0;
            for (int i = 1; i <= 1000; i += 10)
            {
                if (i != check)
                {
                    FileStream SourceStream = File.Open(@"C:\Users\mount\Downloads\image_data 2\image_data\" + check + ".jpg", FileMode.Open);

                    FileStream SourceStream1 = File.Open(@"C:\Users\mount\Downloads\image_data 2\image_data\" + i + ".jpg", FileMode.Open);

                    label1.Text = GetImageHashCode(SourceStream);
                    label2.Text = GetImageHashCode(SourceStream1) + "\n";
                    str1 += GetImageHashCode(SourceStream1) + "\n";
                    if (HammingDistance(GetImageHashCode(SourceStream), GetImageHashCode(SourceStream1)) > 10)
                    {
                        label3.Text = "NO";
                    }
                    else
                    {
                        str += i + ":";
                        count++;
                    }

                    SourceStream.Close();
                    SourceStream1.Close();
                }
            }
            if (count > 0)
            {
                label2.Text = str1;
                label3.Text = str;
            }
            else
            {
                label3.Text = "no";
            }
        }
        public void GetRGB()
        {
            int R = 0, G = 0, B = 0,ttt=0;
            int  k =727,range=900,black=0,white=0,tblack=0,twhite=0,gray=0;
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
                        if ((R + G +B) / 3 >= 127)
                        {
                            white++;
                        }
                        else
                        {
                            black++;
                        }
                    }
                }
                if (white<=twhite+range && white>=twhite-range && black<=tblack+range && black>=tblack-range) {
                    s += k+" : ";
                }

            }
            label7.Text = s;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            GetRGB();
        }
    }
}

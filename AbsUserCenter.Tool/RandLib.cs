using System;
using System.Collections.Generic;
using System.DrawingCore;
using System.Text;

namespace AbsUserCenter.Tool
{
    /// <summary>
    /// 产生随机数、随机字符串等操作
    /// 2010-12-08 ch
    /// </summary>
    public class RandLib
    {
        /// <summary>
        /// 产生指定长度的随机字符串
        /// </summary>
        /// <param name="codeCount"></param>
        /// <returns></returns>
        public static string CreateRandomCode(int codeCount)
        {
            string allChar = "1,2,3,4,5,6,7,8,9,A,B,C,D,E,F,G,H,I,J,K,L,M,N,O,P,Q,R,S,T,U,V,W,X,Y,Z";
            string[] allCharArray = allChar.Split(',');
            string randomCode = "";
            int temp = -1;
            Random rand = new Random();
            for (int i = 0; i < codeCount; i++)
            {
                if (temp != -1)
                {
                    rand = new Random(temp * i * ((int)DateTime.Now.Ticks));
                }

                int t = rand.Next(allCharArray.Length - 1);

                while (temp == t)
                {
                    t = rand.Next(allCharArray.Length - 1);
                }

                temp = t;
                randomCode += allCharArray[t];
            }
            return randomCode;
        }
        /// <summary>
        /// 输出验证码
        /// </summary>
        /// <param name="checkCode"></param>
        public static byte[] CreateImage(string checkCode)
        {
            int iwidth = (int)(checkCode.Length * 15); 
            System.DrawingCore.Bitmap image = new System.DrawingCore.Bitmap(iwidth, 20);
            System.DrawingCore.Graphics g = System.DrawingCore.Graphics.FromImage(image);
            g.Clear(System.DrawingCore.Color.White);
            //定义颜色
            Color[] c = { Color.Black, Color.Red, Color.DarkBlue, Color.Green, Color.Brown, Color.DarkCyan, Color.Purple };
            //定义字体 
            string[] font = { "Times New Roman", "Microsoft Sans Serif", "MS Mincho", "Book Antiqua", "PMingLiU" };
            Random rand = new Random();
            //随机输出噪点
            for (int i = 0; i < 50; i++)
            {
                int x = rand.Next(image.Width);
                int y = rand.Next(image.Height);
                g.DrawRectangle(new Pen(Color.LightGray, 0), x, y, 1, 1);
            }

            //输出不同字体和颜色的验证码字符
            for (int i = 0; i < checkCode.Length; i++)
            {
                int cindex = rand.Next(7);
                int findex = rand.Next(5);

                Font f = new Font(font[findex], 10, FontStyle.Bold);
                Brush b = new SolidBrush(c[cindex]);
                int ii = 4;
                if ((i + 1) % 2 == 0)
                {
                    ii = 2;
                }
                g.DrawString(checkCode.Substring(i, 1), f, b, 3 + (i * 14), ii);
            }
            //画一个边框
            g.DrawRectangle(new Pen(Color.Black, 0), 0, 0, image.Width - 1, image.Height - 1);

            //输出到浏览器
            System.IO.MemoryStream ms = new System.IO.MemoryStream();
            image.Save(ms, System.DrawingCore.Imaging.ImageFormat.Jpeg);
            byte[] buffer = ms.ToArray();
            g.Dispose();
            image.Dispose();

            return buffer;
        }
    }
}

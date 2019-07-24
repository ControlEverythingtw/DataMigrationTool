using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vsan.Common.Safety
{
    /// <summary>
    /// 验证码
    /// </summary>
    public class VerifyerCode
    {
        /// <summary>
        /// 创建验证码的图片
        /// </summary>
        public static MemoryStream Create(string validateCode,int width,int heigth)
        {
            Bitmap image = new Bitmap(width, heigth);
            return Create(validateCode, image);
        }

        /// <summary>
        /// 创建验证码的图片
        /// </summary>
        public static MemoryStream Create2(string validateCode, int width, int heigth)
        {
            Bitmap image = new Bitmap(width, heigth);
            return Create2(validateCode, image);
        }


        /// <summary>
        /// 创建验证码的图片
        /// </summary>
        public static MemoryStream Create2(string validateCode, Bitmap image = null)
        {
            image = image ?? new Bitmap((int)Math.Ceiling((validateCode.Length) * 16.0), 27);
            Graphics g = Graphics.FromImage(image);
            try
            {
                //生成随机生成器
                Random random = new Random();
                //清空图片背景色
                g.Clear(Color.White);

                //颜色随机
                var colorValue = random.Next(0, 200);
                var colorValue2 = random.Next(0, 200);
                var colorValue3 = random.Next(0, 200);
                var colorValue4 = random.Next(100, 255);
                var insertIndex = random.Next(0, validateCode.Length);
                var insertIndex2 = random.Next(0, validateCode.Length);
                //随机加入空格
                validateCode = validateCode.Insert(insertIndex, " ");
                validateCode = validateCode.Insert(insertIndex2, " ");


                #region 画图片的干扰线
                //画图片的干扰线
                for (int i = 0; i < 10; i++)
                {
                    int x1 = random.Next(image.Width);
                    int x2 = random.Next(image.Width);
                    int y1 = random.Next(image.Height);
                    int y2 = random.Next(image.Height);
                    g.DrawLine(new Pen(Color.FromArgb(colorValue4, colorValue2, colorValue4), 0.2f), x1, y1, x2, y2);
                }


                // 画长直线
                if (insertIndex > insertIndex2)
                {
                    g.DrawLine(new Pen(Color.FromArgb(colorValue, colorValue2, colorValue3), 5), 0, random.Next(image.Height), image.Width, random.Next(image.Height));
                }
                else
                {
                    g.DrawBezier(new Pen(Color.FromArgb(colorValue, colorValue2, colorValue3), 3),
                    0, random.Next(image.Height),
                    random.Next(image.Width), random.Next(image.Height),
                    random.Next(image.Width), random.Next(image.Height),
                    image.Width, random.Next(image.Height));
                }

                //画长曲线
                g.DrawBezier(new Pen(Color.FromArgb(colorValue3, colorValue2, colorValue), 3),
                    0, random.Next(image.Height),
                    random.Next(image.Width), random.Next(image.Height),
                    random.Next(image.Width), random.Next(image.Height),
                    image.Width, random.Next(image.Height));

                #endregion



                //画文字

                Font font = new Font("Segoe Script", 65, (FontStyle.Bold | FontStyle.Italic));

                LinearGradientBrush brush = new LinearGradientBrush(new Rectangle(0, 0, image.Width, image.Height),
                Color.FromArgb(colorValue2, colorValue, colorValue3), Color.FromArgb(colorValue, colorValue3, colorValue2), 1.2f, true);
                g.DrawString(validateCode, font, brush, 3, 2);
                
                //画图片的前景干扰点
                for (int i = 0; i < 100; i++)
                {
                    int x = random.Next(image.Width);
                    int y = random.Next(image.Height);
                    image.SetPixel(x, y, Color.FromArgb(random.Next()));
                }
                //画图片的边框线
               // g.DrawRectangle(new Pen(Color.Silver), 0, 0, image.Width - 1, image.Height - 1);
                //保存图片数据
                MemoryStream stream = new MemoryStream();
                image.Save(stream, ImageFormat.Jpeg);
                //输出图片流
                return stream;
            }
            finally
            {
                g.Dispose();
                image.Dispose();
            }

        }



        /// <summary>
        /// 创建验证码的图片
        /// </summary>
        public static MemoryStream Create(string validateCode, Bitmap image = null)
        {
            image = image ?? new Bitmap((int)Math.Ceiling((validateCode.Length) * 16.0), 27);
            Graphics g = Graphics.FromImage(image);
            try
            {
                //生成随机生成器
                Random random = new Random();
                //清空图片背景色
                g.Clear(Color.White);

                //颜色随机
                var colorValue = random.Next(0, 200);
                var colorValue2 = random.Next(0, 200);
                var colorValue3 = random.Next(0, 200);
                var colorValue4 = random.Next(100, 255);
                var insertIndex = random.Next(0, validateCode.Length);
                var insertIndex2 = random.Next(0, validateCode.Length);
                //随机加入空格
                validateCode = validateCode.Insert(insertIndex, " ");
                validateCode = validateCode.Insert(insertIndex2, " ");

                //画图片的干扰线
                //for (int i = 0; i < 5; i++)
                //{
                //    int x1 = random.Next(image.Width);
                //    int x2 = random.Next(image.Width);
                //    int y1 = random.Next(image.Height);
                //    int y2 = random.Next(image.Height);
                //    g.DrawLine(new Pen(Color.FromArgb(colorValue4, colorValue2, colorValue4),0.2f), x1, y1, x2, y2);
                //}
                if (insertIndex > insertIndex2)
                {
                    g.DrawLine(new Pen(Color.FromArgb(colorValue, colorValue2, colorValue3), 1), 0, random.Next(image.Height), image.Width, 0);
                }
                else
                {
                    g.DrawBezier(new Pen(Color.FromArgb(colorValue, colorValue2, colorValue3), 2),
                  0, random.Next(image.Height), random.Next(image.Width), random.Next(image.Height), random.Next(image.Width), random.Next(image.Height), image.Width, 0);
                }
                g.DrawBezier(new Pen(Color.FromArgb(colorValue3, colorValue2, colorValue), 1),
                    0, random.Next(image.Height), random.Next(image.Width), random.Next(image.Height), random.Next(image.Width), random.Next(image.Height), image.Width, 0);

                Font font = new Font("Arial", 13, (FontStyle.Bold | FontStyle.Italic));
                LinearGradientBrush brush = new LinearGradientBrush(new Rectangle(0, 0, image.Width, image.Height),
                Color.FromArgb(colorValue2, colorValue, colorValue3), Color.FromArgb(colorValue, colorValue3, colorValue2), 1.2f, true);
                g.DrawString(validateCode, font, brush, 3, 2);
                //画图片的前景干扰点
                for (int i = 0; i < 10; i++)
                {
                    int x = random.Next(image.Width);
                    int y = random.Next(image.Height);
                    image.SetPixel(x, y, Color.FromArgb(random.Next()));
                }
                //画图片的边框线
                g.DrawRectangle(new Pen(Color.Silver), 0, 0, image.Width - 1, image.Height - 1);
                //保存图片数据
                MemoryStream stream = new MemoryStream();
                image.Save(stream, ImageFormat.Jpeg);
                //输出图片流
                return stream;
            }
            finally
            {
                g.Dispose();
                image.Dispose();
            }

        }
        /// <summary>
        /// 创建验证码的图片
        /// </summary>
        public static byte[] CreateValidateGraphic(string validateCode)
        {
            var stream = Create(validateCode);
            //输出图片流
            return stream.ToArray();
        }
        /// <summary>
        /// 创建验证码的图片
        /// </summary>
        public static byte[] CreateValidateGraphic(string validateCode,int width,int height)
        {
            var stream = Create(validateCode,width,height);
            //输出图片流
            return stream.ToArray();
        }

        /// <summary>
        /// 创建验证码的图片
        /// </summary>
        public static byte[] CreateValidateGraphic2(string validateCode, int width, int height)
        {
            var stream = Create2(validateCode, width, height);
            //输出图片流
            return stream.ToArray();
        }
    }
}

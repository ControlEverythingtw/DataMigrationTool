using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Vsan.Common
{



    /// <summary>
    /// 分享图片帮助类
    /// </summary>
    public class ShareImgHelper
    {
        ///// <summary>
        ///// 加载背景
        ///// </summary>
        //public static Bitmap sImage = null;
        ///// <summary>
        ///// 加载 位置图标
        ///// </summary>
        //public static Bitmap lImage = null;
        /// <summary>
        /// 生成分享职位图片
        /// </summary>
        /// <returns></returns>
        public static MemoryStream GetShareJobImg(string name, string city, string pay, string welfare, string company, string imgUrl)
        {
            Bitmap sImage = new Bitmap(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Content/Img/bg3.png"));
            Bitmap lImage = new Bitmap(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Content/Img/location@3x.png"));
            //初始化画布
            Graphics g = Graphics.FromImage(sImage);
            //画职位图标
            DrawJobIcon(imgUrl, g);
            //画职位名称、职位城市
            DrawName(name, city, g);
            //画城市位置小图标
            DrawLoactionIcon(city, lImage, g);
            //画薪资
            DrawPay(pay, g);
            //画福利
            DrawWelfare(welfare, g);
            //画公司
            DrawCompany(company, g);
            //画图片的边框线
            // g.DrawRectangle(new Pen(Color.Silver), 0, 0, sImage.Width - 1, sImage.Height - 1);
            MemoryStream ms = new MemoryStream();
            sImage.Save(ms, ImageFormat.Png);
            g.Dispose();
            return ms;
        }

        private static void DrawLoactionIcon(string cityName, Bitmap lImage, Graphics g)
        {
            if (!string.IsNullOrWhiteSpace(cityName))
            {
                var p = new Point(204 + cityName.Length * 24 + 19, 129);
                var rec = new Rectangle(p, new Size(16, 19));
                g.DrawImage(lImage, rec);
            }
        }
        /// <summary>
        /// 画职位图像
        /// </summary>
        /// <param name="imgUrl"></param>
        /// <param name="g"></param>
        private static void DrawJobIcon(string imgUrl, Graphics g)
        {
            Bitmap iconBit = GetIconImg(ref imgUrl);
            var p = new Point(85, 81);
            var rec = new Rectangle(p, new Size(70, 70));
            g.DrawImage(iconBit, rec);
            iconBit.Dispose();
        }

        private static void DrawCompany(string company, Graphics g)
        {
            if (string.IsNullOrEmpty(company)) return;
            if (company.Length>20)
            {
                company = company.Remove(19)+ "...";
            }
            Brush brush = new SolidBrush(Color.FromArgb(102, 102, 102));
            Font font = new Font("Arial", 16, FontStyle.Regular, GraphicsUnit.Pixel);
            g.DrawString(company, font, brush, 68, 329);
        }

        private static void DrawWelfare(string welfare, Graphics g)
        {
            if (string.IsNullOrEmpty(welfare)) return;
            if (welfare.Contains("/"))
            {
                var arr = welfare.Split('/');
                var len = arr.Length;
                if (len > 3)
                {
                    welfare = string.Join("/", arr.Take(3))+"/...";
                }
            }
            Brush brush = new SolidBrush(Color.FromArgb(51, 51, 51));
            Font font = new Font("Arial", 22, FontStyle.Bold, GraphicsUnit.Pixel);
            g.DrawString(welfare, font, brush, 68, 256);
        }

        private static void DrawPay(string pay, Graphics g)
        {
            Brush brush = new SolidBrush(Color.FromArgb(249, 80, 73));
            Font font = new Font("Arial", 40, FontStyle.Bold, GraphicsUnit.Pixel);
            g.DrawString(pay, font, brush, 68, 195);
        }

        private static void DrawName(string name, string city, Graphics g)
        {
            if (string.IsNullOrEmpty(name)) return;
            if (name.Length >= 7)
            {
                name = name.Remove(6)+"...";
            }
            Brush brush = new SolidBrush(Color.FromArgb(153, 153, 153));
            Font font = new Font("Arial", 32, FontStyle.Bold, GraphicsUnit.Pixel);
            g.DrawString(name, font, brush, 203, 85);
            font = new Font("Arial", 24, FontStyle.Regular, GraphicsUnit.Pixel);
            g.DrawString(city, font, brush, 204, 126);
        }

        private static Bitmap GetIconImg(ref string imgUrl)
        {
            Bitmap iconBit = null;
            if (!string.IsNullOrWhiteSpace(imgUrl))
            {
                if (!imgUrl.StartsWith("http"))
                {
                    imgUrl = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "App_Data/Upload/", imgUrl);
                    iconBit = new Bitmap(imgUrl);
                }
                else
                {
                    var result = new HttpClient().GetAsync(imgUrl).Result;
                    if (result.StatusCode == System.Net.HttpStatusCode.OK)
                    {
                        iconBit = new Bitmap(result.Content.ReadAsStreamAsync().Result);
                    }
                   
                }
            }
            if(iconBit==null)
            {
                imgUrl = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Content/Img/job_default.png");
                iconBit = new Bitmap(imgUrl);
            }
            return iconBit;
        }

        /// <summary>
        /// 创建
        /// </summary>
        /// <param name="headline"></param>
        /// <param name="fontSize_hl"></param>
        /// <param name="subheading"></param>
        /// <param name="fontSize_sh"></param>
        /// <param name="imgUrl"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <returns></returns>
        public static MemoryStream Create(string headline, int fontSize_hl, string subheading, int fontSize_sh, string imgUrl, int width, int height)
        {
            Bitmap iconBit = null;
            if (!string.IsNullOrWhiteSpace(imgUrl))
            {
                if (!imgUrl.StartsWith("http"))
                {
                    imgUrl = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "App_Data/Upload/", imgUrl);
                    iconBit = new Bitmap(imgUrl);
                }
                else
                {
                    var result = new HttpClient().GetAsync(imgUrl).Result;
                    if (result.StatusCode == System.Net.HttpStatusCode.OK)
                    {
                        iconBit = new Bitmap(result.Content.ReadAsStreamAsync().Result);
                    }
                }
            }
            Bitmap sImage = new Bitmap(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Content/Img/bg3.png"));
            //画布
            Graphics g = Graphics.FromImage(sImage);
            //生成随机生成器
            Random random = new Random();
            //清空图片背景色
            g.Clear(Color.White);
            //颜色随机
            var colorValue = random.Next(0, 200);
            var colorValue2 = random.Next(0, 200);
            var colorValue3 = random.Next(0, 200);
            //画大标题
            {
                Font font = new Font("Arial", fontSize_hl, FontStyle.Bold);
                LinearGradientBrush brush = new LinearGradientBrush(new Rectangle(0, 0, sImage.Width, sImage.Height),
                Color.FromArgb(colorValue2, colorValue, colorValue3), Color.FromArgb(colorValue, colorValue3, colorValue2), 1.2f, true);
                g.DrawString(headline, font, brush, 3, 2);
            }
            //画小标题
            {
                Font font = new Font("Arial", fontSize_sh, FontStyle.Bold);
                LinearGradientBrush brush = new LinearGradientBrush(new Rectangle(0, 0, sImage.Width, sImage.Height),
                Color.FromArgb(colorValue, colorValue2, colorValue3), Color.FromArgb(colorValue3, colorValue, colorValue2), 1.2f, true);
                g.DrawString(subheading, font, brush, 3, 2 + fontSize_hl + 2);
            }
            //把图片画进去
            {
                g.DrawImage(iconBit, new Point(3, 2 + fontSize_hl + 2 + fontSize_sh + 2));
            }
            //画图片的边框线
            g.DrawRectangle(new Pen(Color.Silver), 0, 0, sImage.Width - 1, sImage.Height - 1);

            MemoryStream ms = new MemoryStream();
            sImage.Save(ms, ImageFormat.Jpeg);
            return ms;
        }

        private static ImageCodecInfo GetEncoderInfo(String mimeType)
        {
            int j;
            ImageCodecInfo[] encoders;
            encoders = ImageCodecInfo.GetImageEncoders();
            for (j = 0; j < encoders.Length; ++j)
            {
                if (encoders[j].MimeType == mimeType)
                    return encoders[j];
            }
            return null;
        }

        /// <summary>
        /// 图片压缩(降低质量以减小文件的大小)
        /// </summary>
        /// <param name="srcBitmap">传入的Bitmap对象</param>
        /// <param name="destStream">压缩后的Stream对象</param>
        /// <param name="level">压缩等级，0到100，0 最差质量，100 最佳</param>
        public static void Compress(Bitmap srcBitmap, Stream destStream, long level)
        {
            ImageCodecInfo myImageCodecInfo;
            System.Drawing.Imaging.Encoder myEncoder;
            EncoderParameter myEncoderParameter;
            EncoderParameters myEncoderParameters;

            // Get an ImageCodecInfo object that represents the JPEG codec.
            myImageCodecInfo = GetEncoderInfo("image/jpeg");

            // Create an Encoder object based on the GUID

            // for the Quality parameter category.
            myEncoder = System.Drawing.Imaging.Encoder.Quality;

            // Create an EncoderParameters object.
            // An EncoderParameters object has an array of EncoderParameter
            // objects. In this case, there is only one

            // EncoderParameter object in the array.
            myEncoderParameters = new EncoderParameters(1);

            // Save the bitmap as a JPEG file with 给定的 quality level
            myEncoderParameter = new EncoderParameter(myEncoder, level);
            myEncoderParameters.Param[0] = myEncoderParameter;
            srcBitmap.Save(destStream, myImageCodecInfo, myEncoderParameters);
        }

        /// <summary>
        /// 图片压缩(降低质量以减小文件的大小)
        /// </summary>
        /// <param name="srcBitMap">传入的Bitmap对象</param>
        /// <param name="destFile">压缩后的图片保存路径</param>
        /// <param name="level">压缩等级，0到100，0 最差质量，100 最佳</param>
        public static void Compress(Bitmap srcBitMap, string destFile, long level)
        {
            Stream s = new FileStream(destFile, FileMode.Create);
            Compress(srcBitMap, s, level);
            s.Close();
        }

        #region 图片压缩(亲测有效)
        /// <summary>
        /// 根据指定尺寸得到按比例缩放的尺寸,返回true表示以更改尺寸
        /// </summary>
        /// <param name="picWidth">图片宽度</param>
        /// <param name="picHeight">图片高度</param>
        /// <param name="specifiedWidth">指定宽度</param>
        /// /// <param name="specifiedHeight">指定高度</param>
        /// <returns>返回true表示以更改尺寸</returns>
        private static bool GetPicZoomSize(ref int picWidth, ref int picHeight, int specifiedWidth, int specifiedHeight)
        {
            int sW = 0, sH = 0;
            Boolean isZoomSize = false;
            //按比例缩放
            Size tem_size = new Size(picWidth, picHeight);
            if (tem_size.Width > specifiedWidth || tem_size.Height > specifiedHeight) //将**改成c#中的或者操作符号
            {
                if ((tem_size.Width * specifiedHeight) > (tem_size.Height * specifiedWidth))
                {
                    sW = specifiedWidth;
                    sH = (specifiedWidth * tem_size.Height) / tem_size.Width;
                }
                else
                {
                    sH = specifiedHeight;
                    sW = (tem_size.Width * specifiedHeight) / tem_size.Height;
                }
                isZoomSize = true;
            }
            else
            {
                sW = tem_size.Width;
                sH = tem_size.Height;
            }
            picHeight = sH;
            picWidth = sW;
            return isZoomSize;
        }
        /// <summary>
        /// 无损压缩图片
        /// </summary>
        /// <param name="image">原图片</param>
        /// <param name="dHeight">高度</param>
        /// <param name="dWidth">宽度</param>
        /// <param name="flag">压缩质量 1-100</param>
        /// <returns></returns>

        public static Bitmap GetPicThumbnail(Image image, int dHeight, int dWidth, int flag)
        {
            System.Drawing.Image iSource = image;
            ImageFormat tFormat = iSource.RawFormat;
            int sW = iSource.Width, sH = iSource.Height;

            GetPicZoomSize(ref sW, ref sH, dWidth, dHeight);

            Bitmap ob = new Bitmap(dWidth, dHeight);
            Graphics g = Graphics.FromImage(ob);
            g.Clear(Color.WhiteSmoke);
            g.CompositingQuality = CompositingQuality.HighQuality;
            g.SmoothingMode = SmoothingMode.HighQuality;
            g.InterpolationMode = InterpolationMode.HighQualityBicubic;
            g.DrawImage(iSource, new Rectangle((dWidth - sW) / 2, (dHeight - sH) / 2, sW, sH), 0, 0, iSource.Width, iSource.Height, GraphicsUnit.Pixel);
            g.Dispose();
            //以下代码为保存图片时，设置压缩质量
            EncoderParameters ep = new EncoderParameters();
            long[] qy = new long[1];
            qy[0] = flag;//设置压缩的比例1-100
            EncoderParameter eParam = new EncoderParameter(System.Drawing.Imaging.Encoder.Quality, qy);
            ep.Param[0] = eParam;
            try
            {
                ImageCodecInfo[] arrayICI = ImageCodecInfo.GetImageEncoders();

                ImageCodecInfo jpegICIinfo = null;

                for (int x = 0; x < arrayICI.Length; x++)
                {
                    if (arrayICI[x].FormatDescription.Equals("JPEG"))
                    {
                        jpegICIinfo = arrayICI[x];
                        break;
                    }
                }
                return ob;
            }
            finally
            {
                iSource.Dispose();
            }
        }

        #endregion


    }

}

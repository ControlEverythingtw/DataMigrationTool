using QRCoder;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Vsan.Common
{
    /// <summary>
    /// 二维码工具
    /// </summary>
    public class QRCodeHelper
    {
        /// <summary>
        /// 生成
        /// </summary>
        /// <param name="content">文本内容</param>
        /// <param name="pixelsPerModule">像素</param>
        /// <param name="iconUrl">中间的图标路径</param>
        /// <returns></returns>
        public static MemoryStream Generator(string content, int pixelsPerModule = 5, string iconUrl = null)
        {
            Bitmap iconBit = null;
            Bitmap qrCodeImage = null;
            if ("https://reg.gwyapp.net/upload/NULL".Equals(iconUrl) || string.IsNullOrWhiteSpace(iconUrl))//头像(iconUrl)为空的情况
            {
                iconUrl = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "App_Data/Upload/default.png");
                if (File.Exists(iconUrl))
                {
                    iconBit = new Bitmap(iconUrl);
                }
            }
            else
            {
                if (!iconUrl.StartsWith("http"))//不是远程
                {
                    iconUrl = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "App_Data/Upload/", iconUrl);
                    if (File.Exists(iconUrl))
                    {
                        iconBit = new Bitmap(iconUrl);
                    }
                }
                else
                {
                    try
                    {
                        //去远程下载
                        var result = new HttpClient().GetAsync(iconUrl).Result;
                        if (result.StatusCode == System.Net.HttpStatusCode.OK)
                        {
                            iconBit = new Bitmap(result.Content.ReadAsStreamAsync().Result);
                        }
                    }
                    catch (Exception ex)
                    {
                        Logger.Instance.Error(ex);
                    }
                }
            }
            QRCodeGenerator qrGenerator = new QRCoder.QRCodeGenerator();
            QRCodeData qrCodeData = qrGenerator.CreateQrCode(content, QRCodeGenerator.ECCLevel.Q);
            QRCode qrcode = new QRCode(qrCodeData);
            // qrcode.GetGraphic 方法可参考最下发“补充说明”
            if (iconBit == null)//图标为空
            {
                qrCodeImage = qrcode.GetGraphic(pixelsPerModule);
            }
            else
            {
                qrCodeImage = qrcode.GetGraphic(pixelsPerModule, Color.Black, Color.White, iconBit, 15, 6, false);
            }
            MemoryStream ms = new MemoryStream();
            qrCodeImage.Save(ms, ImageFormat.Jpeg);
            iconBit?.Dispose();
            qrCodeImage?.Dispose();
            return ms;
        }
    }
}

using Vsan.Common;
using Vsan.Common.Cache;
using Vsan.Common.Models;
using Vsan.Common.Safety;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Drawing;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Reflection;
using System.Web;
using System.Web.Http;
using System.Linq;
using Vsan.DataMigration.Models.View;

namespace Vsan.DataMigration.WebApi.Controllers
{
    /// <summary>
    /// 文件操作
    /// </summary>

    public class FileController : ApiController
    {

        /// <summary>
        /// 上传 【Bearer】
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("api/file/uploadAssembly")]
        //[AuthFilter]
        public JsonDataModel<IEnumerable<string>> UploadAssembly()
        {
           var data=SaveFiles();
           return new JsonDataModel<IEnumerable<string>>(ResultCode.Success, "Success!", data);
        }

        private static IEnumerable<string> SaveFiles()
        {
            string root = GetDownloadPath("user1001");
            var files = HttpContext.Current.Request.Files;
            for (int i = 0; i < files.Count; i++)
            {
                var file = files[i];
                var assemblyName = file.FileName;
                var fileFullName = Path.Combine(root, assemblyName);
                file.SaveAs(fileFullName);
                file.InputStream.Close();
                yield return fileFullName;
            }
        }

        /// <summary>
        /// 获取上传过的程序集
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("api/file/assemblys")]
        public JsonDataModel<IEnumerable<string>> GetAssemblys()
        {
            string root = GetDownloadPath("user1001");
            DirectoryInfo directory = new DirectoryInfo(root);
            var files = directory.GetFiles().Select(a => a.Name);

            return new JsonDataModel<IEnumerable<string>>(ResultCode.Success, "Success", files);

        }

        /// <summary>
        /// 获取上传过的程序集
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("api/file/types")]
        public JsonDataModel<IEnumerable<TypeView>> GetTypes(string assemblyName)
        {
            string root = GetDownloadPath("user1001");
            var fileFullName = Path.Combine(root, assemblyName);
            byte[] filedata = File.ReadAllBytes(fileFullName);
            Assembly assembly = Assembly.Load(filedata);
            var types = assembly.GetTypes();
            var typeViews = types.Select(a => new TypeView
            {
                FullName = a.FullName,
                Methods = a.GetMethods().Select(b => new MethodSelectView
                {
                    IsStatic = b.IsStatic,
                    MethodName = b.Name,
                })
            });
           
            assembly = null;

            return new JsonDataModel<IEnumerable<TypeView>>(ResultCode.Success, "Success!", typeViews);

        }

      

        private static string GetDownloadPath(string dirEx)
        {
            var dir = AppDomain.CurrentDomain.BaseDirectory + "App_Data/Upload/" + dirEx;
            if (!Directory.Exists(dir))
            {
                Directory.CreateDirectory(dir);
            }
            return dir;
            //var stram = file.InputStream;
            //var md5Value = md5.GetValue(stram);
            //var suffix = Path.GetExtension(file.FileName);
            //suffix = string.IsNullOrWhiteSpace(suffix) ? ".dll" : suffix;
            //var fileFullName = Path.Combine(root,md5Value+suffix);
        }
    }
}

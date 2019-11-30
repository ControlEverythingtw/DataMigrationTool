using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vsan.Common
{
    /// <summary>
    /// 统一返回Json模型
    /// </summary>
    public class JsonResultModel
    {
        /// <summary>
        /// 无参构造
        /// </summary>
        public JsonResultModel() { }
        /// <summary>
        /// 构造
        /// </summary>
        public JsonResultModel(ResultCode code, string message)
        {
            this.code = code;
            this.message = message;
        }

        /// <summary>
        /// 构造
        /// </summary>
        /// <param name="code"></param>
        /// <param name="message"></param>
        /// <param name="data"></param>
        public JsonResultModel(ResultCode code, string message, object data)
        {
            this.code = code;
            this.message = message;
            this.data = data;
        }

        /// <summary>
        /// 错误码（0表示没有错误）
        /// </summary>
        public ResultCode code { get; set; }
        /// <summary>
        /// 消息
        /// </summary>
        public string message { get; set; } = "Ok";

        /// <summary>
        /// 数据
        /// </summary>
        public object data { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public static JsonResultModel OkResult => new JsonResultModel(ResultCode.Success, "OK");
        /// <summary>
        /// 
        /// </summary>
        public static JsonResultModel ErrorResult => new JsonResultModel(ResultCode.Error, "Error");
        /// <summary>
        /// 
        /// </summary>
        public static JsonResultModel TestOkResult => new JsonResultModel(ResultCode.Success, "Test");
        /// <summary>
        /// 没有找到
        /// </summary>
        public static JsonResultModel NotFind => new JsonResultModel(ResultCode.NotFind, "没有找到此数据");
        /// <summary>
        /// 已经存在了
        /// </summary>
        public static JsonResultModel IsExist  => new JsonResultModel(ResultCode.IsExists, "已经存在了");

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return $"{{\"code\":{code},\"message\":\"{message}\"}}";
        }
        public string ToJson(string msg)
        {
            return $"{{\"code\":{code},\"message\":\"{msg}\"}}";
        }
        public static string ToJson(int code, string msg)
        {
            return $"{{\"code\":{code},\"message\":\"{msg}\"}}";
        }
    }
    /// <summary>
    /// Json数据模型
    /// </summary>
    public class JsonDataModel<T> 
    {
        /// <summary>
        /// 构造
        /// </summary>
        public JsonDataModel()
        {
        }
        /// <summary>
        /// 构造
        /// </summary>
        public JsonDataModel(ResultCode code, string message, T data) 
        {
            this.code = code;
            this.message = message;
            this.data = data;
        }
        /// <summary>
        /// 构造
        /// </summary>
        public JsonDataModel( T data)
        {
            this.code = ResultCode.Success;
            this.message = "Success";
            this.data = data;
        }
        /// <summary>
        /// 错误码（0表示没有错误）
        /// </summary>
        public ResultCode code { get; set; }
        /// <summary>
        /// 消息
        /// </summary>
        public string message { get; set; } = "Ok";
        /// <summary>
        /// 数据
        /// </summary>
        public T data { get; set; }
        /// <summary>
        /// 返回成功的结果
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        public static JsonDataModel<T> OK(T p)
        {
            return new JsonDataModel<T>(ResultCode.Success, "OK", p);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        public static JsonDataModel<T> NotFind()
        {
            return new JsonDataModel<T>(ResultCode.NotFind, "没有找到相关数据", default(T));
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        public static JsonDataModel<T> Bad()
        {
            return new JsonDataModel<T>(ResultCode.Error, "Bad", default(T));
        }
    }
    /// <summary>
    /// Json分页 数据模型
    /// </summary>
    public class JsonPageModel<T> : JsonDataModel<T>
    {
        /// <summary>
        /// 总数
        /// </summary>
        public int total { get; set; }
    }
    /// <summary>
    /// 分页数据结果模型
    /// </summary>
    public class PageResultModel<T> : JsonResultModel
    {
        /// <summary>
        /// 分页数据
        /// </summary>
        public List<T> list { get; set; }

        /// <summary>
        /// 总数
        /// </summary>
        public int total { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="list"></param>
        /// <param name="total"></param>
        public PageResultModel(List<T> list, int total)
        {
            this.list = list;
            this.total = total;
        }

        /// <summary>
        /// 构造
        /// </summary>
        public PageResultModel(ResultCode code, string message, List<T> list) : base(code, message)
        {
            this.list = list;
        }
       
    }

    /// <summary>
    /// 分页数据结果模型
    /// </summary>
    public class PageDataModel<T> 
    {

        /// <summary>
        /// 错误码（0表示没有错误）
        /// </summary>
        public ResultCode code { get; set; }
        /// <summary>
        /// 消息
        /// </summary>
        public string message { get; set; } = "Ok";

        /// <summary>
        /// 分页数据
        /// </summary>
        public PageData<T> data { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public PageDataModel(PageData<T> list)
        {
            this.code = ResultCode.Success;
            this.message = "获取数据成功";
            this.data = list;
        }

        /// <summary>
        /// 构造
        /// </summary>
        public PageDataModel(ResultCode code, string message, PageData<T> list)
        {
            this.code = code;
            this.message = message;
            this.data = list;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="list"></param>
        /// <param name="count"></param>
        /// <param name="index"></param>
        /// <param name="size"></param>
        public PageDataModel(List<T> list, int count, int index, int size)
        {
            this.data = new PageData<T>(list,count,index,size);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="view"></param>
        /// <returns></returns>
        public static PageDataModel<T> OK(PageData<T> view)
        {
            return new PageDataModel<T>(ResultCode.Success, "Success", view);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="list"></param>
        /// <param name="size"></param>
        /// <returns></returns>
        public static PageDataModel<T> OK(List<T> list,int size)
        {
            PageData<T> data = new PageData<T>();
            data.list = list;
            if (list != null&& list.Count()==size)
            {
                data.has_next = true;
            }
            return new PageDataModel<T>(ResultCode.Success, "Success", data);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="resultCode"></param>
        /// <returns></returns>
        public static PageDataModel<T> NoPass(ResultCode resultCode=ResultCode.ArgumentException)
        {
            return new PageDataModel<T>(resultCode, "Argument NoPass", new PageData<T>());
        }
    }
    /// <summary>
    /// 分页数据模型
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class PageData<T> 
    {
        /// <summary>
        /// 
        /// </summary>
        public PageData()
        {
        }
      
        /// <summary>
        /// 
        /// </summary>
        /// <param name="list"></param>
        /// <param name="total"></param>
        public PageData(List<T> list, int total)
        {
            this.list = list;
            this.total = total;
        }
        /// <summary>
        /// 构造，同时计算 has_next
        /// </summary>
        /// <param name="list"></param>
        /// <param name="total"></param>
        /// <param name="index"></param>
        /// <param name="size"></param>
        public PageData(List<T> list, int total,int index,int size)
        {
            this.list = list;
            this.total = total;
            if (total!=0&&list!=null)
            {
                if (size <= 0)//避免除零异常
                {
                    size = 1;
                }
                var pageCount = total % size == 0 ? total / size : (total / size) + 1;
                this.has_next = index < pageCount;
            }
        }


        /// <summary>
        /// 列表
        /// </summary>
        public List<T> list { get; set; }
        /// <summary>
        /// 总数
        /// </summary>
        public int total { get; set; }
        /// <summary>
        /// 是否有下一页
        /// </summary>
        public bool has_next { get; set; }
    }

    /// <summary>
    /// 不分页返回整个集合结构
    /// </summary>
    public class JsonListModel<T>
    {
        /// <summary>
        /// 
        /// </summary>
        public JsonListModel()
        {

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="list"></param>
        public JsonListModel(List<T> list)
        {
            this.data = new ListData<T>() {
                list = list,
                total =list?.Count??0
            };
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="list"></param>
        /// <param name="total"></param>
        public JsonListModel(List<T> list,int total)
        {
            this.code = ResultCode.Success;
            this.message = "Success";
            this.data = new ListData<T>
            {
                list = list,
                total = total
            };
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="code"></param>
        /// <param name="message"></param>
        /// <param name="data"></param>
        public JsonListModel(ResultCode code, string message, ListData<T> data)
        {
            this.code = code;
            this.message = message;
            this.data = data;
        }


        /// <summary>
        /// 错误码
        /// </summary>
        public ResultCode code { get; set; }
        /// <summary>
        /// 消息
        /// </summary>
        public string message { get; set; }
        /// <summary>
        /// 数据
        /// </summary>
        public ListData<T> data { get; set; }


    }

    /// <summary>
    /// 不分页数据模型
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class ListData<T> {

        /// <summary>
        /// 数据集合
        /// </summary>
        public List<T> list { get; set; }
        /// <summary>
        /// 总数
        /// </summary>
        public int total { get; set; }


    }

    /// <summary>
    /// IEnumerable结果模型
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class IEnumerableResult<T>
    {

        /// <summary>
        /// 错误码
        /// </summary>
        public ResultCode code { get; set; }
        /// <summary>
        /// 消息
        /// </summary>
        public string message { get; set; }
        /// <summary>
        /// 数据
        /// </summary>
        public IEnumerableData<T> data { get; set; }


        /// <summary>
        /// 构造
        /// </summary>
        /// <param name="data"></param>
        public IEnumerableResult( IEnumerable<T> data)
        {
            this.code = ResultCode.Success;
            this.message = "获取数据成功";
            this.data = new IEnumerableData<T>(data);
        }

        /// <summary>
        /// 构造
        /// </summary>
        /// <param name="code"></param>
        /// <param name="message"></param>
        /// <param name="data"></param>
        public IEnumerableResult(ResultCode code, string message, IEnumerableData<T> data)
        {
            this.code = code;
            this.message = message;
            this.data = data;
        }
    }
    /// <summary>
    /// 不分页数据模型
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class IEnumerableData<T>
    {
        /// <summary>
        /// 构造
        /// </summary>
        /// <param name="list"></param>
        public IEnumerableData(IEnumerable<T> list)
        {
            this.list = list;
        }

        /// <summary>
        /// 数据集合
        /// </summary>
        public IEnumerable<T> list { get; set; }

    }


    /// <summary>
    /// 返回JSON结果 非泛型
    /// </summary>
    public class JsonResult
    {



        /// <summary>
        /// 
        /// </summary>
        /// <param name="obj"></param>
        public JsonResult(object obj)
        {
            this.data = obj;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="code"></param>
        /// <param name="message"></param>
        public JsonResult(ResultCode code, string message)
        {
            this.code = code;
            this.message = message;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="code"></param>
        /// <param name="message"></param>
        /// <param name="data"></param>
        public JsonResult(ResultCode code, string message, object data)
        {
            this.code = code;
            this.message = message;
            this.data = data;
        }

        public JsonResult()
        {
        }

        public static JsonResult OK()
        {
            return new JsonResult(ResultCode.Success, "Ok");
        }
        public static JsonResult OK(object data)
        {
            return new JsonResult(ResultCode.Success, "Ok", data);
        }

        /// <summary>
        /// 错误码（0表示没有错误）
        /// </summary>
        public ResultCode code { get; set; }
        /// <summary>
        /// 消息
        /// </summary>
        public string message { get; set; } = "Ok";
        /// <summary>
        /// 数据
        /// </summary>
        public object data { get; set; }
        /// <summary>
        /// 未找到相关资源
        /// </summary>
        /// <returns></returns>
        public static JsonResult NotFound()
        {
            return new JsonResult(ResultCode.NotFind, "没有找到相关资源");
        }
        /// <summary>
        /// 未找到相关资源
        /// </summary>
        /// <returns></returns>
        public static JsonDataModel<T> NotFound<T>()
        {
            return new JsonDataModel<T>(ResultCode.NotFind, "没有找到相关资源", default(T));
        }
        /// <summary>
        /// 未找到相关资源
        /// </summary>
        /// <returns></returns>
        public static JsonDataModel<T> DataOK<T>(T data)
        {
            return new JsonDataModel<T>(ResultCode.Success, "获取数据成功", data);
        }

        public static JsonDataModel<T> IsExist<T>()
        {
            return new JsonDataModel<T>(ResultCode.IsExists, "数据已经存在请不要重复调用添加方法",default(T));
        }
    }
  
}

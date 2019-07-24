using System;
using System.Text;

namespace Vsan.Common
{
    /// <summary>
    /// 异常帮助类
    /// </summary>
    public class ExceptionHelper
    {
        /// <summary>
        /// 获取内部异常简单对象
        /// </summary>
        /// <param name="ex"></param>
        /// <param name="inner"></param>
        /// <returns></returns>
        [Obsolete]
        public static SimpleExceptionModel GetSimpleExceptionModel(Exception ex, SimpleExceptionModel inner = null)
        {
            if (inner == null)
            {
                inner = new SimpleExceptionModel
                {
                    Message = ex.Message,
                    StackTrace = ex.StackTrace,
                };
            }
            if (ex.InnerException == null) return inner;

            var model = new SimpleExceptionModel
            {
                Message = ex.Message,
                StackTrace = ex.StackTrace,
            };

            inner.InnerException = model;
            GetSimpleExceptionModel(ex.InnerException, model);

            return inner;
        }

    }

    /// <summary>
    /// 简单的异常对象
    /// </summary>
   
    public class SimpleExceptionModel
    {
        /// <summary>
        /// 消息
        /// </summary>
        public string Message { get; set; }
        /// <summary>
        /// 堆栈
        /// </summary>
        public string StackTrace { get; set; }
        /// <summary>
        /// 内部异常
        /// </summary>
        public SimpleExceptionModel InnerException { get; set; }
    }
}

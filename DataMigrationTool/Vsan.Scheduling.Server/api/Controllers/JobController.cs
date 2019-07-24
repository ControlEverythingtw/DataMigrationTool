using Vsan.Scheduling.Server.api.Models;
using Hangfire;
using System;
using System.Linq.Expressions;
using System.Web.Http;
using Vsan.Scheduling.Server.api.Filter;
using Vsan.Scheduling.Server.api.Helper;
using Hangfire.Logging;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Linq;

namespace Vsan.Scheduling.Server.api.Controllers
{
    /// <summary>
    /// 作业控制器
    /// </summary>
    public class JobController : ApiController
    {
        private static readonly ILog Logger = LogProvider.For<Bootstrap>();
        /// <summary>
        /// 字典缓存
        /// </summary>
        public static ConcurrentDictionary<string, Tuple<string, DateTime>> _Cache=new ConcurrentDictionary<string, Tuple<string, DateTime>>();

        [HttpDelete]
        public JsonResult DeleteJob(string jobId)
        {
            try
            {
               var isOK=BackgroundJob.Delete(jobId);
            }
            catch (Exception ex)
            {
                var msg = "调用Delete api/Job/ 时出现异常 JobController";
                Logger.ErrorException(msg, ex);
                return new JsonResult(1, msg, ex);
            }
            return JsonResult.Ok;
        }

        /// <summary>
        /// 添加Job
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        [HttpPost]
        [AuthFilter]
        public JsonResult PostJob(JobModel param)
        {
            try
            {
                var requestJob = param;
                Expression<Action> methodCall = () => HttpHelper.Send(requestJob.Url, requestJob.Method,
                    requestJob.Param, requestJob.Token, requestJob.ContentType);

                if (param.JobType == JobType.once)
                {
                    var jobId = BackgroundJob.Schedule(methodCall, TimeSpan.FromSeconds(param.Seconds));
                    var key = param.JobId;

                    //移除key一样的任务
                    if (_Cache.TryGetValue(key, out Tuple<string, DateTime> value))
                    {
                        BackgroundJob.Delete(value.Item1);
                        _Cache.TryRemove(key, out value);
                    }

                    //缓存jobId
                    _Cache[key] = new Tuple<string, DateTime>(jobId, DateTime.Now.AddSeconds(param.Seconds));

                    //移除过期缓存
                    var keys = _Cache.Keys.ToArray();
                    foreach (var cacheKey in keys)
                    {
                        if (_Cache[cacheKey].Item2 < DateTime.Now.AddSeconds(-120))
                        {
                            _Cache.TryRemove(cacheKey, out value);
                        }
                    }

                }
                else
                {
                    RecurringJob.AddOrUpdate(requestJob.JobId, methodCall, param.Cron, TimeZoneInfo.Local);
                }

                return new JsonResult()
                {
                    code = 0,
                    message = "添加 HttpRequestJobModel 成功",
                    data = param
                };
            }
            catch (Exception ex)
            {
                var msg = "调用Post api/Job/ 时出现异常 JobController";
                Logger.ErrorException(msg, ex);
                var result = new JsonResult(1, msg, ex.Message);
                return result;
            }
        }
    }
}

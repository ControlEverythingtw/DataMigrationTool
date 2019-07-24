using Newtonsoft.Json;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Vsan.Common.Cache
{
    /// <summary>
    /// 泛型缓存类
    /// (线程安全 、类型安全 、基于内存、高性能、可持久化、可配置)
    /// Time   :2018-3-3
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public  class MeCache<T>
    {
        /// <summary>
        /// 字典缓存
        /// </summary>
        public static ConcurrentDictionary<string, KeyValuePair<T, DateTime>> _Cache;


        /// <summary>
        /// 静态构造
        /// </summary>
        static MeCache()
        {
            #region 加载配置
            _Cache = new ConcurrentDictionary<string, KeyValuePair<T, DateTime>>();
            Statics.CacheConfig = ConfigHelper.Get<CacheConfig>();
            #endregion
            if (!Directory.Exists(Statics.CacheConfig.LoaclDataPath)) Directory.CreateDirectory(Statics.CacheConfig.LoaclDataPath);
            //加载历史数据
            LoadHistoryData(Statics.CacheConfig.LoaclDataPath);
            //启动持久化任务
            StartDataPersistence(Statics.CacheConfig.LoaclDataPath);
            //启动移除过期缓存任务
            StartRemoveExpirationTask();
        }

        /// <summary>
        /// 清空缓存
        /// </summary>
        public static void Clear()
        {
            _Cache.Clear();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static List<string> GetAllKeys()
        {
            return _Cache.Keys.ToList();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static KeyValuePair<T, DateTime> GetValue(string key)
        {
            return _Cache[key];
        }

        /// <summary>
        /// 获取数据(如果缓存中没有就从func中返回)
        /// </summary>
        /// <param name="cacheKey">缓存Key</param>
        /// <param name="func">返回数据的方法</param>
        /// <param name="timeOut">过期超时ms</param>
        /// <returns></returns>
        public static T GetData(string cacheKey, Func<T> func, int timeOut = Statics._Default_TIME_OUT)
        {
            //从缓存中拿
            T data = Get(cacheKey);
            if (data == null)
            {
                //从func拿数据
                data = func.Invoke();
                //缓存十分钟
                Insert(cacheKey, data, timeOut);
            }
            return data;
        }
        /// <summary>
        /// 按条件移除
        /// </summary>
        /// <param name="predicate"></param>
        public static void RemoveByWhere(Func<KeyValuePair<string, KeyValuePair<T, DateTime>>, bool> predicate)
        {
            var keys = _Cache.Where(predicate).Select(a => a.Key);
            foreach (var item in keys)
            {
                 _Cache.TryRemove(item, out KeyValuePair<T, DateTime > data);
            }
        }
        /// <summary>
        /// 按条件更新
        /// </summary>
        /// <param name="predicate"></param>
        /// <param name="t"></param>
        public static void UpdateByWhere(Func<KeyValuePair<string, KeyValuePair<T, DateTime>>, bool> predicate, KeyValuePair<T, DateTime> t)
        {
            var keys = _Cache.Where(predicate).Select(a => a.Key).ToList();
            if (keys!=null&&keys.Count>0)
            {
                foreach (var item in keys)
                {
                    _Cache[item] = t;
                }
            }
        }

        /// <summary>
        /// 插入
        /// </summary>
        /// <param name="key">键</param>
        /// <param name="obj">值</param>
        /// <param name="timeOut">超时时间(单位:毫秒 ;默认:30分)</param>
        /// <returns></returns>
        public static string Insert(string key, T obj, int timeOut = Statics._Default_TIME_OUT)
        {
            AddOrUpdate(key, obj, DateTime.Now.AddMilliseconds(timeOut));
            return key;
        }
        /// <summary>
        /// 插入
        /// </summary>
        /// <param name="key">键</param>
        /// <param name="obj">值</param>
        /// <param name="timeOut">超时时间(单位:毫秒 ;默认:30分)</param>
        /// <returns></returns>
        public static string Insert(string key, T obj, DateTime timeOut)
        {
            AddOrUpdate(key, obj, timeOut);
            return key;
        }
        /// <summary>
        /// 按条件查询
        /// </summary>
        /// <param name="where"></param>
        public static IEnumerable<KeyValuePair<T, DateTime>> QueryByWhere(Func<KeyValuePair<string, KeyValuePair<T, DateTime>>, bool> where)
        {
            var values = _Cache.Where(where).Select(a => a.Value);
            return values;
        }

        /// <summary>
        /// 插入
        /// </summary>
        /// <param name="key">键</param>
        /// <param name="obj">值</param>
        /// <param name="time">超时时间</param>
        /// <returns></returns>
        public static string AddOrUpdate(string key, T obj, DateTime time)
        {
            _Cache.AddOrUpdate(key,
                (k) => { return new KeyValuePair<T, DateTime>(obj, time); },
                (k, v) => { return new KeyValuePair<T, DateTime>(obj, time); });
            return key;
        }


        /// <summary>
        /// 获取
        /// </summary>
        /// <param name="key">键</param>
        /// <returns>值</returns>
        public static T Get(string key)
        {
            if (string.IsNullOrWhiteSpace(key)) throw new ArgumentException("键key,不能为空");
            var kvPair = default(KeyValuePair<T, DateTime>);
            var isExist = _Cache.TryGetValue(key, out kvPair);
            if (!isExist) return default(T);
            if (kvPair.Value <= DateTime.Now)
            {
                _Cache.TryRemove(key, out kvPair);
                return default(T);
            }
            return kvPair.Key;
        }

        /// <summary>
        /// 获取过期时间
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static DateTime GetExpirationTime(string key)
        {
            if (string.IsNullOrWhiteSpace(key)) throw new ArgumentException("键key,不能为空");
            KeyValuePair<T, DateTime> kvPair;
            var isExist = _Cache.TryGetValue(key, out kvPair);
            if (!isExist) return default(DateTime);
            if (kvPair.Value <= DateTime.Now)
            {
                _Cache.TryRemove(key, out kvPair);
                return default(DateTime);
            }
            return kvPair.Value;
        }
        /// <summary>
        /// 尝试获取KeyValuePair
        /// </summary>
        /// <param name="key"></param>
        /// <param name="kvPair"></param>
        /// <returns></returns>
        public static bool TryGetKeyValuePair(string key, out KeyValuePair<T, DateTime> kvPair)
        {
            if (string.IsNullOrWhiteSpace(key)) throw new ArgumentException("键key,不能为空");

            var isExist = _Cache.TryGetValue(key, out kvPair);
            return isExist;
        }

        /// <summary>
        /// 延长过期时间
        /// </summary>
        /// <param name="key">键</param>
        /// <param name="ms">毫秒数</param>
        public static void LengthenExpirationTime(string key, int ms)
        {
            KeyValuePair<T, DateTime> kvPair;
            var isExist = _Cache.TryGetValue(key, out kvPair);
            if (!isExist) return ;
            kvPair.Value.AddMilliseconds(ms);
        }

        /// <summary>
        /// 通过条件获取Key
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        public static string GetKeyByWhere(Func<KeyValuePair<string, KeyValuePair<T, DateTime>>, bool> where)
        {
            var obj = _Cache.FirstOrDefault(where);
            var keyValue = obj.Value;
            if (keyValue.Value > DateTime.Now)
            {
                return obj.Key;
            }
            return null;
        }


        /// <summary>
        /// 移除
        /// </summary>
        /// <param name="key"></param>
        public static void Remove(string key)
        {
            KeyValuePair<T, DateTime> kvPair;
            _Cache.TryRemove(key, out kvPair);
        }


        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="key">键</param>
        /// <param name="t">值</param>
        /// <param name="timeOut">超时时间(单位:毫秒 ;默认:30分)</param>
        public static void Update(string key, T value, int timeOut = Statics._Default_TIME_OUT)
        {
            Update(key, value, DateTime.Now.AddMilliseconds(timeOut));
        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="key">键</param>
        /// <param name="t">值</param>
        /// <param name="timeOut">超时时间(单位:毫秒 ;默认:30分)</param>
        public static void Update(string key, T t, DateTime time)
        {
            _Cache[key] = new KeyValuePair<T, DateTime>(t, time);
        }

        /// <summary>
        /// 加载历史数据
        /// </summary>
        /// <param name="path">路径</param>
        public static void LoadHistoryData(string path)
        {

            var fileName = typeof(T).ToString();
            var filePath = string.Format("{0}\\{1}_MeCache.json", path, fileName);
            if (!File.Exists(filePath)) return;
            try
            {

                var jsonStr = IOHelper.GetStr(filePath);
                var historyData = JsonConvert.DeserializeObject<ConcurrentDictionary<string, KeyValuePair<T, DateTime>>>(jsonStr);
                _Cache = historyData ?? new ConcurrentDictionary<string, KeyValuePair<T, DateTime>>();
            }
            //catch (TypeInitializationException ex)
            //{
            //    IOHelper.SerializedToJsonFile(path + "/log", ex);
            //    File.Delete(filePath);

            //}
            catch (Exception ex)
            {
                IOHelper.SerializedToJsonFile(null, ex, path + "/");
                IOHelper.ReName(filePath, filePath + "_error_data");
                _Cache = new ConcurrentDictionary<string, KeyValuePair<T, DateTime>>();

                //#if DEBUG
                //                throw;
                //#endif
            }
        }

        /// <summary>
        /// 启动持久化任务
        /// </summary>
        /// <param name="path">路径</param>
        public static void StartDataPersistence(string path)
        {
            Task.Run(() =>
            {
                while (Statics.CacheConfig.IsStartPersistence)
                {
                    Thread.Sleep(Statics.CacheConfig.PersistenceInterval);
                    SaveData(path);
                }
            });
        }
        /// <summary>
        /// 持久化
        /// </summary>
        /// <param name="path"></param>
        public static void SaveData(string path)
        {
            path = path ?? Statics.CacheConfig.LoaclDataPath;
            var fileName = typeof(T).ToString();
            var filePath = string.Format("{0}\\{1}_MeCache.json", path, fileName);
            try
            {
                var str = "";

                str = JsonConvert.SerializeObject(_Cache);

                IOHelper.SaveStr(filePath, str);
            }
            catch (Exception ex)
            {
                IOHelper.SerializedToJsonFile(null, ex, path + "/");
#if DEBUG
                throw;
#endif
            }
        }

        /// <summary>
        ///启动定时移除过期缓存 任务
        /// </summary>
        public static void StartRemoveExpirationTask()
        {
            var queue = new Queue<string>();
            Task.Run(() =>
            {
                while (Statics.CacheConfig.IsStartRemove)
                {
                    try
                    {
                        Thread.Sleep(Statics.CacheConfig.RemoveInterval);
                        foreach (var item in _Cache.Keys)
                        {
                            if (_Cache[item].Value < DateTime.Now)
                            {
                                queue.Enqueue(item);
                            }
                        }
                        KeyValuePair<T, DateTime> keyv;
                        for (int i = 0; i < queue.Count; i++)
                        {
                            _Cache.TryRemove(queue.Dequeue(), out keyv);
                        }

                    }
                    catch (Exception ex)
                    {
                        IOHelper.SerializedToJsonFile(null, ex, Statics.CacheConfig.LoaclDataPath + "/");
#if DEBUG
                        throw;
#endif
                    }
                }
            });
        }
        /// <summary>
        /// 获取当前缓存中元素的数量
        /// </summary>
        /// <returns></returns>
        public static int Count()
        {
            return _Cache.Count;
        }

    }
    /// <summary>
    /// 一些静态字段
    /// </summary>
    public static class Statics
    {
        #region 静态字段
        /// <summary>
        /// 默认超时时间(30分钟)
        /// </summary>
        public const int _Default_TIME_OUT = 30 * 60 * 1000;

        /// <summary>
        /// 持久化路径(默认当前程序根目录
        /// </summary>
        public static string LoaclDataPath;

        /// <summary>
        /// 是否开启过期移除任务
        /// </summary>
        public static bool IsStartRemove;

        /// <summary>
        /// 过期移除任务间隔(毫秒)
        /// </summary>
        public static int RemoveInterval;

        /// <summary>
        /// 是否开启持久化任务
        /// </summary>
        public static bool IsStartPersistence;

        /// <summary>
        /// 持久化任务间隔(毫秒)
        /// </summary>
        public static int PersistenceInterval;
        /// <summary>
        /// 
        /// </summary>
        public static CacheConfig CacheConfig = null;
        /// <summary>
        /// 
        /// </summary>
        public static string CacheConfigPath = AppDomain.CurrentDomain.BaseDirectory + "cahce.config.json";
        #endregion
    }
    /// <summary>
    /// 默认缓存配置类
    /// </summary>
    public class CacheConfig
    {

        /// <summary>
        /// 默认超时时间(30分钟)
        /// </summary>
        public const int _Default_TIME_OUT = 30 * 60 * 60 * 1000;

        /// <summary>
        /// 持久化路径(默认当前程序根目录)
        /// </summary>
        public string LoaclDataPath { get; set; } = AppDomain.CurrentDomain.BaseDirectory + "App_Data/Cache";

        /// <summary>
        /// 是否开启过期移除任务
        /// </summary>
        public bool IsStartRemove { get; set; } = true;

        /// <summary>
        /// 过期移除任务间隔(毫秒)
        /// </summary>
        public int RemoveInterval { get; set; } = 600000;

        /// <summary>
        /// 是否开启持久化任务
        /// </summary>
        public bool IsStartPersistence { get; set; } = true;

        /// <summary>
        /// 持久化任务间隔(毫秒)
        /// </summary>
        public int PersistenceInterval { get; set; } = 60000;

    }


}

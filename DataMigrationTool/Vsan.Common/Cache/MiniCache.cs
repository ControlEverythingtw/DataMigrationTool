using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vsan.Common.Cache
{
    /// <summary>
    /// 迷你缓存
    /// </summary>
    public class MiniCache : ICache
    {
        public static ConcurrentDictionary<string, object> Cache { get ; set; }
        /// <summary>
        /// 获取
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public object Get(string key)
        {
            Cache.TryGetValue(key, out object value);
            return value;
        }
        /// <summary>
        /// 移除
        /// </summary>
        /// <param name="key"></param>

        public void Remove(string key)
        {
            Cache.TryRemove(key, out object value);
        }
        /// <summary>
        /// 设值
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>

        public void Set(string key, object value)
        {
            Cache.TryAdd(key,value);
        }
        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public void Update(string key, object value)
        {
            Cache.TryUpdate(key,value,value);
        }
    }
}

using log4net;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace QinSoft.Core.Configure
{
    /// <summary>
    /// 配置器基类
    /// </summary>
    public abstract class Configer : IConfiger
    {
        /// <summary>
        /// 配置缓存
        /// </summary>
        protected IDictionary<string, object> configCache;

        /// <summary>
        /// 配置有效期
        /// </summary>
        protected IDictionary<string, DateTime> configExpire;

        /// <summary>
        /// 有效时间，0：永久
        /// </summary>
        protected ulong expireIn;

        /// <summary>
        /// 锁
        /// </summary>
        protected object lockObj = new object();

        public Configer(ulong expireIn)
        {
            configCache = new Dictionary<string, object>();
            configExpire = new Dictionary<string, DateTime>();

            this.expireIn = expireIn;
        }

        /// <summary>
        /// 存储配置到缓存
        /// </summary>
        public virtual void StoreConfigToCache(string key, object config)
        {
            lock (lockObj)
            {
                configCache[key] = config;
                if (expireIn > 0)
                {
                    configExpire[key] = DateTime.Now.AddSeconds(expireIn);
                }
                else
                {
                    configExpire.Remove(key);
                }
            }
        }

        /// <summary>
        /// 从缓存读取配置
        /// </summary>
        public virtual object GetConfigFromCache(string key)
        {
            lock (lockObj)
            {
                if (configCache.TryGetValue(key, out object config))
                {
                    if (configExpire.TryGetValue(key, out DateTime expire) && expire < DateTime.Now)
                    {
                        RemoveConfigOfCache(key);
                        return null;
                    }
                }
                return config;
            }
        }


        /// <summary>
        /// 从缓存读取配置
        /// </summary>
        public virtual bool RemoveConfigOfCache(string key)
        {
            lock (lockObj)
            {
                configExpire.Remove(key);
                return configCache.Remove(key);
            }
        }

        /// <summary>
        /// 从缓存读取配置
        /// </summary>
        public virtual void ClearConfigOfCache()
        {
            lock (lockObj)
            {
                foreach (object obj in configCache.Values)
                {
                    (obj as IDisposable)?.Dispose();
                }
                configCache.Clear();
                configExpire.Clear();
            }
        }

        /// <summary>
        /// 获取配置
        /// </summary>
        public abstract T Get<T>(string key, ConfigFormat format = ConfigFormat.XML) where T : class;

        /// <summary>
        /// 获取配置
        /// </summary>
        public abstract Task<T> GetAsync<T>(string key, ConfigFormat format = ConfigFormat.XML) where T : class;

        /// <summary>
        /// 移除配置
        /// </summary>
        public virtual bool Remove(string key)
        {
            return RemoveConfigOfCache(key);
        }

        /// <summary>
        ///  释放资源
        /// </summary>
        public virtual void Dispose()
        {
            GC.SuppressFinalize(this);
            ClearConfigOfCache();
        }
    }
}

using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace QinSoft.Core.Configure
{
    /// <summary>
    /// 配置器接口
    /// </summary>
    public interface IConfiger : IDisposable
    {
        /// <summary>
        /// 获取配置
        /// </summary>
        T Get<T>(string key, ConfigFormat format = ConfigFormat.XML) where T : class;

        /// <summary>
        /// 获取配置
        /// </summary>
        Task<T> GetAsync<T>(string key, ConfigFormat format = ConfigFormat.XML) where T : class;
    }
}

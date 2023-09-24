using QinSoft.Core.Data.MongoDB.Core;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace QinSoft.Core.Data.Influx
{
    public interface IInfluxManager : IDisposable
    {
        /// <summary>
        /// 获取Influx客户端
        /// </summary>
        IInflux GetInflux();

        /// <summary>
        /// 获取Influx客户端
        /// </summary>
        Task<IInflux> GetInfluxAsync();

        /// <summary>
        /// 获取Influx客户端
        /// </summary>
        IInflux GetInflux(string name);

        /// <summary>
        /// 获取Influx客户端
        /// </summary>
        Task<IInflux> GetInfluxAsync(string name);
    }
}

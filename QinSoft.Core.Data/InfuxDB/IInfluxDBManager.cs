using InfluxDB.Client;
using QinSoft.Core.Data.InfuxDB.Core;
using QinSoft.Core.Data.MongoDB.Core;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace QinSoft.Core.Data.InfluxDB
{
    public interface IInfluxDBManager : IDisposable
    {
        /// <summary>
        /// 获取Influx客户端
        /// </summary>
        IInfluxClient GetInflux();

        /// <summary>
        /// 获取Influx客户端
        /// </summary>
        Task<IInfluxClient> GetInfluxAsync();

        /// <summary>
        /// 获取Influx客户端
        /// </summary>
        IInfluxClient GetInflux(string name);

        /// <summary>
        /// 获取Influx客户端
        /// </summary>
        Task<IInfluxClient> GetInfluxAsync(string name);
    }
}

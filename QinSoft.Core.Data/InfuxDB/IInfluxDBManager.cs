using InfluxDB.Client;
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
        IInfluxDBClient GetInflux();

        /// <summary>
        /// 获取Influx客户端
        /// </summary>
        Task<IInfluxDBClient> GetInfluxAsync();

        /// <summary>
        /// 获取Influx客户端
        /// </summary>
        IInfluxDBClient GetInflux(string name);

        /// <summary>
        /// 获取Influx客户端
        /// </summary>
        Task<IInfluxDBClient> GetInfluxAsync(string name);
    }
}

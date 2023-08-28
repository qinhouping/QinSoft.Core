using org.apache.zookeeper;
using QinSoft.Core.Data.MongoDB.Core;
using QinSoft.Core.Data.Zookeeper.Core;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace QinSoft.Core.Data.Zookeeper
{
    public interface IZookeeperManager : IDisposable
    {
        /// <summary>
        /// 获取zookeeper客户端
        /// </summary>
        IZookeeper GetZookeeper();

        /// <summary>
        /// 获取zookeeper客户端
        /// </summary>
        Task<IZookeeper> GetZookeeperAsync();

        /// <summary>
        /// 获取zookeeper客户端
        /// </summary>
        IZookeeper GetZookeeper(string name);

        /// <summary>
        /// 获取zookeeper客户端
        /// </summary>
        Task<IZookeeper> GetZookeeperAsync(string name);
    }
}

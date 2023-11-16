using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using org.apache.zookeeper;
using org.apache.zookeeper.data;

namespace QinSoft.Core.Data.Zookeeper.Core
{
    /// <summary>
    /// 增加认证信息
    /// </summary>
    public interface IZookeeper:IDisposable
    {
        /// <summary>
        /// 配置id
        /// </summary>
        string ConfigId { get; }

        /// <summary>
        /// dispose事件
        /// </summary>
        event EventHandler Disposed;

        /// <summary>
        /// 增加认证信息
        /// </summary>
        void AddAuthInfo(string schema, string auth);

        /// <summary>
        /// 创建节点
        /// </summary>
        string Create(string path, string data, CreateMode mode, params ACL[] acls);

        /// <summary>
        /// 创建节点
        /// </summary>
        string Create(string path, string data, CreateMode mode);

        /// <summary>
        /// 创建节点
        /// </summary>
        Task<string> CreateAsync(string path, string data, CreateMode mode, params ACL[] acls);

        /// <summary>
        /// 创建节点
        /// </summary>
        Task<string> CreateAsync(string path, string data, CreateMode mode);

        /// <summary>
        /// 删除节点
        /// </summary>
        void Delete(string path, int version = -1);

        /// <summary>
        /// 删除节点
        /// </summary>
        Task DeleteAsync(string path, int version = -1);

        /// <summary>
        /// 设置节点
        /// </summary>
        Stat SetData(string path, string data, int version = -1);

        /// <summary>
        /// 设置节点
        /// </summary>
        Task<Stat> SetDataAsync(string path, string data, int version = -1);

        /// <summary>
        /// 获取节点
        /// </summary>
        (Stat, string) GetData(string path, Watcher watcher);

        /// <summary>
        /// 获取节点
        /// </summary>
        (Stat, string) GetData(string path);

        /// <summary>
        /// 获取节点
        /// </summary>
        (Stat, string) GetData(string path, bool watch);

        /// <summary>
        /// 获取节点
        /// </summary>
        Task<(Stat, string)> GetDataAsync(string path, Watcher watcher);

        /// <summary>
        /// 获取节点
        /// </summary>
        Task<(Stat, string)> GetDataAsync(string path, bool watch);

        /// <summary>
        /// 获取节点
        /// </summary>
        Task<(Stat, string)> GetDataAsync(string path);

        /// <summary>
        /// 判断是否存在节点
        /// </summary>
        Stat Exists(string path, Watcher watcher);

        /// <summary>
        /// 判断是否存在节点
        /// </summary>
        Stat Exists(string path, bool watch);

        /// <summary>
        /// 判断是否存在节点
        /// </summary>
        Stat Exists(string path);

        /// <summary>
        /// 判断是否存在节点
        /// </summary>
        Task<Stat> ExistsAsync(string path, Watcher watcher);

        /// <summary>
        /// 判断是否存在节点
        /// </summary>
        Task<Stat> ExistsAsync(string path, bool watch);

        /// <summary>
        /// 判断是否存在节点
        /// </summary>
        Task<Stat> ExistsAsync(string path);

        /// <summary>
        /// 设置ACL
        /// </summary>
        Stat SetAcl(string path, params ACL[] acls);

        /// <summary>
        /// 设置ACL
        /// </summary>
        Task<Stat> SetAclAsync(string path, params ACL[] acls);

        /// <summary>
        /// 获取ACL
        /// </summary>
        (Stat, List<ACL>) GetAcl(string path);

        /// <summary>
        /// 获取ACL
        /// </summary>
        Task<(Stat, List<ACL>)> GetAclAsync(string path);

        /// <summary>
        /// 获取子节点
        /// </summary>
        string[] GetChildren(string path, Watcher watcher);

        /// <summary>
        /// 获取子节点
        /// </summary>
        string[] GetChildren(string path, bool watch);

        /// <summary>
        /// 获取子节点
        /// </summary>
        string[] GetChildren(string path);

        /// <summary>
        /// 获取子节点
        /// </summary>
        Task<string[]> GetChildrenAsync(string path, Watcher watcher);

        /// <summary>
        /// 获取子节点
        /// </summary>
        Task<string[]> GetChildrenAsync(string path, bool watch);

        /// <summary>
        /// 获取子节点
        /// </summary>
        Task<string[]> GetChildrenAsync(string path);
    }
}

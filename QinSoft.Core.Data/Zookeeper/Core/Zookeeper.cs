using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using org.apache.zookeeper;
using org.apache.zookeeper.data;
using QinSoft.Core.Common.Utils;

namespace QinSoft.Core.Data.Zookeeper.Core
{
    public class Zookeeper:ZooKeeper,IZookeeper
    {
        protected Encoding Encoding = Encoding.UTF8;

        public event EventHandler Disposed;

        public string ConfigId { get; internal set; }

        public Zookeeper(string connectstring, int sessionTimeout) :base(connectstring, sessionTimeout,null)
        {
            
        }

        public Zookeeper(string connectstring, int sessionTimeout, Encoding encoding) : base(connectstring, sessionTimeout, null)
        {
            this.Encoding = encoding;
        }

        /// <summary>
        /// 增加认证信息
        /// </summary>
        public virtual void AddAuthInfo(string schema, string auth)
        {
            this.addAuthInfo(schema, auth.ToBytes(Encoding));
        }

        /// <summary>
        /// 创建节点
        /// </summary>
        public virtual string Create(string path,string data,CreateMode mode,params ACL[] acls) {
           return this.createAsync(path, data.ToBytes(Encoding), new List<ACL>(acls), mode).Result;
        }

        /// <summary>
        /// 创建节点
        /// </summary>
        public virtual string Create(string path, string data, CreateMode mode)
        {
            return this.createAsync(path, data.ToBytes(Encoding), ZooDefs.Ids.CREATOR_ALL_ACL, mode).Result;
        }

        /// <summary>
        /// 创建节点
        /// </summary>
        public virtual async Task<string> CreateAsync(string path, string data, CreateMode mode, params ACL[] acls)
        {
            return await this.createAsync(path, data.ToBytes(Encoding), new List<ACL>(acls), mode);
        }

        /// <summary>
        /// 创建节点
        /// </summary>
        public virtual async Task<string> CreateAsync(string path, string data, CreateMode mode)
        {
            return await this.createAsync(path, data.ToBytes(Encoding), ZooDefs.Ids.CREATOR_ALL_ACL, mode);
        }

        /// <summary>
        /// 删除节点
        /// </summary>
        public virtual void Delete(string path, int version=-1)
        {
             this.deleteAsync(path, version).Wait();
        }

        /// <summary>
        /// 删除节点
        /// </summary>
        public virtual async Task DeleteAsync(string path, int version = -1)
        {
            await this.deleteAsync(path, version);
        }

        /// <summary>
        /// 设置节点
        /// </summary>
        public virtual Stat SetData(string path,string data, int version = -1)
        {
           return this.setDataAsync(path, data.ToBytes(Encoding), version).Result;
        }

        /// <summary>
        /// 设置节点
        /// </summary>
        public virtual async Task<Stat> SetDataAsync(string path, string data, int version = -1)
        {
           return await this.setDataAsync(path, data.ToBytes(Encoding), version);
        }

        /// <summary>
        /// 获取节点
        /// </summary>
        public virtual (Stat,string) GetData(string path, Watcher watcher)
        {
            DataResult result = this.getDataAsync(path, watcher).Result;
            return (result.Stat, result.Data.ToString(Encoding));
        }

        /// <summary>
        /// 获取节点
        /// </summary>
        public virtual (Stat, string) GetData(string path, bool watch)
        {
            DataResult result = this.getDataAsync(path, watch).Result;
            return (result.Stat, result.Data.ToString(Encoding));
        }

        /// <summary>
        /// 获取节点
        /// </summary>
        public virtual (Stat, string) GetData(string path)
        {
            DataResult result = this.getDataAsync(path, null).Result;
            return (result.Stat, result.Data.ToString(Encoding));
        }

        /// <summary>
        /// 获取节点
        /// </summary>
        public virtual async Task<(Stat, string)> GetDataAsync(string path, Watcher watcher)
        {
            DataResult result = await this.getDataAsync(path, watcher);
            return (result.Stat, result.Data.ToString(Encoding));
        }

        /// <summary>
        /// 获取节点
        /// </summary>
        public virtual async Task<(Stat, string)> GetDataAsync(string path, bool watch)
        {
            DataResult result = await this.getDataAsync(path, watch);
            return (result.Stat, result.Data.ToString(Encoding));
        }


        /// <summary>
        /// 获取节点
        /// </summary>
        public virtual async Task<(Stat, string)> GetDataAsync(string path)
        {
            DataResult result = await this.getDataAsync(path, null);
            return (result.Stat, result.Data.ToString(Encoding));
        }

        /// <summary>
        /// 判断是否存在节点
        /// </summary>
        public virtual Stat Exists(string path, Watcher watcher)
        {
            return this.existsAsync(path, watcher).Result;
        }

        /// <summary>
        /// 判断是否存在节点
        /// </summary>
        public virtual Stat Exists(string path, bool watch)
        {
            return this.existsAsync(path, watch).Result;
        }

        /// <summary>
        /// 判断是否存在节点
        /// </summary>
        public virtual Stat Exists(string path)
        {
            return this.existsAsync(path, null).Result;
        }

        /// <summary>
        /// 判断是否存在节点
        /// </summary>
        public virtual async Task<Stat> ExistsAsync(string path, Watcher watcher)
        {
           return await this.existsAsync(path, watcher);
        }

        /// <summary>
        /// 判断是否存在节点
        /// </summary>
        public virtual async Task<Stat> ExistsAsync(string path, bool watch)
        {
            return await this.existsAsync(path, watch);
        }

        /// <summary>
        /// 判断是否存在节点
        /// </summary>
        public virtual async Task<Stat> ExistsAsync(string path)
        {
            return await this.existsAsync(path, null);
        }

        /// <summary>
        /// 设置ACL
        /// </summary>
        public virtual Stat SetAcl(string path, params ACL[] acls)
        {
           return this.setACLAsync(path, new List<ACL>(acls)).Result;
        }

        /// <summary>
        /// 设置ACL
        /// </summary>
        public virtual async Task<Stat> SetAclAsync(string path, params ACL[] acls)
        {
            return await this.setACLAsync(path, new List<ACL>(acls));
        }

        /// <summary>
        /// 获取ACL
        /// </summary>
        public virtual (Stat, List<ACL>) GetAcl(string path)
        {
            ACLResult result= this.getACLAsync(path).Result;
            return (result.Stat, result.Acls);
        }

        /// <summary>
        /// 获取ACL
        /// </summary>
        public virtual async Task<(Stat, List<ACL>)> GetAclAsync(string path)
        {
            ACLResult result = await this.getACLAsync(path);
            return (result.Stat, result.Acls);
        }

        /// <summary>
        /// 获取子节点
        /// </summary>
        public virtual string[] GetChildren(string path, Watcher watcher)
        {
            ChildrenResult result= this.getChildrenAsync(path, watcher).Result;
            return result.Children.ToArray();
        }

        /// <summary>
        /// 获取子节点
        /// </summary>
        public virtual string[] GetChildren(string path, bool watch)
        {
            ChildrenResult result = this.getChildrenAsync(path, watch).Result;
            return result.Children.ToArray();
        }

        /// <summary>
        /// 获取子节点
        /// </summary>
        public virtual string[] GetChildren(string path)
        {
            ChildrenResult result = this.getChildrenAsync(path, null).Result;
            return result.Children.ToArray();
        }

        /// <summary>
        /// 获取子节点
        /// </summary>
        public virtual async Task<string[]> GetChildrenAsync(string path, Watcher watcher)
        {
            ChildrenResult result = await this.getChildrenAsync(path, watcher);
            return result.Children.ToArray();
        }

        /// <summary>
        /// 获取子节点
        /// </summary>
        public virtual async Task<string[]> GetChildrenAsync(string path, bool watch)
        {
            ChildrenResult result = await this.getChildrenAsync(path, watch);
            return result.Children.ToArray();
        }

        /// <summary>
        /// 获取子节点
        /// </summary>
        public virtual async Task<string[]> GetChildrenAsync(string path)
        {
            ChildrenResult result = await this.getChildrenAsync(path, null);
            return result.Children.ToArray();
        }

        public virtual void Dispose()
        {
            GC.SuppressFinalize(this);
            base.closeAsync().Wait();
            Disposed?.Invoke(this,new EventArgs());
        }
    }
}

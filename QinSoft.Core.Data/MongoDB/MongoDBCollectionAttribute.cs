using QinSoft.Core.Common.Utils;
using System;
using System.Collections.Generic;
using System.Text;

namespace QinSoft.Core.Data.MongoDB
{
    /// <summary>
    /// mongodb集合特性
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public class MongoDBCollectionAttribute : Attribute
    {
        /// <summary>
        /// 上下文名称
        /// </summary>
        public string Name { get; protected set; }

        /// <summary>
        /// 集合名称
        /// </summary>
        public string CollectionName { get; private set; }

        public MongoDBCollectionAttribute(string collectionName)
        {
            ObjectUtils.CheckNull(collectionName, "collectionName");
            this.CollectionName = collectionName;
        }

        public MongoDBCollectionAttribute(string name, string collectionName)
        {
            ObjectUtils.CheckNull(name, "name");
            ObjectUtils.CheckNull(collectionName, "collectionName");
            this.Name = name;
            this.CollectionName = collectionName;
        }
    }
}

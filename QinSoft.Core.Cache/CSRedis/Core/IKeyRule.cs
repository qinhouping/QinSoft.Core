using System;
using System.Collections.Generic;
using System.Text;

namespace QinSoft.Core.Cache.CSRedis.Core
{
    /// <summary>
    /// 定义集群键规则
    /// </summary>
    public interface IKeyRule
    {
        public string[] ConnectionStrings { get; set; }

        public string GetKey(string key);
    }
}

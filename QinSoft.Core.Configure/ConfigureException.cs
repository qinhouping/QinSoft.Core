using System;
using System.Collections.Generic;
using System.Text;

namespace QinSoft.Core.Configure
{
    /// <summary>
    /// 配置异常
    /// </summary>
    public class ConfigureException : Exception
    {
        public ConfigureException(string message) : base(message)
        {

        }
    }
}

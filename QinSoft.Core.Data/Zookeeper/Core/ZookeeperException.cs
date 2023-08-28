using System;
using System.Collections.Generic;
using System.Text;

namespace QinSoft.Core.Data.Zookeeper.Core
{
    public class ZookeeperException : Exception
    {
        public ZookeeperException(string message) : base(message)
        {

        }

        public ZookeeperException(string message, Exception exception) : base(message, exception)
        {

        }
    }
}

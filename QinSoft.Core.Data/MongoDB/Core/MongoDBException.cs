using System;
using System.Collections.Generic;
using System.Text;

namespace QinSoft.Core.Data.MongoDB.Core
{
    public class MongoDBException : Exception
    {
        public MongoDBException(string message) : base(message)
        {

        }

        public MongoDBException(string message, Exception exception) : base(message, exception)
        {

        }
    }
}

using System;
using System.Collections.Generic;
using System.Text;

namespace QinSoft.Core.Data.Elasticsearch
{
    public class ElasticsearchException : Exception
    {
        public ElasticsearchException(string message) : base(message)
        {

        }

        public ElasticsearchException(string message, Exception exception) : base(message, exception)
        {

        }
    }
}

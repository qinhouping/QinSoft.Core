using System;
using System.Collections.Generic;
using System.Text;

namespace QinSoft.Core.Data.Solr
{
    public class SolrException : Exception
    {
        public SolrException(string message) : base(message)
        {

        }

        public SolrException(string message, Exception exception) : base(message, exception)
        {

        }
    }
}

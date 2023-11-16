using InfluxDB.Client;
using InfluxDB.Client.Api.Client;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace QinSoft.Core.Data.InfuxDB.Core
{
    public class InfluxClient : InfluxDBClient, IInfluxClient
    {
        public InfluxClient(string url) : base(url)
        {
        }

        public InfluxClient(string url, string username, string password):base(url,username,password)
        {

        }

        public InfluxClient(string url, string token) : base(url,token)
        {

        }

        public InfluxClient(string url, string username, string password, string database, string retentionPolicy) : base(url, username,password,database,retentionPolicy)
        {

        }

        public InfluxClient(InfluxDBClientOptions options):base(options)
        {

        }
    }
}

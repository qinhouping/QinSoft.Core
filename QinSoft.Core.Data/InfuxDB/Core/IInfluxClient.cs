using InfluxDB.Client;
using System;
using System.Collections.Generic;
using System.Text;

namespace QinSoft.Core.Data.InfuxDB.Core
{
    public interface IInfluxClient:IInfluxDBClient,IDisposable
    {
        void SafeDispose();
    }
}

using SqlSugar;
using System;
using System.Collections.Generic;
using System.Text;

namespace QinSoft.Core.Data.Database
{
    /// <summary>
    /// 数据库异常
    /// </summary>
    public class DatabaseException : Exception
    {
        public DatabaseException(string message) : base(message)
        {

        }

        public DatabaseException(string message, SqlSugarException exception) : base(message, exception)
        {

        }
    }
}

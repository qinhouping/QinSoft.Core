using QinSoft.Core.Common.Utils;
using log4net;
using log4net.Config;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace QinSoft.Core.Test
{
    [TestClass]
    public class LogUnitTest
    {

        [TestMethod]
        public void TestNjLog()
        {
            ILog log = LogManager.GetLogger(typeof(LogUnitTest));
            log.Info("日志测试");

            Thread.Sleep(1000);
        }
    }
}

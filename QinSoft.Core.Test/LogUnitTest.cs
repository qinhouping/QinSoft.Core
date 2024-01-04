using QinSoft.Core.Common.Utils;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Log4Net.AspNetCore.Extensions;
using Microsoft.Extensions.Configuration;

namespace QinSoft.Core.Test
{
    [TestClass]
    public class LogUnitTest
    {

        [TestMethod]
        public void TestLog4Net()
        {
            log4net.Config.XmlConfigurator.Configure(new FileInfo("log4net.config"));
            log4net.ILog log = log4net.LogManager.GetLogger(typeof(LogUnitTest));

            log.Trace(DateTime.Now.ToString(), null);
            log.Debug(DateTime.Now.ToString());
            log.Info(DateTime.Now.ToString());
            log.Warn(DateTime.Now.ToString());
            log.Error(DateTime.Now.ToString(), new Exception("测试异常"));
            log.Critical(DateTime.Now.ToString(), new Exception("测试异常"));
            log.Fatal(DateTime.Now.ToString(), new Exception("测试异常"));
        }

        [TestMethod]
        public void TestNLog()
        {
            NLog.LogManager.Configuration = new NLog.Config.XmlLoggingConfiguration("nlog.config");
            NLog.ILogger log = NLog.LogManager.GetLogger((typeof(LogUnitTest)).FullName);

            log.Trace(DateTime.Now.ToString());
            log.Debug(DateTime.Now.ToString());
            log.Info(DateTime.Now.ToString());
            log.Warn(DateTime.Now.ToString());
            log.Error(DateTime.Now.ToString(), new Exception("测试异常"));
            log.Fatal(DateTime.Now.ToString(), new Exception("测试异常"));
        }

        [TestMethod]
        public void TestSerilLog()
        {
            IConfiguration configuration = (new ConfigurationBuilder()).AddJsonFile("serilog.json").Build();
            Serilog.ILogger log = Serilog.ConfigurationLoggerConfigurationExtensions.Configuration((new Serilog.LoggerConfiguration()).ReadFrom, configuration).CreateLogger();

            log.Debug(DateTime.Now.ToString());
            log.Information(DateTime.Now.ToString());
            log.Warning(DateTime.Now.ToString());
            log.Error(new Exception("测试异常"), (DateTime.Now.ToString()));
            log.Fatal(new Exception("测试异常"), (DateTime.Now.ToString()));
        }

        [TestMethod]
        public void TestLog()
        {
            string cwd = Directory.GetCurrentDirectory();
            ILoggerFactory logFactory = Programe.ServiceProvider.GetService<ILoggerFactory>();
            ILogger logger = logFactory.CreateLogger<LogUnitTest>();
            logger.LogInformation("日志测试");
            Thread.Sleep(1000);
        }
    }
}

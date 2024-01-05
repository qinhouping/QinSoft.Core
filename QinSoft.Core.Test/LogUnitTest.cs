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

            log.Trace(Guid.NewGuid().ToString(), null);
            log.Debug(Guid.NewGuid().ToString());
            log.Info(Guid.NewGuid().ToString());
            log.Warn(Guid.NewGuid().ToString());
            log.Error(Guid.NewGuid().ToString(), new Exception("测试异常"));
            log.Critical(Guid.NewGuid().ToString(), new Exception("测试异常"));
            log.Fatal(Guid.NewGuid().ToString(), new Exception("测试异常"));

            Thread.Sleep(3000);
        }

        [TestMethod]
        public void TestNLog()
        {
            NLog.LogManager.Configuration = new NLog.Config.XmlLoggingConfiguration("nlog.config");
            NLog.ILogger log = NLog.LogManager.GetLogger((typeof(LogUnitTest)).FullName);

            log.Trace(Guid.NewGuid().ToString());
            log.Debug(Guid.NewGuid().ToString());
            log.Info(Guid.NewGuid().ToString());
            log.Warn(Guid.NewGuid().ToString());
            log.Error(Guid.NewGuid().ToString(), new Exception("测试异常"));
            log.Fatal(Guid.NewGuid().ToString(), new Exception("测试异常"));

            Thread.Sleep(3000);
        }

        [TestMethod]
        public void TestSerilLog()
        {
            IConfiguration configuration = (new ConfigurationBuilder()).AddJsonFile("serilog.json").Build();
            Serilog.ILogger log = Serilog.ConfigurationLoggerConfigurationExtensions.Configuration((new Serilog.LoggerConfiguration()).ReadFrom, configuration).CreateLogger();

            log.Debug(Guid.NewGuid().ToString());
            log.Information(Guid.NewGuid().ToString());
            log.Warning(Guid.NewGuid().ToString());
            log.Error(new Exception("测试异常"), (Guid.NewGuid().ToString()));
            log.Fatal(new Exception("测试异常"), (Guid.NewGuid().ToString()));

            Thread.Sleep(3000);
        }

        [TestMethod]
        public void TestLog()
        {
            ILoggerFactory logFactory = Programe.ServiceProvider.GetService<ILoggerFactory>();
            ILogger log = logFactory.CreateLogger<LogUnitTest>();

            while (true)
            {
                log.LogTrace("LogTrace:" + Guid.NewGuid().ToString());
                log.LogDebug("LogDebug:" + Guid.NewGuid().ToString());
                log.LogInformation("LogInformation:" + Guid.NewGuid().ToString());
                log.LogWarning("LogWarning:" + Guid.NewGuid().ToString());
                log.LogError(new Exception("ERROR测试异常"), "LogError:" + (Guid.NewGuid().ToString()));
                log.LogCritical(new Exception("FATAL测试异常"), "LogCritical:" + (Guid.NewGuid().ToString()));

                Thread.Sleep(1000);
            }
        }
    }
}

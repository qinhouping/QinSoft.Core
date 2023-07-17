using QinSoft.Core.Configure;
using QinSoft.Core.Configure.FileConfiger;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml.Serialization;

namespace QinSoft.Core.Test
{
    [TestClass]
    public class ConfigUnitTest
    {
        [TestMethod]
        public void TestFileConfig()
        {
            IConfiger configer = new FileConfiger(new FileConfigerOptions());

            ParamsConfig paramConfig = configer.Get<ParamsConfig>("ParamsConfig");
            Assert.IsNotNull(paramConfig);
        }
    }

    [XmlRoot("params", Namespace = "http://www.qinsoft.com")]
    public class ParamsConfig
    {
        [XmlElement("param")]
        public ParamItem[] Items { get; set; }
    }

    public class ParamItem
    {
        [XmlAttribute("key")]
        public string Key { get; set; }

        [XmlText]
        public string Value { get; set; }
    }
}

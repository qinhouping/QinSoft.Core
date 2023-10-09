using Microsoft.VisualStudio.TestTools.UnitTesting;
using QinSoft.Core.Common.Utils;
using System;
using System.IO;
using System.Threading;
using System.Collections.Generic;
using System.Text;

namespace QinSoft.Core.Test
{
    [TestClass]
    public class CommonUnitTest
    {

        [TestMethod]
        public void TestBaseUtils()
        {
            Assert.AreEqual(2, ObjectUtils.CheckRange(10, 1, 5, () => 2));
        }

        [TestMethod]
        public void TestJsonUtils()
        {
            DateTime dateTime = DateTime.Parse("2022-10-12 10:47:58");
            string timeStr = dateTime.ToJson();
            Assert.AreEqual(timeStr, "\"2022-10-12 10:47:58\"");

            DateTime dateTime2 = timeStr.FromJson<DateTime>();
            Assert.AreEqual(dateTime, dateTime2);
        }

        [TestMethod]
        public void TestXmlUtils()
        {
            DateTime dateTime = DateTime.Parse("2022-10-12 10:47:58");
            string timeStr = dateTime.ToXml();

            DateTime dateTime2 = timeStr.FromXml<DateTime>();
            Assert.AreEqual(dateTime, dateTime2);
        }

        [TestMethod]
        public void TestDateTimeUtils()
        {
            DateTime dateTime = DateTime.Parse("2022-10-12 10:47:58");
            long timeStamp = dateTime.ToTimestamp();
            Assert.AreEqual(timeStamp, 1665542878000);

            DateTime dateTime2 = timeStamp.ToDateTime();
            Assert.AreEqual(dateTime, dateTime2);
        }

        [TestMethod]
        public void TestHttpUtils()
        {
            HttpUtils.DownloadAsync("http://www.baidu.com/");
            HttpUtils.GetAsync<Dictionary<string, string>>("https://qyapi.weixin.qq.com/cgi-bin/gettoken?corpid=ww8f955c057f740831&corpsecret=OVwuHVXpJVuG1b-A7QX0meCNjN9-feC3tgSGqjNJPYM");

            Thread.Sleep(10000);
        }

        [TestMethod]
        public void TestCipherUtils()
        {
            string hash = CipherUtils.MD5("test");
            Assert.AreEqual(hash, "098F6BCD4621D373CADE4E832627B4F6");

            string r = CipherUtils.DESEncrypt("test", "12345678", "12345678");
            Assert.AreEqual(r, "SiQu8unOTKY=");

            string r2 = CipherUtils.DESDecrypt(r, "12345678", "12345678");
            Assert.AreEqual(r2, "test");

            string r3 = CipherUtils.AESEncrypt("test", "FIbsyly50QyjI534", "FIbsyly50QyjI534");
            Assert.AreEqual(r3, "xLQv/Klm38qSPOdANIXQUQ==");

            string r4 = CipherUtils.AESDecrypt(r3, "FIbsyly50QyjI534", "FIbsyly50QyjI534");
            Assert.AreEqual(r4, "test");
        }

        [TestMethod]
        public void TestCancel()
        {
            try
            {
                CancellationTokenSource source = new CancellationTokenSource();
                CancellationToken token = source.Token;
                CancellationTokenRegistration? registration = null;
                registration= token.Register(() =>
                {
                    registration?.Dispose();
                });
                bool r1 = token.CanBeCanceled;
                bool r2 = token.IsCancellationRequested;
                token.ThrowIfCancellationRequested();

                source.Cancel();
                source.Cancel();

                bool r3 = token.CanBeCanceled;
                bool r4 = token.IsCancellationRequested;
                token.ThrowIfCancellationRequested();

            }
            catch(OperationCanceledException e)
            {
                Console.WriteLine(e);
            }
        }

        [TestMethod]
        public void TestProtobuf()
        {
            Person person = new Person
            {
                Name = "test",
                Age = 30,
                Hobbies = { "test" }
            };

            // 序列化
            byte[] bytes = ProtobufUtils.ToProtobuf(person);

            // 反序列化
            Person p = ProtobufUtils.FromProtobuf<Person>(bytes);

            // 使用反序列化后的 Person 实例
            Assert.AreEqual(p.Name, person.Name);
        }
    }
}

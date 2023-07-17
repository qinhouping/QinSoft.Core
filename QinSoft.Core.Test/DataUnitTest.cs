using QinSoft.Core.Configure;
using QinSoft.Core.Configure.FileConfiger;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MySql.Data.MySqlClient;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml.Serialization;
using QinSoft.Core.Common.Utils;
using System.Threading;
using System.Threading.Tasks;
using System.Diagnostics;
using QinSoft.Core.Data.Database;
using System.Data.SqlClient;
using MongoDB.Driver;
using System.Collections.Concurrent;
using System.Text.RegularExpressions;

namespace QinSoft.Core.Test
{
    [TestClass]
    public class DataUnitTest
    {
        public ThreadLocal<string> ThreadVar { get; set; }

        [TestMethod]
        public void TestDatabaseManager()
        {
            IConfiger configer = new FileConfiger(new FileConfigerOptions());
            DatabaseManagerConfig databaseManagerConfig = configer.Get<DatabaseManagerConfig>("DatabaseManager");
            using (IDatabaseManager databaseManager = new DatabaseManager(databaseManagerConfig))
            {
                using (ISqlSugarClient client = databaseManager.GetDatabase("test"))
                {
                    Assert.AreEqual(client.Ado.GetInt("select 1 from dual"), 1);
                    Assert.AreEqual(client.Ado.GetInt("select 1 from dual"), 1);
                    Assert.AreEqual(client.Ado.GetInt("select 1 from dual"), 1);
                    Assert.AreEqual(client.Ado.GetInt("select 1 from dual"), 1);
                }
            }
        }

        [TestMethod]
        public void TestDatabaseRepository()
        {
            ITestTableRepository repository = Programe.ServiceProvider.GetService<ITestTableRepository>();
            Assert.AreEqual(repository.Save(new TestTable()
            {
                id = Guid.NewGuid().ToString(),
                value = DateTime.Now.ToString()
            }), true);
        }

        private string GetEvaluationKey(string value)
        {
            Dictionary<string, string> values = new Dictionary<string, string>()
            {
                {"1","问题" },
                {"100","初期拜访" },
                {"101","中期跟踪" },
                {"102","达成成交" },
                {"103","维护录音" },
                {"104","技术安装" },
                {"105","售后服务" },
                {"106","一通电话成交" },
                {"110","过度身份或能力包装" },
                {"111","明确承诺或暗示收益" },
                {"112","主观判断荐股或指导" },
                {"113","虚拟活动及服务形式" },
                {"114","索要相关联系方式" },
                {"115","谈论母公司高管或母公司股票" },
                {"116","其他规范" },
                {"120","未提示风险" },
                {"121","未明确软件功能" },
                {"122","未明确服务内容" },
                {"123","未明确服务岗位" },
                {"124","留私人联系方式" },
                {"125","泄露客户资料" },
                {"126","违规提供咨询服务" },
                {"127","其他技术服务问题" },
                {"2","优质" },
                {"3","正常" },
                {"4","优质转正常" },
                {"5","复核" }
            };
            foreach (KeyValuePair<string, string> keyValue in values)
            {
                if (keyValue.Value == value)
                {
                    return keyValue.Key;
                }
            }
            return null;
        }

        private string GetSceneKey(string value)
        {
            Dictionary<string, string> values = new Dictionary<string, string>()
            {
                {"1","适宜新人学习" },
                {"10","引导极速版客户升级，过渡性好" },
                {"11","解决问题顾客，反应灵活" },
                {"12","开发续费顾客，思路新颖、用语灵活" },
                {"13","掌握沟通主动权、引导顾客到位" },
                {"14","*高端版续费" },
                {"15","*体验版客户" },
                {"16","*高端版开发" },
                {"17","*低端版开发" },
                {"18","*其他" },
                {"2","一通达成成交" },
                {"3","逼单过程技巧佳" },
                {"4","解答客户顾虑、用语较为灵活" },
                {"5","适合开发投资大师两年版续费顾客" },
                {"6","适合开发年纪较大客户" },
                {"7","适合开发领航版续费" },
                {"8","开始语简练、快速引荐产品" },
                {"9","客户表述软件没有用，人员应对方式较好" }
            };
            foreach (KeyValuePair<string, string> keyValue in values)
            {
                if (keyValue.Value == value)
                {
                    return keyValue.Key;
                }
            }
            return null;
        }

        [TestMethod]
        public void Export()
        {
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            List<string> files = new List<string>() {
                @"C:\Users\Administrator\Desktop\录音评价.json",
                @"C:\Users\Administrator\Desktop\录音评价1.json",
                @"C:\Users\Administrator\Desktop\录音评价2.json",
                @"C:\Users\Administrator\Desktop\录音评价3.json",
                @"C:\Users\Administrator\Desktop\录音评价4.json"};
            List<string> files2 = new List<string>() {
                @"C:\Users\Administrator\Desktop\录音评价.sql",
                @"C:\Users\Administrator\Desktop\录音评价1.sql",
                @"C:\Users\Administrator\Desktop\录音评价2.sql",
                @"C:\Users\Administrator\Desktop\录音评价3.sql",
                @"C:\Users\Administrator\Desktop\录音评价4.sql"};

            for (int i = 0; i < files.Count; i++)
            {
                List<RA> data = new List<RA>();
                List<string> sqls = new List<string>();
                string content = File.ReadAllText(files[i]);
                List<RA> ras = content.FromJson<List<RA>>();
                data.AddRange(ras);
                foreach (RA ra in data)
                {
                    if (ra.comment != null)
                    {
                        ra.comment = Regex.Replace(ra.comment.Trim(), "[\r\n]", " ");
                        ra.comment = (new Comment[] { new Comment() { comment = ra.comment } }).ToJson();
                    }
                    ra.evaluation = GetEvaluationKey(ra.evaluation);
                    ra.sound_type = GetEvaluationKey(ra.sound_type);
                    ra.sound_scene = GetSceneKey(ra.sound_scene);

                    string sql = string.Format("insert into cmpl_jcjh_sound_evaluation(sound_source,sound_id,evaluation,comment,sound_type,sound_scene,evaluator_code,evaluator_name,create_time,remark) values('{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}');", ra.sound_source, ra.sound_id, ra.evaluation, ra.comment, ra.sound_type, ra.sound_scene, ra.evaluator_code, ra.evaluator_name, ra.create_time, "历史数据");
                    sqls.Add(sql.Replace(@"'',", "null,"));
                }
                File.WriteAllText(files2[i], string.Join("\n", sqls), Encoding.GetEncoding("gb2312"));
            }
            //delete from cmpl_jcjh_sound_evaluation where remark = '历史数据';
        }

        [TestMethod]
        public void Export2()
        {
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            List<string> files = new List<string>() {
                @"C:\Users\Administrator\Desktop\线上订单.json"};
            List<string> files2 = new List<string>() {
                @"C:\Users\Administrator\Desktop\线上订单.sql"};

            for (int i = 0; i < files.Count; i++)
            {
                List<string> sqls = new List<string>();
                string content = File.ReadAllText(files[i]);
                List<ORDER> ras = content.FromJson<List<ORDER>>();
                foreach (ORDER ra in ras)
                {
                    string sql = string.Format("insert into temp_order(order_number,start_date,end_date) values('{0}','{1}','{2}');", ra.pay_order_number, ra.start_date, ra.end_date);
                    sqls.Add(sql.Replace(@"'',", "null,"));
                    File.WriteAllText(files2[i], string.Join("\n", sqls), Encoding.GetEncoding("gb2312"));
                }
                File.WriteAllText(files2[i], string.Join("\n", sqls), Encoding.GetEncoding("gb2312"));
            }
            //delete from cmpl_jcjh_sound_evaluation where remark = '历史数据';
        }
    }

    class ORDER
    {
        public string pay_order_number { get; set; }
        public DateTime start_date { get; set; }

        public DateTime end_date { get; set; }
    }

    class RA
    {
        public string id { get; set; }

        public int sound_source { get; set; }

        public string sound_id { get; set; }

        public string evaluation { get; set; }

        public string comment { get; set; }

        public string sound_type { get; set; }

        public string sound_scene { get; set; }

        public string evaluator_code { get; set; }

        public string evaluator_name { get; set; }

        public DateTime create_time { get; set; }
    }

    class Comment
    {
        public string comment { get; set; }
        public string commentCode { get; set; } = "历史数据";
        public string commentName { get; set; } = "历史数据";
        public DateTime commentTime { get; set; } = DateTime.Now;
    }

    [SugarTable("t")]
    public class TestTable
    {
        [SugarColumn(IsPrimaryKey = true)]
        public string id { get; set; }

        public string value { get; set; }
    }

    public interface ITestTableRepository : IDatabaseRepository<TestTable>
    {

    }

    public class TestTableRepository : DatabaseRepository<TestTable>, ITestTableRepository
    {
    }

    public class Country : Dictionary<string, string>
    {

    }
}

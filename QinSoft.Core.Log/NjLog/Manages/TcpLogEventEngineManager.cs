using System;
using System.Collections.Generic;
using System.Net;

namespace QinSoft.Core.Log.NjLog.Manages
{
	// Token: 0x02000006 RID: 6
	public class TcpLogEventEngineManager
	{
		// Token: 0x0600002C RID: 44 RVA: 0x000029D0 File Offset: 0x00000BD0
		private TcpLogEventEngineManager()
		{
		}

		// Token: 0x0600002D RID: 45 RVA: 0x000029DC File Offset: 0x00000BDC
		public static TcpLogEventEngineManager Instance()
		{
			bool flag = TcpLogEventEngineManager.singleton == null;
			if (flag)
			{
				object obj = TcpLogEventEngineManager.obj;
				lock (obj)
				{
					bool flag3 = TcpLogEventEngineManager.singleton == null;
					if (flag3)
					{
						TcpLogEventEngineManager.singleton = new TcpLogEventEngineManager();
					}
				}
			}
			return TcpLogEventEngineManager.singleton;
		}

		// Token: 0x0600002E RID: 46 RVA: 0x00002A4C File Offset: 0x00000C4C
		public TcpLogEventEngine StartEngine(IPAddress ip, int port, string appenderId, int cacheSize, int connTime = 15)
		{
			bool flag = ip == null;
			if (flag)
			{
				throw new Exception("ip值出错");
			}
			bool flag2 = port <= 0;
			if (flag2)
			{
				throw new Exception(string.Format("端口值出错 port:{0}", port));
			}
			string text = string.Format("{0}_{1}", ip.ToString(), port);
			bool flag3 = TcpLogEventEngineManager.engines.ContainsKey(text);
			TcpLogEventEngine tcpLogEventEngine;
			if (flag3)
			{
				tcpLogEventEngine = TcpLogEventEngineManager.engines[text];
				object syncRoot = tcpLogEventEngine.appenderList.SyncRoot;
				lock (syncRoot)
				{
					bool flag5 = !tcpLogEventEngine.appenderList.Contains(appenderId);
					if (flag5)
					{
						tcpLogEventEngine.appenderList.Add(appenderId);
					}
				}
			}
			else
			{
				tcpLogEventEngine = this.CreateEngine(ip, port, appenderId, cacheSize, connTime);
			}
			Console.WriteLine(string.Concat(new string[]
			{
				"创建完 key:",
				text,
				" appenderId:",
				appenderId,
				"引擎"
			}));
			Console.WriteLine(string.Format("创建完当前引擎集合数:{0}", TcpLogEventEngineManager.engines.Count));
			Console.WriteLine(string.Format("创建完当前引擎key:{0} Appender数量:{1}", tcpLogEventEngine.Key, tcpLogEventEngine.appenderList.Count));
			return tcpLogEventEngine;
		}

		// Token: 0x0600002F RID: 47 RVA: 0x00002BB0 File Offset: 0x00000DB0
		private TcpLogEventEngine CreateEngine(IPAddress ip, int port, string appenderId, int cacheSize, int connTime)
		{
			string text = string.Format("{0}_{1}", ip.ToString(), port);
			TcpLogEventEngine tcpLogEventEngine = new TcpLogEventEngine(ip, port, cacheSize, connTime);
			object syncRoot = tcpLogEventEngine.appenderList.SyncRoot;
			lock (syncRoot)
			{
				Console.WriteLine(string.Concat(new string[]
				{
					"正在创建key:",
					text,
					" appenderId:",
					appenderId,
					"引擎"
				}));
				Console.WriteLine(string.Format("当前引擎数:{0}", TcpLogEventEngineManager.engines.Count));
				Console.WriteLine(string.Format("当前引擎key:{0} Appender数量:{1}", text, tcpLogEventEngine.appenderList.Count));
				tcpLogEventEngine.appenderList.Add(appenderId);
			}
			tcpLogEventEngine.EnginStart();
			TcpLogEventEngineManager.engines.Add(text, tcpLogEventEngine);
			return tcpLogEventEngine;
		}

		// Token: 0x06000030 RID: 48 RVA: 0x00002CB0 File Offset: 0x00000EB0
		public void RemoveEngine(string key, string appenderId)
		{
			Console.WriteLine(string.Concat(new string[]
			{
				"正在回收key:",
				key,
				" appenderId:",
				appenderId,
				"引擎"
			}));
			bool flag = TcpLogEventEngineManager.engines == null;
			if (!flag)
			{
				Console.WriteLine(string.Format("当前引擎数:{0}", TcpLogEventEngineManager.engines.Count));
				bool flag2 = !TcpLogEventEngineManager.engines.ContainsKey(key);
				if (!flag2)
				{
					Console.WriteLine(string.Format("当前引擎key:{0} Appender数量:{1}", key, TcpLogEventEngineManager.engines[key].appenderList.Count));
					bool flag3 = !TcpLogEventEngineManager.engines[key].appenderList.Contains(appenderId);
					if (!flag3)
					{
						Console.WriteLine("当前引擎key:" + key + " 找到AppenderId:" + appenderId);
						object syncRoot = TcpLogEventEngineManager.engines[key].appenderList.SyncRoot;
						lock (syncRoot)
						{
							TcpLogEventEngineManager.engines[key].appenderList.Remove(appenderId);
							Console.WriteLine(string.Format("当前引擎key:{0} 移除AppenderId:{1}成功,appenders 数量：{2}", key, appenderId, TcpLogEventEngineManager.engines[key].appenderList.Count));
						}
					}
				}
			}
		}

		// Token: 0x06000031 RID: 49 RVA: 0x00002E20 File Offset: 0x00001020
		public void RemoveEngine(IPAddress ip, int port, string appenderId)
		{
			string key = string.Format("{0}_{1}", ip.ToString(), port);
			this.RemoveEngine(key, appenderId);
		}

		// Token: 0x04000024 RID: 36
		private static TcpLogEventEngineManager singleton = null;

		// Token: 0x04000025 RID: 37
		private static readonly object obj = new object();

		// Token: 0x04000026 RID: 38
		private static Dictionary<string, TcpLogEventEngine> engines = new Dictionary<string, TcpLogEventEngine>();
	}
}

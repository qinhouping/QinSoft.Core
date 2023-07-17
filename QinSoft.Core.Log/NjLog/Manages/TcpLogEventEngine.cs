using System;
using System.Collections;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;
using log4net.Util;

namespace QinSoft.Core.Log.NjLog.Manages
{
	// Token: 0x02000005 RID: 5
	public class TcpLogEventEngine
	{
		// Token: 0x1700000D RID: 13
		// (get) Token: 0x0600001F RID: 31 RVA: 0x000023E8 File Offset: 0x000005E8
		public string Key
		{
			get
			{
				return this.key;
			}
		}

		// Token: 0x06000020 RID: 32 RVA: 0x00002400 File Offset: 0x00000600
		public TcpLogEventEngine(IPAddress ip, int port, int memorySize = 2048, int connTime = 15)
		{
			bool flag = ip == null;
			if (flag)
			{
				throw new Exception("ip值出错");
			}
			this.remoteAddress = ip;
			this.remotePort = port;
			this.remoteEndPoint = new IPEndPoint(ip, port);
			this.connTime = connTime;
			this.memorySize = memorySize;
			this.eventsList = new ArrayList(memorySize);
			this.key = string.Format("{0}_{1}", ip.ToString(), port);
		}

		// Token: 0x06000021 RID: 33 RVA: 0x0000249B File Offset: 0x0000069B
		public void EnginStart()
		{
			Task.Run(delegate ()
			{
				this.EnginConnect();
			});
			this.EnginRetryConnectTaskStart();
			this.SendCacheLogTaskStart();
		}

		// Token: 0x06000022 RID: 34 RVA: 0x000024C0 File Offset: 0x000006C0
		private void EnginConnect()
		{
			try
			{
				object obj = this.locker;
				lock (obj)
				{
					bool flag2 = this.client == null;
					if (flag2)
					{
						this.client = new TcpClient();
					}
					this.client.Connect(this.remoteEndPoint);
					this.ntwStream = this.client.GetStream();
				}
			}
			catch (Exception ex)
			{
				LogLog.Error(TcpLogEventEngine.declaringType, "Failed to create TcpClient " + this.remoteEndPoint.ToString(), ex);
				bool flag3 = this.ntwStream != null;
				if (flag3)
				{
					this.ntwStream.Close();
				}
				bool flag4 = this.client != null;
				if (flag4)
				{
					this.client.Close();
				}
				this.ntwStream = null;
				this.client = null;
			}
		}

		// Token: 0x06000023 RID: 35 RVA: 0x000025B8 File Offset: 0x000007B8
		protected void SendCacheLogTaskStart()
		{
			bool flag = this.memorySize <= 0;
			if (!flag)
			{
				Task.Run(delegate ()
				{
					for (; ; )
					{
						Thread.Sleep(500);
						try
						{
							bool flag2 = this.client == null;
							if (!flag2)
							{
								bool flag3 = !this.client.Connected;
								if (!flag3)
								{
									bool flag4 = this.eventsList == null || this.eventsList.Count <= 0;
									if (!flag4)
									{
										byte[][] bytes = this.PopAllEvents();
										this.Send(bytes);
									}
								}
							}
						}
						catch (Exception ex)
						{
							LogLog.Error(TcpLogEventEngine.declaringType, string.Concat(new object[]
							{
								"Unable to send logging event to remote host ",
								this.remoteAddress.ToString(),
								" on port ",
								this.remotePort,
								"."
							}), ex);
						}
					}
				});
			}
		}

		// Token: 0x06000024 RID: 36 RVA: 0x000025EB File Offset: 0x000007EB
		protected void EnginRetryConnectTaskStart()
		{
			Task.Run(delegate ()
			{
				for (; ; )
				{
					Thread.Sleep(this.connTime * 1000);
					try
					{
						bool flag = this.client == null;
						if (flag)
						{
							this.EnginConnect();
						}
						else
						{
							bool flag2 = !this.client.Connected;
							if (flag2)
							{
								this.EnginConnect();
							}
						}
					}
					catch (Exception ex)
					{
						LogLog.Error(TcpLogEventEngine.declaringType, string.Concat(new string[]
						{
							"Reconn Unable to send logging event to remote host ",
							this.remoteAddress.ToString(),
							" on port ",
							this.remoteEndPoint.ToString(),
							"."
						}), ex);
					}
				}
			});
		}

		// Token: 0x06000025 RID: 37 RVA: 0x00002600 File Offset: 0x00000800
		public bool Send(byte[] bytes)
		{
			bool result;
			try
			{
				bool flag = this.client == null || !this.client.Connected;
				if (flag)
				{
					result = false;
				}
				else
				{
					this.ntwStream.Write(bytes, 0, bytes.Length);
					result = true;
				}
			}
			catch (Exception ex)
			{
				LogLog.Error(TcpLogEventEngine.declaringType, string.Concat(new string[]
				{
					"Reconn Unable to send logging event to remote host ",
					this.remoteAddress.ToString(),
					" on port ",
					this.remoteEndPoint.ToString(),
					"."
				}), ex);
				result = false;
			}
			return result;
		}

		// Token: 0x06000026 RID: 38 RVA: 0x000026A8 File Offset: 0x000008A8
		public bool Send(byte[][] bytes)
		{
			bool result;
			try
			{
				bool flag = this.client == null;
				if (flag)
				{
					result = false;
				}
				else
				{
					bool flag2 = !this.client.Connected;
					if (flag2)
					{
						result = false;
					}
					else
					{
						bool flag3 = bytes == null || bytes.Length == 0;
						if (flag3)
						{
							result = false;
						}
						else
						{
							foreach (byte[] array in bytes)
							{
								this.ntwStream.Write(array, 0, array.Length);
							}
							result = true;
						}
					}
				}
			}
			catch (Exception ex)
			{
				LogLog.Error(TcpLogEventEngine.declaringType, string.Concat(new string[]
				{
					"Reconn Unable to send logging event to remote host ",
					this.remoteAddress.ToString(),
					" on port ",
					this.remoteEndPoint.ToString(),
					"."
				}), ex);
				result = false;
			}
			return result;
		}

		// Token: 0x06000027 RID: 39 RVA: 0x00002798 File Offset: 0x00000998
		protected byte[][] PopAllEvents()
		{
			object syncRoot = this.eventsList.SyncRoot;
			byte[][] result;
			lock (syncRoot)
			{
				byte[][] array = (byte[][])this.eventsList.ToArray(typeof(byte[]));
				this.eventsList.Clear();
				result = array;
			}
			return result;
		}

		// Token: 0x04000018 RID: 24
		private object locker = new object();

		// Token: 0x04000019 RID: 25
		private static readonly Type declaringType = typeof(TcpLogEventEngine);

		// Token: 0x0400001A RID: 26
		public ArrayList eventsList = new ArrayList();

		// Token: 0x0400001B RID: 27
		public ArrayList appenderList = new ArrayList();

		// Token: 0x0400001C RID: 28
		private string key;

		// Token: 0x0400001D RID: 29
		private int remotePort;

		// Token: 0x0400001E RID: 30
		private IPAddress remoteAddress;

		// Token: 0x0400001F RID: 31
		private IPEndPoint remoteEndPoint;

		// Token: 0x04000020 RID: 32
		private int connTime;

		// Token: 0x04000021 RID: 33
		private int memorySize;

		// Token: 0x04000022 RID: 34
		private TcpClient client;

		// Token: 0x04000023 RID: 35
		private NetworkStream ntwStream;
	}
}

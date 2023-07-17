using System;
using System.Net;
using System.Net.Sockets;

namespace QinSoft.Core.Log.NjLog.Util
{
	// Token: 0x02000003 RID: 3
	public static class IpHelper
	{
		// Token: 0x17000001 RID: 1
		// (get) Token: 0x06000006 RID: 6 RVA: 0x00002274 File Offset: 0x00000474
		public static string ip
		{
			get
			{
				bool flag = string.IsNullOrEmpty(IpHelper.m_ip);
				if (flag)
				{
					IpHelper.m_ip = IpHelper.GetLocalIP();
				}
				return IpHelper.m_ip;
			}
		}

		// Token: 0x06000007 RID: 7 RVA: 0x000022A8 File Offset: 0x000004A8
		public static string GetLocalIP()
		{
			string result;
			try
			{
				string hostName = Dns.GetHostName();
				IPHostEntry hostEntry = Dns.GetHostEntry(hostName);
				for (int i = 0; i < hostEntry.AddressList.Length; i++)
				{
					bool flag = hostEntry.AddressList[i].AddressFamily == AddressFamily.InterNetwork;
					if (flag)
					{
						return hostEntry.AddressList[i].ToString();
					}
				}
				result = "";
			}
			catch (Exception ex)
			{
				result = "";
			}
			return result;
		}

		// Token: 0x0400000C RID: 12
		private static string m_ip;
	}
}

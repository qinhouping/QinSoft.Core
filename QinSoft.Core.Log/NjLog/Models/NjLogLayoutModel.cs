using System;

namespace QinSoft.Core.Log.NjLog.Models
{
	// Token: 0x02000004 RID: 4
	[Serializable]
	public class NjLogLayoutModel
	{
		// Token: 0x17000002 RID: 2
		// (get) Token: 0x06000008 RID: 8 RVA: 0x0000232C File Offset: 0x0000052C
		// (set) Token: 0x06000009 RID: 9 RVA: 0x00002334 File Offset: 0x00000534
		public string datetime { get; set; }

		// Token: 0x17000003 RID: 3
		// (get) Token: 0x0600000A RID: 10 RVA: 0x0000233D File Offset: 0x0000053D
		// (set) Token: 0x0600000B RID: 11 RVA: 0x00002345 File Offset: 0x00000545
		public string logger { get; set; }

		// Token: 0x17000004 RID: 4
		// (get) Token: 0x0600000C RID: 12 RVA: 0x0000234E File Offset: 0x0000054E
		// (set) Token: 0x0600000D RID: 13 RVA: 0x00002356 File Offset: 0x00000556
		public string msg { get; set; }

		// Token: 0x17000005 RID: 5
		// (get) Token: 0x0600000E RID: 14 RVA: 0x0000235F File Offset: 0x0000055F
		// (set) Token: 0x0600000F RID: 15 RVA: 0x00002367 File Offset: 0x00000567
		public string thread { get; set; }

		// Token: 0x17000006 RID: 6
		// (get) Token: 0x06000010 RID: 16 RVA: 0x00002370 File Offset: 0x00000570
		// (set) Token: 0x06000011 RID: 17 RVA: 0x00002378 File Offset: 0x00000578
		public string level { get; set; }

		// Token: 0x17000007 RID: 7
		// (get) Token: 0x06000012 RID: 18 RVA: 0x00002381 File Offset: 0x00000581
		// (set) Token: 0x06000013 RID: 19 RVA: 0x00002389 File Offset: 0x00000589
		public string projectCode { get; set; }

		// Token: 0x17000008 RID: 8
		// (get) Token: 0x06000014 RID: 20 RVA: 0x00002392 File Offset: 0x00000592
		// (set) Token: 0x06000015 RID: 21 RVA: 0x0000239A File Offset: 0x0000059A
		public string appCode { get; set; }

		// Token: 0x17000009 RID: 9
		// (get) Token: 0x06000016 RID: 22 RVA: 0x000023A3 File Offset: 0x000005A3
		// (set) Token: 0x06000017 RID: 23 RVA: 0x000023AB File Offset: 0x000005AB
		public string host { get; set; }

		// Token: 0x1700000A RID: 10
		// (get) Token: 0x06000018 RID: 24 RVA: 0x000023B4 File Offset: 0x000005B4
		// (set) Token: 0x06000019 RID: 25 RVA: 0x000023BC File Offset: 0x000005BC
		public string logId { get; set; }

		// Token: 0x1700000B RID: 11
		// (get) Token: 0x0600001A RID: 26 RVA: 0x000023C5 File Offset: 0x000005C5
		// (set) Token: 0x0600001B RID: 27 RVA: 0x000023CD File Offset: 0x000005CD
		public string logVer { get; set; }

		// Token: 0x1700000C RID: 12
		// (get) Token: 0x0600001C RID: 28 RVA: 0x000023D6 File Offset: 0x000005D6
		// (set) Token: 0x0600001D RID: 29 RVA: 0x000023DE File Offset: 0x000005DE
		public string extend { get; set; }
	}
}

using System;
using System.IO;
using QinSoft.Core.Log.NjLog.Util;
using log4net.Core;
using log4net.Layout.Pattern;

namespace QinSoft.Core.Log.NjLog.Layout.Pattern
{
	// Token: 0x02000009 RID: 9
	internal sealed class IpPatternConverter : PatternLayoutConverter
	{
		// Token: 0x0600003F RID: 63 RVA: 0x00002FEC File Offset: 0x000011EC
		protected override void Convert(TextWriter writer, LoggingEvent loggingEvent)
		{
			writer.Write(IpHelper.ip);
		}
	}
}

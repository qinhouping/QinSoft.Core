using System;
using System.IO;
using QinSoft.Core.Log.NjLog.Util;
using log4net.Core;
using log4net.Layout.Pattern;

namespace QinSoft.Core.Log.NjLog.Layout.Pattern
{
	// Token: 0x02000008 RID: 8
	internal sealed class LogIdPatternConverter : PatternLayoutConverter
	{
		// Token: 0x0600003D RID: 61 RVA: 0x00002FB8 File Offset: 0x000011B8
		protected override void Convert(TextWriter writer, LoggingEvent loggingEvent)
		{
			writer.Write(IdWorker.nextId().ToString().PadLeft(20, '0'));
		}
	}
}

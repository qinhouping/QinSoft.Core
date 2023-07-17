using System;

namespace QinSoft.Core.Log.NjLog.Util
{
	// Token: 0x02000002 RID: 2
	public class IdWorker
	{
		// Token: 0x06000001 RID: 1 RVA: 0x00002050 File Offset: 0x00000250
		static IdWorker()
		{
			bool flag = IdWorker.workerId > IdWorker.maxWorkerId || IdWorker.workerId < 0L;
			if (flag)
			{
				throw new Exception(string.Format("worker Id can't be greater than {0} or less than 0 ", IdWorker.workerId));
			}
		}

		// Token: 0x06000002 RID: 2 RVA: 0x00002118 File Offset: 0x00000318
		public static long nextId()
		{
			object obj = IdWorker.obj;
			long result;
			lock (obj)
			{
				long num = IdWorker.timeGen();
				bool flag2 = IdWorker.lastTimestamp == num;
				if (flag2)
				{
					IdWorker.sequence = (IdWorker.sequence + 1L & IdWorker.sequenceMask);
					bool flag3 = IdWorker.sequence == 0L;
					if (flag3)
					{
						num = IdWorker.tillNextMillis(IdWorker.lastTimestamp);
					}
				}
				else
				{
					IdWorker.sequence = 0L;
				}
				bool flag4 = num < IdWorker.lastTimestamp;
				if (flag4)
				{
					throw new Exception(string.Format("Clock moved backwards.  Refusing to generate id for {0} milliseconds", IdWorker.lastTimestamp - num));
				}
				IdWorker.lastTimestamp = num;
				long num2 = num - IdWorker.twepoch << IdWorker.timestampLeftShift | IdWorker.workerId << IdWorker.workerIdShift | IdWorker.sequence;
				result = num2;
			}
			return result;
		}

		// Token: 0x06000003 RID: 3 RVA: 0x00002204 File Offset: 0x00000404
		private static long tillNextMillis(long lastTimestamp)
		{
			long num;
			for (num = IdWorker.timeGen(); num <= lastTimestamp; num = IdWorker.timeGen())
			{
			}
			return num;
		}

		// Token: 0x06000004 RID: 4 RVA: 0x00002234 File Offset: 0x00000434
		private static long timeGen()
		{
			return (long)(DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)).TotalMilliseconds;
		}

		// Token: 0x04000001 RID: 1
		private static long workerId = 1L;

		// Token: 0x04000002 RID: 2
		private static long twepoch = 687888001020L;

		// Token: 0x04000003 RID: 3
		private static long sequence = 0L;

		// Token: 0x04000004 RID: 4
		private static int workerIdBits = 4;

		// Token: 0x04000005 RID: 5
		public static long maxWorkerId = -1L ^ -1L << IdWorker.workerIdBits;

		// Token: 0x04000006 RID: 6
		private static int sequenceBits = 10;

		// Token: 0x04000007 RID: 7
		private static int workerIdShift = IdWorker.sequenceBits;

		// Token: 0x04000008 RID: 8
		private static int timestampLeftShift = IdWorker.sequenceBits + IdWorker.workerIdBits;

		// Token: 0x04000009 RID: 9
		public static long sequenceMask = -1L ^ -1L << IdWorker.sequenceBits;

		// Token: 0x0400000A RID: 10
		private static long lastTimestamp = -1L;

		// Token: 0x0400000B RID: 11
		private static object obj = new object();
	}
}

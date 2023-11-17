using System;
using System.Collections.Generic;
using System.Text;

namespace QinSoft.Core.Job
{
    /// <summary>
    /// 作业接口
    /// </summary>
    public interface IJob
    {
        void Execute(JobContext context);
    }
}

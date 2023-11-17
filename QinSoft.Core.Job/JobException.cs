using System;
using System.Collections.Generic;
using System.Text;

namespace QinSoft.Core.Job
{
    public class JobException : Exception
    {
        public JobException(string message) : base(message)
        {

        }

        public JobException(string message, Exception exception) : base(message, exception)
        {

        }
    }
}

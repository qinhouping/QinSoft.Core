using QinSoft.Core.Common.Utils;
using System;
using System.Collections.Generic;
using System.Text;

namespace QinSoft.Core.EventBus
{
    [AttributeUsage(AttributeTargets.Method)]
    public class HandlerMethodAttribute : Attribute
    {
        public string[] Topics { get; protected set; }

        public HandlerMethodAttribute(params string[] topics)
        {
            ObjectUtils.CheckEmpty(topics, nameof(topics));
            this.Topics = topics;
        }
    }
}

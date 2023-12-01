using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Options;
using QinSoft.Core.Cache.CSRedis;
using QinSoft.Core.Cache.CSRedis.Core;
using QinSoft.Core.Common.Utils;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using static CSRedis.CSRedisClient;

namespace QinSoft.Core.EventBus.Channels
{
    public class CSRedisChannel : Channel
    {
        protected CSRedisCache RedisClient { get; set; }

        /// <summary>
        /// Redis通道主题
        /// </summary>
        public string CSRedis_CHANNEL { get; set; } = "QinSoft.Core:__EventBus__";

        protected SubscribeObject subscribeObject;

        public CSRedisChannel(CSRedisCache client) : this(client, NullLoggerFactory.Instance)
        {

        }

        public CSRedisChannel(CSRedisCache client, ILogger logger) : base(logger)
        {
            ObjectUtils.CheckNull(client, nameof(client));
            this.RedisClient = client;

            this.SubscribeRedis();
        }

        public CSRedisChannel(CSRedisCache client, ILoggerFactory loggerFactory) : this(client, loggerFactory.CreateLogger<CSRedisChannel>())
        {

        }

        public CSRedisChannel(CSRedisCacheManager manager, CSRedisChannelOptions options) : this(manager, options, NullLoggerFactory.Instance)
        {

        }

        public CSRedisChannel(ICSRedisCacheManager manager, CSRedisChannelOptions options, ILoggerFactory loggerFactory) : base(loggerFactory.CreateLogger<CSRedisChannel>())
        {
            ObjectUtils.CheckNull(manager, nameof(manager));
            ObjectUtils.CheckNull(options, nameof(options));
            ObjectUtils.CheckEmpty(options.Channel, nameof(options.Channel));
            this.RedisClient = string.IsNullOrEmpty(options.Name) ? manager.GetCache() : manager.GetCache(options.Name);
            this.CSRedis_CHANNEL = options.Channel;

            this.SubscribeRedis();
        }

        public CSRedisChannel(ICSRedisCacheManager manager, IOptions<CSRedisChannelOptions> options) : this(manager, options, NullLoggerFactory.Instance)
        {

        }

        public CSRedisChannel(ICSRedisCacheManager manager, IOptions<CSRedisChannelOptions> options, ILoggerFactory loggerFactory) : base(loggerFactory.CreateLogger<CSRedisChannel>())
        {
            ObjectUtils.CheckNull(manager, nameof(manager));
            ObjectUtils.CheckNull(options, nameof(options));
            ObjectUtils.CheckEmpty(options.Value.Channel, nameof(options.Value.Channel));
            this.RedisClient = string.IsNullOrEmpty(options.Value.Name) ? manager.GetCache() : manager.GetCache(options.Value.Name);
            this.CSRedis_CHANNEL = options.Value.Channel;

            this.SubscribeRedis();
        }

        /// <summary>
        /// 订阅Redis
        /// </summary>
        protected virtual void SubscribeRedis()
        {
            this.subscribeObject = this.RedisClient.Subscribe((CSRedis_CHANNEL, (e) =>
            {
                this.Read(e.Body.FromJson<ChannelData>());
            }
            ));
        }

        protected override void WriteCore(ChannelData data)
        {
            this.RedisClient.Publish(CSRedis_CHANNEL, data.ToJson());
        }

        public override void Dispose()
        {
            this.subscribeObject.Dispose();
            base.Dispose();
        }
    }

    public class CSRedisChannelOptions
    {
        public string Name { get; set; }

        public string Channel { get; set; } = "QinSoft.Core:__EventBus__";
    }
}

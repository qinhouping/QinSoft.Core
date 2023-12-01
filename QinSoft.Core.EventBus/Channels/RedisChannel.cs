using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Options;
using QinSoft.Core.Cache.Redis;
using QinSoft.Core.Cache.Redis.Core;
using QinSoft.Core.Common.Utils;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace QinSoft.Core.EventBus.Channels
{
    public class RedisChannel : Channel
    {
        protected IRedisCache RedisClient { get; set; }

        protected StackExchange.Redis.ISubscriber Subscriber { get; set; }

        /// <summary>
        /// Redis通道主题
        /// </summary>
        public string Redis_CHANNEL { get; set; } = "QinSoft.Core:__EventBus__";

        public RedisChannel(IRedisCache client) : this(client, NullLoggerFactory.Instance)
        {

        }

        public RedisChannel(IRedisCache client, ILogger logger) : base(logger)
        {
            ObjectUtils.CheckNull(client, nameof(client));
            this.RedisClient = client;
            this.Subscriber = this.RedisClient.ConnectionMultiplexer.GetSubscriber();

            this.SubscribeRedis();
        }

        public RedisChannel(IRedisCache client, ILoggerFactory loggerFactory) : this(client, loggerFactory.CreateLogger<RedisChannel>())
        {

        }

        public RedisChannel(IRedisCacheManager manager, RedisChannelOptions options) : this(manager, options, NullLoggerFactory.Instance)
        {

        }

        public RedisChannel(IRedisCacheManager manager, RedisChannelOptions options, ILoggerFactory loggerFactory) : base(loggerFactory.CreateLogger<RedisChannel>())
        {
            ObjectUtils.CheckNull(manager, nameof(manager));
            ObjectUtils.CheckNull(options, nameof(options));
            ObjectUtils.CheckEmpty(options.Channel, nameof(options.Channel));
            this.RedisClient = string.IsNullOrEmpty(options.Name) ? manager.GetCache() : manager.GetCache(options.Name);
            this.Subscriber = this.RedisClient.ConnectionMultiplexer.GetSubscriber();
            this.Redis_CHANNEL = options.Channel;

            this.SubscribeRedis();
        }

        public RedisChannel(IRedisCacheManager manager, IOptions<RedisChannelOptions> options) : this(manager, options, NullLoggerFactory.Instance)
        {

        }

        public RedisChannel(IRedisCacheManager manager, IOptions<RedisChannelOptions> options, ILoggerFactory loggerFactory) : base(loggerFactory.CreateLogger<RedisChannel>())
        {
            ObjectUtils.CheckNull(manager, nameof(manager));
            ObjectUtils.CheckNull(options, nameof(options));
            ObjectUtils.CheckEmpty(options.Value.Channel, nameof(options.Value.Channel));
            this.RedisClient = string.IsNullOrEmpty(options.Value.Name) ? manager.GetCache() : manager.GetCache(options.Value.Name);
            this.Subscriber = this.RedisClient.ConnectionMultiplexer.GetSubscriber();
            this.Redis_CHANNEL = options.Value.Channel;

            this.SubscribeRedis();
        }

        /// <summary>
        /// 订阅Redis
        /// </summary>
        protected virtual void SubscribeRedis()
        {
            this.Subscriber.Subscribe(Redis_CHANNEL, (c, m) =>
            {
                this.Read(((string)m).FromJson<ChannelData>());
            });
        }

        protected override void WriteCore(ChannelData data)
        {
            this.Subscriber.Publish(Redis_CHANNEL, data.ToJson());
        }

        public override void Dispose()
        {
            this.Subscriber.Unsubscribe(Redis_CHANNEL);
            base.Dispose();
        }
    }

    public class RedisChannelOptions
    {
        public string Name { get; set; }

        public string Channel { get; set; } = "QinSoft.Core:__EventBus__";
    }
}

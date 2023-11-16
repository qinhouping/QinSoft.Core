using QinSoft.Core.Common.Utils;
using StackExchange.Redis;
using StackExchange.Redis.MultiplexerPool;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace QinSoft.Core.Cache.Redis.Core
{
    /// <summary>
    /// Redis缓存实现
    /// </summary>
    internal class RedisCache : IRedisCache
    {
        /// <summary>
        /// Redis连接
        /// </summary>
        protected IConnectionMultiplexer ConnectionMultiplexer { get; set; }

        /// <summary>
        /// 是否是池连接
        /// </summary>
        protected bool IsPoolConnection { get; set; }

        /// <summary>
        /// Redis数据库
        /// </summary>
        protected IDatabase DB { get; set; }

        /// <summary>
        /// Redis数据库
        /// </summary>
        public virtual int Database => DB.Database;

        /// <summary>
        /// Redis连接
        /// </summary>
        public IConnectionMultiplexer Multiplexer => ConnectionMultiplexer;

        public RedisCache(ConfigurationOptions configurationOptions, bool isSentinel = false)
        {
            ObjectUtils.CheckNull(configurationOptions, "configurationOptions");
            if (isSentinel)
            {
                this.ConnectionMultiplexer = StackExchange.Redis.ConnectionMultiplexer.SentinelConnect(configurationOptions).GetSentinelMasterConnection(configurationOptions.Clone());
            }
            else
            {
                this.ConnectionMultiplexer = StackExchange.Redis.ConnectionMultiplexer.Connect(configurationOptions);
            }
            this.IsPoolConnection = false;
            this.DB = this.ConnectionMultiplexer.GetDatabase();
        }

        public RedisCache(string configuration, bool isSentinel = false)
        {
            ObjectUtils.CheckNull(configuration, "configuration");
            ConfigurationOptions configurationOptions = ConfigurationOptions.Parse(configuration);
            if (isSentinel)
            {
                this.ConnectionMultiplexer = StackExchange.Redis.ConnectionMultiplexer.SentinelConnect(configuration).GetSentinelMasterConnection(configurationOptions.Clone());
            }
            else
            {
                this.ConnectionMultiplexer = StackExchange.Redis.ConnectionMultiplexer.Connect(configurationOptions);
            }
            this.IsPoolConnection = false;
            this.DB = this.ConnectionMultiplexer.GetDatabase();
        }

        public RedisCache(RedisCacheOptions redisCacheOptions)
        {
            ObjectUtils.CheckNull(redisCacheOptions, "redisCacheOptions");
            if (redisCacheOptions.IsSentinel)
            {
                this.ConnectionMultiplexer = StackExchange.Redis.ConnectionMultiplexer.SentinelConnect(redisCacheOptions.ConfigurationOptions).GetSentinelMasterConnection(redisCacheOptions.ConfigurationOptions.Clone());
            }
            else
            {
                this.ConnectionMultiplexer = StackExchange.Redis.ConnectionMultiplexer.Connect(redisCacheOptions.ConfigurationOptions);
            }
            this.IsPoolConnection = false;
            this.DB = this.ConnectionMultiplexer.GetDatabase();
        }

        public RedisCache(IConnectionMultiplexer connectionMultiplexer, bool isPoolConnection = false)
        {
            ObjectUtils.CheckNull(connectionMultiplexer, "connectionMultiplexer");
            this.ConnectionMultiplexer = connectionMultiplexer;
            this.IsPoolConnection = isPoolConnection;
            this.DB = this.ConnectionMultiplexer.GetDatabase();
        }

        public RedisCache(IReconnectableConnectionMultiplexer reconnectableConnectionMultiplexer)
        {
            ObjectUtils.CheckNull(reconnectableConnectionMultiplexer, "reconnectableConnectionMultiplexer");
            this.ConnectionMultiplexer = reconnectableConnectionMultiplexer.Connection;
            this.IsPoolConnection = true;
            this.DB = this.ConnectionMultiplexer.GetDatabase();
        }

        public virtual void Select(int database)
        {
            this.DB = this.ConnectionMultiplexer.GetDatabase(database);
        }

        /// <summary>
        /// 释放资源
        /// </summary>
        public virtual void Dispose()
        {
            GC.SuppressFinalize(this);
            if (!IsPoolConnection)
            {
                this.ConnectionMultiplexer.Dispose();
            }
        }

        public virtual IBatch CreateBatch(object asyncState = null)
        {
            return DB.CreateBatch(asyncState);
        }

        public virtual ITransaction CreateTransaction(object asyncState = null)
        {
            return DB.CreateTransaction(asyncState);
        }

        public virtual void KeyMigrate(RedisKey key, EndPoint toServer, int toDatabase = 0, int timeoutMilliseconds = 0, MigrateOptions migrateOptions = MigrateOptions.None, CommandFlags flags = CommandFlags.None)
        {
            DB.KeyMigrate(key, toServer, toDatabase, timeoutMilliseconds, migrateOptions, flags);
        }

        public virtual RedisValue DebugObject(RedisKey key, CommandFlags flags = CommandFlags.None)
        {
            return DB.DebugObject(key, flags);
        }

        public virtual bool GeoAdd(RedisKey key, double longitude, double latitude, RedisValue member, CommandFlags flags = CommandFlags.None)
        {
            return DB.GeoAdd(key, longitude, latitude, member, flags);
        }

        public virtual bool GeoAdd(RedisKey key, GeoEntry value, CommandFlags flags = CommandFlags.None)
        {
            return DB.GeoAdd(key, value, flags);
        }

        public virtual long GeoAdd(RedisKey key, GeoEntry[] values, CommandFlags flags = CommandFlags.None)
        {
            return DB.GeoAdd(key, values, flags);
        }

        public virtual bool GeoRemove(RedisKey key, RedisValue member, CommandFlags flags = CommandFlags.None)
        {
            return DB.GeoRemove(key, member, flags);
        }

        public virtual double? GeoDistance(RedisKey key, RedisValue member1, RedisValue member2, GeoUnit unit = GeoUnit.Meters, CommandFlags flags = CommandFlags.None)
        {
            return DB.GeoDistance(key, member1, member2, unit, flags);
        }

        public virtual string[] GeoHash(RedisKey key, RedisValue[] members, CommandFlags flags = CommandFlags.None)
        {
            return DB.GeoHash(key, members, flags);
        }

        public virtual string GeoHash(RedisKey key, RedisValue member, CommandFlags flags = CommandFlags.None)
        {
            return DB.GeoHash(key, member, flags);
        }

        public virtual GeoPosition?[] GeoPosition(RedisKey key, RedisValue[] members, CommandFlags flags = CommandFlags.None)
        {
            return DB.GeoPosition(key, members, flags);
        }

        public virtual GeoPosition? GeoPosition(RedisKey key, RedisValue member, CommandFlags flags = CommandFlags.None)
        {
            return DB.GeoPosition(key, member, flags);
        }

        public virtual GeoRadiusResult[] GeoRadius(RedisKey key, RedisValue member, double radius, GeoUnit unit = GeoUnit.Meters, int count = -1, Order? order = null, GeoRadiusOptions options = GeoRadiusOptions.Default, CommandFlags flags = CommandFlags.None)
        {
            return DB.GeoRadius(key, member, radius, unit, count, order, options, flags);
        }

        public virtual GeoRadiusResult[] GeoRadius(RedisKey key, double longitude, double latitude, double radius, GeoUnit unit = GeoUnit.Meters, int count = -1, Order? order = null, GeoRadiusOptions options = GeoRadiusOptions.Default, CommandFlags flags = CommandFlags.None)
        {
            return DB.GeoRadius(key, longitude, latitude, radius, unit, count, order, options, flags);
        }

        public virtual long HashDecrement(RedisKey key, RedisValue hashField, long value = 1, CommandFlags flags = CommandFlags.None)
        {
            return DB.HashDecrement(key, hashField, value, flags);
        }

        public virtual double HashDecrement(RedisKey key, RedisValue hashField, double value, CommandFlags flags = CommandFlags.None)
        {
            return DB.HashDecrement(key, hashField, value, flags);
        }

        public virtual bool HashDelete(RedisKey key, RedisValue hashField, CommandFlags flags = CommandFlags.None)
        {
            return DB.HashDelete(key, hashField, flags);
        }

        public virtual long HashDelete(RedisKey key, RedisValue[] hashFields, CommandFlags flags = CommandFlags.None)
        {
            return DB.HashDelete(key, hashFields, flags);
        }

        public virtual bool HashExists(RedisKey key, RedisValue hashField, CommandFlags flags = CommandFlags.None)
        {
            return DB.HashExists(key, hashField, flags);
        }

        public virtual RedisValue HashGet(RedisKey key, RedisValue hashField, CommandFlags flags = CommandFlags.None)
        {
            return DB.HashGet(key, hashField, flags);
        }

        public virtual Lease<byte> HashGetLease(RedisKey key, RedisValue hashField, CommandFlags flags = CommandFlags.None)
        {
            return DB.HashGetLease(key, hashField, flags);
        }

        public virtual RedisValue[] HashGet(RedisKey key, RedisValue[] hashFields, CommandFlags flags = CommandFlags.None)
        {
            return DB.HashGet(key, hashFields, flags);
        }

        public virtual HashEntry[] HashGetAll(RedisKey key, CommandFlags flags = CommandFlags.None)
        {
            return DB.HashGetAll(key, flags);
        }

        public virtual long HashIncrement(RedisKey key, RedisValue hashField, long value = 1, CommandFlags flags = CommandFlags.None)
        {
            return DB.HashIncrement(key, hashField, value, flags);
        }

        public virtual double HashIncrement(RedisKey key, RedisValue hashField, double value, CommandFlags flags = CommandFlags.None)
        {
            return DB.HashIncrement(key, hashField, value, flags);
        }

        public virtual RedisValue[] HashKeys(RedisKey key, CommandFlags flags = CommandFlags.None)
        {
            return DB.HashKeys(key, flags);
        }

        public virtual long HashLength(RedisKey key, CommandFlags flags = CommandFlags.None)
        {
            return DB.HashLength(key, flags);
        }

        public virtual IEnumerable<HashEntry> HashScan(RedisKey key, RedisValue pattern, int pageSize, CommandFlags flags)
        {
            return DB.HashScan(key, pattern, pageSize, flags);
        }

        public virtual IEnumerable<HashEntry> HashScan(RedisKey key, RedisValue pattern = default, int pageSize = 250, long cursor = 0, int pageOffset = 0, CommandFlags flags = CommandFlags.None)
        {
            return DB.HashScan(key, pattern, pageSize, cursor, pageOffset, flags);
        }

        public virtual void HashSet(RedisKey key, HashEntry[] hashFields, CommandFlags flags = CommandFlags.None)
        {
            DB.HashSet(key, hashFields, flags);
        }

        public virtual bool HashSet(RedisKey key, RedisValue hashField, RedisValue value, When when = When.Always, CommandFlags flags = CommandFlags.None)
        {
            return DB.HashSet(key, hashField, value, when, flags);
        }

        public virtual long HashStringLength(RedisKey key, RedisValue hashField, CommandFlags flags = CommandFlags.None)
        {
            return DB.HashStringLength(key, hashField, flags);
        }

        public virtual RedisValue[] HashValues(RedisKey key, CommandFlags flags = CommandFlags.None)
        {
            return DB.HashValues(key, flags);
        }

        public virtual bool HyperLogLogAdd(RedisKey key, RedisValue value, CommandFlags flags = CommandFlags.None)
        {
            return DB.HyperLogLogAdd(key, value, flags);
        }

        public virtual bool HyperLogLogAdd(RedisKey key, RedisValue[] values, CommandFlags flags = CommandFlags.None)
        {
            return DB.HyperLogLogAdd(key, values, flags);
        }

        public virtual long HyperLogLogLength(RedisKey key, CommandFlags flags = CommandFlags.None)
        {
            return DB.HyperLogLogLength(key, flags);
        }

        public virtual long HyperLogLogLength(RedisKey[] keys, CommandFlags flags = CommandFlags.None)
        {
            return DB.HyperLogLogLength(keys, flags);
        }

        public virtual void HyperLogLogMerge(RedisKey destination, RedisKey first, RedisKey second, CommandFlags flags = CommandFlags.None)
        {
            DB.HyperLogLogMerge(destination, first, second, flags);
        }

        public virtual void HyperLogLogMerge(RedisKey destination, RedisKey[] sourceKeys, CommandFlags flags = CommandFlags.None)
        {
            DB.HyperLogLogMerge(destination, sourceKeys, flags);
        }

        public virtual EndPoint IdentifyEndpoint(RedisKey key = default, CommandFlags flags = CommandFlags.None)
        {
            return DB.IdentifyEndpoint(key, flags);
        }

        public virtual bool KeyDelete(RedisKey key, CommandFlags flags = CommandFlags.None)
        {
            return DB.KeyDelete(key, flags);
        }

        public virtual long KeyDelete(RedisKey[] keys, CommandFlags flags = CommandFlags.None)
        {
            return DB.KeyDelete(keys, flags);
        }

        public virtual byte[] KeyDump(RedisKey key, CommandFlags flags = CommandFlags.None)
        {
            return DB.KeyDump(key, flags);
        }

        public virtual bool KeyExists(RedisKey key, CommandFlags flags = CommandFlags.None)
        {
            return DB.KeyExists(key, flags);
        }

        public virtual long KeyExists(RedisKey[] keys, CommandFlags flags = CommandFlags.None)
        {
            return DB.KeyExists(keys, flags);
        }

        public virtual bool KeyExpire(RedisKey key, TimeSpan? expiry, CommandFlags flags = CommandFlags.None)
        {
            return DB.KeyExpire(key, expiry, flags);
        }

        public virtual bool KeyExpire(RedisKey key, DateTime? expiry, CommandFlags flags = CommandFlags.None)
        {
            return DB.KeyExpire(key, expiry, flags);
        }

        public virtual TimeSpan? KeyIdleTime(RedisKey key, CommandFlags flags = CommandFlags.None)
        {
            return DB.KeyIdleTime(key, flags);
        }

        public virtual bool KeyMove(RedisKey key, int database, CommandFlags flags = CommandFlags.None)
        {
            return DB.KeyMove(key, database, flags);
        }

        public virtual bool KeyPersist(RedisKey key, CommandFlags flags = CommandFlags.None)
        {
            return DB.KeyPersist(key, flags);
        }

        public virtual RedisKey KeyRandom(CommandFlags flags = CommandFlags.None)
        {
            return DB.KeyRandom(flags);
        }

        public virtual bool KeyRename(RedisKey key, RedisKey newKey, When when = When.Always, CommandFlags flags = CommandFlags.None)
        {
            return DB.KeyRename(key, newKey, when, flags);
        }

        public virtual void KeyRestore(RedisKey key, byte[] value, TimeSpan? expiry = null, CommandFlags flags = CommandFlags.None)
        {
            DB.KeyRestore(key, value, expiry, flags);
        }

        public virtual TimeSpan? KeyTimeToLive(RedisKey key, CommandFlags flags = CommandFlags.None)
        {
            return DB.KeyTimeToLive(key, flags);
        }

        public virtual RedisType KeyType(RedisKey key, CommandFlags flags = CommandFlags.None)
        {
            return DB.KeyType(key, flags);
        }

        public virtual RedisValue ListGetByIndex(RedisKey key, long index, CommandFlags flags = CommandFlags.None)
        {
            return DB.ListGetByIndex(key, index, flags);
        }

        public virtual long ListInsertAfter(RedisKey key, RedisValue pivot, RedisValue value, CommandFlags flags = CommandFlags.None)
        {
            return DB.ListInsertAfter(key, pivot, value, flags);
        }

        public virtual long ListInsertBefore(RedisKey key, RedisValue pivot, RedisValue value, CommandFlags flags = CommandFlags.None)
        {
            return DB.ListInsertBefore(key, pivot, value, flags);
        }

        public virtual RedisValue ListLeftPop(RedisKey key, CommandFlags flags = CommandFlags.None)
        {
            return DB.ListLeftPop(key, flags);
        }

        public virtual RedisValue[] ListLeftPop(RedisKey key, long count, CommandFlags flags = CommandFlags.None)
        {
            return DB.ListLeftPop(key, count, flags);
        }

        public virtual long ListLeftPush(RedisKey key, RedisValue value, When when = When.Always, CommandFlags flags = CommandFlags.None)
        {
            return DB.ListLeftPush(key, value, when, flags);
        }

        public virtual long ListLeftPush(RedisKey key, RedisValue[] values, When when = When.Always, CommandFlags flags = CommandFlags.None)
        {
            return DB.ListLeftPush(key, values, when, flags);
        }

        public virtual long ListLeftPush(RedisKey key, RedisValue[] values, CommandFlags flags)
        {
            return DB.ListLeftPush(key, values, flags);
        }

        public virtual long ListLength(RedisKey key, CommandFlags flags = CommandFlags.None)
        {
            return DB.ListLength(key, flags);
        }

        public virtual RedisValue[] ListRange(RedisKey key, long start = 0, long stop = -1, CommandFlags flags = CommandFlags.None)
        {
            return DB.ListRange(key, start, stop, flags);
        }

        public virtual long ListRemove(RedisKey key, RedisValue value, long count = 0, CommandFlags flags = CommandFlags.None)
        {
            return DB.ListRemove(key, value, count, flags);
        }

        public virtual RedisValue ListRightPop(RedisKey key, CommandFlags flags = CommandFlags.None)
        {
            return DB.ListRightPop(key, flags);
        }
        public virtual RedisValue[] ListRightPop(RedisKey key, long count, CommandFlags flags = CommandFlags.None)
        {
            return DB.ListRightPop(key, count, flags);
        }

        public virtual RedisValue ListRightPopLeftPush(RedisKey source, RedisKey destination, CommandFlags flags = CommandFlags.None)
        {
            return DB.ListRightPopLeftPush(source, destination, flags);
        }

        public virtual long ListRightPush(RedisKey key, RedisValue value, When when = When.Always, CommandFlags flags = CommandFlags.None)
        {
            return DB.ListRightPush(key, value, when, flags);
        }

        public virtual long ListRightPush(RedisKey key, RedisValue[] values, When when = When.Always, CommandFlags flags = CommandFlags.None)
        {
            return DB.ListRightPush(key, values, when, flags);
        }

        public virtual long ListRightPush(RedisKey key, RedisValue[] values, CommandFlags flags)
        {
            return DB.ListRightPush(key, values, flags);
        }

        public virtual void ListSetByIndex(RedisKey key, long index, RedisValue value, CommandFlags flags = CommandFlags.None)
        {
            DB.ListSetByIndex(key, index, value, flags);
        }

        public virtual void ListTrim(RedisKey key, long start, long stop, CommandFlags flags = CommandFlags.None)
        {
            DB.ListTrim(key, start, stop, flags);
        }

        public virtual bool LockExtend(RedisKey key, RedisValue value, TimeSpan expiry, CommandFlags flags = CommandFlags.None)
        {
            return DB.LockExtend(key, value, expiry, flags);
        }

        public virtual RedisValue LockQuery(RedisKey key, CommandFlags flags = CommandFlags.None)
        {
            return DB.LockQuery(key, flags);
        }

        public virtual bool LockRelease(RedisKey key, RedisValue value, CommandFlags flags = CommandFlags.None)
        {
            return DB.LockRelease(key, value, flags);
        }

        public virtual bool LockTake(RedisKey key, RedisValue value, TimeSpan expiry, CommandFlags flags = CommandFlags.None)
        {
            return DB.LockTake(key, value, expiry, flags);
        }

        public virtual long Publish(RedisChannel channel, RedisValue message, CommandFlags flags = CommandFlags.None)
        {
            return DB.Publish(channel, message, flags);
        }

        public virtual RedisResult Execute(string command, params object[] args)
        {
            return DB.Execute(command, args);
        }

        public virtual RedisResult Execute(string command, ICollection<object> args, CommandFlags flags = CommandFlags.None)
        {
            return DB.Execute(command, args, flags);
        }

        public virtual RedisResult ScriptEvaluate(string script, RedisKey[] keys = null, RedisValue[] values = null, CommandFlags flags = CommandFlags.None)
        {
            return DB.ScriptEvaluate(script, keys, values, flags);
        }

        public virtual RedisResult ScriptEvaluate(byte[] hash, RedisKey[] keys = null, RedisValue[] values = null, CommandFlags flags = CommandFlags.None)
        {
            return DB.ScriptEvaluate(hash, keys, values, flags);
        }

        public virtual RedisResult ScriptEvaluate(LuaScript script, object parameters = null, CommandFlags flags = CommandFlags.None)
        {
            return DB.ScriptEvaluate(script, parameters, flags);
        }

        public virtual RedisResult ScriptEvaluate(LoadedLuaScript script, object parameters = null, CommandFlags flags = CommandFlags.None)
        {
            return DB.ScriptEvaluate(script, parameters, flags);
        }

        public virtual bool SetAdd(RedisKey key, RedisValue value, CommandFlags flags = CommandFlags.None)
        {
            return DB.SetAdd(key, value, flags);
        }

        public virtual long SetAdd(RedisKey key, RedisValue[] values, CommandFlags flags = CommandFlags.None)
        {
            return DB.SetAdd(key, values, flags);
        }

        public virtual RedisValue[] SetCombine(SetOperation operation, RedisKey first, RedisKey second, CommandFlags flags = CommandFlags.None)
        {
            return DB.SetCombine(operation, first, second, flags);
        }

        public virtual RedisValue[] SetCombine(SetOperation operation, RedisKey[] keys, CommandFlags flags = CommandFlags.None)
        {
            return DB.SetCombine(operation, keys, flags);
        }

        public virtual long SetCombineAndStore(SetOperation operation, RedisKey destination, RedisKey first, RedisKey second, CommandFlags flags = CommandFlags.None)
        {
            return DB.SetCombineAndStore(operation, destination, first, second, flags);
        }

        public virtual long SetCombineAndStore(SetOperation operation, RedisKey destination, RedisKey[] keys, CommandFlags flags = CommandFlags.None)
        {
            return DB.SetCombineAndStore(operation, destination, keys, flags);
        }

        public virtual bool SetContains(RedisKey key, RedisValue value, CommandFlags flags = CommandFlags.None)
        {
            return DB.SetContains(key, value, flags);
        }

        public virtual long SetLength(RedisKey key, CommandFlags flags = CommandFlags.None)
        {
            return DB.SetLength(key, flags);
        }

        public virtual RedisValue[] SetMembers(RedisKey key, CommandFlags flags = CommandFlags.None)
        {
            return DB.SetMembers(key, flags);
        }

        public virtual bool SetMove(RedisKey source, RedisKey destination, RedisValue value, CommandFlags flags = CommandFlags.None)
        {
            return DB.SetMove(source, destination, value, flags);
        }

        public virtual RedisValue SetPop(RedisKey key, CommandFlags flags = CommandFlags.None)
        {
            return DB.SetPop(key, flags);
        }

        public virtual RedisValue[] SetPop(RedisKey key, long count, CommandFlags flags = CommandFlags.None)
        {
            return DB.SetPop(key, count, flags);
        }

        public virtual RedisValue SetRandomMember(RedisKey key, CommandFlags flags = CommandFlags.None)
        {
            return DB.SetRandomMember(key, flags);
        }

        public virtual RedisValue[] SetRandomMembers(RedisKey key, long count, CommandFlags flags = CommandFlags.None)
        {
            return DB.SetRandomMembers(key, count, flags);
        }

        public virtual bool SetRemove(RedisKey key, RedisValue value, CommandFlags flags = CommandFlags.None)
        {
            return DB.SetRemove(key, value, flags);
        }

        public virtual long SetRemove(RedisKey key, RedisValue[] values, CommandFlags flags = CommandFlags.None)
        {
            return DB.SetRemove(key, values, flags);
        }

        public virtual IEnumerable<RedisValue> SetScan(RedisKey key, RedisValue pattern, int pageSize, CommandFlags flags)
        {
            return DB.SetScan(key, pattern, pageSize, flags);
        }

        public virtual IEnumerable<RedisValue> SetScan(RedisKey key, RedisValue pattern = default, int pageSize = 250, long cursor = 0, int pageOffset = 0, CommandFlags flags = CommandFlags.None)
        {
            return DB.SetScan(key, pattern, pageSize, cursor, pageOffset, flags);
        }

        public virtual RedisValue[] Sort(RedisKey key, long skip = 0, long take = -1, Order order = Order.Ascending, SortType sortType = SortType.Numeric, RedisValue by = default, RedisValue[] get = null, CommandFlags flags = CommandFlags.None)
        {
            return DB.Sort(key, skip, take, order, sortType, by, get, flags);
        }

        public virtual long SortAndStore(RedisKey destination, RedisKey key, long skip = 0, long take = -1, Order order = Order.Ascending, SortType sortType = SortType.Numeric, RedisValue by = default, RedisValue[] get = null, CommandFlags flags = CommandFlags.None)
        {
            return DB.SortAndStore(destination, key, skip, take, order, sortType, by, get, flags);
        }

        public virtual bool SortedSetAdd(RedisKey key, RedisValue member, double score, CommandFlags flags)
        {
            return DB.SortedSetAdd(key, member, score, flags);
        }

        public virtual bool SortedSetAdd(RedisKey key, RedisValue member, double score, When when = When.Always, CommandFlags flags = CommandFlags.None)
        {
            return DB.SortedSetAdd(key, member, score, when, flags);
        }

        public virtual long SortedSetAdd(RedisKey key, SortedSetEntry[] values, CommandFlags flags)
        {
            return DB.SortedSetAdd(key, values, flags);
        }

        public virtual long SortedSetAdd(RedisKey key, SortedSetEntry[] values, When when = When.Always, CommandFlags flags = CommandFlags.None)
        {
            return DB.SortedSetAdd(key, values, when, flags);
        }

        public virtual long SortedSetCombineAndStore(SetOperation operation, RedisKey destination, RedisKey first, RedisKey second, Aggregate aggregate = Aggregate.Sum, CommandFlags flags = CommandFlags.None)
        {
            return DB.SortedSetCombineAndStore(operation, destination, first, second, aggregate, flags);
        }

        public virtual long SortedSetCombineAndStore(SetOperation operation, RedisKey destination, RedisKey[] keys, double[] weights = null, Aggregate aggregate = Aggregate.Sum, CommandFlags flags = CommandFlags.None)
        {
            return DB.SortedSetCombineAndStore(operation, destination, keys, weights, aggregate, flags);
        }

        public virtual double SortedSetDecrement(RedisKey key, RedisValue member, double value, CommandFlags flags = CommandFlags.None)
        {
            return DB.SortedSetDecrement(key, member, value, flags);
        }

        public virtual double SortedSetIncrement(RedisKey key, RedisValue member, double value, CommandFlags flags = CommandFlags.None)
        {
            return DB.SortedSetIncrement(key, member, value, flags);
        }

        public virtual long SortedSetLength(RedisKey key, double min = double.NegativeInfinity, double max = double.PositiveInfinity, Exclude exclude = Exclude.None, CommandFlags flags = CommandFlags.None)
        {
            return DB.SortedSetLength(key, min, max, exclude, flags);
        }

        public virtual long SortedSetLengthByValue(RedisKey key, RedisValue min, RedisValue max, Exclude exclude = Exclude.None, CommandFlags flags = CommandFlags.None)
        {
            return DB.SortedSetLengthByValue(key, min, max, exclude, flags);
        }

        public virtual RedisValue[] SortedSetRangeByRank(RedisKey key, long start = 0, long stop = -1, Order order = Order.Ascending, CommandFlags flags = CommandFlags.None)
        {
            return DB.SortedSetRangeByRank(key, start, stop, order, flags);
        }

        public virtual SortedSetEntry[] SortedSetRangeByRankWithScores(RedisKey key, long start = 0, long stop = -1, Order order = Order.Ascending, CommandFlags flags = CommandFlags.None)
        {
            return DB.SortedSetRangeByRankWithScores(key, start, stop, order, flags);
        }

        public virtual RedisValue[] SortedSetRangeByScore(RedisKey key, double start = double.NegativeInfinity, double stop = double.PositiveInfinity, Exclude exclude = Exclude.None, Order order = Order.Ascending, long skip = 0, long take = -1, CommandFlags flags = CommandFlags.None)
        {
            return DB.SortedSetRangeByScore(key, start, stop, exclude, order, skip, take, flags);
        }

        public virtual SortedSetEntry[] SortedSetRangeByScoreWithScores(RedisKey key, double start = double.NegativeInfinity, double stop = double.PositiveInfinity, Exclude exclude = Exclude.None, Order order = Order.Ascending, long skip = 0, long take = -1, CommandFlags flags = CommandFlags.None)
        {
            return DB.SortedSetRangeByScoreWithScores(key, start, stop, exclude, order, skip, take, flags);
        }

        public virtual RedisValue[] SortedSetRangeByValue(RedisKey key, RedisValue min, RedisValue max, Exclude exclude, long skip, long take = -1, CommandFlags flags = CommandFlags.None)
        {
            return DB.SortedSetRangeByValue(key, min, max, exclude, skip, take, flags);
        }

        public virtual RedisValue[] SortedSetRangeByValue(RedisKey key, RedisValue min = default, RedisValue max = default, Exclude exclude = Exclude.None, Order order = Order.Ascending, long skip = 0, long take = -1, CommandFlags flags = CommandFlags.None)
        {
            return DB.SortedSetRangeByValue(key, min, max, exclude, order, skip, take, flags);
        }

        public virtual long? SortedSetRank(RedisKey key, RedisValue member, Order order = Order.Ascending, CommandFlags flags = CommandFlags.None)
        {
            return DB.SortedSetRank(key, member, order, flags);
        }

        public virtual bool SortedSetRemove(RedisKey key, RedisValue member, CommandFlags flags = CommandFlags.None)
        {
            return DB.SortedSetRemove(key, member, flags);
        }

        public virtual long SortedSetRemove(RedisKey key, RedisValue[] members, CommandFlags flags = CommandFlags.None)
        {
            return DB.SortedSetRemove(key, members, flags);
        }

        public virtual long SortedSetRemoveRangeByRank(RedisKey key, long start, long stop, CommandFlags flags = CommandFlags.None)
        {
            return DB.SortedSetRemoveRangeByRank(key, start, stop, flags);
        }

        public virtual long SortedSetRemoveRangeByScore(RedisKey key, double start, double stop, Exclude exclude = Exclude.None, CommandFlags flags = CommandFlags.None)
        {
            return DB.SortedSetRemoveRangeByScore(key, start, stop, exclude, flags);
        }

        public virtual long SortedSetRemoveRangeByValue(RedisKey key, RedisValue min, RedisValue max, Exclude exclude = Exclude.None, CommandFlags flags = CommandFlags.None)
        {
            return DB.SortedSetRemoveRangeByValue(key, min, max, exclude, flags);
        }

        public virtual IEnumerable<SortedSetEntry> SortedSetScan(RedisKey key, RedisValue pattern, int pageSize, CommandFlags flags)
        {
            return DB.SortedSetScan(key, pattern, pageSize, flags);
        }

        public virtual IEnumerable<SortedSetEntry> SortedSetScan(RedisKey key, RedisValue pattern = default, int pageSize = 250, long cursor = 0, int pageOffset = 0, CommandFlags flags = CommandFlags.None)
        {
            return DB.SortedSetScan(key, pattern, pageSize, cursor, pageOffset, flags);
        }

        public virtual double? SortedSetScore(RedisKey key, RedisValue member, CommandFlags flags = CommandFlags.None)
        {
            return DB.SortedSetScore(key, member, flags);
        }

        public virtual SortedSetEntry? SortedSetPop(RedisKey key, Order order = Order.Ascending, CommandFlags flags = CommandFlags.None)
        {
            return DB.SortedSetPop(key, order, flags);
        }

        public virtual SortedSetEntry[] SortedSetPop(RedisKey key, long count, Order order = Order.Ascending, CommandFlags flags = CommandFlags.None)
        {
            return DB.SortedSetPop(key, count, order, flags);
        }

        public virtual long StreamAcknowledge(RedisKey key, RedisValue groupName, RedisValue messageId, CommandFlags flags = CommandFlags.None)
        {
            return DB.StreamAcknowledge(key, groupName, messageId, flags);
        }

        public virtual long StreamAcknowledge(RedisKey key, RedisValue groupName, RedisValue[] messageIds, CommandFlags flags = CommandFlags.None)
        {
            return DB.StreamAcknowledge(key, groupName, messageIds, flags);
        }

        public virtual RedisValue StreamAdd(RedisKey key, RedisValue streamField, RedisValue streamValue, RedisValue? messageId = null, int? maxLength = null, bool useApproximateMaxLength = false, CommandFlags flags = CommandFlags.None)
        {
            return DB.StreamAdd(key, streamField, streamValue, messageId, maxLength, useApproximateMaxLength, flags);
        }

        public virtual RedisValue StreamAdd(RedisKey key, NameValueEntry[] streamPairs, RedisValue? messageId = null, int? maxLength = null, bool useApproximateMaxLength = false, CommandFlags flags = CommandFlags.None)
        {
            return DB.StreamAdd(key, streamPairs, messageId, maxLength, useApproximateMaxLength, flags);
        }

        public virtual StreamEntry[] StreamClaim(RedisKey key, RedisValue consumerGroup, RedisValue claimingConsumer, long minIdleTimeInMs, RedisValue[] messageIds, CommandFlags flags = CommandFlags.None)
        {
            return DB.StreamClaim(key, consumerGroup, claimingConsumer, minIdleTimeInMs, messageIds, flags);
        }

        public virtual RedisValue[] StreamClaimIdsOnly(RedisKey key, RedisValue consumerGroup, RedisValue claimingConsumer, long minIdleTimeInMs, RedisValue[] messageIds, CommandFlags flags = CommandFlags.None)
        {
            return DB.StreamClaimIdsOnly(key, consumerGroup, claimingConsumer, minIdleTimeInMs, messageIds, flags);
        }

        public virtual bool StreamConsumerGroupSetPosition(RedisKey key, RedisValue groupName, RedisValue position, CommandFlags flags = CommandFlags.None)
        {
            return DB.StreamConsumerGroupSetPosition(key, groupName, position, flags);
        }

        public virtual StreamConsumerInfo[] StreamConsumerInfo(RedisKey key, RedisValue groupName, CommandFlags flags = CommandFlags.None)
        {
            return DB.StreamConsumerInfo(key, groupName, flags);
        }

        public virtual bool StreamCreateConsumerGroup(RedisKey key, RedisValue groupName, RedisValue? position, CommandFlags flags)
        {
            return DB.StreamCreateConsumerGroup(key, groupName, position, flags);
        }

        public virtual bool StreamCreateConsumerGroup(RedisKey key, RedisValue groupName, RedisValue? position = null, bool createStream = true, CommandFlags flags = CommandFlags.None)
        {
            return DB.StreamCreateConsumerGroup(key, groupName, position, createStream, flags);
        }

        public virtual long StreamDelete(RedisKey key, RedisValue[] messageIds, CommandFlags flags = CommandFlags.None)
        {
            return DB.StreamDelete(key, messageIds, flags);
        }

        public virtual long StreamDeleteConsumer(RedisKey key, RedisValue groupName, RedisValue consumerName, CommandFlags flags = CommandFlags.None)
        {
            return DB.StreamDeleteConsumer(key, groupName, consumerName, flags);
        }

        public virtual bool StreamDeleteConsumerGroup(RedisKey key, RedisValue groupName, CommandFlags flags = CommandFlags.None)
        {
            return DB.StreamDeleteConsumerGroup(key, groupName, flags);
        }

        public virtual StreamGroupInfo[] StreamGroupInfo(RedisKey key, CommandFlags flags = CommandFlags.None)
        {
            return DB.StreamGroupInfo(key, flags);
        }

        public virtual StreamInfo StreamInfo(RedisKey key, CommandFlags flags = CommandFlags.None)
        {
            return DB.StreamInfo(key, flags);
        }

        public virtual long StreamLength(RedisKey key, CommandFlags flags = CommandFlags.None)
        {
            return DB.StreamLength(key, flags);
        }

        public virtual StreamPendingInfo StreamPending(RedisKey key, RedisValue groupName, CommandFlags flags = CommandFlags.None)
        {
            return DB.StreamPending(key, groupName, flags);
        }

        public virtual StreamPendingMessageInfo[] StreamPendingMessages(RedisKey key, RedisValue groupName, int count, RedisValue consumerName, RedisValue? minId = null, RedisValue? maxId = null, CommandFlags flags = CommandFlags.None)
        {
            return DB.StreamPendingMessages(key, groupName, count, groupName, minId, maxId, flags);
        }

        public virtual StreamEntry[] StreamRange(RedisKey key, RedisValue? minId = null, RedisValue? maxId = null, int? count = null, Order messageOrder = Order.Ascending, CommandFlags flags = CommandFlags.None)
        {
            return DB.StreamRange(key, minId, maxId, count, messageOrder, flags);
        }

        public virtual StreamEntry[] StreamRead(RedisKey key, RedisValue position, int? count = null, CommandFlags flags = CommandFlags.None)
        {
            return DB.StreamRead(key, position, count, flags);
        }

        public virtual RedisStream[] StreamRead(StreamPosition[] streamPositions, int? countPerStream = null, CommandFlags flags = CommandFlags.None)
        {
            return DB.StreamRead(streamPositions, countPerStream, flags);
        }

        public virtual StreamEntry[] StreamReadGroup(RedisKey key, RedisValue groupName, RedisValue consumerName, RedisValue? position, int? count, CommandFlags flags)
        {
            return DB.StreamReadGroup(key, groupName, consumerName, position, count, flags);
        }

        public virtual StreamEntry[] StreamReadGroup(RedisKey key, RedisValue groupName, RedisValue consumerName, RedisValue? position = null, int? count = null, bool noAck = false, CommandFlags flags = CommandFlags.None)
        {
            return DB.StreamReadGroup(key, groupName, consumerName, position, count, noAck, flags);
        }

        public virtual RedisStream[] StreamReadGroup(StreamPosition[] streamPositions, RedisValue groupName, RedisValue consumerName, int? countPerStream, CommandFlags flags)
        {
            return DB.StreamReadGroup(streamPositions, groupName, consumerName, countPerStream, flags);
        }

        public virtual RedisStream[] StreamReadGroup(StreamPosition[] streamPositions, RedisValue groupName, RedisValue consumerName, int? countPerStream = null, bool noAck = false, CommandFlags flags = CommandFlags.None)
        {
            return DB.StreamReadGroup(streamPositions, groupName, consumerName, countPerStream, noAck, flags);
        }

        public virtual long StreamTrim(RedisKey key, int maxLength, bool useApproximateMaxLength = false, CommandFlags flags = CommandFlags.None)
        {
            return DB.StreamTrim(key, maxLength, useApproximateMaxLength, flags);
        }

        public virtual long StringAppend(RedisKey key, RedisValue value, CommandFlags flags = CommandFlags.None)
        {
            return DB.StringAppend(key, value, flags);
        }

        public virtual long StringBitCount(RedisKey key, long start = 0, long end = -1, CommandFlags flags = CommandFlags.None)
        {
            return DB.StringBitCount(key, start, end, flags);
        }

        public virtual long StringBitOperation(Bitwise operation, RedisKey destination, RedisKey first, RedisKey second = default, CommandFlags flags = CommandFlags.None)
        {
            return DB.StringBitOperation(operation, destination, first, second, flags);
        }

        public virtual long StringBitOperation(Bitwise operation, RedisKey destination, RedisKey[] keys, CommandFlags flags = CommandFlags.None)
        {
            return DB.StringBitOperation(operation, destination, keys, flags);
        }

        public virtual long StringBitPosition(RedisKey key, bool bit, long start = 0, long end = -1, CommandFlags flags = CommandFlags.None)
        {
            return DB.StringBitPosition(key, bit, start, end, flags);
        }

        public virtual long StringDecrement(RedisKey key, long value = 1, CommandFlags flags = CommandFlags.None)
        {
            return DB.StringDecrement(key, value, flags);
        }

        public virtual double StringDecrement(RedisKey key, double value, CommandFlags flags = CommandFlags.None)
        {
            return DB.StringDecrement(key, value, flags);
        }

        public virtual RedisValue StringGet(RedisKey key, CommandFlags flags = CommandFlags.None)
        {
            return DB.StringGet(key, flags);
        }

        public virtual RedisValue[] StringGet(RedisKey[] keys, CommandFlags flags = CommandFlags.None)
        {
            return DB.StringGet(keys, flags);
        }

        public virtual Lease<byte> StringGetLease(RedisKey key, CommandFlags flags = CommandFlags.None)
        {
            return DB.StringGetLease(key, flags);
        }

        public virtual bool StringGetBit(RedisKey key, long offset, CommandFlags flags = CommandFlags.None)
        {
            return DB.StringGetBit(key, offset, flags);
        }

        public virtual RedisValue StringGetDelete(RedisKey key, CommandFlags flags = CommandFlags.None)
        {
            return DB.StringGetDelete(key, flags);
        }

        public virtual RedisValue StringGetRange(RedisKey key, long start, long end, CommandFlags flags = CommandFlags.None)
        {
            return DB.StringGetRange(key, start, end, flags);
        }

        public virtual RedisValue StringGetSet(RedisKey key, RedisValue value, CommandFlags flags = CommandFlags.None)
        {
            return DB.StringGetSet(key, value, flags);
        }

        public virtual RedisValueWithExpiry StringGetWithExpiry(RedisKey key, CommandFlags flags = CommandFlags.None)
        {
            return DB.StringGetWithExpiry(key, flags);
        }

        public virtual long StringIncrement(RedisKey key, long value = 1, CommandFlags flags = CommandFlags.None)
        {
            return DB.StringIncrement(key, value, flags);
        }

        public virtual double StringIncrement(RedisKey key, double value, CommandFlags flags = CommandFlags.None)
        {
            return DB.StringIncrement(key, value, flags);
        }

        public virtual long StringLength(RedisKey key, CommandFlags flags = CommandFlags.None)
        {
            return DB.StringLength(key, flags);
        }

        public virtual bool StringSet(RedisKey key, RedisValue value, TimeSpan? expiry = null, When when = When.Always, CommandFlags flags = CommandFlags.None)
        {
            return DB.StringSet(key, value, expiry, when, flags);
        }

        public virtual bool StringSet(KeyValuePair<RedisKey, RedisValue>[] values, When when = When.Always, CommandFlags flags = CommandFlags.None)
        {
            return DB.StringSet(values, when, flags);
        }

        public virtual bool StringSetBit(RedisKey key, long offset, bool bit, CommandFlags flags = CommandFlags.None)
        {
            return DB.StringSetBit(key, offset, bit, flags);
        }

        public virtual RedisValue StringSetRange(RedisKey key, long offset, RedisValue value, CommandFlags flags = CommandFlags.None)
        {
            return DB.StringSetRange(key, offset, value, flags);
        }

        public virtual bool KeyTouch(RedisKey key, CommandFlags flags = CommandFlags.None)
        {
            return DB.KeyTouch(key, flags);
        }

        public virtual long KeyTouch(RedisKey[] keys, CommandFlags flags = CommandFlags.None)
        {
            return DB.KeyTouch(keys, flags);
        }

        public virtual TimeSpan Ping(CommandFlags flags = CommandFlags.None)
        {
            return DB.Ping(flags);
        }

        public virtual bool IsConnected(RedisKey key, CommandFlags flags = CommandFlags.None)
        {
            return DB.IsConnected(key, flags);
        }

        public virtual async Task KeyMigrateAsync(RedisKey key, EndPoint toServer, int toDatabase = 0, int timeoutMilliseconds = 0, MigrateOptions migrateOptions = MigrateOptions.None, CommandFlags flags = CommandFlags.None)
        {
            await DB.KeyMigrateAsync(key, toServer, toDatabase, timeoutMilliseconds, migrateOptions, flags);
        }

        public virtual async Task<RedisValue> DebugObjectAsync(RedisKey key, CommandFlags flags = CommandFlags.None)
        {
            return await DB.DebugObjectAsync(key, flags);
        }

        public virtual async Task<bool> GeoAddAsync(RedisKey key, double longitude, double latitude, RedisValue member, CommandFlags flags = CommandFlags.None)
        {
            return await DB.GeoAddAsync(key, longitude, latitude, member, flags);
        }

        public virtual async Task<bool> GeoAddAsync(RedisKey key, GeoEntry value, CommandFlags flags = CommandFlags.None)
        {
            return await DB.GeoAddAsync(key, value, flags);
        }

        public virtual async Task<long> GeoAddAsync(RedisKey key, GeoEntry[] values, CommandFlags flags = CommandFlags.None)
        {
            return await DB.GeoAddAsync(key, values, flags);
        }

        public virtual async Task<bool> GeoRemoveAsync(RedisKey key, RedisValue member, CommandFlags flags = CommandFlags.None)
        {
            return await DB.GeoRemoveAsync(key, member, flags);
        }

        public virtual async Task<double?> GeoDistanceAsync(RedisKey key, RedisValue member1, RedisValue member2, GeoUnit unit = GeoUnit.Meters, CommandFlags flags = CommandFlags.None)
        {
            return await DB.GeoDistanceAsync(key, member1, member2, unit, flags);
        }

        public virtual async Task<string[]> GeoHashAsync(RedisKey key, RedisValue[] members, CommandFlags flags = CommandFlags.None)
        {
            return await DB.GeoHashAsync(key, members, flags);
        }

        public virtual async Task<string> GeoHashAsync(RedisKey key, RedisValue member, CommandFlags flags = CommandFlags.None)
        {
            return await DB.GeoHashAsync(key, member, flags);
        }

        public virtual async Task<GeoPosition?[]> GeoPositionAsync(RedisKey key, RedisValue[] members, CommandFlags flags = CommandFlags.None)
        {
            return await DB.GeoPositionAsync(key, members, flags);
        }

        public virtual async Task<GeoPosition?> GeoPositionAsync(RedisKey key, RedisValue member, CommandFlags flags = CommandFlags.None)
        {
            return await DB.GeoPositionAsync(key, member, flags);
        }

        public virtual async Task<GeoRadiusResult[]> GeoRadiusAsync(RedisKey key, RedisValue member, double radius, GeoUnit unit = GeoUnit.Meters, int count = -1, Order? order = null, GeoRadiusOptions options = GeoRadiusOptions.Default, CommandFlags flags = CommandFlags.None)
        {
            return await DB.GeoRadiusAsync(key, member, radius, unit, count, order, options, flags);
        }

        public virtual async Task<GeoRadiusResult[]> GeoRadiusAsync(RedisKey key, double longitude, double latitude, double radius, GeoUnit unit = GeoUnit.Meters, int count = -1, Order? order = null, GeoRadiusOptions options = GeoRadiusOptions.Default, CommandFlags flags = CommandFlags.None)
        {
            return await DB.GeoRadiusAsync(key, longitude, latitude, radius, unit, count, order, options, flags);
        }

        public virtual async Task<long> HashDecrementAsync(RedisKey key, RedisValue hashField, long value = 1, CommandFlags flags = CommandFlags.None)
        {
            return await DB.HashDecrementAsync(key, hashField, value, flags);
        }

        public virtual async Task<double> HashDecrementAsync(RedisKey key, RedisValue hashField, double value, CommandFlags flags = CommandFlags.None)
        {
            return await DB.HashDecrementAsync(key, hashField, value, flags);
        }

        public virtual async Task<bool> HashDeleteAsync(RedisKey key, RedisValue hashField, CommandFlags flags = CommandFlags.None)
        {
            return await DB.HashDeleteAsync(key, hashField, flags);
        }

        public virtual async Task<long> HashDeleteAsync(RedisKey key, RedisValue[] hashFields, CommandFlags flags = CommandFlags.None)
        {
            return await DB.HashDeleteAsync(key, hashFields, flags);
        }

        public virtual async Task<bool> HashExistsAsync(RedisKey key, RedisValue hashField, CommandFlags flags = CommandFlags.None)
        {
            return await DB.HashExistsAsync(key, hashField, flags);
        }

        public virtual async Task<RedisValue> HashGetAsync(RedisKey key, RedisValue hashField, CommandFlags flags = CommandFlags.None)
        {
            return await DB.HashGetAsync(key, hashField, flags);
        }

        public virtual async Task<Lease<byte>> HashGetLeaseAsync(RedisKey key, RedisValue hashField, CommandFlags flags = CommandFlags.None)
        {
            return await DB.HashGetLeaseAsync(key, hashField, flags);
        }

        public virtual async Task<RedisValue[]> HashGetAsync(RedisKey key, RedisValue[] hashFields, CommandFlags flags = CommandFlags.None)
        {
            return await DB.HashGetAsync(key, hashFields, flags);
        }

        public virtual async Task<HashEntry[]> HashGetAllAsync(RedisKey key, CommandFlags flags = CommandFlags.None)
        {
            return await DB.HashGetAllAsync(key, flags);
        }

        public virtual async Task<long> HashIncrementAsync(RedisKey key, RedisValue hashField, long value = 1, CommandFlags flags = CommandFlags.None)
        {
            return await DB.HashIncrementAsync(key, hashField, value, flags);
        }

        public virtual async Task<double> HashIncrementAsync(RedisKey key, RedisValue hashField, double value, CommandFlags flags = CommandFlags.None)
        {
            return await DB.HashIncrementAsync(key, hashField, value, flags);
        }

        public virtual async Task<RedisValue[]> HashKeysAsync(RedisKey key, CommandFlags flags = CommandFlags.None)
        {
            return await DB.HashKeysAsync(key, flags);
        }

        public virtual async Task<long> HashLengthAsync(RedisKey key, CommandFlags flags = CommandFlags.None)
        {
            return await DB.HashLengthAsync(key, flags);
        }

        public virtual IAsyncEnumerable<HashEntry> HashScanAsync(RedisKey key, RedisValue pattern = default, int pageSize = 250, long cursor = 0, int pageOffset = 0, CommandFlags flags = CommandFlags.None)
        {
            return DB.HashScanAsync(key, pattern, pageSize, cursor, pageOffset, flags);
        }

        public virtual async Task HashSetAsync(RedisKey key, HashEntry[] hashFields, CommandFlags flags = CommandFlags.None)
        {
            await DB.HashSetAsync(key, hashFields, flags);
        }

        public virtual async Task<bool> HashSetAsync(RedisKey key, RedisValue hashField, RedisValue value, When when = When.Always, CommandFlags flags = CommandFlags.None)
        {
            return await DB.HashSetAsync(key, hashField, value, when, flags);
        }

        public virtual async Task<long> HashStringLengthAsync(RedisKey key, RedisValue hashField, CommandFlags flags = CommandFlags.None)
        {
            return await DB.HashStringLengthAsync(key, hashField, flags);
        }

        public virtual async Task<RedisValue[]> HashValuesAsync(RedisKey key, CommandFlags flags = CommandFlags.None)
        {
            return await DB.HashValuesAsync(key, flags);
        }

        public virtual async Task<bool> HyperLogLogAddAsync(RedisKey key, RedisValue value, CommandFlags flags = CommandFlags.None)
        {
            return await DB.HyperLogLogAddAsync(key, value, flags);
        }

        public virtual async Task<bool> HyperLogLogAddAsync(RedisKey key, RedisValue[] values, CommandFlags flags = CommandFlags.None)
        {
            return await DB.HyperLogLogAddAsync(key, values, flags);
        }

        public virtual async Task<long> HyperLogLogLengthAsync(RedisKey key, CommandFlags flags = CommandFlags.None)
        {
            return await DB.HyperLogLogLengthAsync(key, flags);
        }

        public virtual async Task<long> HyperLogLogLengthAsync(RedisKey[] keys, CommandFlags flags = CommandFlags.None)
        {
            return await DB.HyperLogLogLengthAsync(keys, flags);
        }

        public virtual async Task HyperLogLogMergeAsync(RedisKey destination, RedisKey first, RedisKey second, CommandFlags flags = CommandFlags.None)
        {
            await DB.HyperLogLogMergeAsync(destination, first, second, flags);
        }

        public virtual async Task HyperLogLogMergeAsync(RedisKey destination, RedisKey[] sourceKeys, CommandFlags flags = CommandFlags.None)
        {
            await DB.HyperLogLogMergeAsync(destination, sourceKeys, flags);
        }

        public virtual async Task<EndPoint> IdentifyEndpointAsync(RedisKey key = default, CommandFlags flags = CommandFlags.None)
        {
            return await DB.IdentifyEndpointAsync(key, flags);
        }

        public virtual async Task<bool> KeyDeleteAsync(RedisKey key, CommandFlags flags = CommandFlags.None)
        {
            return await DB.KeyDeleteAsync(key, flags);
        }

        public virtual async Task<long> KeyDeleteAsync(RedisKey[] keys, CommandFlags flags = CommandFlags.None)
        {
            return await DB.KeyDeleteAsync(keys, flags);
        }

        public virtual async Task<byte[]> KeyDumpAsync(RedisKey key, CommandFlags flags = CommandFlags.None)
        {
            return await DB.KeyDumpAsync(key, flags);
        }

        public virtual async Task<bool> KeyExistsAsync(RedisKey key, CommandFlags flags = CommandFlags.None)
        {
            return await DB.KeyExistsAsync(key, flags);
        }

        public virtual async Task<long> KeyExistsAsync(RedisKey[] keys, CommandFlags flags = CommandFlags.None)
        {
            return await DB.KeyExistsAsync(keys, flags);
        }

        public virtual async Task<bool> KeyExpireAsync(RedisKey key, TimeSpan? expiry, CommandFlags flags = CommandFlags.None)
        {
            return await DB.KeyExpireAsync(key, expiry, flags);
        }

        public virtual async Task<bool> KeyExpireAsync(RedisKey key, DateTime? expiry, CommandFlags flags = CommandFlags.None)
        {
            return await DB.KeyExpireAsync(key, expiry, flags);
        }

        public virtual async Task<TimeSpan?> KeyIdleTimeAsync(RedisKey key, CommandFlags flags = CommandFlags.None)
        {
            return await DB.KeyIdleTimeAsync(key, flags);
        }

        public virtual async Task<bool> KeyMoveAsync(RedisKey key, int database, CommandFlags flags = CommandFlags.None)
        {
            return await DB.KeyMoveAsync(key, database, flags);
        }

        public virtual async Task<bool> KeyPersistAsync(RedisKey key, CommandFlags flags = CommandFlags.None)
        {
            return await DB.KeyPersistAsync(key, flags);
        }

        public virtual async Task<RedisKey> KeyRandomAsync(CommandFlags flags = CommandFlags.None)
        {
            return await DB.KeyRandomAsync(flags);
        }

        public virtual async Task<bool> KeyRenameAsync(RedisKey key, RedisKey newKey, When when = When.Always, CommandFlags flags = CommandFlags.None)
        {
            return await DB.KeyRenameAsync(key, newKey, when, flags);
        }

        public virtual async Task KeyRestoreAsync(RedisKey key, byte[] value, TimeSpan? expiry = null, CommandFlags flags = CommandFlags.None)
        {
            await DB.KeyRestoreAsync(key, value, expiry, flags);
        }

        public virtual async Task<TimeSpan?> KeyTimeToLiveAsync(RedisKey key, CommandFlags flags = CommandFlags.None)
        {
            return await DB.KeyTimeToLiveAsync(key, flags);
        }

        public virtual async Task<RedisType> KeyTypeAsync(RedisKey key, CommandFlags flags = CommandFlags.None)
        {
            return await DB.KeyTypeAsync(key, flags);
        }

        public virtual async Task<RedisValue> ListGetByIndexAsync(RedisKey key, long index, CommandFlags flags = CommandFlags.None)
        {
            return await DB.ListGetByIndexAsync(key, index, flags);
        }

        public virtual async Task<long> ListInsertAfterAsync(RedisKey key, RedisValue pivot, RedisValue value, CommandFlags flags = CommandFlags.None)
        {
            return await DB.ListInsertAfterAsync(key, pivot, value, flags);
        }

        public virtual async Task<long> ListInsertBeforeAsync(RedisKey key, RedisValue pivot, RedisValue value, CommandFlags flags = CommandFlags.None)
        {
            return await DB.ListInsertBeforeAsync(key, pivot, value, flags);
        }

        public virtual async Task<RedisValue> ListLeftPopAsync(RedisKey key, CommandFlags flags = CommandFlags.None)
        {
            return await DB.ListLeftPopAsync(key, flags);
        }

        public virtual async Task<RedisValue[]> ListLeftPopAsync(RedisKey key, long count, CommandFlags flags = CommandFlags.None)
        {
            return await DB.ListLeftPopAsync(key, count, flags);
        }

        public virtual async Task<long> ListLeftPushAsync(RedisKey key, RedisValue value, When when = When.Always, CommandFlags flags = CommandFlags.None)
        {
            return await DB.ListLeftPushAsync(key, value, when, flags);
        }

        public virtual async Task<long> ListLeftPushAsync(RedisKey key, RedisValue[] values, When when = When.Always, CommandFlags flags = CommandFlags.None)
        {
            return await DB.ListLeftPushAsync(key, values, when, flags);
        }

        public virtual async Task<long> ListLeftPushAsync(RedisKey key, RedisValue[] values, CommandFlags flags)
        {
            return await DB.ListLeftPushAsync(key, values, flags);
        }

        public virtual async Task<long> ListLengthAsync(RedisKey key, CommandFlags flags = CommandFlags.None)
        {
            return await DB.ListLengthAsync(key, flags);
        }

        public virtual async Task<RedisValue[]> ListRangeAsync(RedisKey key, long start = 0, long stop = -1, CommandFlags flags = CommandFlags.None)
        {
            return await DB.ListRangeAsync(key, start, stop, flags);
        }

        public virtual async Task<long> ListRemoveAsync(RedisKey key, RedisValue value, long count = 0, CommandFlags flags = CommandFlags.None)
        {
            return await DB.ListRemoveAsync(key, value, count, flags);
        }

        public virtual async Task<RedisValue> ListRightPopAsync(RedisKey key, CommandFlags flags = CommandFlags.None)
        {
            return await DB.ListRightPopAsync(key, flags);
        }


        public virtual async Task<RedisValue[]> ListRightPopAsync(RedisKey key, long count, CommandFlags flags = CommandFlags.None)
        {
            return await DB.ListRightPopAsync(key, count, flags);
        }

        public virtual async Task<RedisValue> ListRightPopLeftPushAsync(RedisKey source, RedisKey destination, CommandFlags flags = CommandFlags.None)
        {
            return await DB.ListRightPopLeftPushAsync(source, destination, flags);
        }

        public virtual async Task<long> ListRightPushAsync(RedisKey key, RedisValue value, When when = When.Always, CommandFlags flags = CommandFlags.None)
        {
            return await DB.ListRightPushAsync(key, value, when, flags);
        }

        public virtual async Task<long> ListRightPushAsync(RedisKey key, RedisValue[] values, When when = When.Always, CommandFlags flags = CommandFlags.None)
        {
            return await DB.ListRightPushAsync(key, values, when, flags);
        }

        public virtual async Task<long> ListRightPushAsync(RedisKey key, RedisValue[] values, CommandFlags flags)
        {
            return await DB.ListRightPushAsync(key, values, flags);
        }

        public virtual async Task ListSetByIndexAsync(RedisKey key, long index, RedisValue value, CommandFlags flags = CommandFlags.None)
        {
            await DB.ListSetByIndexAsync(key, index, value, flags);
        }

        public virtual async Task ListTrimAsync(RedisKey key, long start, long stop, CommandFlags flags = CommandFlags.None)
        {
            await DB.ListTrimAsync(key, start, stop, flags);
        }

        public virtual async Task<bool> LockExtendAsync(RedisKey key, RedisValue value, TimeSpan expiry, CommandFlags flags = CommandFlags.None)
        {
            return await DB.LockExtendAsync(key, value, expiry, flags);
        }

        public virtual async Task<RedisValue> LockQueryAsync(RedisKey key, CommandFlags flags = CommandFlags.None)
        {
            return await DB.LockQueryAsync(key, flags);
        }

        public virtual async Task<bool> LockReleaseAsync(RedisKey key, RedisValue value, CommandFlags flags = CommandFlags.None)
        {
            return await DB.LockReleaseAsync(key, value, flags);
        }

        public virtual async Task<bool> LockTakeAsync(RedisKey key, RedisValue value, TimeSpan expiry, CommandFlags flags = CommandFlags.None)
        {
            return await DB.LockTakeAsync(key, value, expiry, flags);
        }

        public virtual async Task<long> PublishAsync(RedisChannel channel, RedisValue message, CommandFlags flags = CommandFlags.None)
        {
            return await DB.PublishAsync(channel, message, flags);
        }

        public virtual async Task<RedisResult> ExecuteAsync(string command, params object[] args)
        {
            return await DB.ExecuteAsync(command, args);
        }

        public virtual async Task<RedisResult> ExecuteAsync(string command, ICollection<object> args, CommandFlags flags = CommandFlags.None)
        {
            return await DB.ExecuteAsync(command, args, flags);
        }

        public virtual async Task<RedisResult> ScriptEvaluateAsync(string script, RedisKey[] keys = null, RedisValue[] values = null, CommandFlags flags = CommandFlags.None)
        {
            return await DB.ScriptEvaluateAsync(script, keys, values, flags);
        }

        public virtual async Task<RedisResult> ScriptEvaluateAsync(byte[] hash, RedisKey[] keys = null, RedisValue[] values = null, CommandFlags flags = CommandFlags.None)
        {
            return await DB.ScriptEvaluateAsync(hash, keys, values, flags);
        }

        public virtual async Task<RedisResult> ScriptEvaluateAsync(LuaScript script, object parameters = null, CommandFlags flags = CommandFlags.None)
        {
            return await DB.ScriptEvaluateAsync(script, parameters, flags);
        }

        public virtual async Task<RedisResult> ScriptEvaluateAsync(LoadedLuaScript script, object parameters = null, CommandFlags flags = CommandFlags.None)
        {
            return await DB.ScriptEvaluateAsync(script, parameters, flags);
        }

        public virtual async Task<bool> SetAddAsync(RedisKey key, RedisValue value, CommandFlags flags = CommandFlags.None)
        {
            return await DB.SetAddAsync(key, value, flags);
        }

        public virtual async Task<long> SetAddAsync(RedisKey key, RedisValue[] values, CommandFlags flags = CommandFlags.None)
        {
            return await DB.SetAddAsync(key, values, flags);
        }

        public virtual async Task<RedisValue[]> SetCombineAsync(SetOperation operation, RedisKey first, RedisKey second, CommandFlags flags = CommandFlags.None)
        {
            return await DB.SetCombineAsync(operation, first, second, flags);
        }

        public virtual async Task<RedisValue[]> SetCombineAsync(SetOperation operation, RedisKey[] keys, CommandFlags flags = CommandFlags.None)
        {
            return await DB.SetCombineAsync(operation, keys, flags);
        }

        public virtual async Task<long> SetCombineAndStoreAsync(SetOperation operation, RedisKey destination, RedisKey first, RedisKey second, CommandFlags flags = CommandFlags.None)
        {
            return await DB.SetCombineAndStoreAsync(operation, destination, first, second, flags);
        }

        public virtual async Task<long> SetCombineAndStoreAsync(SetOperation operation, RedisKey destination, RedisKey[] keys, CommandFlags flags = CommandFlags.None)
        {
            return await DB.SetCombineAndStoreAsync(operation, destination, keys, flags);
        }

        public virtual async Task<bool> SetContainsAsync(RedisKey key, RedisValue value, CommandFlags flags = CommandFlags.None)
        {
            return await DB.SetContainsAsync(key, value, flags);
        }

        public virtual async Task<long> SetLengthAsync(RedisKey key, CommandFlags flags = CommandFlags.None)
        {
            return await DB.SetLengthAsync(key, flags);
        }

        public virtual async Task<RedisValue[]> SetMembersAsync(RedisKey key, CommandFlags flags = CommandFlags.None)
        {
            return await DB.SetMembersAsync(key, flags);
        }

        public virtual async Task<bool> SetMoveAsync(RedisKey source, RedisKey destination, RedisValue value, CommandFlags flags = CommandFlags.None)
        {
            return await DB.SetMoveAsync(source, destination, value, flags);
        }

        public virtual async Task<RedisValue> SetPopAsync(RedisKey key, CommandFlags flags = CommandFlags.None)
        {
            return await DB.SetPopAsync(key, flags);
        }

        public virtual async Task<RedisValue[]> SetPopAsync(RedisKey key, long count, CommandFlags flags = CommandFlags.None)
        {
            return await DB.SetPopAsync(key, count, flags);
        }

        public virtual async Task<RedisValue> SetRandomMemberAsync(RedisKey key, CommandFlags flags = CommandFlags.None)
        {
            return await DB.SetRandomMemberAsync(key, flags);
        }

        public virtual async Task<RedisValue[]> SetRandomMembersAsync(RedisKey key, long count, CommandFlags flags = CommandFlags.None)
        {
            return await DB.SetRandomMembersAsync(key, count, flags);
        }

        public virtual async Task<bool> SetRemoveAsync(RedisKey key, RedisValue value, CommandFlags flags = CommandFlags.None)
        {
            return await DB.SetRemoveAsync(key, value, flags);
        }

        public virtual async Task<long> SetRemoveAsync(RedisKey key, RedisValue[] values, CommandFlags flags = CommandFlags.None)
        {
            return await DB.SetRemoveAsync(key, values, flags);
        }

        public virtual async Task<RedisValue[]> SortAsync(RedisKey key, long skip = 0, long take = -1, Order order = Order.Ascending, SortType sortType = SortType.Numeric, RedisValue by = default, RedisValue[] get = null, CommandFlags flags = CommandFlags.None)
        {
            return await DB.SortAsync(key, skip, take, order, sortType, by, get, flags);
        }

        public virtual async Task<long> SortAndStoreAsync(RedisKey destination, RedisKey key, long skip = 0, long take = -1, Order order = Order.Ascending, SortType sortType = SortType.Numeric, RedisValue by = default, RedisValue[] get = null, CommandFlags flags = CommandFlags.None)
        {
            return await DB.SortAndStoreAsync(destination, key, skip, take, order, sortType, by, get, flags);
        }

        public virtual async Task<bool> SortedSetAddAsync(RedisKey key, RedisValue member, double score, CommandFlags flags)
        {
            return await DB.SortedSetAddAsync(key, member, score, flags);
        }

        public virtual async Task<bool> SortedSetAddAsync(RedisKey key, RedisValue member, double score, When when = When.Always, CommandFlags flags = CommandFlags.None)
        {
            return await DB.SortedSetAddAsync(key, member, score, when, flags);
        }

        public virtual async Task<long> SortedSetAddAsync(RedisKey key, SortedSetEntry[] values, CommandFlags flags)
        {
            return await DB.SortedSetAddAsync(key, values, flags);
        }

        public virtual async Task<long> SortedSetAddAsync(RedisKey key, SortedSetEntry[] values, When when = When.Always, CommandFlags flags = CommandFlags.None)
        {
            return await DB.SortedSetAddAsync(key, values, flags);
        }

        public virtual async Task<long> SortedSetCombineAndStoreAsync(SetOperation operation, RedisKey destination, RedisKey first, RedisKey second, Aggregate aggregate = Aggregate.Sum, CommandFlags flags = CommandFlags.None)
        {
            return await DB.SortedSetCombineAndStoreAsync(operation, destination, first, second, aggregate, flags);
        }

        public virtual async Task<long> SortedSetCombineAndStoreAsync(SetOperation operation, RedisKey destination, RedisKey[] keys, double[] weights = null, Aggregate aggregate = Aggregate.Sum, CommandFlags flags = CommandFlags.None)
        {
            return await DB.SortedSetCombineAndStoreAsync(operation, destination, keys, weights, aggregate, flags);
        }

        public virtual async Task<double> SortedSetDecrementAsync(RedisKey key, RedisValue member, double value, CommandFlags flags = CommandFlags.None)
        {
            return await DB.SortedSetDecrementAsync(key, member, value, flags);
        }

        public virtual async Task<double> SortedSetIncrementAsync(RedisKey key, RedisValue member, double value, CommandFlags flags = CommandFlags.None)
        {
            return await DB.SortedSetIncrementAsync(key, member, value, flags);
        }

        public virtual async Task<long> SortedSetLengthAsync(RedisKey key, double min = double.NegativeInfinity, double max = double.PositiveInfinity, Exclude exclude = Exclude.None, CommandFlags flags = CommandFlags.None)
        {
            return await DB.SortedSetLengthAsync(key, min, max, exclude, flags);
        }

        public virtual async Task<long> SortedSetLengthByValueAsync(RedisKey key, RedisValue min, RedisValue max, Exclude exclude = Exclude.None, CommandFlags flags = CommandFlags.None)
        {
            return await DB.SortedSetLengthByValueAsync(key, min, max, exclude, flags);
        }

        public virtual async Task<RedisValue[]> SortedSetRangeByRankAsync(RedisKey key, long start = 0, long stop = -1, Order order = Order.Ascending, CommandFlags flags = CommandFlags.None)
        {
            return await DB.SortedSetRangeByRankAsync(key, start, stop, order, flags);
        }

        public virtual async Task<SortedSetEntry[]> SortedSetRangeByRankWithScoresAsync(RedisKey key, long start = 0, long stop = -1, Order order = Order.Ascending, CommandFlags flags = CommandFlags.None)
        {
            return await DB.SortedSetRangeByRankWithScoresAsync(key, start, stop, order, flags);
        }

        public virtual async Task<RedisValue[]> SortedSetRangeByScoreAsync(RedisKey key, double start = double.NegativeInfinity, double stop = double.PositiveInfinity, Exclude exclude = Exclude.None, Order order = Order.Ascending, long skip = 0, long take = -1, CommandFlags flags = CommandFlags.None)
        {
            return await DB.SortedSetRangeByScoreAsync(key, start, stop, exclude, order, skip, take, flags);
        }

        public virtual async Task<SortedSetEntry[]> SortedSetRangeByScoreWithScoresAsync(RedisKey key, double start = double.NegativeInfinity, double stop = double.PositiveInfinity, Exclude exclude = Exclude.None, Order order = Order.Ascending, long skip = 0, long take = -1, CommandFlags flags = CommandFlags.None)
        {
            return await DB.SortedSetRangeByScoreWithScoresAsync(key, start, stop, exclude, order, skip, take, flags);
        }

        public virtual async Task<RedisValue[]> SortedSetRangeByValueAsync(RedisKey key, RedisValue min, RedisValue max, Exclude exclude, long skip, long take = -1, CommandFlags flags = CommandFlags.None)
        {
            return await DB.SortedSetRangeByValueAsync(key, min, max, exclude, skip, take, flags);
        }

        public virtual async Task<RedisValue[]> SortedSetRangeByValueAsync(RedisKey key, RedisValue min = default, RedisValue max = default, Exclude exclude = Exclude.None, Order order = Order.Ascending, long skip = 0, long take = -1, CommandFlags flags = CommandFlags.None)
        {
            return await DB.SortedSetRangeByValueAsync(key, min, max, exclude, order, skip, take, flags);
        }

        public virtual async Task<long?> SortedSetRankAsync(RedisKey key, RedisValue member, Order order = Order.Ascending, CommandFlags flags = CommandFlags.None)
        {
            return await DB.SortedSetRankAsync(key, member, order, flags);
        }

        public virtual async Task<bool> SortedSetRemoveAsync(RedisKey key, RedisValue member, CommandFlags flags = CommandFlags.None)
        {
            return await DB.SortedSetRemoveAsync(key, member, flags);
        }

        public virtual async Task<long> SortedSetRemoveAsync(RedisKey key, RedisValue[] members, CommandFlags flags = CommandFlags.None)
        {
            return await DB.SortedSetRemoveAsync(key, members, flags);
        }

        public virtual async Task<long> SortedSetRemoveRangeByRankAsync(RedisKey key, long start, long stop, CommandFlags flags = CommandFlags.None)
        {
            return await DB.SortedSetRemoveRangeByRankAsync(key, start, stop, flags);
        }

        public virtual async Task<long> SortedSetRemoveRangeByScoreAsync(RedisKey key, double start, double stop, Exclude exclude = Exclude.None, CommandFlags flags = CommandFlags.None)
        {
            return await DB.SortedSetRemoveRangeByScoreAsync(key, start, stop, exclude, flags);
        }

        public virtual async Task<long> SortedSetRemoveRangeByValueAsync(RedisKey key, RedisValue min, RedisValue max, Exclude exclude = Exclude.None, CommandFlags flags = CommandFlags.None)
        {
            return await DB.SortedSetRemoveRangeByValueAsync(key, min, max, exclude, flags);
        }

        public virtual IAsyncEnumerable<RedisValue> SetScanAsync(RedisKey key, RedisValue pattern = default, int pageSize = 250, long cursor = 0, int pageOffset = 0, CommandFlags flags = CommandFlags.None)
        {
            return DB.SetScanAsync(key, pattern, pageSize, cursor, pageOffset, flags);
        }

        public virtual IAsyncEnumerable<SortedSetEntry> SortedSetScanAsync(RedisKey key, RedisValue pattern = default, int pageSize = 250, long cursor = 0, int pageOffset = 0, CommandFlags flags = CommandFlags.None)
        {
            return DB.SortedSetScanAsync(key, pattern, pageSize, cursor, pageOffset, flags);
        }

        public virtual async Task<double?> SortedSetScoreAsync(RedisKey key, RedisValue member, CommandFlags flags = CommandFlags.None)
        {
            return await DB.SortedSetScoreAsync(key, member, flags);
        }

        public virtual async Task<SortedSetEntry?> SortedSetPopAsync(RedisKey key, Order order = Order.Ascending, CommandFlags flags = CommandFlags.None)
        {
            return await DB.SortedSetPopAsync(key, order, flags);
        }

        public virtual async Task<SortedSetEntry[]> SortedSetPopAsync(RedisKey key, long count, Order order = Order.Ascending, CommandFlags flags = CommandFlags.None)
        {
            return await DB.SortedSetPopAsync(key, count, order, flags);
        }

        public virtual async Task<long> StreamAcknowledgeAsync(RedisKey key, RedisValue groupName, RedisValue messageId, CommandFlags flags = CommandFlags.None)
        {
            return await DB.StreamAcknowledgeAsync(key, groupName, messageId, flags);
        }

        public virtual async Task<long> StreamAcknowledgeAsync(RedisKey key, RedisValue groupName, RedisValue[] messageIds, CommandFlags flags = CommandFlags.None)
        {
            return await DB.StreamAcknowledgeAsync(key, groupName, messageIds, flags);
        }

        public virtual async Task<RedisValue> StreamAddAsync(RedisKey key, RedisValue streamField, RedisValue streamValue, RedisValue? messageId = null, int? maxLength = null, bool useApproximateMaxLength = false, CommandFlags flags = CommandFlags.None)
        {
            return await DB.StreamAddAsync(key, streamField, streamValue, messageId, maxLength, useApproximateMaxLength, flags);
        }

        public virtual async Task<RedisValue> StreamAddAsync(RedisKey key, NameValueEntry[] streamPairs, RedisValue? messageId = null, int? maxLength = null, bool useApproximateMaxLength = false, CommandFlags flags = CommandFlags.None)
        {
            return await DB.StreamAddAsync(key, streamPairs, messageId, maxLength, useApproximateMaxLength, flags);
        }

        public virtual async Task<StreamEntry[]> StreamClaimAsync(RedisKey key, RedisValue consumerGroup, RedisValue claimingConsumer, long minIdleTimeInMs, RedisValue[] messageIds, CommandFlags flags = CommandFlags.None)
        {
            return await DB.StreamClaimAsync(key, consumerGroup, claimingConsumer, minIdleTimeInMs, messageIds, flags);
        }

        public virtual async Task<RedisValue[]> StreamClaimIdsOnlyAsync(RedisKey key, RedisValue consumerGroup, RedisValue claimingConsumer, long minIdleTimeInMs, RedisValue[] messageIds, CommandFlags flags = CommandFlags.None)
        {
            return await DB.StreamClaimIdsOnlyAsync(key, consumerGroup, claimingConsumer, minIdleTimeInMs, messageIds, flags);
        }

        public virtual async Task<bool> StreamConsumerGroupSetPositionAsync(RedisKey key, RedisValue groupName, RedisValue position, CommandFlags flags = CommandFlags.None)
        {
            return await DB.StreamConsumerGroupSetPositionAsync(key, groupName, position, flags);
        }

        public virtual async Task<StreamConsumerInfo[]> StreamConsumerInfoAsync(RedisKey key, RedisValue groupName, CommandFlags flags = CommandFlags.None)
        {
            return await DB.StreamConsumerInfoAsync(key, groupName, flags);
        }

        public virtual async Task<bool> StreamCreateConsumerGroupAsync(RedisKey key, RedisValue groupName, RedisValue? position, CommandFlags flags)
        {
            return await DB.StreamCreateConsumerGroupAsync(key, groupName, position, flags);
        }

        public virtual async Task<bool> StreamCreateConsumerGroupAsync(RedisKey key, RedisValue groupName, RedisValue? position = null, bool createStream = true, CommandFlags flags = CommandFlags.None)
        {
            return await DB.StreamCreateConsumerGroupAsync(key, groupName, position, createStream, flags);
        }

        public virtual async Task<long> StreamDeleteAsync(RedisKey key, RedisValue[] messageIds, CommandFlags flags = CommandFlags.None)
        {
            return await DB.StreamDeleteAsync(key, messageIds, flags);
        }

        public virtual async Task<long> StreamDeleteConsumerAsync(RedisKey key, RedisValue groupName, RedisValue consumerName, CommandFlags flags = CommandFlags.None)
        {
            return await DB.StreamDeleteConsumerAsync(key, groupName, consumerName, flags);
        }

        public virtual async Task<bool> StreamDeleteConsumerGroupAsync(RedisKey key, RedisValue groupName, CommandFlags flags = CommandFlags.None)
        {
            return await DB.StreamDeleteConsumerGroupAsync(key, groupName, flags);
        }

        public virtual async Task<StreamGroupInfo[]> StreamGroupInfoAsync(RedisKey key, CommandFlags flags = CommandFlags.None)
        {
            return await DB.StreamGroupInfoAsync(key, flags);
        }

        public virtual async Task<StreamInfo> StreamInfoAsync(RedisKey key, CommandFlags flags = CommandFlags.None)
        {
            return await DB.StreamInfoAsync(key, flags);
        }

        public virtual async Task<long> StreamLengthAsync(RedisKey key, CommandFlags flags = CommandFlags.None)
        {
            return await DB.StreamLengthAsync(key, flags);
        }

        public virtual async Task<StreamPendingInfo> StreamPendingAsync(RedisKey key, RedisValue groupName, CommandFlags flags = CommandFlags.None)
        {
            return await DB.StreamPendingAsync(key, groupName, flags);
        }

        public virtual async Task<StreamPendingMessageInfo[]> StreamPendingMessagesAsync(RedisKey key, RedisValue groupName, int count, RedisValue consumerName, RedisValue? minId = null, RedisValue? maxId = null, CommandFlags flags = CommandFlags.None)
        {
            return await DB.StreamPendingMessagesAsync(key, groupName, count, consumerName, minId, maxId, flags);
        }

        public virtual async Task<StreamEntry[]> StreamRangeAsync(RedisKey key, RedisValue? minId = null, RedisValue? maxId = null, int? count = null, Order messageOrder = Order.Ascending, CommandFlags flags = CommandFlags.None)
        {
            return await DB.StreamRangeAsync(key, minId, maxId, count, messageOrder, flags);
        }

        public virtual async Task<StreamEntry[]> StreamReadAsync(RedisKey key, RedisValue position, int? count = null, CommandFlags flags = CommandFlags.None)
        {
            return await DB.StreamReadAsync(key, position, count, flags);
        }

        public virtual async Task<RedisStream[]> StreamReadAsync(StreamPosition[] streamPositions, int? countPerStream = null, CommandFlags flags = CommandFlags.None)
        {
            return await DB.StreamReadAsync(streamPositions, countPerStream, flags);
        }

        public virtual async Task<StreamEntry[]> StreamReadGroupAsync(RedisKey key, RedisValue groupName, RedisValue consumerName, RedisValue? position, int? count, CommandFlags flags)
        {
            return await DB.StreamReadGroupAsync(key, groupName, consumerName, position, count, flags);
        }

        public virtual async Task<StreamEntry[]> StreamReadGroupAsync(RedisKey key, RedisValue groupName, RedisValue consumerName, RedisValue? position = null, int? count = null, bool noAck = false, CommandFlags flags = CommandFlags.None)
        {
            return await DB.StreamReadGroupAsync(key, groupName, consumerName, position, count, noAck, flags);
        }

        public virtual async Task<RedisStream[]> StreamReadGroupAsync(StreamPosition[] streamPositions, RedisValue groupName, RedisValue consumerName, int? countPerStream, CommandFlags flags)
        {
            return await DB.StreamReadGroupAsync(streamPositions, groupName, consumerName, countPerStream, flags);
        }

        public virtual async Task<RedisStream[]> StreamReadGroupAsync(StreamPosition[] streamPositions, RedisValue groupName, RedisValue consumerName, int? countPerStream = null, bool noAck = false, CommandFlags flags = CommandFlags.None)
        {
            return await DB.StreamReadGroupAsync(streamPositions, groupName, consumerName, countPerStream, noAck, flags);
        }

        public virtual async Task<long> StreamTrimAsync(RedisKey key, int maxLength, bool useApproximateMaxLength = false, CommandFlags flags = CommandFlags.None)
        {
            return await DB.StreamTrimAsync(key, maxLength, useApproximateMaxLength, flags);
        }

        public virtual async Task<long> StringAppendAsync(RedisKey key, RedisValue value, CommandFlags flags = CommandFlags.None)
        {
            return await DB.StringAppendAsync(key, value, flags);
        }

        public virtual async Task<long> StringBitCountAsync(RedisKey key, long start = 0, long end = -1, CommandFlags flags = CommandFlags.None)
        {
            return await DB.StringBitCountAsync(key, start, end, flags);
        }

        public virtual async Task<long> StringBitOperationAsync(Bitwise operation, RedisKey destination, RedisKey first, RedisKey second = default, CommandFlags flags = CommandFlags.None)
        {
            return await DB.StringBitOperationAsync(operation, destination, first, second, flags);
        }

        public virtual async Task<long> StringBitOperationAsync(Bitwise operation, RedisKey destination, RedisKey[] keys, CommandFlags flags = CommandFlags.None)
        {
            return await DB.StringBitOperationAsync(operation, destination, keys, flags);
        }

        public virtual async Task<long> StringBitPositionAsync(RedisKey key, bool bit, long start = 0, long end = -1, CommandFlags flags = CommandFlags.None)
        {
            return await DB.StringBitPositionAsync(key, bit, start, end, flags);
        }

        public virtual async Task<long> StringDecrementAsync(RedisKey key, long value = 1, CommandFlags flags = CommandFlags.None)
        {
            return await DB.StringDecrementAsync(key, value, flags);
        }

        public virtual async Task<double> StringDecrementAsync(RedisKey key, double value, CommandFlags flags = CommandFlags.None)
        {
            return await DB.StringDecrementAsync(key, value, flags);
        }

        public virtual async Task<RedisValue> StringGetAsync(RedisKey key, CommandFlags flags = CommandFlags.None)
        {
            return await DB.StringGetAsync(key, flags);
        }

        public virtual async Task<RedisValue[]> StringGetAsync(RedisKey[] keys, CommandFlags flags = CommandFlags.None)
        {
            return await DB.StringGetAsync(keys, flags);
        }

        public virtual async Task<Lease<byte>> StringGetLeaseAsync(RedisKey key, CommandFlags flags = CommandFlags.None)
        {
            return await DB.StringGetLeaseAsync(key, flags);
        }

        public virtual async Task<bool> StringGetBitAsync(RedisKey key, long offset, CommandFlags flags = CommandFlags.None)
        {
            return await DB.StringGetBitAsync(key, offset, flags);
        }

        public virtual async Task<RedisValue> StringGetDeleteAsync(RedisKey key, CommandFlags flags = CommandFlags.None)
        {
            return await DB.StringGetDeleteAsync(key, flags);
        }

        public virtual async Task<RedisValue> StringGetRangeAsync(RedisKey key, long start, long end, CommandFlags flags = CommandFlags.None)
        {
            return await DB.StringGetRangeAsync(key, start, end, flags);
        }

        public virtual async Task<RedisValue> StringGetSetAsync(RedisKey key, RedisValue value, CommandFlags flags = CommandFlags.None)
        {
            return await DB.StringGetSetAsync(key, value, flags);
        }

        public virtual async Task<RedisValueWithExpiry> StringGetWithExpiryAsync(RedisKey key, CommandFlags flags = CommandFlags.None)
        {
            return await DB.StringGetWithExpiryAsync(key, flags);
        }

        public virtual async Task<long> StringIncrementAsync(RedisKey key, long value = 1, CommandFlags flags = CommandFlags.None)
        {
            return await DB.StringIncrementAsync(key, value, flags);
        }

        public virtual async Task<double> StringIncrementAsync(RedisKey key, double value, CommandFlags flags = CommandFlags.None)
        {
            return await DB.StringIncrementAsync(key, value, flags);
        }

        public virtual async Task<long> StringLengthAsync(RedisKey key, CommandFlags flags = CommandFlags.None)
        {
            return await DB.StringLengthAsync(key, flags);
        }

        public virtual async Task<bool> StringSetAsync(RedisKey key, RedisValue value, TimeSpan? expiry = null, When when = When.Always, CommandFlags flags = CommandFlags.None)
        {
            return await DB.StringSetAsync(key, value, expiry, when, flags);
        }

        public virtual async Task<bool> StringSetAsync(KeyValuePair<RedisKey, RedisValue>[] values, When when = When.Always, CommandFlags flags = CommandFlags.None)
        {
            return await DB.StringSetAsync(values, when, flags);
        }

        public virtual async Task<bool> StringSetBitAsync(RedisKey key, long offset, bool bit, CommandFlags flags = CommandFlags.None)
        {
            return await DB.StringSetBitAsync(key, offset, bit, flags);
        }

        public virtual async Task<RedisValue> StringSetRangeAsync(RedisKey key, long offset, RedisValue value, CommandFlags flags = CommandFlags.None)
        {
            return await DB.StringSetRangeAsync(key, offset, value, flags);
        }

        public virtual async Task<bool> KeyTouchAsync(RedisKey key, CommandFlags flags = CommandFlags.None)
        {
            return await DB.KeyTouchAsync(key, flags);
        }

        public virtual async Task<long> KeyTouchAsync(RedisKey[] keys, CommandFlags flags = CommandFlags.None)
        {
            return await DB.KeyTouchAsync(keys, flags);
        }

        public virtual async Task<TimeSpan> PingAsync(CommandFlags flags = CommandFlags.None)
        {
            return await DB.PingAsync(flags);
        }

        public virtual bool TryWait(Task task)
        {
            return DB.TryWait(task);
        }

        public virtual void Wait(Task task)
        {
            DB.Wait(task);
        }

        public virtual T Wait<T>(Task<T> task)
        {
            return DB.Wait(task);
        }

        public virtual void WaitAll(params Task[] tasks)
        {
            DB.WaitAll(tasks);
        }
    }
}

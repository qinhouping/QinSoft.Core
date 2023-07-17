using QinSoft.Core.Common.Utils;
using StackExchange.Redis;
using StackExchange.Redis.MultiplexerPool;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace QinSoft.Core.Cache.Redis.Core
{
    /// <summary>
    /// 可备份的Redis缓存，支持多写
    /// </summary>
    class BackupableRedisCache : RedisCache
    {
        /// <summary>
        /// 备份Redis连接
        /// </summary>
        protected IList<IConnectionMultiplexer> BackupsConnections { get; private set; }

        /// <summary>
        /// 备份Redis数据库
        /// </summary>
        protected IList<IDatabase> BackupDBs { get; private set; }

        public BackupableRedisCache(RedisCacheOptions options, RedisCacheOptions[] backupOptions = null) : base(options)
        {
            backupOptions = backupOptions ?? new RedisCacheOptions[0];
            BackupsConnections = new List<IConnectionMultiplexer>();
            BackupDBs = new List<IDatabase>();
            foreach (RedisCacheOptions backupOpt in backupOptions)
            {
                IConnectionMultiplexer backupConnection = null;
                if (backupOpt.ConfigurationOptions != null)
                {
                    backupConnection = ConnectionMultiplexer.Connect(options.ConfigurationOptions, null);
                }
                else
                {
                    backupConnection = ConnectionMultiplexer.Connect(options.Configuration, null);
                }
                IDatabase backupDB = backupConnection.GetDatabase(-1, null);
                BackupsConnections.Add(backupConnection);
                BackupDBs.Add(backupDB);
            }
        }

        public BackupableRedisCache(IConnectionMultiplexer connection, IConnectionMultiplexer[] backupConnections, bool isPoolConnection = false) : base(connection, isPoolConnection)
        {
            BackupsConnections = backupConnections == null ? new List<IConnectionMultiplexer>() : new List<IConnectionMultiplexer>(backupConnections);
            BackupDBs = BackupsConnections.Select(u => u.GetDatabase()).ToList();
        }

        public BackupableRedisCache(IReconnectableConnectionMultiplexer connection, IReconnectableConnectionMultiplexer[] backupConnections) : base(connection)
        {
            BackupsConnections = backupConnections == null ? new List<IConnectionMultiplexer>() : new List<IConnectionMultiplexer>(backupConnections.Select(u => u.Connection));
            BackupDBs = BackupsConnections.Select(u => u.GetDatabase()).ToList();
        }

        public override void Select(int database)
        {
            BackupDBs = BackupsConnections.Select(u => u.GetDatabase(database)).ToList();
            base.Select(database);
        }

        /// <summary>
        /// 安全释放资源
        /// </summary>
        public override void SafeDispose()
        {
            GC.SuppressFinalize(this);
            if (!IsPoolConnection)
            {
                this.Connection.Dispose();
                foreach (IConnectionMultiplexer backupConnection in BackupsConnections)
                {
                    backupConnection.Dispose();
                }
            }
        }

        public override long HashDecrement(RedisKey key, RedisValue hashField, long value = 1, CommandFlags flags = CommandFlags.None)
        {
            Task.Factory.StartNew(() =>
            {
                foreach (IDatabase backupDB in BackupDBs)
                {
                    backupDB.HashDecrement(key, hashField, value, flags);
                }
            });
            return base.HashDecrement(key, hashField, value, flags);
        }

        public override bool HashDelete(RedisKey key, RedisValue hashField, CommandFlags flags = CommandFlags.None)
        {
            Task.Factory.StartNew(() =>
            {
                foreach (IDatabase backupDB in BackupDBs)
                {
                    backupDB.HashDelete(key, hashField, flags);
                }
            });
            return base.HashDelete(key, hashField, flags);
        }

        public override long HashDelete(RedisKey key, RedisValue[] hashFields, CommandFlags flags = CommandFlags.None)
        {
            Task.Factory.StartNew(() =>
            {
                foreach (IDatabase backupDB in BackupDBs)
                {
                    backupDB.HashDelete(key, hashFields, flags);
                }
            });
            return base.HashDelete(key, hashFields, flags);
        }

        public override long HashIncrement(RedisKey key, RedisValue hashField, long value = 1, CommandFlags flags = CommandFlags.None)
        {
            Task.Factory.StartNew(() =>
            {
                foreach (IDatabase backupDB in BackupDBs)
                {
                    backupDB.HashIncrement(key, hashField, value, flags);
                }
            });
            return base.HashIncrement(key, hashField, value, flags);
        }

        public override bool KeyDelete(RedisKey key, CommandFlags flags = CommandFlags.None)
        {
            Task.Factory.StartNew(() =>
            {
                foreach (IDatabase backupDB in BackupDBs)
                {
                    backupDB.KeyDelete(key, flags);
                }
            });
            return base.KeyDelete(key, flags);
        }
        public override long KeyDelete(RedisKey[] keys, CommandFlags flags = CommandFlags.None)
        {
            Task.Factory.StartNew(() =>
            {
                foreach (IDatabase backupDB in BackupDBs)
                {
                    backupDB.KeyDelete(keys, flags);
                }
            });
            return base.KeyDelete(keys, flags);
        }

        public override bool KeyExpire(RedisKey key, TimeSpan? expiry, CommandFlags flags = CommandFlags.None)
        {
            Task.Factory.StartNew(() =>
            {
                foreach (IDatabase backupDB in BackupDBs)
                {
                    backupDB.KeyExpire(key, expiry, flags);
                }
            });
            return base.KeyExpire(key, expiry, flags);
        }
        public override bool KeyMove(RedisKey key, int database, CommandFlags flags = CommandFlags.None)
        {
            Task.Factory.StartNew(() =>
            {
                foreach (IDatabase backupDB in BackupDBs)
                {
                    backupDB.KeyMove(key, database, flags);
                }
            });
            return base.KeyMove(key, database, flags);
        }

        public override bool KeyPersist(RedisKey key, CommandFlags flags = CommandFlags.None)
        {
            Task.Factory.StartNew(() =>
            {
                foreach (IDatabase backupDB in BackupDBs)
                {
                    backupDB.KeyPersist(key, flags);
                }
            });
            return base.KeyPersist(key, flags);
        }

        public override bool KeyRename(RedisKey key, RedisKey newKey, When when = When.Always, CommandFlags flags = CommandFlags.None)
        {
            Task.Factory.StartNew(() =>
            {
                foreach (IDatabase backupDB in BackupDBs)
                {
                    backupDB.KeyRename(key, newKey, when, flags);
                }
            });
            return base.KeyRename(key, newKey, when, flags);
        }

        public override void KeyRestore(RedisKey key, byte[] value, TimeSpan? expiry = null, CommandFlags flags = CommandFlags.None)
        {
            Task.Factory.StartNew(() =>
            {
                foreach (IDatabase backupDB in BackupDBs)
                {
                    backupDB.KeyRestore(key, value, expiry, flags);
                }
            });
            base.KeyRestore(key, value, expiry, flags);
        }

        public override long ListInsertAfter(RedisKey key, RedisValue pivot, RedisValue value, CommandFlags flags = CommandFlags.None)
        {
            Task.Factory.StartNew(() =>
            {
                foreach (IDatabase backupDB in BackupDBs)
                {
                    backupDB.ListInsertAfter(key, pivot, value, flags);
                }
            });
            return base.ListInsertAfter(key, pivot, value, flags);
        }
        public override long ListInsertBefore(RedisKey key, RedisValue pivot, RedisValue value, CommandFlags flags = CommandFlags.None)
        {
            Task.Factory.StartNew(() =>
            {
                foreach (IDatabase backupDB in BackupDBs)
                {
                    backupDB.ListInsertBefore(key, pivot, value, flags);
                }
            });
            return base.ListInsertBefore(key, pivot, value, flags);
        }

        public override RedisValue ListLeftPop(RedisKey key, CommandFlags flags = CommandFlags.None)
        {
            Task.Factory.StartNew(() =>
            {
                foreach (IDatabase backupDB in BackupDBs)
                {
                    backupDB.ListLeftPop(key, flags);
                }
            });
            return base.ListLeftPop(key, flags);
        }

        public override long ListLeftPush(RedisKey key, RedisValue value, When when = When.Always, CommandFlags flags = CommandFlags.None)
        {
            Task.Factory.StartNew(() =>
            {
                foreach (IDatabase backupDB in BackupDBs)
                {
                    backupDB.ListLeftPush(key, value, when, flags);
                }
            });
            return base.ListLeftPush(key, value, when, flags);
        }

        public override long ListLeftPush(RedisKey key, RedisValue[] values, When when = When.Always, CommandFlags flags = CommandFlags.None)
        {
            Task.Factory.StartNew(() =>
            {
                foreach (IDatabase backupDB in BackupDBs)
                {
                    backupDB.ListLeftPush(key, values, when, flags);
                }
            });
            return base.ListLeftPush(key, values, when, flags);
        }

        public override long ListLeftPush(RedisKey key, RedisValue[] values, CommandFlags flags)
        {
            Task.Factory.StartNew(() =>
            {
                foreach (IDatabase backupDB in BackupDBs)
                {
                    backupDB.ListLeftPush(key, values, flags);
                }
            });
            return base.ListLeftPush(key, values, flags);
        }

        public override long ListRemove(RedisKey key, RedisValue value, long count = 0, CommandFlags flags = CommandFlags.None)
        {
            Task.Factory.StartNew(() =>
            {
                foreach (IDatabase backupDB in BackupDBs)
                {
                    backupDB.ListRemove(key, value, count, flags);
                }
            });
            return base.ListRemove(key, value, count, flags);
        }

        public override RedisValue ListRightPop(RedisKey key, CommandFlags flags = CommandFlags.None)
        {
            Task.Factory.StartNew(() =>
            {
                foreach (IDatabase backupDB in BackupDBs)
                {
                    backupDB.ListRightPop(key, flags);
                }
            });
            return base.ListRightPop(key, flags);
        }
        public override RedisValue ListRightPopLeftPush(RedisKey source, RedisKey destination, CommandFlags flags = CommandFlags.None)
        {
            Task.Factory.StartNew(() =>
            {
                foreach (IDatabase backupDB in BackupDBs)
                {
                    backupDB.ListRightPopLeftPush(source, destination, flags);
                }
            });
            return base.ListRightPopLeftPush(source, destination, flags);
        }

        public override long ListRightPush(RedisKey key, RedisValue value, When when = When.Always, CommandFlags flags = CommandFlags.None)
        {
            Task.Factory.StartNew(() =>
            {
                foreach (IDatabase backupDB in BackupDBs)
                {
                    backupDB.ListRightPush(key, value, when, flags);
                }
            });
            return base.ListRightPush(key, value, when, flags);
        }

        public override long ListRightPush(RedisKey key, RedisValue[] values, When when = When.Always, CommandFlags flags = CommandFlags.None)
        {
            Task.Factory.StartNew(() =>
            {
                foreach (IDatabase backupDB in BackupDBs)
                {
                    backupDB.ListRightPush(key, values, when, flags);
                }
            });
            return base.ListRightPush(key, values, when, flags);
        }

        public override void ListSetByIndex(RedisKey key, long index, RedisValue value, CommandFlags flags = CommandFlags.None)
        {
            Task.Factory.StartNew(() =>
            {
                foreach (IDatabase backupDB in BackupDBs)
                {
                    backupDB.ListSetByIndex(key, index, value, flags);
                }
            });
            base.ListSetByIndex(key, index, value, flags);
        }

        public override void ListTrim(RedisKey key, long start, long stop, CommandFlags flags = CommandFlags.None)
        {
            Task.Factory.StartNew(() =>
            {
                foreach (IDatabase backupDB in BackupDBs)
                {
                    backupDB.ListTrim(key, start, stop, flags);
                }
            });
            base.ListTrim(key, start, stop, flags);
        }

        public override RedisResult Execute(string command, params object[] args)
        {
            Task.Factory.StartNew(() =>
            {
                foreach (IDatabase backupDB in BackupDBs)
                {
                    backupDB.Execute(command, args);
                }
            });
            return base.Execute(command, args);
        }

        public override RedisResult Execute(string command, ICollection<object> args, CommandFlags flags = CommandFlags.None)
        {
            Task.Factory.StartNew(() =>
            {
                foreach (IDatabase backupDB in BackupDBs)
                {
                    backupDB.Execute(command, args, flags);
                }
            });
            return base.Execute(command, args, flags);
        }

        public override RedisResult ScriptEvaluate(string script, RedisKey[] keys = null, RedisValue[] values = null, CommandFlags flags = CommandFlags.None)
        {
            Task.Factory.StartNew(() =>
            {
                foreach (IDatabase backupDB in BackupDBs)
                {
                    backupDB.ScriptEvaluate(script, keys, values, flags);
                }
            });
            return base.ScriptEvaluate(script, keys, values, flags);
        }

        public override RedisResult ScriptEvaluate(byte[] hash, RedisKey[] keys = null, RedisValue[] values = null, CommandFlags flags = CommandFlags.None)
        {
            Task.Factory.StartNew(() =>
            {
                foreach (IDatabase backupDB in BackupDBs)
                {
                    backupDB.ScriptEvaluate(hash, keys, values, flags);
                }
            });
            return base.ScriptEvaluate(hash, keys, values, flags);
        }

        public override RedisResult ScriptEvaluate(LuaScript script, object parameters = null, CommandFlags flags = CommandFlags.None)
        {
            Task.Factory.StartNew(() =>
            {
                foreach (IDatabase backupDB in BackupDBs)
                {
                    backupDB.ScriptEvaluate(script, parameters, flags);
                }
            });
            return base.ScriptEvaluate(script, parameters, flags);
        }

        public override RedisResult ScriptEvaluate(LoadedLuaScript script, object parameters = null, CommandFlags flags = CommandFlags.None)
        {
            Task.Factory.StartNew(() =>
            {
                foreach (IDatabase backupDB in BackupDBs)
                {
                    backupDB.ScriptEvaluate(script, parameters, flags);
                }
            });
            return base.ScriptEvaluate(script, parameters, flags);
        }

        public override bool SetAdd(RedisKey key, RedisValue value, CommandFlags flags = CommandFlags.None)
        {
            Task.Factory.StartNew(() =>
            {
                foreach (IDatabase backupDB in BackupDBs)
                {
                    backupDB.SetAdd(key, value, flags);
                }
            });
            return base.SetAdd(key, value, flags);
        }

        public override long SetAdd(RedisKey key, RedisValue[] values, CommandFlags flags = CommandFlags.None)
        {
            Task.Factory.StartNew(() =>
            {
                foreach (IDatabase backupDB in BackupDBs)
                {
                    backupDB.SetAdd(key, values, flags);
                }
            });
            return base.SetAdd(key, values, flags);
        }

        public override long SetCombineAndStore(SetOperation operation, RedisKey destination, RedisKey first, RedisKey second, CommandFlags flags = CommandFlags.None)
        {
            Task.Factory.StartNew(() =>
            {
                foreach (IDatabase backupDB in BackupDBs)
                {
                    backupDB.SetCombineAndStore(operation, destination, first, second, flags);
                }
            });
            return base.SetCombineAndStore(operation, destination, first, second, flags);
        }

        public override long SetCombineAndStore(SetOperation operation, RedisKey destination, RedisKey[] keys, CommandFlags flags = CommandFlags.None)
        {
            Task.Factory.StartNew(() =>
            {
                foreach (IDatabase backupDB in BackupDBs)
                {
                    backupDB.SetCombineAndStore(operation, destination, keys, flags);
                }
            });
            return base.SetCombineAndStore(operation, destination, keys, flags);
        }

        public override bool SetMove(RedisKey source, RedisKey destination, RedisValue value, CommandFlags flags = CommandFlags.None)
        {
            Task.Factory.StartNew(() =>
            {
                foreach (IDatabase backupDB in BackupDBs)
                {
                    backupDB.SetMove(source, destination, value, flags);
                }
            });
            return base.SetMove(source, destination, value, flags);
        }

        public override RedisValue SetPop(RedisKey key, CommandFlags flags = CommandFlags.None)
        {
            Task.Factory.StartNew(() =>
            {
                foreach (IDatabase backupDB in BackupDBs)
                {
                    backupDB.SetPop(key, flags);
                }
            });
            return base.SetPop(key, flags);
        }

        public override RedisValue[] SetPop(RedisKey key, long count, CommandFlags flags = CommandFlags.None)
        {
            Task.Factory.StartNew(() =>
            {
                foreach (IDatabase backupDB in BackupDBs)
                {
                    backupDB.SetPop(key, count, flags);
                }
            });
            return base.SetPop(key, count, flags);
        }

        public override bool SetRemove(RedisKey key, RedisValue value, CommandFlags flags = CommandFlags.None)
        {
            Task.Factory.StartNew(() =>
            {
                foreach (IDatabase backupDB in BackupDBs)
                {
                    backupDB.SetRemove(key, value, flags);
                }
            });
            return base.SetRemove(key, value, flags);
        }

        public override long SetRemove(RedisKey key, RedisValue[] values, CommandFlags flags = CommandFlags.None)
        {
            Task.Factory.StartNew(() =>
            {
                foreach (IDatabase backupDB in BackupDBs)
                {
                    backupDB.SetRemove(key, values, flags);
                }
            });
            return base.SetRemove(key, values, flags);
        }

        public override long SortAndStore(RedisKey destination, RedisKey key, long skip = 0, long take = -1, Order order = Order.Ascending, SortType sortType = SortType.Numeric, RedisValue by = default, RedisValue[] get = null, CommandFlags flags = CommandFlags.None)
        {
            Task.Factory.StartNew(() =>
            {
                foreach (IDatabase backupDB in BackupDBs)
                {
                    backupDB.SortAndStore(destination, key, skip, take, order, sortType, by, get, flags);
                }
            });
            return base.SortAndStore(destination, key, skip, take, order, sortType, by, get, flags);
        }

        public override bool SortedSetAdd(RedisKey key, RedisValue member, double score, CommandFlags flags)
        {
            Task.Factory.StartNew(() =>
            {
                foreach (IDatabase backupDB in BackupDBs)
                {
                    backupDB.SortedSetAdd(key, member, score, flags);
                }
            });
            return base.SortedSetAdd(key, member, score, flags);
        }

        public override bool SortedSetAdd(RedisKey key, RedisValue member, double score, When when = When.Always, CommandFlags flags = CommandFlags.None)
        {
            Task.Factory.StartNew(() =>
            {
                foreach (IDatabase backupDB in BackupDBs)
                {
                    backupDB.SortedSetAdd(key, member, score, when, flags);
                }
            });
            return base.SortedSetAdd(key, member, score, when, flags);
        }

        public override long SortedSetAdd(RedisKey key, SortedSetEntry[] values, CommandFlags flags)
        {
            Task.Factory.StartNew(() =>
            {
                foreach (IDatabase backupDB in BackupDBs)
                {
                    backupDB.SortedSetAdd(key, values, flags);
                }
            });
            return base.SortedSetAdd(key, values, flags);
        }

        public override long SortedSetAdd(RedisKey key, SortedSetEntry[] values, When when = When.Always, CommandFlags flags = CommandFlags.None)
        {
            Task.Factory.StartNew(() =>
            {
                foreach (IDatabase backupDB in BackupDBs)
                {
                    backupDB.SortedSetAdd(key, values, when, flags);
                }
            });
            return base.SortedSetAdd(key, values, when, flags);
        }

        public override long SortedSetCombineAndStore(SetOperation operation, RedisKey destination, RedisKey first, RedisKey second, Aggregate aggregate = Aggregate.Sum, CommandFlags flags = CommandFlags.None)
        {
            Task.Factory.StartNew(() =>
            {
                foreach (IDatabase backupDB in BackupDBs)
                {
                    backupDB.SortedSetCombineAndStore(operation, destination, first, second, aggregate, flags);
                }
            });
            return base.SortedSetCombineAndStore(operation, destination, first, second, aggregate, flags);
        }

        public override long SortedSetCombineAndStore(SetOperation operation, RedisKey destination, RedisKey[] keys, double[] weights = null, Aggregate aggregate = Aggregate.Sum, CommandFlags flags = CommandFlags.None)
        {
            Task.Factory.StartNew(() =>
            {
                foreach (IDatabase backupDB in BackupDBs)
                {
                    backupDB.SortedSetCombineAndStore(operation, destination, keys, weights, aggregate, flags);
                }
            });
            return base.SortedSetCombineAndStore(operation, destination, keys, weights, aggregate, flags);
        }

        public override double SortedSetDecrement(RedisKey key, RedisValue member, double value, CommandFlags flags = CommandFlags.None)
        {
            Task.Factory.StartNew(() =>
            {
                foreach (IDatabase backupDB in BackupDBs)
                {
                    backupDB.SortedSetDecrement(key, member, value, flags);
                }
            });
            return base.SortedSetDecrement(key, member, value, flags);
        }

        public override double SortedSetIncrement(RedisKey key, RedisValue member, double value, CommandFlags flags = CommandFlags.None)
        {
            Task.Factory.StartNew(() =>
            {
                foreach (IDatabase backupDB in BackupDBs)
                {
                    backupDB.SortedSetIncrement(key, member, value, flags);
                }
            });
            return base.SortedSetIncrement(key, member, value, flags);
        }

        public override bool SortedSetRemove(RedisKey key, RedisValue member, CommandFlags flags = CommandFlags.None)
        {
            Task.Factory.StartNew(() =>
            {
                foreach (IDatabase backupDB in BackupDBs)
                {
                    backupDB.SortedSetRemove(key, member, flags);
                }
            });
            return base.SortedSetRemove(key, member, flags);
        }

        public override long SortedSetRemove(RedisKey key, RedisValue[] members, CommandFlags flags = CommandFlags.None)
        {
            Task.Factory.StartNew(() =>
            {
                foreach (IDatabase backupDB in BackupDBs)
                {
                    backupDB.SortedSetRemove(key, members, flags);
                }
            });
            return base.SortedSetRemove(key, members, flags);
        }

        public override long SortedSetRemoveRangeByRank(RedisKey key, long start, long stop, CommandFlags flags = CommandFlags.None)
        {
            Task.Factory.StartNew(() =>
            {
                foreach (IDatabase backupDB in BackupDBs)
                {
                    backupDB.SortedSetRemoveRangeByRank(key, start, stop, flags);
                }
            });
            return base.SortedSetRemoveRangeByRank(key, start, stop, flags);
        }

        public override long SortedSetRemoveRangeByScore(RedisKey key, double start, double stop, Exclude exclude = Exclude.None, CommandFlags flags = CommandFlags.None)
        {
            Task.Factory.StartNew(() =>
            {
                foreach (IDatabase backupDB in BackupDBs)
                {
                    backupDB.SortedSetRemoveRangeByScore(key, start, stop, exclude, flags);
                }
            });
            return base.SortedSetRemoveRangeByScore(key, start, stop, exclude, flags);
        }

        public override long SortedSetRemoveRangeByValue(RedisKey key, RedisValue min, RedisValue max, Exclude exclude = Exclude.None, CommandFlags flags = CommandFlags.None)
        {
            Task.Factory.StartNew(() =>
            {
                foreach (IDatabase backupDB in BackupDBs)
                {
                    backupDB.SortedSetRemoveRangeByValue(key, min, max, exclude, flags);
                }
            });
            return base.SortedSetRemoveRangeByValue(key, min, max, exclude, flags);
        }

        public override SortedSetEntry? SortedSetPop(RedisKey key, Order order = Order.Ascending, CommandFlags flags = CommandFlags.None)
        {
            Task.Factory.StartNew(() =>
            {
                foreach (IDatabase backupDB in BackupDBs)
                {
                    backupDB.SortedSetPop(key, order, flags);
                }
            });
            return base.SortedSetPop(key, order, flags);
        }

        public override SortedSetEntry[] SortedSetPop(RedisKey key, long count, Order order = Order.Ascending, CommandFlags flags = CommandFlags.None)
        {
            Task.Factory.StartNew(() =>
            {
                foreach (IDatabase backupDB in BackupDBs)
                {
                    backupDB.SortedSetPop(key, count, order, flags);
                }
            });
            return base.SortedSetPop(key, count, order, flags);
        }

        public override long StringAppend(RedisKey key, RedisValue value, CommandFlags flags = CommandFlags.None)
        {
            Task.Factory.StartNew(() =>
            {
                foreach (IDatabase backupDB in BackupDBs)
                {
                    backupDB.StringAppend(key, value, flags);
                }
            });
            return base.StringAppend(key, value, flags);
        }

        public override long StringBitOperation(Bitwise operation, RedisKey destination, RedisKey first, RedisKey second = default, CommandFlags flags = CommandFlags.None)
        {
            Task.Factory.StartNew(() =>
            {
                foreach (IDatabase backupDB in BackupDBs)
                {
                    backupDB.StringBitOperation(operation, destination, first, second, flags);
                }
            });
            return base.StringBitOperation(operation, destination, first, second, flags);
        }

        public override long StringDecrement(RedisKey key, long value = 1, CommandFlags flags = CommandFlags.None)
        {
            Task.Factory.StartNew(() =>
            {
                foreach (IDatabase backupDB in BackupDBs)
                {
                    backupDB.StringDecrement(key, value, flags);
                }
            });
            return base.StringDecrement(key, value, flags);
        }

        public override double StringDecrement(RedisKey key, double value, CommandFlags flags = CommandFlags.None)
        {
            Task.Factory.StartNew(() =>
            {
                foreach (IDatabase backupDB in BackupDBs)
                {
                    backupDB.StringDecrement(key, value, flags);
                }
            });
            return base.StringDecrement(key, value, flags);
        }

        public override long StringIncrement(RedisKey key, long value = 1, CommandFlags flags = CommandFlags.None)
        {
            Task.Factory.StartNew(() =>
            {
                foreach (IDatabase backupDB in BackupDBs)
                {
                    backupDB.StringIncrement(key, value, flags);
                }
            });
            return base.StringIncrement(key, value, flags);
        }

        public override double StringIncrement(RedisKey key, double value, CommandFlags flags = CommandFlags.None)
        {
            Task.Factory.StartNew(() =>
            {
                foreach (IDatabase backupDB in BackupDBs)
                {
                    backupDB.StringIncrement(key, value, flags);
                }
            });
            return base.StringIncrement(key, value, flags);
        }

        public override bool StringSet(RedisKey key, RedisValue value, TimeSpan? expiry = null, When when = When.Always, CommandFlags flags = CommandFlags.None)
        {
            Task.Factory.StartNew(() =>
            {
                foreach (IDatabase backupDB in BackupDBs)
                {
                    backupDB.StringSet(key, value, expiry, when, flags);
                }
            });
            return base.StringSet(key, value, expiry, when, flags);
        }

        public override bool StringSet(KeyValuePair<RedisKey, RedisValue>[] values, When when = When.Always, CommandFlags flags = CommandFlags.None)
        {
            Task.Factory.StartNew(() =>
            {
                foreach (IDatabase backupDB in BackupDBs)
                {
                    backupDB.StringSet(values, when, flags);
                }
            });
            return base.StringSet(values, when, flags);
        }

        public override bool StringSetBit(RedisKey key, long offset, bool bit, CommandFlags flags = CommandFlags.None)
        {
            Task.Factory.StartNew(() =>
            {
                foreach (IDatabase backupDB in BackupDBs)
                {
                    backupDB.StringSetBit(key, offset, bit, flags);
                }
            });
            return base.StringSetBit(key, offset, bit, flags);
        }

        public override RedisValue StringSetRange(RedisKey key, long offset, RedisValue value, CommandFlags flags = CommandFlags.None)
        {
            Task.Factory.StartNew(() =>
            {
                foreach (IDatabase backupDB in BackupDBs)
                {
                    backupDB.StringSetRange(key, offset, value, flags);
                }
            });
            return base.StringSetRange(key, offset, value, flags);
        }

        public override bool KeyTouch(RedisKey key, CommandFlags flags = CommandFlags.None)
        {
            Task.Factory.StartNew(() =>
            {
                foreach (IDatabase backupDB in BackupDBs)
                {
                    backupDB.KeyTouch(key, flags);
                }
            });
            return base.KeyTouch(key, flags);
        }

        public override long KeyTouch(RedisKey[] keys, CommandFlags flags = CommandFlags.None)
        {
            Task.Factory.StartNew(() =>
            {
                foreach (IDatabase backupDB in BackupDBs)
                {
                    backupDB.KeyTouch(keys, flags);
                }
            });
            return base.KeyTouch(keys, flags);
        }

        public override async Task<long> HashDecrementAsync(RedisKey key, RedisValue hashField, long value = 1, CommandFlags flags = CommandFlags.None)
        {
            await Task.Factory.StartNew(() =>
            {
                foreach (IDatabase backupDB in BackupDBs)
                {
                    backupDB.HashDecrementAsync(key, hashField, value, flags);
                }
            });
            return await base.HashDecrementAsync(key, hashField, value, flags);
        }

        public override async Task<bool> HashDeleteAsync(RedisKey key, RedisValue hashField, CommandFlags flags = CommandFlags.None)
        {
            await Task.Factory.StartNew(() =>
            {
                foreach (IDatabase backupDB in BackupDBs)
                {
                    backupDB.HashDeleteAsync(key, hashField, flags);
                }
            });
            return await base.HashDeleteAsync(key, hashField, flags);
        }

        public override async Task<long> HashDeleteAsync(RedisKey key, RedisValue[] hashFields, CommandFlags flags = CommandFlags.None)
        {
            await Task.Factory.StartNew(() =>
            {
                foreach (IDatabase backupDB in BackupDBs)
                {
                    backupDB.HashDeleteAsync(key, hashFields, flags);
                }
            });
            return await base.HashDeleteAsync(key, hashFields, flags);
        }

        public override async Task<long> HashIncrementAsync(RedisKey key, RedisValue hashField, long value = 1, CommandFlags flags = CommandFlags.None)
        {
            await Task.Factory.StartNew(() =>
            {
                foreach (IDatabase backupDB in BackupDBs)
                {
                    backupDB.HashIncrementAsync(key, hashField, value, flags);
                }
            });
            return await base.HashIncrementAsync(key, hashField, value, flags);
        }

        public override async Task<bool> KeyDeleteAsync(RedisKey key, CommandFlags flags = CommandFlags.None)
        {
            await Task.Factory.StartNew(() =>
            {
                foreach (IDatabase backupDB in BackupDBs)
                {
                    backupDB.KeyDeleteAsync(key, flags);
                }
            });
            return await base.KeyDeleteAsync(key, flags);
        }
        public override async Task<long> KeyDeleteAsync(RedisKey[] keys, CommandFlags flags = CommandFlags.None)
        {
            await Task.Factory.StartNew(() =>
            {
                foreach (IDatabase backupDB in BackupDBs)
                {
                    backupDB.KeyDeleteAsync(keys, flags);
                }
            });
            return await base.KeyDeleteAsync(keys, flags);
        }

        public override async Task<bool> KeyExpireAsync(RedisKey key, TimeSpan? expiry, CommandFlags flags = CommandFlags.None)
        {
            await Task.Factory.StartNew(() =>
            {
                foreach (IDatabase backupDB in BackupDBs)
                {
                    backupDB.KeyExpireAsync(key, expiry, flags);
                }
            });
            return await base.KeyExpireAsync(key, expiry, flags);
        }
        public override async Task<bool> KeyMoveAsync(RedisKey key, int database, CommandFlags flags = CommandFlags.None)
        {
            await Task.Factory.StartNew(() =>
            {
                foreach (IDatabase backupDB in BackupDBs)
                {
                    backupDB.KeyMoveAsync(key, database, flags);
                }
            });
            return await base.KeyMoveAsync(key, database, flags);
        }

        public override async Task<bool> KeyPersistAsync(RedisKey key, CommandFlags flags = CommandFlags.None)
        {
            await Task.Factory.StartNew(() =>
            {
                foreach (IDatabase backupDB in BackupDBs)
                {
                    backupDB.KeyPersistAsync(key, flags);
                }
            });
            return await base.KeyPersistAsync(key, flags);
        }

        public override async Task<bool> KeyRenameAsync(RedisKey key, RedisKey newKey, When when = When.Always, CommandFlags flags = CommandFlags.None)
        {
            await Task.Factory.StartNew(() =>
            {
                foreach (IDatabase backupDB in BackupDBs)
                {
                    backupDB.KeyRenameAsync(key, newKey, when, flags);
                }
            });
            return await base.KeyRenameAsync(key, newKey, when, flags);
        }

        public override async Task KeyRestoreAsync(RedisKey key, byte[] value, TimeSpan? expiry = null, CommandFlags flags = CommandFlags.None)
        {
            await Task.Factory.StartNew(() =>
            {
                foreach (IDatabase backupDB in BackupDBs)
                {
                    backupDB.KeyRestoreAsync(key, value, expiry, flags);
                }
            });
            await base.KeyRestoreAsync(key, value, expiry, flags);
        }

        public override async Task<long> ListInsertAfterAsync(RedisKey key, RedisValue pivot, RedisValue value, CommandFlags flags = CommandFlags.None)
        {
            await Task.Factory.StartNew(() =>
            {
                foreach (IDatabase backupDB in BackupDBs)
                {
                    backupDB.ListInsertAfterAsync(key, pivot, value, flags);
                }
            });
            return await base.ListInsertAfterAsync(key, pivot, value, flags);
        }
        public override async Task<long> ListInsertBeforeAsync(RedisKey key, RedisValue pivot, RedisValue value, CommandFlags flags = CommandFlags.None)
        {
            await Task.Factory.StartNew(() =>
            {
                foreach (IDatabase backupDB in BackupDBs)
                {
                    backupDB.ListInsertBeforeAsync(key, pivot, value, flags);
                }
            });
            return await base.ListInsertBeforeAsync(key, pivot, value, flags);
        }

        public override async Task<RedisValue> ListLeftPopAsync(RedisKey key, CommandFlags flags = CommandFlags.None)
        {
            await Task.Factory.StartNew(() =>
            {
                foreach (IDatabase backupDB in BackupDBs)
                {
                    backupDB.ListLeftPopAsync(key, flags);
                }
            });
            return await base.ListLeftPopAsync(key, flags);
        }

        public override async Task<long> ListLeftPushAsync(RedisKey key, RedisValue value, When when = When.Always, CommandFlags flags = CommandFlags.None)
        {
            await Task.Factory.StartNew(() =>
            {
                foreach (IDatabase backupDB in BackupDBs)
                {
                    backupDB.ListLeftPushAsync(key, value, when, flags);
                }
            });
            return await base.ListLeftPushAsync(key, value, when, flags);
        }

        public override async Task<long> ListLeftPushAsync(RedisKey key, RedisValue[] values, When when = When.Always, CommandFlags flags = CommandFlags.None)
        {
            await Task.Factory.StartNew(() =>
            {
                foreach (IDatabase backupDB in BackupDBs)
                {
                    backupDB.ListLeftPushAsync(key, values, when, flags);
                }
            });
            return await base.ListLeftPushAsync(key, values, when, flags);
        }

        public override async Task<long> ListLeftPushAsync(RedisKey key, RedisValue[] values, CommandFlags flags)
        {
            await Task.Factory.StartNew(() =>
            {
                foreach (IDatabase backupDB in BackupDBs)
                {
                    backupDB.ListLeftPushAsync(key, values, flags);
                }
            });
            return await base.ListLeftPushAsync(key, values, flags);
        }

        public override async Task<long> ListRemoveAsync(RedisKey key, RedisValue value, long count = 0, CommandFlags flags = CommandFlags.None)
        {
            await Task.Factory.StartNew(() =>
            {
                foreach (IDatabase backupDB in BackupDBs)
                {
                    backupDB.ListRemoveAsync(key, value, count, flags);
                }
            });
            return await base.ListRemoveAsync(key, value, count, flags);
        }

        public override async Task<RedisValue> ListRightPopAsync(RedisKey key, CommandFlags flags = CommandFlags.None)
        {
            await Task.Factory.StartNew(() =>
            {
                foreach (IDatabase backupDB in BackupDBs)
                {
                    backupDB.ListRightPopAsync(key, flags);
                }
            });
            return await base.ListRightPopAsync(key, flags);
        }
        public override async Task<RedisValue> ListRightPopLeftPushAsync(RedisKey source, RedisKey destination, CommandFlags flags = CommandFlags.None)
        {
            await Task.Factory.StartNew(() =>
            {
                foreach (IDatabase backupDB in BackupDBs)
                {
                    backupDB.ListRightPopLeftPush(source, destination, flags);
                }
            });
            return await base.ListRightPopLeftPushAsync(source, destination, flags);
        }

        public override async Task<long> ListRightPushAsync(RedisKey key, RedisValue value, When when = When.Always, CommandFlags flags = CommandFlags.None)
        {
            await Task.Factory.StartNew(() =>
            {
                foreach (IDatabase backupDB in BackupDBs)
                {
                    backupDB.ListRightPushAsync(key, value, when, flags);
                }
            });
            return await base.ListRightPushAsync(key, value, when, flags);
        }

        public override async Task<long> ListRightPushAsync(RedisKey key, RedisValue[] values, When when = When.Always, CommandFlags flags = CommandFlags.None)
        {
            await Task.Factory.StartNew(() =>
            {
                foreach (IDatabase backupDB in BackupDBs)
                {
                    backupDB.ListRightPushAsync(key, values, when, flags);
                }
            });
            return await base.ListRightPushAsync(key, values, when, flags);
        }

        public override async Task ListSetByIndexAsync(RedisKey key, long index, RedisValue value, CommandFlags flags = CommandFlags.None)
        {
            await Task.Factory.StartNew(() =>
            {
                foreach (IDatabase backupDB in BackupDBs)
                {
                    backupDB.ListSetByIndexAsync(key, index, value, flags);
                }
            });
            await base.ListSetByIndexAsync(key, index, value, flags);
        }

        public override async Task ListTrimAsync(RedisKey key, long start, long stop, CommandFlags flags = CommandFlags.None)
        {
            await Task.Factory.StartNew(() =>
            {
                foreach (IDatabase backupDB in BackupDBs)
                {
                    backupDB.ListTrimAsync(key, start, stop, flags);
                }
            });
            await base.ListTrimAsync(key, start, stop, flags);
        }

        public override async Task<RedisResult> ExecuteAsync(string command, params object[] args)
        {
            await Task.Factory.StartNew(() =>
            {
                foreach (IDatabase backupDB in BackupDBs)
                {
                    backupDB.Execute(command, args);
                }
            });
            return await base.ExecuteAsync(command, args);
        }

        public override async Task<RedisResult> ExecuteAsync(string command, ICollection<object> args, CommandFlags flags = CommandFlags.None)
        {
            await Task.Factory.StartNew(() =>
            {
                foreach (IDatabase backupDB in BackupDBs)
                {
                    backupDB.Execute(command, args, flags);
                }
            });
            return await base.ExecuteAsync(command, args, flags);
        }

        public override async Task<RedisResult> ScriptEvaluateAsync(string script, RedisKey[] keys = null, RedisValue[] values = null, CommandFlags flags = CommandFlags.None)
        {
            await Task.Factory.StartNew(() =>
            {
                foreach (IDatabase backupDB in BackupDBs)
                {
                    backupDB.ScriptEvaluateAsync(script, keys, values, flags);
                }
            });
            return await base.ScriptEvaluateAsync(script, keys, values, flags);
        }

        public override async Task<RedisResult> ScriptEvaluateAsync(byte[] hash, RedisKey[] keys = null, RedisValue[] values = null, CommandFlags flags = CommandFlags.None)
        {
            await Task.Factory.StartNew(() =>
            {
                foreach (IDatabase backupDB in BackupDBs)
                {
                    backupDB.ScriptEvaluate(hash, keys, values, flags);
                }
            });
            return await base.ScriptEvaluateAsync(hash, keys, values, flags);
        }

        public override async Task<RedisResult> ScriptEvaluateAsync(LuaScript script, object parameters = null, CommandFlags flags = CommandFlags.None)
        {
            await Task.Factory.StartNew(() =>
            {
                foreach (IDatabase backupDB in BackupDBs)
                {
                    backupDB.ScriptEvaluateAsync(script, parameters, flags);
                }
            });
            return await base.ScriptEvaluateAsync(script, parameters, flags);
        }

        public override async Task<RedisResult> ScriptEvaluateAsync(LoadedLuaScript script, object parameters = null, CommandFlags flags = CommandFlags.None)
        {
            await Task.Factory.StartNew(() =>
            {
                foreach (IDatabase backupDB in BackupDBs)
                {
                    backupDB.ScriptEvaluateAsync(script, parameters, flags);
                }
            });
            return await base.ScriptEvaluateAsync(script, parameters, flags);
        }

        public override async Task<bool> SetAddAsync(RedisKey key, RedisValue value, CommandFlags flags = CommandFlags.None)
        {
            await Task.Factory.StartNew(() =>
            {
                foreach (IDatabase backupDB in BackupDBs)
                {
                    backupDB.SetAddAsync(key, value, flags);
                }
            });
            return await base.SetAddAsync(key, value, flags);
        }

        public override async Task<long> SetAddAsync(RedisKey key, RedisValue[] values, CommandFlags flags = CommandFlags.None)
        {
            await Task.Factory.StartNew(() =>
            {
                foreach (IDatabase backupDB in BackupDBs)
                {
                    backupDB.SetAddAsync(key, values, flags);
                }
            });
            return await base.SetAddAsync(key, values, flags);
        }

        public override async Task<long> SetCombineAndStoreAsync(SetOperation operation, RedisKey destination, RedisKey first, RedisKey second, CommandFlags flags = CommandFlags.None)
        {
            await Task.Factory.StartNew(() =>
            {
                foreach (IDatabase backupDB in BackupDBs)
                {
                    backupDB.SetCombineAndStore(operation, destination, first, second, flags);
                }
            });
            return await base.SetCombineAndStoreAsync(operation, destination, first, second, flags);
        }

        public override async Task<long> SetCombineAndStoreAsync(SetOperation operation, RedisKey destination, RedisKey[] keys, CommandFlags flags = CommandFlags.None)
        {
            await Task.Factory.StartNew(() =>
            {
                foreach (IDatabase backupDB in BackupDBs)
                {
                    backupDB.SetCombineAndStore(operation, destination, keys, flags);
                }
            });
            return await base.SetCombineAndStoreAsync(operation, destination, keys, flags);
        }

        public override async Task<bool> SetMoveAsync(RedisKey source, RedisKey destination, RedisValue value, CommandFlags flags = CommandFlags.None)
        {
            await Task.Factory.StartNew(() =>
            {
                foreach (IDatabase backupDB in BackupDBs)
                {
                    backupDB.SetMove(source, destination, value, flags);
                }
            });
            return await base.SetMoveAsync(source, destination, value, flags);
        }

        public override async Task<RedisValue> SetPopAsync(RedisKey key, CommandFlags flags = CommandFlags.None)
        {
            await Task.Factory.StartNew(() =>
            {
                foreach (IDatabase backupDB in BackupDBs)
                {
                    backupDB.SetPopAsync(key, flags);
                }
            });
            return await base.SetPopAsync(key, flags);
        }

        public override async Task<RedisValue[]> SetPopAsync(RedisKey key, long count, CommandFlags flags = CommandFlags.None)
        {
            await Task.Factory.StartNew(() =>
            {
                foreach (IDatabase backupDB in BackupDBs)
                {
                    backupDB.SetPopAsync(key, count, flags);
                }
            });
            return await base.SetPopAsync(key, count, flags);
        }

        public override async Task<bool> SetRemoveAsync(RedisKey key, RedisValue value, CommandFlags flags = CommandFlags.None)
        {
            await Task.Factory.StartNew(() =>
            {
                foreach (IDatabase backupDB in BackupDBs)
                {
                    backupDB.SetRemoveAsync(key, value, flags);
                }
            });
            return await base.SetRemoveAsync(key, value, flags);
        }

        public override async Task<long> SetRemoveAsync(RedisKey key, RedisValue[] values, CommandFlags flags = CommandFlags.None)
        {
            await Task.Factory.StartNew(() =>
            {
                foreach (IDatabase backupDB in BackupDBs)
                {
                    backupDB.SetRemoveAsync(key, values, flags);
                }
            });
            return await base.SetRemoveAsync(key, values, flags);
        }

        public override async Task<long> SortAndStoreAsync(RedisKey destination, RedisKey key, long skip = 0, long take = -1, Order order = Order.Ascending, SortType sortType = SortType.Numeric, RedisValue by = default, RedisValue[] get = null, CommandFlags flags = CommandFlags.None)
        {
            await Task.Factory.StartNew(() =>
            {
                foreach (IDatabase backupDB in BackupDBs)
                {
                    backupDB.SortAndStore(destination, key, skip, take, order, sortType, by, get, flags);
                }
            });
            return await base.SortAndStoreAsync(destination, key, skip, take, order, sortType, by, get, flags);
        }

        public override async Task<bool> SortedSetAddAsync(RedisKey key, RedisValue member, double score, CommandFlags flags)
        {
            await Task.Factory.StartNew(() =>
            {
                foreach (IDatabase backupDB in BackupDBs)
                {
                    backupDB.SortedSetAddAsync(key, member, score, flags);
                }
            });
            return await base.SortedSetAddAsync(key, member, score, flags);
        }

        public override async Task<bool> SortedSetAddAsync(RedisKey key, RedisValue member, double score, When when = When.Always, CommandFlags flags = CommandFlags.None)
        {
            await Task.Factory.StartNew(() =>
            {
                foreach (IDatabase backupDB in BackupDBs)
                {
                    backupDB.SortedSetAddAsync(key, member, score, when, flags);
                }
            });
            return await base.SortedSetAddAsync(key, member, score, when, flags);
        }

        public override async Task<long> SortedSetAddAsync(RedisKey key, SortedSetEntry[] values, CommandFlags flags)
        {
            await Task.Factory.StartNew(() =>
            {
                foreach (IDatabase backupDB in BackupDBs)
                {
                    backupDB.SortedSetAddAsync(key, values, flags);
                }
            });
            return await base.SortedSetAddAsync(key, values, flags);
        }

        public override async Task<long> SortedSetAddAsync(RedisKey key, SortedSetEntry[] values, When when = When.Always, CommandFlags flags = CommandFlags.None)
        {
            await Task.Factory.StartNew(() =>
            {
                foreach (IDatabase backupDB in BackupDBs)
                {
                    backupDB.SortedSetAddAsync(key, values, when, flags);
                }
            });
            return await base.SortedSetAddAsync(key, values, when, flags);
        }

        public override async Task<long> SortedSetCombineAndStoreAsync(SetOperation operation, RedisKey destination, RedisKey first, RedisKey second, Aggregate aggregate = Aggregate.Sum, CommandFlags flags = CommandFlags.None)
        {
            await Task.Factory.StartNew(() =>
            {
                foreach (IDatabase backupDB in BackupDBs)
                {
                    backupDB.SortedSetCombineAndStore(operation, destination, first, second, aggregate, flags);
                }
            });
            return await base.SortedSetCombineAndStoreAsync(operation, destination, first, second, aggregate, flags);
        }

        public override async Task<long> SortedSetCombineAndStoreAsync(SetOperation operation, RedisKey destination, RedisKey[] keys, double[] weights = null, Aggregate aggregate = Aggregate.Sum, CommandFlags flags = CommandFlags.None)
        {
            await Task.Factory.StartNew(() =>
            {
                foreach (IDatabase backupDB in BackupDBs)
                {
                    backupDB.SortedSetCombineAndStore(operation, destination, keys, weights, aggregate, flags);
                }
            });
            return await base.SortedSetCombineAndStoreAsync(operation, destination, keys, weights, aggregate, flags);
        }

        public override async Task<double> SortedSetDecrementAsync(RedisKey key, RedisValue member, double value, CommandFlags flags = CommandFlags.None)
        {
            await Task.Factory.StartNew(() =>
            {
                foreach (IDatabase backupDB in BackupDBs)
                {
                    backupDB.SortedSetDecrementAsync(key, member, value, flags);
                }
            });
            return await base.SortedSetDecrementAsync(key, member, value, flags);
        }

        public override async Task<double> SortedSetIncrementAsync(RedisKey key, RedisValue member, double value, CommandFlags flags = CommandFlags.None)
        {
            await Task.Factory.StartNew(() =>
            {
                foreach (IDatabase backupDB in BackupDBs)
                {
                    backupDB.SortedSetIncrementAsync(key, member, value, flags);
                }
            });
            return await base.SortedSetIncrementAsync(key, member, value, flags);
        }

        public override async Task<bool> SortedSetRemoveAsync(RedisKey key, RedisValue member, CommandFlags flags = CommandFlags.None)
        {
            await Task.Factory.StartNew(() =>
            {
                foreach (IDatabase backupDB in BackupDBs)
                {
                    backupDB.SortedSetRemoveAsync(key, member, flags);
                }
            });
            return await base.SortedSetRemoveAsync(key, member, flags);
        }

        public override async Task<long> SortedSetRemoveAsync(RedisKey key, RedisValue[] members, CommandFlags flags = CommandFlags.None)
        {
            await Task.Factory.StartNew(() =>
            {
                foreach (IDatabase backupDB in BackupDBs)
                {
                    backupDB.SortedSetRemoveAsync(key, members, flags);
                }
            });
            return await base.SortedSetRemoveAsync(key, members, flags);
        }

        public override async Task<long> SortedSetRemoveRangeByRankAsync(RedisKey key, long start, long stop, CommandFlags flags = CommandFlags.None)
        {
            await Task.Factory.StartNew(() =>
            {
                foreach (IDatabase backupDB in BackupDBs)
                {
                    backupDB.SortedSetRemoveRangeByRankAsync(key, start, stop, flags);
                }
            });
            return await base.SortedSetRemoveRangeByRankAsync(key, start, stop, flags);
        }

        public override async Task<long> SortedSetRemoveRangeByScoreAsync(RedisKey key, double start, double stop, Exclude exclude = Exclude.None, CommandFlags flags = CommandFlags.None)
        {
            await Task.Factory.StartNew(() =>
            {
                foreach (IDatabase backupDB in BackupDBs)
                {
                    backupDB.SortedSetRemoveRangeByScoreAsync(key, start, stop, exclude, flags);
                }
            });
            return await base.SortedSetRemoveRangeByScoreAsync(key, start, stop, exclude, flags);
        }

        public override async Task<long> SortedSetRemoveRangeByValueAsync(RedisKey key, RedisValue min, RedisValue max, Exclude exclude = Exclude.None, CommandFlags flags = CommandFlags.None)
        {
            await Task.Factory.StartNew(() =>
            {
                foreach (IDatabase backupDB in BackupDBs)
                {
                    backupDB.SortedSetRemoveRangeByValueAsync(key, min, max, exclude, flags);
                }
            });
            return await base.SortedSetRemoveRangeByValueAsync(key, min, max, exclude, flags);
        }

        public override async Task<SortedSetEntry?> SortedSetPopAsync(RedisKey key, Order order = Order.Ascending, CommandFlags flags = CommandFlags.None)
        {
            await Task.Factory.StartNew(() =>
            {
                foreach (IDatabase backupDB in BackupDBs)
                {
                    backupDB.SortedSetPopAsync(key, order, flags);
                }
            });
            return await base.SortedSetPopAsync(key, order, flags);
        }

        public override async Task<SortedSetEntry[]> SortedSetPopAsync(RedisKey key, long count, Order order = Order.Ascending, CommandFlags flags = CommandFlags.None)
        {
            await Task.Factory.StartNew(() =>
            {
                foreach (IDatabase backupDB in BackupDBs)
                {
                    backupDB.SortedSetPopAsync(key, count, order, flags);
                }
            });
            return await base.SortedSetPopAsync(key, count, order, flags);
        }

        public override async Task<long> StringAppendAsync(RedisKey key, RedisValue value, CommandFlags flags = CommandFlags.None)
        {
            await Task.Factory.StartNew(() =>
            {
                foreach (IDatabase backupDB in BackupDBs)
                {
                    backupDB.StringAppendAsync(key, value, flags);
                }
            });
            return await base.StringAppendAsync(key, value, flags);
        }

        public override async Task<long> StringBitOperationAsync(Bitwise operation, RedisKey destination, RedisKey first, RedisKey second = default, CommandFlags flags = CommandFlags.None)
        {
            await Task.Factory.StartNew(() =>
            {
                foreach (IDatabase backupDB in BackupDBs)
                {
                    backupDB.StringBitOperation(operation, destination, first, second, flags);
                }
            });
            return await base.StringBitOperationAsync(operation, destination, first, second, flags);
        }

        public override async Task<long> StringDecrementAsync(RedisKey key, long value = 1, CommandFlags flags = CommandFlags.None)
        {
            await Task.Factory.StartNew(() =>
            {
                foreach (IDatabase backupDB in BackupDBs)
                {
                    backupDB.StringDecrementAsync(key, value, flags);
                }
            });
            return await base.StringDecrementAsync(key, value, flags);
        }

        public override async Task<double> StringDecrementAsync(RedisKey key, double value, CommandFlags flags = CommandFlags.None)
        {
            await Task.Factory.StartNew(() =>
            {
                foreach (IDatabase backupDB in BackupDBs)
                {
                    backupDB.StringDecrementAsync(key, value, flags);
                }
            });
            return await base.StringDecrementAsync(key, value, flags);
        }

        public override async Task<long> StringIncrementAsync(RedisKey key, long value = 1, CommandFlags flags = CommandFlags.None)
        {
            await Task.Factory.StartNew(() =>
            {
                foreach (IDatabase backupDB in BackupDBs)
                {
                    backupDB.StringIncrementAsync(key, value, flags);
                }
            });
            return await base.StringIncrementAsync(key, value, flags);
        }

        public override async Task<double> StringIncrementAsync(RedisKey key, double value, CommandFlags flags = CommandFlags.None)
        {
            await Task.Factory.StartNew(() =>
            {
                foreach (IDatabase backupDB in BackupDBs)
                {
                    backupDB.StringIncrementAsync(key, value, flags);
                }
            });
            return await base.StringIncrementAsync(key, value, flags);
        }

        public override async Task<bool> StringSetAsync(RedisKey key, RedisValue value, TimeSpan? expiry = null, When when = When.Always, CommandFlags flags = CommandFlags.None)
        {
            await Task.Factory.StartNew(() =>
            {
                foreach (IDatabase backupDB in BackupDBs)
                {
                    backupDB.StringSetAsync(key, value, expiry, when, flags);
                }
            });
            return await base.StringSetAsync(key, value, expiry, when, flags);
        }

        public override async Task<bool> StringSetAsync(KeyValuePair<RedisKey, RedisValue>[] values, When when = When.Always, CommandFlags flags = CommandFlags.None)
        {
            await Task.Factory.StartNew(() =>
            {
                foreach (IDatabase backupDB in BackupDBs)
                {
                    backupDB.StringSet(values, when, flags);
                }
            });
            return await base.StringSetAsync(values, when, flags);
        }

        public override async Task<bool> StringSetBitAsync(RedisKey key, long offset, bool bit, CommandFlags flags = CommandFlags.None)
        {
            await Task.Factory.StartNew(() =>
            {
                foreach (IDatabase backupDB in BackupDBs)
                {
                    backupDB.StringSetBitAsync(key, offset, bit, flags);
                }
            });
            return await base.StringSetBitAsync(key, offset, bit, flags);
        }

        public override async Task<RedisValue> StringSetRangeAsync(RedisKey key, long offset, RedisValue value, CommandFlags flags = CommandFlags.None)
        {
            await Task.Factory.StartNew(() =>
            {
                foreach (IDatabase backupDB in BackupDBs)
                {
                    backupDB.StringSetRangeAsync(key, offset, value, flags);
                }
            });
            return await base.StringSetRangeAsync(key, offset, value, flags);
        }

        public override async Task<bool> KeyTouchAsync(RedisKey key, CommandFlags flags = CommandFlags.None)
        {
            await Task.Factory.StartNew(() =>
            {
                foreach (IDatabase backupDB in BackupDBs)
                {
                    backupDB.KeyTouchAsync(key, flags);
                }
            });
            return await base.KeyTouchAsync(key, flags);
        }

        public override async Task<long> KeyTouchAsync(RedisKey[] keys, CommandFlags flags = CommandFlags.None)
        {
            await Task.Factory.StartNew(() =>
            {
                foreach (IDatabase backupDB in BackupDBs)
                {
                    backupDB.KeyTouchAsync(keys, flags);
                }
            });
            return await base.KeyTouchAsync(keys, flags);
        }

        public override RedisValue[] ListLeftPop(RedisKey key, long count, CommandFlags flags = CommandFlags.None)
        {
            Task.Factory.StartNew(() =>
            {
                foreach (IDatabase backupDB in BackupDBs)
                {
                    backupDB.ListLeftPop(key, count, flags);
                }
            });
            return base.ListLeftPop(key, count, flags);
        }

        public override RedisValue[] ListRightPop(RedisKey key, long count, CommandFlags flags = CommandFlags.None)
        {
            Task.Factory.StartNew(() =>
            {
                foreach (IDatabase backupDB in BackupDBs)
                {
                    backupDB.ListRightPop(key, count, flags);
                }
            });
            return base.ListRightPop(key, count, flags);
        }

        public override RedisValue StringGetDelete(RedisKey key, CommandFlags flags = CommandFlags.None)
        {
            Task.Factory.StartNew(() =>
            {
                foreach (IDatabase backupDB in BackupDBs)
                {
                    backupDB.StringGetDelete(key, flags);
                }
            });
            return base.StringGetDelete(key, flags);
        }

        public override async Task<RedisValue[]> ListLeftPopAsync(RedisKey key, long count, CommandFlags flags = CommandFlags.None)
        {
            await Task.Factory.StartNew(() =>
            {
                foreach (IDatabase backupDB in BackupDBs)
                {
                    backupDB.ListLeftPopAsync(key, count, flags);
                }
            });
            return await base.ListLeftPopAsync(key, count, flags);
        }

        public override async Task<RedisValue[]> ListRightPopAsync(RedisKey key, long count, CommandFlags flags = CommandFlags.None)
        {
            await Task.Factory.StartNew(() =>
            {
                foreach (IDatabase backupDB in BackupDBs)
                {
                    backupDB.ListRightPopAsync(key, count, flags);
                }
            });
            return await base.ListRightPopAsync(key, count, flags);
        }

        public override async Task<RedisValue> StringGetDeleteAsync(RedisKey key, CommandFlags flags = CommandFlags.None)
        {
            await Task.Factory.StartNew(() =>
            {
                foreach (IDatabase backupDB in BackupDBs)
                {
                    backupDB.StringGetDeleteAsync(key, flags);
                }
            });
            return await base.StringGetDeleteAsync(key, flags);
        }
    }
}

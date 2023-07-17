using log4net;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace QinSoft.Core.Common.Utils
{
    /// <summary>
    /// 重试工具类
    /// </summary>
    public static class RetryUtils
    {
        /// <summary>
        /// 重试默认日志
        /// </summary>
        public static ILog DefaultLog { get; set; } = LogManager.GetLogger(typeof(RetryUtils));

        public delegate void Function();

        public delegate Task FunctionAsync();

        public delegate void Function<T>(T obj);

        public delegate Task FunctionAsync<T>(T obj);

        public delegate O Function<T, O>(T obj);

        public delegate Task<O> FunctionAsync<T, O>(T obj);

        public static void Retry(this Function exec, int count = 3, int interval = 1000)
        {
            for (int index = 0; index < count; index++)
            {
                try
                {
                    exec.Invoke();
                    break;
                }
                catch (Exception e)
                {
                    if (index + 1 == count)
                    {
                        DefaultLog?.Debug("retry", e);
                        throw e;
                    }
                    Thread.Sleep(interval);
                }
            }
        }

        public static async Task RetryAsync(this FunctionAsync exec, int count = 3, int interval = 1000)
        {
            for (int index = 0; index < count; index++)
            {
                try
                {
                    await exec.Invoke();
                    break;
                }
                catch (Exception e)
                {
                    if (index + 1 == count)
                    {
                        DefaultLog?.Debug("retry", e);
                        throw e;
                    }
                    Thread.Sleep(interval);
                }
            }
        }

        public static void Retry<T>(this Function<T> exec, T param, int count = 3, int interval = 1000)
        {
            for (int index = 0; index < count; index++)
            {
                try
                {
                    exec.Invoke(param);
                    break;
                }
                catch (Exception e)
                {
                    if (index + 1 == count)
                    {
                        DefaultLog?.Debug("retry", e);
                        throw e;
                    }
                    Thread.Sleep(interval);
                }
            }
        }

        public static async Task RetryAsync<T>(this FunctionAsync<T> exec, T param, int count = 3, int interval = 1000)
        {
            for (int index = 0; index < count; index++)
            {
                try
                {
                    await exec.Invoke(param);
                    break;
                }
                catch (Exception e)
                {
                    if (index + 1 == count)
                    {
                        DefaultLog?.Debug("retry", e);
                        throw e;
                    }
                    Thread.Sleep(interval);
                }
            }
        }

        public static O Retry<T, O>(this Function<T, O> exec, T param, int count = 3, int interval = 1000)
        {
            for (int index = 0; index < count; index++)
            {
                try
                {
                    return exec.Invoke(param);
                }
                catch (Exception e)
                {
                    if (index + 1 == count)
                    {
                        DefaultLog?.Debug("retry", e);
                        throw e;
                    }
                    Thread.Sleep(interval);
                }
            }
            return default;
        }

        public static async Task<O> RetryAsync<T, O>(this FunctionAsync<T, O> exec, T param, int count = 3, int interval = 1000)
        {
            for (int index = 0; index < count; index++)
            {
                try
                {
                    return await exec.Invoke(param);
                }
                catch (Exception e)
                {
                    if (index + 1 == count)
                    {
                        DefaultLog?.Debug("retry", e);
                        throw e;
                    }
                    Thread.Sleep(interval);
                }
            }
            return default;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace QinSoft.Core.Common.Utils
{
    /// <summary>
    /// 执行工具
    /// </summary>
    public static class ExecuteUtils
    {
        /// <summary>
        /// 在线程中执行
        /// </summary>
        public static Thread ExecuteInThread(this Action action, string name = null, bool isBackground = true, ThreadPriority priority = ThreadPriority.Normal)
        {
            Thread thread = new Thread(new ThreadStart(action));
            if (!string.IsNullOrEmpty(name))
            {
                thread.Name = name;
            }
            thread.IsBackground = isBackground;
            thread.Priority = priority;
            thread.Start();
            return thread;
        }

        /// <summary>
        /// 在线程中执行
        /// </summary>
        public static Thread ExecuteInThread(this Action<object> action, object param, string name = null, bool isBackground = true, ThreadPriority priority = ThreadPriority.Normal)
        {
            Thread thread = new Thread(new ParameterizedThreadStart(action));
            if (!string.IsNullOrEmpty(name))
            {
                thread.Name = name;
            }
            thread.IsBackground = isBackground;
            thread.Priority = priority;
            thread.Start(param);
            return thread;
        }

        /// <summary>
        /// 在线程池中执行
        /// </summary>
        public static void ExecuteThreadPool(this Action action)
        {
            ThreadPool.QueueUserWorkItem(state => action(), null);
        }

        /// <summary>
        /// 在线程池中执行
        /// </summary>
        public static void ExecuteThreadPool(this Action<object> action, object state)
        {
            ThreadPool.QueueUserWorkItem(new WaitCallback(action), state);
        }

        /// <summary>
        /// 在任务中执行
        /// </summary>
        public static Task ExecuteInTask(this Action action)
        {
            return Task.Factory.StartNew(action);
        }

        /// <summary>
        /// 在任务中执行
        /// </summary>
        public static Task ExecuteInTask<T>(this Action<T> action, T param)
        {
            return Task.Factory.StartNew(() => action(param));
        }

        /// <summary>
        /// 在任务中执行
        /// </summary>
        public static Task<O> ExecuteInTask<O>(this Func<O> action)
        {
            return Task.Factory.StartNew<O>(action);
        }

        /// <summary>
        /// 在任务中执行
        /// </summary>
        public static Task<O> ExecuteInTask<T, O>(this Func<T, O> action, T param)
        {
            return Task.Factory.StartNew<O>(() => action(param));
        }
    }
}

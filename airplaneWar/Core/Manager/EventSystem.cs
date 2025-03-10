//using System;
//using System.Collections.Concurrent;
//using System.Collections.Generic;
//using System.Threading;
//using System.Windows.Forms;

//namespace airplaneWar.Core.Events
//{
//    /// <summary>
//    /// 事件系统核心类（线程安全）
//    /// </summary>
//    public static class EventSystem
//    {
//        #region 内部数据结构
//        private class EventHandlerWrapper
//        {
//            public Delegate Action { get; }
//            public SynchronizationContext Context { get; }
//            public ThreadPriority Priority { get; }

//            public EventHandlerWrapper(Delegate action, SynchronizationContext context, ThreadPriority priority)
//            {
//                Action = action;
//                Context = context;
//                Priority = priority;
//            }
//        }

//        // 使用ConcurrentDictionary保证线程安全
//        private static readonly ConcurrentDictionary<Type, List<EventHandlerWrapper>>
//            _eventHandlers = new ConcurrentDictionary<Type, List<EventHandlerWrapper>>();
//        #endregion

//        #region 公共API
//        /// <summary>
//        /// 注册事件监听（带泛型类型推断）
//        /// </summary>
//        public static void Register<T>(Action<T> handler,
//            ThreadPriority priority = ThreadPriority.Normal,
//            SynchronizationContext? context = null) where T : IGameEvent
//        {
//            var type = typeof(T);
//            var context = SynchronizationContext.Current;

//            _eventHandlers.AddOrUpdate(type,
//                _ => new List<EventHandlerWrapper>
//                    { new EventHandlerWrapper(handler, context, priority) },
//                (_, list) =>
//                {
//                    list.Add(new EventHandlerWrapper(handler, context, priority));
//                    return list;
//                });
//        }

//        /// <summary>
//        /// 注销事件监听
//        /// </summary>
//        public static void Unregister<T>(Action<T> handler) where T : IGameEvent
//        {
//            var type = typeof(T);
//            if (_eventHandlers.TryGetValue(type, out var handlers))
//            {
//                handlers.RemoveAll(w => w.Action.Equals(handler));
//            }
//        }

//        /// <summary>
//        /// 触发事件（支持跨线程）
//        /// </summary>
//        public static void Trigger<T>(T eventData) where T : IGameEvent
//        {
//            if (_eventHandlers.TryGetValue(typeof(T), out var handlers))
//            {
//                // 根据优先级排序
//                handlers.Sort((a, b) => b.Priority.CompareTo(a.Priority));

//                foreach (var wrapper in handlers.ToArray()) // 使用ToArray防止迭代时修改
//                {
//                    DispatchEvent(wrapper, eventData);
//                }
//            }
//        }
//        #endregion

//        #region 私有方法
//        private static void DispatchEvent<T>(EventHandlerWrapper wrapper, T eventData)
//            where T : IGameEvent
//        {
//            var handler = (Action<T>)wrapper.Action;

//            if (wrapper.Context != null)
//            {
//                // 使用原始同步上下文（如UI线程）
//                wrapper.Context.Post(_ => handler(eventData), null);
//            }
//            else
//            {
//                // 使用线程池调度
//                ThreadPool.QueueUserWorkItem(_ =>
//                {
//                    SetThreadPriority(wrapper.Priority);
//                    handler(eventData);
//                });
//            }
//        }

//        private static void SetThreadPriority(ThreadPriority priority)
//        {
//            try
//            {
//                Thread.CurrentThread.Priority = priority;
//            }
//            catch
//            {
//                // 处理权限不足的情况
//            }
//        }
//        #endregion

//        #region 扩展功能
//        /// <summary>
//        /// 一次性监听（自动注销）
//        /// </summary>
//        public static void RegisterOnce<T>(Action<T> handler) where T : IGameEvent
//        {
//            Action<T> wrapper = null;
//            wrapper = evt =>
//            {
//                handler(evt);
//                Unregister(wrapper);
//            };
//            Register(wrapper);
//        }

//        /// <summary>
//        /// 异步触发并等待所有处理器完成
//        /// </summary>
//        public static async System.Threading.Tasks.Task TriggerAsync<T>(T eventData)
//            where T : IGameEvent
//        {
//            var completionSource = new System.Threading.Tasks.TaskCompletionSource<bool>();
//            int remaining = 0;

//            if (_eventHandlers.TryGetValue(typeof(T), out var handlers))
//            {
//                remaining = handlers.Count;
//                if (remaining == 0)
//                {
//                    completionSource.SetResult(true);
//                    return;
//                }

//                foreach (var wrapper in handlers)
//                {
//                    var handler = (Action<T>)wrapper.Action;
//                    if (wrapper.Context != null)
//                    {
//                        wrapper.Context.Post(_ =>
//                        {
//                            handler(eventData);
//                            if (Interlocked.Decrement(ref remaining) == 0)
//                                completionSource.SetResult(true);
//                        }, null);
//                    }
//                    else
//                    {
//                        ThreadPool.QueueUserWorkItem(_ =>
//                        {
//                            handler(eventData);
//                            if (Interlocked.Decrement(ref remaining) == 0)
//                                completionSource.SetResult(true);
//                        });
//                    }
//                }
//            }

//            await completionSource.Task;
//        }
//        #endregion
//    }

//    /// <summary>
//    /// 游戏事件标记接口
//    /// </summary>
//    public interface IGameEvent { }

//    /// <summary>
//    /// UI线程同步上下文扩展
//    /// </summary>
//    public static class EventSystemExtensions
//    {
//        /// <summary>
//        /// 确保在UI线程执行
//        /// </summary>
//        public static void RegisterOnUI<T>(this Control control, Action<T> handler)
//            where T : IGameEvent
//        {
//            var context = SynchronizationContext.Current;
//            EventSystem.Register<T>(evt =>
//            {
//                if (control.InvokeRequired)
//                {
//                    control.BeginInvoke(new Action(() => handler(evt)));
//                }
//                else
//                {
//                    handler(evt);
//                }
//            }, context: context);
//        }
//    }
//}
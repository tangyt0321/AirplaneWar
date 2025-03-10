//using System;
//using System.Collections.Concurrent;
//using System.Collections.Generic;
//using System.Threading;
//using System.Windows.Forms;

//namespace airplaneWar.Core.Events
//{
//    /// <summary>
//    /// �¼�ϵͳ�����ࣨ�̰߳�ȫ��
//    /// </summary>
//    public static class EventSystem
//    {
//        #region �ڲ����ݽṹ
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

//        // ʹ��ConcurrentDictionary��֤�̰߳�ȫ
//        private static readonly ConcurrentDictionary<Type, List<EventHandlerWrapper>>
//            _eventHandlers = new ConcurrentDictionary<Type, List<EventHandlerWrapper>>();
//        #endregion

//        #region ����API
//        /// <summary>
//        /// ע���¼������������������ƶϣ�
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
//        /// ע���¼�����
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
//        /// �����¼���֧�ֿ��̣߳�
//        /// </summary>
//        public static void Trigger<T>(T eventData) where T : IGameEvent
//        {
//            if (_eventHandlers.TryGetValue(typeof(T), out var handlers))
//            {
//                // �������ȼ�����
//                handlers.Sort((a, b) => b.Priority.CompareTo(a.Priority));

//                foreach (var wrapper in handlers.ToArray()) // ʹ��ToArray��ֹ����ʱ�޸�
//                {
//                    DispatchEvent(wrapper, eventData);
//                }
//            }
//        }
//        #endregion

//        #region ˽�з���
//        private static void DispatchEvent<T>(EventHandlerWrapper wrapper, T eventData)
//            where T : IGameEvent
//        {
//            var handler = (Action<T>)wrapper.Action;

//            if (wrapper.Context != null)
//            {
//                // ʹ��ԭʼͬ�������ģ���UI�̣߳�
//                wrapper.Context.Post(_ => handler(eventData), null);
//            }
//            else
//            {
//                // ʹ���̳߳ص���
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
//                // ����Ȩ�޲�������
//            }
//        }
//        #endregion

//        #region ��չ����
//        /// <summary>
//        /// һ���Լ������Զ�ע����
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
//        /// �첽�������ȴ����д��������
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
//    /// ��Ϸ�¼���ǽӿ�
//    /// </summary>
//    public interface IGameEvent { }

//    /// <summary>
//    /// UI�߳�ͬ����������չ
//    /// </summary>
//    public static class EventSystemExtensions
//    {
//        /// <summary>
//        /// ȷ����UI�߳�ִ��
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
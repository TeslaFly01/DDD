using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.Concurrent;
using System.Threading;
using System.Threading.Tasks;

namespace DDD.Utility
{
    /// <summary>
    /// 生产消费模式类
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class ProducerConsumer<T>
    {
        //消息队列
        public struct MessageQueue
        {
            public T Data;
        }
        //申明生产者
        private readonly ThreadRunner _producer;
        //申明消费者列表
        private readonly List<ThreadRunner> _listConsumer;

        public ProducerConsumer(Func<List<T>> produceFunc, Action<T> consumeAction, int remainStandbyCount, int consumerThreadCount)
        {
            //构造一个线程安全集合的消息队列
            var messageQueue = new BlockingCollection<MessageQueue>();
            //构造一个生产者，传入生产方法，线程安全集合，小于指定数量进行生产的依据
            _producer = new Producer(produceFunc, messageQueue, remainStandbyCount);
            //构造指定数量的消费者，传入消费方法，线程安全集合
            var tempListConsumer = Enumerable.Range(0, consumerThreadCount).Select(c => new Consumer(consumeAction, messageQueue)).ToList();
            _listConsumer = new List<ThreadRunner>();
            tempListConsumer.ForEach(c =>
            {
                _listConsumer.Add(c);
            });

        }

        //进行生产-消费
        public void Start()
        {
            _producer.Run();
            _listConsumer.ForEach(c => c.Run());
        }
        //停止生产-消费
        public void Stop()
        {
            _listConsumer.ForEach(c => c.Stop());
            _producer.Stop();
        }
        /// <summary>
        /// 生产者类
        /// </summary>
        private class Producer : ThreadRunner
        {
            //定义生产方法，批量生产
            private readonly Func<List<T>> _produceFunc;
            //定义消息队列数量小于指定数量时进行生产的依据
            private readonly int _RemainStandbyCount;

            // private static object _producerLockObj = new object();

            public Producer(Func<List<T>> produceFunc, BlockingCollection<MessageQueue> messageQueue, int remainStandbyCount)
                : base(messageQueue)
            {
                _produceFunc = produceFunc;
                _RemainStandbyCount = remainStandbyCount;
            }

            //重写基类的工作者业务逻辑
            public override void Worker()
            {
                //一直运行，进行生产
                while (KeepRunning)
                {
                    try
                    {
                        //如果消息队列小于指定数量时，进行生产行为
                        if (_messageQueue.Count < this._RemainStandbyCount)
                        {
                            //获取消息数据源
                            var listSMSDto = _produceFunc();
                            //将消息加入到消息队列中
                            foreach (var item in listSMSDto)
                            {
                                _messageQueue.TryAdd(new MessageQueue { Data = item });
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        WasInterrupted = true;
                    }
                }
            }
        }
        /// <summary>
        /// 消费者类
        /// </summary>
        private class Consumer : ThreadRunner
        {
            //定义消费方法，进行消费行为
            private readonly Action<T> _consumeAction;

            public Consumer(Action<T> consumeAction, BlockingCollection<MessageQueue> messageQueue)
                : base(messageQueue)
            {
                _consumeAction = consumeAction;
            }

            //重写基类的工作者业务逻辑
            public override void Worker()
            {
                //一直运行，进行消费
                while (KeepRunning)
                {
                    try
                    {
                        //从消息队列取出消息，进行处理消息，如果没有，就休眠1秒中。
                        MessageQueue message;
                        if (_messageQueue.TryTake(out message, TimeSpan.FromMilliseconds(500)))
                        {
                            //进行消费行为
                            _consumeAction(message.Data);
                        }
                        else
                        {
                            //There's nothing in the Q so I have some spare time...
                            //Excellent moment to update my statisics or update some history to logfiles
                            //for now we sleep:
                            Thread.Sleep(TimeSpan.FromMilliseconds(1000));
                        }
                    }
                    catch (Exception ex)
                    {
                        WasInterrupted = true;
                    }
                }
            }
        }

        /// <summary>
        /// 工作者基类，为生产者和消费者提供复用代码
        /// </summary>
        public abstract class ThreadRunner
        {
            //定义线程安全集合的消息队列
            protected readonly BlockingCollection<MessageQueue> _messageQueue;
            protected readonly CancellationTokenSource tokenSource = new CancellationTokenSource();
            protected ThreadRunner(BlockingCollection<MessageQueue> messageQueue)
            {
                _messageQueue = messageQueue;
            }

            //工作线程，进行生产或者消费
            protected Task Runner;
            //工作运行标志
            protected bool KeepRunning = true;
            //是否被中断
            public bool WasInterrupted;
            //工作方法
            public abstract void Worker();
            //工作线程运行
            public void Run()
            {
                KeepRunning = true;
                Runner = Task.Factory.StartNew(() => { Worker(); }, tokenSource.Token);
            }
            //工作线程停止
            public void Stop()
            {
                KeepRunning = false;
                tokenSource.Cancel();
                Runner.Wait(); //TryAdd TryTake 会观察tokenSource.Token，所以手动停止后，等到之前task执行完成后才能再次启动
                //，避免再次启动后，部分tokenSource还在Cancel，导致无法正常run task
            }
        }
    }
}

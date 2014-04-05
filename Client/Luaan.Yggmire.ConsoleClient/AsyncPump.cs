using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Luaan.Yggmire.ConsoleClient
{
    /// <summary>Provides a pump that supports running asynchronous methods on the current thread.</summary>
    public static class AsyncPump
    {
        /// <summary>Runs the specified asynchronous function.</summary>
        /// <param name="func">The asynchronous function to execute.</param>
        public static void Run(Task task)
        {
            if (task == null) throw new ArgumentNullException("task");

            var prevCtx = SynchronizationContext.Current;
            try
            {
                // Establish the new context
                var syncCtx = new SingleThreadSynchronizationContext();
                SynchronizationContext.SetSynchronizationContext(syncCtx);

                task.ContinueWith(delegate { syncCtx.Complete(); }, TaskScheduler.Default);

                // Pump continuations and propagate any exceptions
                syncCtx.RunOnCurrentThread();
                task.GetAwaiter().GetResult();
            }
            finally { SynchronizationContext.SetSynchronizationContext(prevCtx); }
        }

        /// <summary>Provides a SynchronizationContext that's single-threaded.</summary>
        private sealed class SingleThreadSynchronizationContext : SynchronizationContext
        {
            /// <summary>The queue of work items.</summary>
            private readonly BlockingCollection<KeyValuePair<SendOrPostCallback, object>> m_queue =
                new BlockingCollection<KeyValuePair<SendOrPostCallback, object>>();
            /// <summary>The processing thread.</summary>
            private readonly Thread m_thread = Thread.CurrentThread;

            /// <summary>Dispatches an asynchronous message to the synchronization context.</summary>
            /// <param name="d">The System.Threading.SendOrPostCallback delegate to call.</param>
            /// <param name="state">The object passed to the delegate.</param>
            public override void Post(SendOrPostCallback d, object state)
            {
                if (d == null) throw new ArgumentNullException("d");
                m_queue.Add(new KeyValuePair<SendOrPostCallback, object>(d, state));
            }

            /// <summary>Not supported.</summary>
            public override void Send(SendOrPostCallback d, object state)
            {
                throw new NotSupportedException("Synchronously sending is not supported.");
            }

            /// <summary>Runs an loop to process all queued work items.</summary>
            public void RunOnCurrentThread()
            {
                foreach (var workItem in m_queue.GetConsumingEnumerable())
                    workItem.Key(workItem.Value);
            }

            /// <summary>Notifies the context that no more work will arrive.</summary>
            public void Complete() { m_queue.CompleteAdding(); }
        }
    }
}

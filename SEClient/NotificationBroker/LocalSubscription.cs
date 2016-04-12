using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace MT.Platform.Common
{
    /// <summary>
    /// Application specific Notification Broker message handler delegate.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="notification"></param>
    public delegate void NotificationHandler(object sender, Notification notification);

    /// <summary>
    /// Subscriber using in-AppDomain communication via delegates.
    /// </summary>
    public class LocalSubscription : Subscription
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="type">type of notification this subscriber is interested to receive</param>
        /// <param name="filters">notification message filter defining rules for filter in or out of messages</param>
        /// <param name="handler">(Multicast-)Delegate to invoke for notification delivery</param>
        public LocalSubscription(Type type, NotificationHandler handler, IList<ISubscriptionFilter> filters)
            : base(type, filters)
        {
            lock (LockObject)
            {
                this.handler = handler;
            }
        }

        private NotificationHandler handler;
        /// <summary>
        /// Delegate to invoke when a notification of a desired type arrives.
        /// </summary>
        public NotificationHandler Handler
        {
            get { lock (LockObject) { return handler; } }
        }

        /// <summary>
        /// Safely invoke delegates to fire notification. Done on a separate thread pool thread.
        /// </summary>
        /// <param name="notification"></param>
        public override void Send(Notification notification)
        {
            // Broadcast messages individually
            Delegate[] invocationList = null;
            lock (LockObject)
            {
                if (this.Handler == null) return;
                invocationList = (Delegate[])this.Handler.GetInvocationList().Clone();
            }

            // If not a multicast delegate, then invoke in this thread, as Subscriptions.Fire already
            // scheduled invocation onto a separate thread from the one calling ServiceAccessor.GetService<INotificationBroker>().Fire()
            if (invocationList.Length == 1)
            {
                // singlecast delegate (only 1 handler). Call directly from this thread pool thread.
                // For performance reasons do not switch to yet another thread.
                invocationList[0].DynamicInvoke(this, notification);
            }
            else
            {
                // Invoke every delegate separately to allow for exception handling. 
                // Otherwise following handlers are not invoked.
                foreach (NotificationHandler oneNotiticationHandler in invocationList)
                {
                    try
                    {
                        ThreadPool.QueueUserWorkItem(delegate(object state)
                            {
                                NotificationHandler handler = state as NotificationHandler;

                                // invoke one separate invocation handler on pool thread to decouple.
                                handler(this, notification);
                            }, oneNotiticationHandler);
                    }
                    catch (Exception ex)
                    {
                        throw new NotImplementedException("One of the multicast delegate invocations on "
                            + "subscription handlers failed. Decide on actions in this catch block!", ex);
                    }
                }
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Diagnostics.CodeAnalysis;

namespace MT.Platform.Common
{
    /// <summary>
    /// Subscription base class. Contains notification "type" and notification message filters.
    /// </summary>
    public abstract class Subscription
    {
        private object lockObject = new object();
        /// <summary>
        /// Lock for ensuring thread safety of subscriptions.
        /// </summary>
        protected object LockObject { get { return lockObject; } }

        /// <summary>
        /// Constructor
        /// </summary>
		/// <param name="notificationType">type of notification this subscriber is interested to receive</param>
        /// <param name="filters">notification message filter defining rules for filter in or out of messages</param>
        protected Subscription(Type notificationType, IList<ISubscriptionFilter> filters)
        {
            lock (lockObject)
            {
                this.notificationType = notificationType;
                this.filters = filters;
            }
        }

        private Type notificationType = null;
        /// <summary>
        /// Notification type. Type.Missing means to subscribe for any type of notification.
        /// </summary>
        public Type NotificationType
        {
            get { return notificationType; }
        }

        private IList<ISubscriptionFilter> filters;
        /// <summary>
        /// List of notification message filters. Depending on the notification's content it is sent or not.
        /// Allows for more fine-grained control over notification subscriptions and who gets which notifications.
        /// </summary>
        public IList<ISubscriptionFilter> Filters
        {
            get { return filters; }
        }

        /// <summary>
        /// Indicates whether subscription is local or remote.
        /// </summary>
        public virtual NotificationScope Scope { get { return NotificationScope.Local; } }

        /// <summary>
        /// Send notification using this subscriber type.
        /// </summary>
        /// <param name="notification"></param>
        [SuppressMessage("Microsoft.Design", "CA1030", Justification="there are no events in a WCF contract")]
        public abstract void Send(Notification notification);
    }
}

using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Diagnostics.CodeAnalysis;
using log4net;
using System.Globalization;

namespace MT.Platform.Common
{
    /// <summary>
    /// Notification Broker to broadcast local and/or remote notifications.
    /// </summary>
	[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1724:TypeNamesShouldNotMatchNamespaces")]
	public static class NotificationBroker
    {
		#region [auto] Fields

		/// <summary>
		/// Writes to logging infrastructure.
		/// </summary>
		private static readonly ILog logger = LogManager.GetLogger(typeof(NotificationBroker));

		/// <summary>
		/// An identifier to distinguish instances of NB (client, server), 
		/// so not to sound notifications in circles. To check if process name is sufficient.
		/// </summary>
        private static string id = System.Diagnostics.Process.GetCurrentProcess().ProcessName.Replace(".vshost", "");

        /// <summary>
        /// List of subscriptions
        /// </summary>
        private static Subscriptions subscriptions = new Subscriptions();

		#endregion [auto] Fields

		#region [auto] Properties

        /// <summary>
        /// NotificationBroker identification used as sender. Do not dispatch notification sent yourself.
        /// </summary>
        public static string Id { get { return id; } }

		#endregion [auto] Properties

		#region [auto] Methods

        /// <summary>
        /// Remove and disposes all subscriptions.
        /// </summary>
        public static void Clear()
        {
            // remove all local ones
            subscriptions.Clear();
        }

		/// <summary>
		/// Registers a notification subscription (including type, handlers and filters).
		/// </summary>
		/// <param name="subscription">The subscription (local, web service, db, etc.) to register.</param>
        public static void Register(Subscription subscription)
        {
            lock (subscriptions)
            {
                subscriptions.Add(subscription);
            }
        }

        /// <summary>
        /// Registers a notification subscription (including type, handlers and filters).
        /// </summary>
        /// <param name="type">The type of the message to register for.</param>
        /// <param name="handler">The handler to invoke when a message of the given type arrives.</param>
        /// <param name="filters">The filters that apply for this subscription so not to get certain messages.</param>
        public static void Register(Type type, NotificationHandler handler, IList<ISubscriptionFilter> filters)
        {
            LocalSubscription subscription = new LocalSubscription(type, handler, filters);
			lock (subscriptions)
            {
                subscriptions.Add(subscription);
            }
        }

		/// <summary>
		/// Publishes a notification message.
		/// </summary>
		/// <param name="notification">The notification message.</param>
		/// <param name="scope">The scope of the notification (Local or Remote/Local).</param>
		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes"), SuppressMessage("Microsoft.Design", "CA1030", Justification = "there are no events in a WCF contract")]        
        public static void Send(Notification notification, NotificationScope scope)
        {
            // test inputs
			//Validate.IsNotNull<ArgumentNullException>(notification, "Must pass a valid notification");

			// adds the sender (current named instance of NB) to the notification
			// to find out that a certain notification was sent by yourself!
			if (string.IsNullOrEmpty(notification.Sender))
			{
				// only the initial sender is important. Do not overwrite by intermediary service callback
				// implementations calling this Send() too.
				notification.Sender = Id;
			}

            // fire locally. Get a copied list of subscription references.
			lock (subscriptions)
            {
                IList<Subscription> copiedSubscriptions = subscriptions.Get(notification.GetType());
				foreach (Subscription subscription in copiedSubscriptions)
                {
                    try
                    {
                        // fire when no filter specified
                        if (subscription.Filters == null)
                        {
                            SendAsync(subscription, notification, scope);
                            continue;
                        }
                        if (subscription.Filters.Count == 0)
                        {
                            SendAsync(subscription, notification, scope);
                            continue;
                        }

                        // fire when one of the filters evaluates to true
                        bool filterIncluded = true;
                        bool filterExcluded = false;
                        foreach (ISubscriptionFilter subscriptionFilter in subscription.Filters)
                        {
                            // fire when one (and only when; not and/or logic) filter include evaluates true
                            if (subscriptionFilter.FilterOperation == SubscriptionFilterOperation.Include
                                && !subscriptionFilter.IsMatch(notification))
                            {
                                filterIncluded = false;                     // filter out
                            }
                            // however, does not fire when one (and only when; not and/or logic) filter exclude evaluates true
                            if (subscriptionFilter.FilterOperation == SubscriptionFilterOperation.Exclude
                                && subscriptionFilter.IsMatch(notification))
                            {
                                filterExcluded = true;                      // filter out
                            }

                            if (filterIncluded && !filterExcluded)
                            {
                                SendAsync(subscription, notification, scope);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
						logger.Warn("One of the subscription handler invocations failed.", ex);
                    }
                }
            }
        }

        /// <summary>
        /// Unregisters a notification subscription.
        /// </summary>
		/// <param name="subscription">The subscription to remove (local, web service, db, etc.).</param>
        public static void Unregister(Subscription subscription)
        {
			lock (subscriptions)
            {
                subscriptions.Remove(subscription);
            }
        }

		/// <summary>
		/// Unregisters a notification subscription.
		/// </summary>
		/// <param name="type">The type of the subscription to remove.</param>
		/// <param name="handler">The handler that was registered to receive notifications.</param>
        public static void Unregister(Type type, NotificationHandler handler)
        {
			lock (subscriptions)
            {
                subscriptions.Remove(type, handler);
            }
        }

		/// <summary>
		/// Unregisters the specified type.
		/// </summary>
		/// <param name="type">The type of message to unregister.</param>
        public static void Unregister(Type type)
        {
			lock (subscriptions)
            {
                subscriptions.Remove(type);
            }
        }

		/// <summary>
		/// Asynchronously send notification using a thread pool thread.
		/// </summary>
		/// <param name="subscription">The subscription.</param>
		/// <param name="notification">The notification.</param>
		/// <param name="scope">The scope of the notification.</param>
		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
		private static void SendAsync(Subscription subscription, Notification notification, NotificationScope scope)
        {
            if (subscription.Scope == scope || scope == NotificationScope.Remote)
            {
                // go to another thread to execute delegate!
                ThreadPool.QueueUserWorkItem(
                    delegate(object state)
                    {
                        try
                        {
                            // invoke one separate invocation handler on pool thread to decouple.
                            subscription.Send(notification);
                        }
                        catch (Exception ex)
                        {
							logger.Warn(string.Format(CultureInfo.InvariantCulture, "{0} subscription handler {1} invocations failed.", subscription.Scope, subscription.NotificationType.FullName), ex);
                        }
                    }, null);
            }
        }

		#endregion [auto] Methods
	}
}

using System;
using System.Collections.Generic;
using System.Text;

namespace MT.Platform.Common
{
    /// <summary>
    /// Manages list of notification subscriptions. Can be of local or remote kind.
    /// </summary>
    public sealed class Subscriptions
    {
        /// <summary>
        /// List of subscriptions
        /// </summary>
        private IList<Subscription> subscribers;

        /// <summary>
        /// Default Constructor
        /// </summary>
        public Subscriptions()
        {
            subscribers = new List<Subscription>();
        }

        /// <summary>
        /// Add subscription to list
        /// </summary>
        /// <param name="subscriber"></param>
        public void Add(Subscription subscriber)
        {
            lock (this)
            {
                subscribers.Add(subscriber);
            }
        }

        /// <summary>
        /// Get a list of subscriptions of the specified type. Filters are not taken into account yet.
        /// Also return subscriptions for all type of notifications (type = null).
        /// </summary>
        /// <param name="type">Notification type or "null" type for all messages.</param>
        /// <returns>List of subscriptions</returns>
        public IList<Subscription> Get(Type type)
        {
            IList<Subscription> getSubscribers = new List<Subscription>();
            lock (this)
            {
                foreach (Subscription subscriber in this.subscribers)
                {
                    // send to registered subscriptions
                    // with all types
                    if (type == null 
                        || (subscriber.NotificationType != null && subscriber.NotificationType.Equals(type))
                        || (type != null && subscriber.NotificationType == null))
                    {
                        getSubscribers.Add(subscriber);
                    }
                }
            }
            return getSubscribers;
        }

        /// <summary>
        /// Remove a subscription from the list
        /// </summary>
        /// <param name="subscriber"></param>
        public void Remove(Subscription subscriber)
        {
            Remove(subscriber.NotificationType);
        }

		/// <summary>
		/// Remove a delegate from local subscriptions in the list.
		/// </summary>
		/// <param name="type">The type of the notification.</param>
		/// <param name="handler">The handler for the notification.</param>
        public void Remove(Type type, NotificationHandler handler)
        {
            IList<Subscription> removeList = new List<Subscription>();

            lock (this)
            {
                // first the ones to remove
                foreach (Subscription subscription in subscribers)
                {
                    LocalSubscription localSubscription = subscription as LocalSubscription;
                    if (localSubscription != null)
                    {
                        if (localSubscription.NotificationType != null 
                            && localSubscription.NotificationType.Equals(type) 
                            && localSubscription.Handler.Equals(handler))
                        {
                            removeList.Add(localSubscription);
                        }
                    }
                }
            }

            // then remove separately
            foreach (Subscription subscription in removeList)
            {
                RemoveOne(subscription);
            }
        }

        /// <summary>
        /// Removes and disposes one subscription.
        /// </summary>
        /// <param name="subscription"></param>
        private void RemoveOne(Subscription subscription)
        {
            lock (this)
            {
                // remove from list
                subscribers.Remove(subscription);

                // and dispose
                IDisposable disposableSubscription = subscription as IDisposable;
                if (disposableSubscription != null)
                {
                    disposableSubscription.Dispose();
                }
            }
        }

		/// <summary>
		/// Remove all subscription for the given type from the list
		/// </summary>
		/// <param name="type">The type of the notification to remove.</param>
        public void Remove(Type type)
        {
            IList<Subscription> removeList = new List<Subscription>();

            lock (this)
            {
                // first the ones to remove
                foreach (Subscription subscription in subscribers)
                {
                    if (subscription.NotificationType != null && subscription.NotificationType.Equals(type))
                    {
                        removeList.Add(subscription);
                    }
                }
            }

            // then remove separately
            foreach (Subscription subscription in removeList)
            {
                RemoveOne(subscription);
            }
        }

        /// <summary>
        /// Remove and disposes all subscriptions.
        /// </summary>
        public void Clear()
        {
            lock (this)
            {
                IList<Subscription> removeList = new List<Subscription>();

                // get the ones to remove in separate list. Cannot loop over a list and remove items ...!
                foreach (Subscription subscription in subscribers)
                {
                    removeList.Add(subscription);
                }

                // then remove separately
                foreach (Subscription subscription in removeList)
                {
                    RemoveOne(subscription);
                }
            }
        }
    }
}

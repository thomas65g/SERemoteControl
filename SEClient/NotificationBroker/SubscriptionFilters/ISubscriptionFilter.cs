using System;
using System.Collections.Generic;
using System.Text;

namespace MT.Platform.Common
{
    /// <summary>
    /// NotificationBroker subscription filter interface for custom filter implementations
    /// </summary>
    public interface ISubscriptionFilter
    {
        /// <summary>
        /// Return true when filter evaluates true.
        /// </summary>
        /// <param name="notification"></param>
        /// <returns></returns>
        bool IsMatch(object notification);

        /// <summary>
        /// Indicates, when this filter evaluating to true, that the message is explicitely included or excluded.
        /// </summary>
        SubscriptionFilterOperation FilterOperation { get; set; }
    }
}

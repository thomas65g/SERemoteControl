using System;
using System.Collections.Generic;
using System.Text;

namespace MT.Platform.Common
{
    /// <summary>
    /// Subscription filter operation type
    /// </summary>
    public enum SubscriptionFilterOperation
    {
        /// <summary>
        /// Include in filter when rule matches true
        /// </summary>
        Include,

        /// <summary>
        /// Exclude in filter when rule matches true
        /// </summary>
        Exclude
    }
}

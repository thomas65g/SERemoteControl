using System;
using System.Collections.Generic;
using System.Text;

namespace MT.Platform.Common
{
    /// <summary>
    /// Notifications are sent in-process, cross-process, etc.
    /// </summary>
    public enum NotificationScope
    {
        /// <summary>
        /// Subscribers are in the same process and AppDomain
        /// </summary>
        Local,
        /// <summary>
        /// Subscribers can also be in other processes. Needs remote notification strategy attached.
        /// </summary>
        Remote
    }
}

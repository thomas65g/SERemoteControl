using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MT.Platform.Common;

namespace Services.NotificationBroker.Notifications
{
    /// <summary>
    /// Notification to be used to send written log texts to interested parties.
    /// </summary>
    public class LogWrittenNotification : Notification
    {
        public string Text { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Serialization;

namespace MT.Platform.Common
{
    /// <summary>
    /// Abstract base class for all NotificationBroker messages.
    /// </summary>
    public class Notification
    {
        /// <summary>
        /// Constructor to initialize content values.
        /// </summary>
        public Notification()
        {
            this.Stamp = DateTime.Now;
            this.Id = Guid.NewGuid();
        }

        private Guid id;
        /// <summary>
        /// Notification identier.
        /// </summary>
        public Guid Id
        {
            get { return id; }
            set { id = value; }
        }

        private DateTime stamp;
        /// <summary>
        /// Notification creator's time stamp.
        /// </summary>
        public DateTime Stamp
        {
            get { return stamp; }
            set { stamp = value; }
        }

        private string sender;
        /// <summary>
        /// Sender identification of notification. Used so not to send back to publisher?
        /// </summary>
        public string Sender
        {
            get { return sender; }
            set { sender = value; }
        }
	
    }
}

using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Serialization;
using System.Xml.Serialization;
using System.IO;

namespace MT.Platform.Common
{
    /// <summary>
    /// Supporting functionality around notifications
    /// </summary>
    public static class NotificationHelper
    {
        /// <summary>
        /// Serialize notification object into a string.
        /// </summary>
        /// <param name="notification"></param>
        /// <returns></returns>
        public static string Serialize(Notification notification)
        {
			Validate.IsNotNull<ArgumentNullException>(notification, "must pass a notification");

            XmlSerializer serializer = new XmlSerializer(typeof(Notification));

            using (MemoryStream ms = new MemoryStream())
            {
                serializer.Serialize(ms, notification);

                // convert to string
                return Encoding.UTF8.GetString(ms.ToArray());
            }
        }

        /// <summary>
        /// Deserialize string into a notification object.
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static Notification Deserialize(string data)
        {
			Validate.IsTrue<ArgumentNullException>(!string.IsNullOrEmpty(data), "must pass a notification data");
			
			// use .NET 3.0 (WCF) serializer
            XmlSerializer serializer = new XmlSerializer(typeof(Notification));
            using (MemoryStream ms = new MemoryStream(Encoding.UTF8.GetBytes(data)))
            {
                Notification notification = (Notification)serializer.Deserialize(ms);
                return notification;
            }
        }
    }
}

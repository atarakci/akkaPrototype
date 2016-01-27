using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Notifications.Common.Message
{   
    public class NotificationMessage
    {
        public string UserId { get; private set; }
        public string Notification { get; private set; }

        public NotificationMessage(string userId, string notification)
        {
            UserId = userId;
            Notification = notification;
        }
    }
}

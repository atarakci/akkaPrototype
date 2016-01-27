using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Notifications.Common.Common
{
    public class NotificationAnalyzer : INotificationAnalyzer
    {
        public static int counter;

        public void CountNotifications()
        {
            counter++;
        }
    }
}

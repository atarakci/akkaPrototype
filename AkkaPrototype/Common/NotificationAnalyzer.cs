using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AkkaPrototype.Common
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

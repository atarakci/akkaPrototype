using Akka.Actor;
using Akka.Event;
using AkkaPrototype.Common;
using AkkaPrototype.Message;
using System;
using System.IO;

namespace AkkaPrototype.Actor
{
    public class PushNotificationActor : ReceiveActor
    {
        private readonly ILoggingAdapter _logger = Context.GetLogger();
        private readonly INotificationAnalyzer _analyzer;

        public PushNotificationActor(INotificationAnalyzer analyzer)
        {
            Console.WriteLine("Pushnotification Actor Created");

            _analyzer = analyzer;

            Receive<NotificationMessage>(message => WriteToFile(message));
        }

        public void WriteToFile(NotificationMessage message)
        {
            StreamWriter file = new StreamWriter(@"C:\Users\tarakci\Downloads\AWS\notifications.txt", true);
            file.WriteLine($"User ID: {message.UserId} - Message: {message.Notification}");
            file.Close();

            _logger.Info($"Notification has sent to -> {message.UserId}");

            _analyzer.CountNotifications();
        }

        protected override void PreStart()
        {
            _logger.Info("Pushnotification Actor pre start called");
            Console.WriteLine("Pushnotification Actor PreStart");
        }

        protected override void PostStop()
        {
            Console.WriteLine("Pushnotification Actor PostStop");
        }

        protected override void PreRestart(Exception reason, object message)
        {
            Console.WriteLine($"Pushnotification Actor PreRestart because: {reason}");

            base.PreRestart(reason, message);
        }

        protected override void PostRestart(Exception reason)
        {
            Console.WriteLine($"Pushnotification Actor PostRestart because: {reason}");

            base.PostRestart(reason);
        }
    }
}
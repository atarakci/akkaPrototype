using Akka.Actor;
using Akka.Event;
using Notifications.Common.Common;
using Notifications.Common.Message;
using System;
using System.IO;
using System.Threading.Tasks;

namespace Notifications.Common.Actor
{
    public class PushNotificationActor : ReceiveActor
    {
        private readonly ILoggingAdapter _logger = Context.GetLogger();
        private readonly INotificationAnalyzer _analyzer;

        public PushNotificationActor(INotificationAnalyzer analyzer)
        {
            Console.WriteLine("Pushnotification Actor Created");

            _analyzer = analyzer;

            Receive<NotificationMessage>(message => HandleMessage(message));
            Receive<ResultMessage>(message => HandleResultMessage(message));
        }

        public void HandleMessage(NotificationMessage message)
        {
            WriteToFile(message).PipeTo(Self, Sender);
        }

        public void HandleResultMessage(ResultMessage message)
        {
            Sender.Tell(new ResultMessage()
            {
                Result = message.Result,
                CreateDate = message.CreateDate
            });
        }

        public async Task<ResultMessage> WriteToFile(NotificationMessage message)
        {
            StreamWriter file = new StreamWriter(@"C:\Users\tarakci\Downloads\AWS\notifications.txt", true);
            file.WriteLine($"User ID: {message.UserId} - Message: {message.Notification}");
            file.Close();

            _logger.Info($"Notification has sent to -> {message.UserId}");

            _analyzer.CountNotifications();

            return new ResultMessage()
            {
                Result = "done",
                CreateDate = DateTime.Now
            };
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
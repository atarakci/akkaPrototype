using Akka.Actor;
using Akka.Event;
using Akka.DI.Core;
using System;
using Akka.Routing;
using Notifications.Common.Message;
using System.Threading.Tasks;

namespace Notifications.Common.Actor
{
    public class NotificationActor : ReceiveActor
    {
        private readonly ILoggingAdapter _logger = Context.GetLogger();
        private readonly IActorRef _pushNotificationActor;

        public NotificationActor()
        {
            //Context.ActorOf(Context.DI().Props<PushNotificationActor>(), "PushNotificationActor");
            _pushNotificationActor = Context.ActorOf(Context.DI().Props<PushNotificationActor>().WithRouter(FromConfig.Instance), "PushNotificationActor");

            Receive<NotificationMessage>(message => HandleNotification(message));
        }

        public void HandleNotification(NotificationMessage message)
        {
            _pushNotificationActor.Tell(message);
        }

        protected override SupervisorStrategy SupervisorStrategy()
        {
            return new OneForOneStrategy
            (
                5, // maximum number of retries
                TimeSpan.FromSeconds(30), // within time range
                exception =>
                {
                    if (exception is ArithmeticException)
                    {
                        return Directive.Restart;
                    }
                    if (exception is DuplicateWaitObjectException)
                    {
                        return Directive.Resume;
                    }
                    if (exception is NotSupportedException)
                    {
                        return Directive.Stop;
                    }

                    return Directive.Escalate;
                }
            );
        }
    }
}

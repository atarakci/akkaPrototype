using Akka.Actor;
using Akka.Event;
using Akka.DI.Core;
using System;

namespace Notifications.Common.Actor
{
    public class NotificationActor : ReceiveActor
    {
        private readonly ILoggingAdapter _logger = Context.GetLogger();

        public NotificationActor()
        {
            Context.ActorOf(Context.DI().Props<PushNotificationActor>(), "PushNotificationActor");
        }

        protected override SupervisorStrategy SupervisorStrategy()
        {
            return new OneForOneStrategy
            (
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

                    return Directive.Escalate;
                }
            );
        }
    }
}

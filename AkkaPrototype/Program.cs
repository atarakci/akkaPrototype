using Akka.Actor;
using Akka.DI.AutoFac;
using Akka.DI.Core;
using Autofac;
using System;
using Notifications.Common.Actor;
using Notifications.Common.Message;
using Notifications.Common.Common;

namespace AkkaPrototype
{
    class Program
    {
        private static ActorSystem NotificationsActorSystem;

        static void Main(string[] args)
        {
            #region DI things
            var builder = new ContainerBuilder();
            builder.RegisterType<NotificationAnalyzer>().As<INotificationAnalyzer>();
            builder.RegisterType<PushNotificationActor>();
            var container = builder.Build();
            #endregion

            NotificationsActorSystem = ActorSystem.Create("NotificationsActorSystem");
            IDependencyResolver resolver = new AutoFacDependencyResolver(container, NotificationsActorSystem);

            Props notificationActor = Props.Create<NotificationActor>();
            IActorRef pushNotificationActorRef = NotificationsActorSystem.ActorOf(notificationActor, "NotificationActor");

            Console.ReadKey();
            Console.WriteLine("Sending notification");

            pushNotificationActorRef.Tell(new NotificationMessage("akin@armut.com", "New Job"));

            //This is for reaching from lower level actor to high level actor

            //NotificationsActorSystem.ActorSelection("user/NotificationActor/PushNotificationActor")
            //    .Tell(new NotificationMessage("akin@armut.com", "New Job"));

            Console.ReadKey();

            NotificationsActorSystem.Terminate();
        }
    }
}

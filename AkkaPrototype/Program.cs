using Akka.Actor;
using AkkaPrototype.Actor;
using AkkaPrototype.Message;
using Akka.DI.AutoFac;
using Akka.DI.Core;
using Autofac;
using AkkaPrototype.Common;
using System;

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
            //pushNotificationActorRef.Tell(new NotificationMessage("akin@armut.com", "New Job"));
            NotificationsActorSystem.ActorSelection("/user/NotificationActor/PushNotificationActor")
                .Tell(new NotificationMessage("akin@armut.com", "New Job"));
            Console.ReadKey();

            NotificationsActorSystem.Terminate();
        }
    }
}

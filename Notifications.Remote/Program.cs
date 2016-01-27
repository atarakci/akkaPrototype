using Akka.Actor;
using System;

namespace Notifications.Remote
{
    class Program
    {
        private static ActorSystem NotificationsActorSystem;

        static void Main(string[] args)
        {
            Console.WriteLine("Remote actor system created");

            NotificationsActorSystem = ActorSystem.Create("NotificationsActorSystem");

            Console.ReadKey();

            NotificationsActorSystem.Terminate();
        }
    }
}
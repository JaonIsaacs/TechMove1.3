using TechMove1._3.Domain.Interfaces;

namespace TechMove1._3.Application.Observers
{
    public class NotificationObserver : IObserver
    {
        public void Update(string message)
        {
            Console.WriteLine($"Notification: {message}");
        }
    }
}

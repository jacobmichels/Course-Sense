using course_sense_dotnet.Models;

namespace course_sense_dotnet.NotificationManager
{
    public interface INotificationManager
    {
        bool SendNotification(NotificationRequest notificationRequest);
    }
}
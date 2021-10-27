using course_sense_dotnet.Models;

namespace course_sense_dotnet.AlertManager
{
    public interface IAlertManager
    {
        bool SendNotification(NotificationRequest notificationRequest);
    }
}
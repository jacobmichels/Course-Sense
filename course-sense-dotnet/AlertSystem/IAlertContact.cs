using course_sense_dotnet.Utility;

namespace course_sense_dotnet.AlertSystem
{
    public interface IAlertContact
    {
        bool SendNotification(NotificationRequest notificationRequest);
    }
}
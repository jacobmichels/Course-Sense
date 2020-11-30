using course_sense_dotnet.Utility;

namespace course_sense_dotnet.AlertSystem
{
    public interface IEmailClient
    {
        bool SendEmail(NotificationRequest requestData);
    }
}
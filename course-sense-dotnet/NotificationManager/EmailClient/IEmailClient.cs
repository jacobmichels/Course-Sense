using course_sense_dotnet.Models;

namespace course_sense_dotnet.NotificationManager.EmailClient
{
    public interface IEmailClient
    {
        bool SendEmail(NotificationRequest requestData);
    }
}
using course_sense_dotnet.Models;

namespace course_sense_dotnet.Application.NotificationManager.EmailClient
{
    public interface IEmailClient
    {
        bool SendEmail(NotificationRequest requestData);
    }
}
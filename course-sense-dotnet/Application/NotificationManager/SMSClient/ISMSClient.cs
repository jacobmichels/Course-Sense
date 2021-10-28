using course_sense_dotnet.Models;

namespace course_sense_dotnet.Application.NotificationManager.SMSClient
{
    public interface ISMSClient
    {
        bool LookupPhone(string phone);
        bool SendSMS(string phone, CourseInfo course);
    }
}
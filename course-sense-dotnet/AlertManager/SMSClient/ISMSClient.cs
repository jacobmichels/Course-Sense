using course_sense_dotnet.Models;

namespace course_sense_dotnet.AlertManager.SMSClient
{
    public interface ISMSClient
    {
        bool LookupPhone(string phone);
        void SendSMS(string phone, CourseInfo course);
    }
}
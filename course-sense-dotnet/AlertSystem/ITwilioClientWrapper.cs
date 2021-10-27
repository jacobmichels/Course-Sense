using course_sense_dotnet.Models.WebAdvisor;

namespace course_sense_dotnet.AlertSystem
{
    public interface ITwilioClientWrapper
    {
        bool LookupPhone(string phone);
        void SendSMS(string phone, CourseInfo course);
    }
}
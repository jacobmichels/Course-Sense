using course_sense_dotnet.WebAdvisor;

namespace course_sense_dotnet.AlertSystem
{
    public interface ITwilioClientWrapper
    {
        bool LookupPhone(string phone);
        void SendSMS(string phone, Course course);
    }
}
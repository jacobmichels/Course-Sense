namespace course_sense_dotnet.Utility
{
    public interface IContactValidation
    {
        bool ValidateContactInfo(string phone, string email);
        bool ValidateEmail(string email);
        bool ValidatePhone(string phone);
    }
}
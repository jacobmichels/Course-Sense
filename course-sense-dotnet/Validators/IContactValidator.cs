namespace course_sense_dotnet.Validators
{
    public interface IContactValidator
    {
        bool ValidateContactInfo(string phone, string email);
        bool ValidateEmail(string email);
        bool ValidatePhone(string phone);
    }
}
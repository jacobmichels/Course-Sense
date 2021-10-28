using course_sense_dotnet.NotificationManager.SMSClient;
using Microsoft.Extensions.Logging;
using System;
using System.Text.RegularExpressions;

namespace course_sense_dotnet.Validators
{
    public class ContactValidator : IContactValidator
    {
        private readonly ILogger logger;
        private readonly ISMSClient twilioClient;
        public ContactValidator(ILogger<ContactValidator> logger, ISMSClient twilioClient)
        {
            this.logger = logger;
            this.twilioClient = twilioClient;

        }
        public bool ValidateContactInfo(string phone, string email)
        {
            if (string.IsNullOrEmpty(phone) && string.IsNullOrEmpty(email))
            {
                return false;
            }
            return ValidateEmail(email) && ValidatePhone(phone);
        }
        public bool ValidateEmail(string email)
        {
            if (string.IsNullOrEmpty(email)) return true;

            //taken from https://www.rhyous.com/2010/06/15/csharp-email-regular-expression/
            const string emailRegex = @"^[\w!#$%&'*+\-/=?\^_`{|}~]+(\.[\w!#$%&'*+\-/=?\^_`{|}~]+)*"
                                   + "@"
                                   + @"((([\-\w]+\.)+[a-zA-Z]{2,4})|(([0-9]{1,3}\.){3}[0-9]{1,3}))\z";
            bool match = false;
            try
            {
                match = Regex.IsMatch(email, emailRegex, RegexOptions.IgnoreCase, TimeSpan.FromMilliseconds(250));
            }
            catch (TimeoutException)
            {
                logger.LogWarning($"Email has failed validation (timeout): {email}");
            }
            if (!match)
            {
                logger.LogWarning($"Email has failed validation (bad RE match): {email}");
            }
            return match;
        }
        public bool ValidatePhone(string phone)
        {
            if (string.IsNullOrEmpty(phone)) return true;
            return twilioClient.LookupPhone(phone);
        }
    }
}

using course_sense_dotnet.Application.NotificationManager.SMSClient;
using Microsoft.Extensions.Logging;
using System;
using System.Text.RegularExpressions;

namespace course_sense_dotnet.Validators
{
    // This class handles the validation of a user's contact information.
    public class ContactValidator : IContactValidator
    {
        private readonly ILogger logger;
        private readonly ISMSClient twilioClient;
        public ContactValidator(ILogger<ContactValidator> logger, ISMSClient twilioClient)
        {
            this.logger = logger;
            this.twilioClient = twilioClient;

        }

        // This methold takes in a phone and email, and returns true if they pass validation.
        public bool ValidateContactInfo(string phone, string email)
        {
            // Ensure that the user suppled at least one contact method.
            if (string.IsNullOrEmpty(phone) && string.IsNullOrEmpty(email))
            {
                return false;
            }
            return ValidateEmail(email) && ValidatePhone(phone);
        }

        // This method validates an email address. It uses a regular expression.
        public bool ValidateEmail(string email)
        {
            if (string.IsNullOrEmpty(email)) return true;

            // Taken from https://www.rhyous.com/2010/06/15/csharp-email-regular-expression/
            const string emailRegex = @"^[\w!#$%&'*+\-/=?\^_`{|}~]+(\.[\w!#$%&'*+\-/=?\^_`{|}~]+)*"
                                   + "@"
                                   + @"((([\-\w]+\.)+[a-zA-Z]{2,4})|(([0-9]{1,3}\.){3}[0-9]{1,3}))\z";
            
            bool match = false;
            try
            {
                // Specify a timeout for the match to prevent the regex parser infinite looping.
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

        // This method validates a phone number.
        // The twilio client class handles the validation.
        public bool ValidatePhone(string phone)
        {
            if (string.IsNullOrEmpty(phone)) return true;
            return twilioClient.LookupPhone(phone);
        }
    }
}

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Twilio;
using Twilio.Rest.Lookups.V1;
using Twilio.Rest.Api.V2010.Account;
using Twilio.Exceptions;
using course_sense_dotnet.Models;

namespace course_sense_dotnet.NotificationManager.SMSClient
{
    public class TwilioSMSClient : ISMSClient
    {
        private readonly ILogger<ISMSClient> logger;
        private readonly IConfiguration configuration;
        public TwilioSMSClient(ILogger<ISMSClient> logger,
            IConfiguration configuration)
        {
            this.logger = logger;
            this.configuration = configuration;
            TwilioClient.Init(configuration["Twilio:AccountSID"], configuration["Twilio:AuthToken"]);
        }
        public void SendSMS(string phone, CourseInfo course)
        {
            MessageResource message = MessageResource.Create(
                body: $"This is course-sense.ca. {course.Subject} {course.Code} ({course.Section}) just had a space open up!",
                from: configuration["Twilio:FromNumber"],
                to: phone
            );
            //No easy way to check if the message was sent properly, here I just hope the message is sent fine
        }
        public bool LookupPhone(string phone)
        {
            PhoneNumberResource lookupResult;
            try
            {
                lookupResult = PhoneNumberResource.Fetch(
                    pathPhoneNumber: new Twilio.Types.PhoneNumber(phone)
                );
            }
            catch (ApiException)
            {
                return false;
            }

            if (string.IsNullOrEmpty(lookupResult.PhoneNumber.ToString()))
            {
                return false;
            }
            return true;
        }
    }
}

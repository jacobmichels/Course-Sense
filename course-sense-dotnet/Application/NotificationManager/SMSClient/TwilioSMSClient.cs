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

namespace course_sense_dotnet.Application.NotificationManager.SMSClient
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
        public bool SendSMS(string phone, CourseInfo course)
        {
            MessageResource message = MessageResource.Create(
                body: $"This is course-sense.ca. {course.Subject} {course.Code} ({course.Section}) just had a space open up!",
                from: configuration["Twilio:FromNumber"],
                to: phone
            );
            logger.LogInformation($"Message status for #{phone}: {message.Status}");
            if (message.Status == MessageResource.StatusEnum.Failed)
            {
                logger.LogError($"Failed to send SMS to user with #{phone}");
                return false;
            }
            logger.LogInformation($"Successfully sent SMS to user with #{phone}");
            return true;
        }
        public bool LookupPhone(string phone)
        {
            logger.LogInformation($"Validating #{phone} with API lookup.");
            PhoneNumberResource lookupResult;
            try
            {
                lookupResult = PhoneNumberResource.Fetch(
                    pathPhoneNumber: new Twilio.Types.PhoneNumber(phone)
                );
            }
            catch (ApiException)
            {
                logger.LogError("ApiException caught while attempting to lookup phone number.");
                return false;
            }

            if (string.IsNullOrEmpty(lookupResult.PhoneNumber.ToString()))
            {
                logger.LogWarning($"Phone number {phone} failed lookup validation.");
                return false;
            }
            logger.LogInformation($"Phone number {phone} passed lookup validation.");
            return true;
        }
    }
}

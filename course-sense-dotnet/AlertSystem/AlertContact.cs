using course_sense_dotnet.DataAccessLayer;
using course_sense_dotnet.Utility;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace course_sense_dotnet.AlertSystem
{
    public class AlertContact : IAlertContact
    {
        private readonly ILogger logger;
        private readonly ITwilioClientWrapper twilioClientWrapper;
        private readonly IEmailClient emailClient;
        public AlertContact(ILogger<AlertContact> logger,
            ITwilioClientWrapper twilioClientWrapper,
            IEmailClient emailClient)
        {
            this.logger = logger;
            this.twilioClientWrapper = twilioClientWrapper;
            this.emailClient = emailClient;
        }
        public bool SendNotification(NotificationRequest notificationRequest)
        {
            bool notificationSent = false;
            if (!string.IsNullOrEmpty(notificationRequest.Phone))
            {
                twilioClientWrapper.SendSMS(notificationRequest.Phone, notificationRequest.RequestedCourse);
                notificationSent = true;
            }
            if (!string.IsNullOrEmpty(notificationRequest.Email))
            {
                emailClient.SendEmail(notificationRequest);
                notificationSent = true;
            }
            return notificationSent;
        }

    }
}

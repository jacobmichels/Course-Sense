using course_sense_dotnet.AlertManager.EmailClient;
using course_sense_dotnet.AlertManager.SMSClient;
using course_sense_dotnet.Models;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace course_sense_dotnet.AlertManager
{
    public class AlertManager : IAlertManager
    {
        private readonly ILogger logger;
        private readonly ISMSClient twilioClientWrapper;
        private readonly IEmailClient emailClient;
        public AlertManager(ILogger<AlertManager> logger,
            ISMSClient twilioClientWrapper,
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

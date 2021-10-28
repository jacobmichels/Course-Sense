using course_sense_dotnet.Application.NotificationManager.EmailClient;
using course_sense_dotnet.Application.NotificationManager.SMSClient;
using course_sense_dotnet.Models;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace course_sense_dotnet.Application.NotificationManager
{
    // This class determines whether to send an email, an SMS, or both to notify a user of space in a course.
    public class NotificationManager : INotificationManager
    {
        private readonly ILogger logger;
        private readonly ISMSClient twilioClientWrapper;
        private readonly IEmailClient emailClient;
        public NotificationManager(ILogger<NotificationManager> logger,
            ISMSClient twilioClientWrapper,
            IEmailClient emailClient)
        {
            this.logger = logger;
            this.twilioClientWrapper = twilioClientWrapper;
            this.emailClient = emailClient;
        }

        // This method will call into the SMS client and/or the email client to send a message to the user, alerting them of capacity.
        // Returns true if at least one method was used to successfully notify the user.
        public bool SendNotification(NotificationRequest notificationRequest)
        {
            bool notificationSent = false;
            if (!string.IsNullOrEmpty(notificationRequest.Phone))
            {
                bool sent = twilioClientWrapper.SendSMS(notificationRequest.Phone, notificationRequest.RequestedCourse);
                if (sent)
                {
                    notificationSent = true;
                }
            }
            if (!string.IsNullOrEmpty(notificationRequest.Email))
            {
                bool sent = emailClient.SendEmail(notificationRequest);
                if (sent)
                {
                    notificationSent = true;
                }
            }
            return notificationSent;
        }

    }
}

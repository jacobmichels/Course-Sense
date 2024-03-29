﻿using course_sense_dotnet.Models;
using MailKit.Net.Smtp;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using MimeKit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace course_sense_dotnet.Application.NotificationManager.EmailClient
{
    // This class is responsible for sending an email notification to the user
    public class EmailClient : IEmailClient
    {
        private readonly ILogger logger;
        private readonly IConfiguration configuration;
        public EmailClient(ILogger<EmailClient> logger, IConfiguration configuration)
        {
            this.logger = logger;
            this.configuration = configuration;
        }

        // This method will send an email to the user associated with this NotificationRequest
        public bool SendEmail(NotificationRequest requestData)
        {
            logger.LogInformation($"Attempting to email user: {requestData.Email}");

            // Construct the email
            MimeMessage message = new MimeMessage();
            message.From.Add(new MailboxAddress("Course sense", configuration["EmailConfig:NotificationSenderEmail"]));
            message.To.Add(MailboxAddress.Parse(requestData.Email));
            message.Subject = "Space found - course-sense.ca";
            message.Body = new TextPart("plain")
            {
                Text = $"Hello {requestData.Email}!\n" +
                $"\n" +
                $"You're receiving this email to let you know course-sense.ca found a space in the course you requested below.\n" +
                $"\n" +
                $"Term: {requestData.RequestedCourse.Term} Course: {requestData.RequestedCourse.Subject} {requestData.RequestedCourse.Code} Section: {requestData.RequestedCourse.Section}\n" +
                $"\n" +
                $"Get on WebAdvisor and grab this spot!\n" +
                $"\n" +
                $"Have a nice day,\n" +
                $"course-sense.ca"
            };

            // Send the email
            try
            {
                using (SmtpClient smtpClient = new SmtpClient())
                {
                    smtpClient.Connect(configuration["EmailConfig:SmtpServerAddress"], 465, true);
                    smtpClient.Authenticate(configuration["EmailConfig:NotificationSenderEmail"], configuration["EmailConfig:Password"]);
                    smtpClient.Send(message);
                    smtpClient.Disconnect(true);
                }
            }
            catch (Exception e)
            {
                logger.LogError($"Error in Smtp client: {e.Message}. Email not sent to user.");
                return false;
            }
            logger.LogInformation($"Email sent to {requestData.Email}.");
            return true;

        }
    }
}

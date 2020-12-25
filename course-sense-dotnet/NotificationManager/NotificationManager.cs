using course_sense_dotnet.AlertSystem;
using course_sense_dotnet.DataAccessLayer;
using course_sense_dotnet.Models;
using course_sense_dotnet.Utility;
using course_sense_dotnet.WebAdvisor;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace course_sense_dotnet.NotificationManager
{
    public class NotificationManager : INotificationManager
    {
        private readonly ILogger logger;
        private readonly IRequests requests;
        private readonly IAlertContact alertContact;
        private readonly IDataAccess dataAccess;
        public NotificationManager(ILogger<NotificationManager> logger,
            IRequests requests,
            IAlertContact alertContact,
            IDataAccess dataAccess)
        {
            this.logger = logger;
            this.requests = requests;
            this.alertContact = alertContact;
            this.dataAccess = dataAccess;
        }
        public async Task CheckCapacityAndAlert(NotificationRequest notificationRequest,
            SynchronizedCollection<NotificationRequest> notificationRequests)
        {
            CourseCapacity capacity = await requests.GetCapacity(notificationRequest.RequestedCourse);
            logger.LogInformation($"Capacity for {notificationRequest.RequestedCourse.Term}|{notificationRequest.RequestedCourse.Subject}|{notificationRequest.RequestedCourse.Code}|{notificationRequest.RequestedCourse.Section} is {capacity.CurrentCapacity}");
            if (capacity.CurrentCapacity > 0)
            {
                //alert the user and remove from in-memory collection and db
                if (!alertContact.SendNotification(notificationRequest))
                {
                    logger.LogError($"Failed to send notification to user: {notificationRequest.Email}|{notificationRequest.Phone}|{notificationRequest.RequestedCourse.Term}|{notificationRequest.RequestedCourse.Subject}|{notificationRequest.RequestedCourse.Code}|{notificationRequest.RequestedCourse.Section}");
                }
                else
                {
                    logger.LogInformation($"Space found in course {notificationRequest.RequestedCourse.ToString()}"
                    + $" for user: {notificationRequest.Email} | {notificationRequest.Phone}");
                    notificationRequests.Remove(notificationRequest);
                    if (!dataAccess.RemoveRequestFromDB(notificationRequest))
                    {
                        logger.LogError($"Did not remove request from db: {notificationRequest.Email}|{notificationRequest.Phone}|{notificationRequest.RequestedCourse.Term}|{notificationRequest.RequestedCourse.Subject}|{notificationRequest.RequestedCourse.Code}|{notificationRequest.RequestedCourse.Section}");
                    }
                }
                
            }
        }
    }
}

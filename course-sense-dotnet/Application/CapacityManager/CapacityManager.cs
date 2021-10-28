using course_sense_dotnet.Application.NotificationManager;
using course_sense_dotnet.Application.WebAdvisor.RequestManager;
using course_sense_dotnet.Models;
using course_sense_dotnet.Repository;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace course_sense_dotnet.Application.CapacityManager
{
    // This class is reponsible for checking course capacity and notifying users if there is space.
    public class CapacityManager : ICapacityManager
    {
        private readonly ILogger logger;
        private readonly IRequestManager requests;
        private readonly INotificationManager alertContact;
        private readonly IDBRepository repository;
        public CapacityManager(ILogger<CapacityManager> logger,
            IRequestManager requests,
            INotificationManager alertContact,
            IDBRepository repository)
        {
            this.logger = logger;
            this.requests = requests;
            this.alertContact = alertContact;
            this.repository = repository;
        }

        // This method is called for each NotificationRequests on each iteration of the polling loop.
        // It checks the capacity of the course in the request, and notifies the user if space is found.
        public async Task NotifyIfSpaceFound(NotificationRequest notificationRequest,
            SynchronizedCollection<NotificationRequest> notificationRequests)
        {
            // Fetch and log the capacity for the course.
            CourseCapacity capacity = await requests.GetCapacity(notificationRequest.RequestedCourse);
            logger.LogInformation($"Capacity for {notificationRequest.RequestedCourse.Term}|{notificationRequest.RequestedCourse.Subject}|{notificationRequest.RequestedCourse.Code}|{notificationRequest.RequestedCourse.Section} is {capacity.CurrentCapacity}");

            // If we find capacity in the course, notify the user using their supplied contact method(s), logging any errors.
            if (capacity.CurrentCapacity > 0)
            {
                if (alertContact.SendNotification(notificationRequest))
                {
                    logger.LogInformation($"Space found in course {notificationRequest.RequestedCourse.Subject} {notificationRequest.RequestedCourse.Code} for user: {notificationRequest.Email} | {notificationRequest.Phone}");

                    // Remove the NotificationRequest from the in-memory collection, and repository.
                    notificationRequests.Remove(notificationRequest);
                    if (!repository.RemoveRequest(notificationRequest))
                    {
                        logger.LogError($"Did not remove request from db: {notificationRequest.Email}|{notificationRequest.Phone}|{notificationRequest.RequestedCourse.Term}|{notificationRequest.RequestedCourse.Subject}|{notificationRequest.RequestedCourse.Code}|{notificationRequest.RequestedCourse.Section}");
                    }
                }
                else
                {
                    logger.LogError($"Failed to send notification to user: {notificationRequest.Email}|{notificationRequest.Phone}|{notificationRequest.RequestedCourse.Term}|{notificationRequest.RequestedCourse.Subject}|{notificationRequest.RequestedCourse.Code}|{notificationRequest.RequestedCourse.Section}");
                }
            }
        }
    }
}

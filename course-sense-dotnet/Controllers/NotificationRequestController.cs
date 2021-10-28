using course_sense_dotnet.Application.WebAdvisor.RequestManager;
using course_sense_dotnet.Models;
using course_sense_dotnet.Repository;
using course_sense_dotnet.Validators;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace course_sense_dotnet.Controllers
{
    // This class is a Controller for the route /NotificationRequest.
    [Route("[controller]")]
    [ApiController]
    public class NotificationRequestController : ControllerBase
    {
        private readonly ILogger logger;
        private readonly IRequestManager requests;
        private readonly IContactValidator contactValidation;
        private readonly SynchronizedCollection<NotificationRequest> notificationRequests;
        private readonly IDBRepository repository;

        public NotificationRequestController(ILogger<NotificationRequestController> logger,
            IRequestManager requests,
            SynchronizedCollection<NotificationRequest> notificationRequests,
            IContactValidator contactValidation,
            IDBRepository repository)
        {
            this.logger = logger;
            this.requests = requests;
            this.notificationRequests = notificationRequests;
            this.contactValidation = contactValidation;
            this.repository = repository;
        }

        // This method handles HTTP POST requests to the route /NotificationRequest.
        [HttpPost]
        public async Task<IActionResult> RequestCourseNotification(NotificationRequest requestData)
        {
            // Perform validation on the course and supplied contact info, logging any errors.
            try
            {
                if (!await requests.CheckCourseExists(requestData.RequestedCourse))
                {
                    return BadRequest("Bad course");
                }
                if (!contactValidation.ValidateContactInfo(requestData.Phone, requestData.Email))
                {
                    return BadRequest("Bad contact");
                }
                
            }
            catch(Exception e)
            {
                logger.LogError($"Error in {nameof(NotificationRequestController)}: {e.Message} | Status 500 returned");
                return StatusCode(500,"Server error during validation");
            }

            // If the request passes validation, add it to the repository and to the in-memory collection.
            if (!repository.AddRequest(requestData))
            {
                logger.LogError($"Error in {nameof(NotificationRequestController)} DB Insert failed, Status 500 returned");
                return StatusCode(500,"Server error: failed DB insert");
            }
            notificationRequests.Add(requestData);
            logger.LogInformation($"New request added: {requestData.Email}|{requestData.Phone}|{requestData.RequestedCourse.Term}|{requestData.RequestedCourse.Subject}|{requestData.RequestedCourse.Code}|{requestData.RequestedCourse.Section}");
            
            return Ok();
        }
    }
}

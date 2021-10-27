using course_sense_dotnet.DataAccessLayer;
using course_sense_dotnet.Models.WebAdvisor;
using course_sense_dotnet.Utility;
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
    [Route("[controller]")]
    [ApiController]
    public class NotificationRequestController : ControllerBase
    {
        private readonly ILogger logger;
        private readonly IRequests requests;
        private readonly IContactValidation contactValidation;
        private readonly SynchronizedCollection<NotificationRequest> notificationRequests;
        private readonly IDataAccess dataAccess;
        public NotificationRequestController(ILogger<NotificationRequestController> logger,
            IRequests requests,
            SynchronizedCollection<NotificationRequest> notificationRequests,
            IContactValidation contactValidation,
            IDataAccess dataAccess)
        {
            this.logger = logger;
            this.requests = requests;
            this.notificationRequests = notificationRequests;
            this.contactValidation = contactValidation;
            this.dataAccess = dataAccess;
        }
        [HttpPost]
        public async Task<IActionResult> RequestCourseNotification(NotificationRequest requestData)
        {
            try
            {
                if (!await requests.CheckCourseExists(requestData.RequestedCourse))
                {
                    return UnprocessableEntity("Bad course");
                }
                if (!contactValidation.ValidateContactInfo(requestData.Phone, requestData.Email))
                {
                    return UnprocessableEntity("Bad contact");
                }
                
            }
            catch(Exception e)
            {
                logger.LogError($"Error in {nameof(NotificationRequestController)}: {e.Message} | Status 500 returned");
                return StatusCode(500,"Error during validation");
            }
            if (!dataAccess.InsertRequestIntoDB(requestData))
            {
                logger.LogError($"Error in {nameof(NotificationRequestController)} DB Insert failed, Status 500 returned");
                return StatusCode(500,"Failed DB insert");
            }
            logger.LogInformation($"New request added: {requestData.Email}|{requestData.Phone}|{requestData.RequestedCourse.Term}|{requestData.RequestedCourse.Subject}|{requestData.RequestedCourse.Code}|{requestData.RequestedCourse.Section}");
            notificationRequests.Add(requestData);
            return Ok();
        }
    }
}

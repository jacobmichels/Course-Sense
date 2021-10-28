using course_sense_dotnet.Application.WebAdvisor.RequestManager;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace course_sense_dotnet.Controllers
{
    // This class is a Controller for the route /Subjects
    [Route("[controller]")]
    [ApiController]
    public class SubjectsController : ControllerBase
    {
        private readonly ILogger logger;
        private readonly IRequestManager requests;
        public SubjectsController(ILogger<SubjectsController> logger, IRequestManager requests)
        {
            this.logger = logger;
            this.requests = requests;
        }
        
        // This method corresponds to the route GET /Subjects and returns a list of subjects offered on WebAdvisor.
        [HttpGet]
        public async Task<IActionResult> GetSubjectsList()
        {
            logger.LogDebug("GetSubjectsList called.");
            List<string> subjects = await requests.GetSubjects();
            return Ok(subjects);
        }
    }
}

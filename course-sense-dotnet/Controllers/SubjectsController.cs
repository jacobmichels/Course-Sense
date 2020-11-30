using course_sense_dotnet.WebAdvisor;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace course_sense_dotnet.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class SubjectsController : ControllerBase
    {
        private readonly ILogger logger;
        private readonly IRequests requests;
        public SubjectsController(ILogger<SubjectsController> logger, IRequests requests)
        {
            this.logger = logger;
            this.requests = requests;
        }
        [HttpGet]
        public async Task<IActionResult> GetSubjectsList()
        {
            logger.LogDebug("GetSubjectsList called.");
            List<string> subjects = await requests.GetSubjects();
            return Ok(subjects);
        }
    }
}

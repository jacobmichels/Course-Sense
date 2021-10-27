using course_sense_dotnet.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace course_sense_dotnet.Models.WebAdvisor
{
    public interface IRequests
    {
        Task<bool> CheckCourseExists(CourseInfo course);
        Task<CourseCapacity> GetCapacity(CourseInfo course);
        Task<List<string>> GetSubjects();
    }
}
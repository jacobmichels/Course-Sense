using course_sense_dotnet.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace course_sense_dotnet.Application.WebAdvisor.RequestManager
{
    public interface IRequestManager
    {
        Task<bool> CheckCourseExists(CourseInfo course);
        Task<CourseCapacity> GetCapacity(CourseInfo course);
        Task<List<string>> GetSubjects();
    }
}
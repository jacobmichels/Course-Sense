using course_sense_dotnet.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace course_sense_dotnet.WebAdvisor
{
    public interface IRequests
    {
        Task<bool> CheckCourseExists(Course course);
        Task<CourseCapacity> GetCapacity(Course course);
        Task<List<string>> GetSubjects();
    }
}
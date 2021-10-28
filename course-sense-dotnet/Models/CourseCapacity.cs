using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace course_sense_dotnet.Models
{
    // This class is a model for the capacity of a course.
    public class CourseCapacity
    {
        public CourseCapacity(int currentCapacity, int totalCapacity)
        {
            CurrentCapacity = currentCapacity;
            TotalCapacity = totalCapacity;
        }

        public int TotalCapacity { get; set; }
        public int CurrentCapacity { get; set; }
    }
}

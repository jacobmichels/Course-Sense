using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace course_sense_dotnet.Models
{
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

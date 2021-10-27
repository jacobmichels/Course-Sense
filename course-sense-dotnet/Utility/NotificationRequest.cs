using course_sense_dotnet.Models.WebAdvisor;
using LiteDB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace course_sense_dotnet.Utility
{
    public class NotificationRequest
    {
        public CourseInfo RequestedCourse { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        [System.Text.Json.Serialization.JsonIgnore]
        public ObjectId _id { get; set; }
    }
}

using LiteDB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace course_sense_dotnet.Models
{
    // This class is a model for a user's notification request.
    // It includes the course the user wants to get into, and methods to contact the user once space is found.
    public class NotificationRequest
    {
        public CourseInfo RequestedCourse { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        
        //This _id property is required for litedb
        [System.Text.Json.Serialization.JsonIgnore]
        public ObjectId _id { get; set; }
    }
}

﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace course_sense_dotnet.Models
{
    // This class is the model for a course on WebAdvisor.
    public class CourseInfo
    {
        public string Term { get; set; }
        public string Subject { get; set; }
        public string Code { get; set; }
        public string Section { get; set; }
    }
}

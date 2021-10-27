using course_sense_dotnet.Models;
using System.Collections;
using System.Collections.Generic;

namespace course_sense_dotnet_tests.TestData
{
    public class BadCourseClassData:IEnumerable<object[]>
    {
        public IEnumerator<object[]> GetEnumerator()
        {
            yield return new object[] { new CourseInfo() { Term = "W21", Section = "0101", Code = "3751", Subject = "CIS" } };
            yield return new object[] { new CourseInfo() { Term = "W21", Section = "0104", Code = "10", Subject = "BIOL" } };
            yield return new object[] { new CourseInfo() { Term = "W21", Section = "0101", Code = "2040", Subject = "EN" } };
            yield return new object[] { new CourseInfo() { Term = "W51", Section = "01", Code = "3300", Subject = "CROP" } };
            yield return new object[] { new CourseInfo() { Term = "1", Section = "03", Code = "1100", Subject = "ECON" } };
            yield return new object[] { new CourseInfo() { Term = "1", Section = "7329", Code = "1100", Subject = "ECON" } };
        }
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}

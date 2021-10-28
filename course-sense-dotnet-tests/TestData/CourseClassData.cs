using course_sense_dotnet.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace course_sense_dotnet_tests.TestData
{
    public class CourseClassData:IEnumerable<object[]>
    {
        public IEnumerator<object[]> GetEnumerator()
        {
            yield return new object[] { new CourseInfo() { Term = "W21", Section = "0101", Code = "3750", Subject = "CIS" } };
            yield return new object[] { new CourseInfo() { Term = "W21", Section = "0104", Code = "1070", Subject = "BIOL" } };
            yield return new object[] { new CourseInfo() { Term = "W21", Section = "0101", Code = "2040", Subject = "EQN" } };
            yield return new object[] { new CourseInfo() { Term = "W21", Section = "01", Code = "3300", Subject = "CROP" } };
            yield return new object[] { new CourseInfo() { Term = "W21", Section = "03", Code = "1100", Subject = "ECON" } };
        }
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}

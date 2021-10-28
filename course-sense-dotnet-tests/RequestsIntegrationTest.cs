using course_sense_dotnet.Application.WebAdvisor.RequestHelper;
using course_sense_dotnet.Application.WebAdvisor.RequestManager;
using course_sense_dotnet.Models;
using course_sense_dotnet_tests.TestData;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace course_sense_dotnet_tests
{
    public class RequestsIntegrationTest
    {
        private RequestManager requests;
        private RequestsHelper requestsHelper;
        public RequestsIntegrationTest()
        {
            requestsHelper = new RequestsHelper(new Mock<ILogger<RequestsHelper>>().Object);
            requests = new RequestManager(new Mock<ILogger<RequestManager>>().Object,requestsHelper);
        }

        [Theory]
        [ClassData(typeof(CourseClassData))]
        public async Task CheckCourseExists_Should_ReturnTrue(CourseInfo course)
        {
            bool result = await requests.CheckCourseExists(course);
            result.Should().BeTrue();
        }
        [Theory]
        [ClassData(typeof(BadCourseClassData))]
        public async Task CheckCourseExists_Should_ReturnFalse(CourseInfo course)
        {
            bool result = await requests.CheckCourseExists(course);
            result.Should().BeFalse();
        }

        [Fact]
        public async Task GetSubjects_Should_ReturnSubjectList()
        {
            List<string> subjects = await requests.GetSubjects();
            subjects.Should().NotBeEmpty();
        }
        [Theory]
        [ClassData(typeof(CourseClassData))]
        public async Task GetCapacity_Should_ReturnValidCapacity(CourseInfo course)
        {
            CourseCapacity capacity = await requests.GetCapacity(course);
            capacity.CurrentCapacity.Should().BeGreaterOrEqualTo(0);
            capacity.TotalCapacity.Should().BeGreaterThan(0);
        }
    }
}

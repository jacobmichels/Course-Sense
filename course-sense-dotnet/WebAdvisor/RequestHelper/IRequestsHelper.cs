using course_sense_dotnet.Models;
using HtmlAgilityPack;
using System.Net.Http;

namespace course_sense_dotnet.WebAdvisor.RequestHelper
{
    public interface IRequestsHelper
    {
        HttpClient CreateHttpClient();
        HttpRequestMessage CreateHttpRequestMessage(HttpMethod method, string url);
        string GetTokenFromResponse(HttpResponseMessage response);
        string CreatePostUrl(string token);
        HttpContent CreateFormData(CourseInfo course);
        CourseCapacity GetCourseCapacity(HtmlNode capacityNode);
    }
}
using course_sense_dotnet.Models;
using HtmlAgilityPack;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace course_sense_dotnet.WebAdvisor.RequestHelper
{
    public class RequestsHelper : IRequestsHelper
    {
        private readonly ILogger logger;
        public RequestsHelper(ILogger<RequestsHelper> logger)
        {
            this.logger = logger;
        }
        public HttpClient CreateHttpClient()
        {
            HttpClientHandler handler = new HttpClientHandler()
            {
                UseCookies = true,
                AllowAutoRedirect = true,
                CookieContainer = new CookieContainer()
            };
            return new HttpClient(handler);
        }
        public HttpRequestMessage CreateHttpRequestMessage(HttpMethod httpMethod, string url)
        {
            return new HttpRequestMessage(httpMethod, url);
        }
        public string GetTokenFromResponse(HttpResponseMessage response)
        {
            if (response == null)
            {
                logger.LogError($"GetTokenFromResponse: Parameter {nameof(response)} is null.");
                throw new ArgumentNullException($"{nameof(response)} is null.");
            }
            List<string> cookies = response.Headers.GetValues("Set-Cookie").ToList();
            return cookies[1].Split("=")[1];
        }
        public string CreatePostUrl(string token)
        {
            if (token == null)
            {
                logger.LogError("CreatePostUrl: Parameter {nameof(token)} is null.");
                throw new ArgumentNullException($"{nameof(token)} is null.");
            }
            return "https://webadvisor.uoguelph.ca/WebAdvisor/WebAdvisor?TOKENIDX=" + token + "&SS=1&APP=ST&CONSTITUENCY=WBST";
        }
        public HttpContent CreateFormData(CourseInfo course)
        {
            if (course == null)
            {
                logger.LogError("CreateFormData: Parameter { nameof(course)} is null.");
                throw new ArgumentNullException($"{nameof(course)} is null.");
            }
            Dictionary<string, string> formdict = new Dictionary<string, string>();

            formdict.Add("VAR1", course.Term);
            formdict.Add("DATE.VAR1", "");
            formdict.Add("DATE.VAR2", "");
            formdict.Add("LIST.VAR1_CONTROLLER", "LIST.VAR1");
            formdict.Add("LIST.VAR1_MEMBERS", "LIST.VAR1*LIST.VAR2*LIST.VAR3*LIST.VAR4");
            formdict.Add("LIST.VAR1_MAX", "5");
            formdict.Add("LIST.VAR2_MAX", "5");
            formdict.Add("LIST.VAR3_MAX", "5");
            formdict.Add("LIST.VAR4_MAX", "5");
            formdict.Add("LIST.VAR1_1", course.Subject);
            formdict.Add("LIST.VAR2_1", "");
            formdict.Add("LIST.VAR3_1", course.Code);
            formdict.Add("LIST.VAR4_1", course.Section);
            formdict.Add("LIST.VAR1_2", "");
            formdict.Add("LIST.VAR2_2", "");
            formdict.Add("LIST.VAR3_2", "");
            formdict.Add("LIST.VAR4_2", "");
            formdict.Add("LIST.VAR1_3", "");
            formdict.Add("LIST.VAR2_3", "");
            formdict.Add("LIST.VAR3_3", "");
            formdict.Add("LIST.VAR4_3", "");
            formdict.Add("LIST.VAR1_4", "");
            formdict.Add("LIST.VAR2_4", "");
            formdict.Add("LIST.VAR3_4", "");
            formdict.Add("LIST.VAR4_4", "");
            formdict.Add("LIST.VAR1_5", "");
            formdict.Add("LIST.VAR2_5", "");
            formdict.Add("LIST.VAR3_5", "");
            formdict.Add("LIST.VAR4_5", "");
            formdict.Add("VAR7", "");
            formdict.Add("VAR8", "");
            formdict.Add("VAR3", "");
            formdict.Add("VAR6", "");
            formdict.Add("VAR21", "");
            formdict.Add("VAR9", "");
            formdict.Add("SUBMIT_OPTIONS", "");

            FormUrlEncodedContent content = new FormUrlEncodedContent(formdict);
            content.Headers.ContentType = MediaTypeHeaderValue.Parse("application/x-www-form-urlencoded");

            return content;
        }
        public CourseCapacity GetCourseCapacity(HtmlNode capacityNode)
        {
            if (capacityNode == null)
            {
                logger.LogError($"GetCourseCapacity: Parameter {nameof(capacityNode)} is null.");
                throw new ArgumentNullException($"{nameof(capacityNode)} is null.");
            }
            string[] capacities = capacityNode.InnerText.Split(" / ");
            return new CourseCapacity(int.Parse(capacities[0]), int.Parse(capacities[1]));
        }

    }
}

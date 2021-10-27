using course_sense_dotnet.Models;
using course_sense_dotnet.Utility;
using HtmlAgilityPack;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace course_sense_dotnet.Models.WebAdvisor
{
    public class Requests : IRequests
    {
        private ILogger logger;
        private HttpClient httpClient;
        private IRequestsHelper requestsHelper;
        public Requests(ILogger<Requests> logger, IRequestsHelper requestsHelper)
        {
            this.logger = logger;
            this.requestsHelper = requestsHelper;
            httpClient = requestsHelper.CreateHttpClient();
        }
        public async Task<CourseCapacity> GetCapacity(CourseInfo course)
        {
            try
            {
                HttpRequestMessage request = requestsHelper.CreateHttpRequestMessage(HttpMethod.Get, Constants.WebAdvisorInitialConnectionUrl);
                HttpResponseMessage response = await httpClient.SendAsync(request);
                string token = requestsHelper.GetTokenFromResponse(response);

                request = requestsHelper.CreateHttpRequestMessage(HttpMethod.Get, Constants.WebAdvisorInitialConnectionUrl + token);
                response = await httpClient.SendAsync(request);
                token = requestsHelper.GetTokenFromResponse(response);

                string postUrl = requestsHelper.CreatePostUrl(token);
                request = requestsHelper.CreateHttpRequestMessage(HttpMethod.Post, postUrl);
                request.Content = requestsHelper.CreateFormData(course);
                response = await httpClient.SendAsync(request);

                string responseHtml = await response.Content.ReadAsStringAsync();
                HtmlDocument htmlDoc = new HtmlDocument();
                htmlDoc.LoadHtml(responseHtml);

                HtmlNode capacityNode = htmlDoc.GetElementbyId(Constants.CapacityNodeID);
                CourseCapacity capacity = requestsHelper.GetCourseCapacity(capacityNode);

                return capacity;
            }
            catch (Exception e)
            {
                logger.LogInformation($"An exception occured in GetCapacity request: {e.Message}");
                throw;
            }
        }
        public async Task<bool> CheckCourseExists(CourseInfo course)
        {
            HttpRequestMessage request = requestsHelper.CreateHttpRequestMessage(HttpMethod.Get, Constants.WebAdvisorInitialConnectionUrl);
            HttpResponseMessage response = await httpClient.SendAsync(request);
            string token = requestsHelper.GetTokenFromResponse(response);

            request = requestsHelper.CreateHttpRequestMessage(HttpMethod.Get, Constants.WebAdvisorInitialConnectionUrl + token);
            response = await httpClient.SendAsync(request);
            token = requestsHelper.GetTokenFromResponse(response);

            string postUrl = requestsHelper.CreatePostUrl(token);
            request = requestsHelper.CreateHttpRequestMessage(HttpMethod.Post, postUrl);
            request.Content = requestsHelper.CreateFormData(course);
            response = await httpClient.SendAsync(request);

            string responseHtml = await response.Content.ReadAsStringAsync();
            HtmlDocument htmlDoc = new HtmlDocument();
            htmlDoc.LoadHtml(responseHtml);

            HtmlNode mainContentNode = htmlDoc.GetElementbyId("main");
            HtmlNodeCollection errorNodes = mainContentNode.SelectNodes("//div[contains(@class, 'errorText')]");
            if (errorNodes != null && errorNodes.Any())
            {
                return false;
            }
            return true;
        }
        public async Task<List<string>> GetSubjects()
        {
            HttpRequestMessage request = requestsHelper.CreateHttpRequestMessage(HttpMethod.Get, Constants.WebAdvisorInitialConnectionUrl);
            HttpResponseMessage response = await httpClient.SendAsync(request);
            string token = requestsHelper.GetTokenFromResponse(response);

            request = requestsHelper.CreateHttpRequestMessage(HttpMethod.Get, Constants.WebAdvisorInitialConnectionUrl + token);
            response = await httpClient.SendAsync(request);

            string responseHtml = await response.Content.ReadAsStringAsync();
            HtmlDocument htmlDoc = new HtmlDocument();
            htmlDoc.LoadHtml(responseHtml);

            HtmlNode subjectListNode = htmlDoc.GetElementbyId("LIST_VAR1_1");
            List<string> subjectList = new List<string>();
            foreach (HtmlNode childNode in subjectListNode.ChildNodes)
            {
                subjectList.Add(childNode.InnerText.Split(" - ")[0]);
            }
            subjectList.Remove("");
            return subjectList;
        }
    }
}

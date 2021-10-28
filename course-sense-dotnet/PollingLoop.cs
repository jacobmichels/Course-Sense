using course_sense_dotnet.CapacityManager;
using course_sense_dotnet.Models;
using course_sense_dotnet.Repository;
using course_sense_dotnet.WebAdvisor;
using LiteDB;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace course_sense_dotnet
{
    public class PollingLoop : BackgroundService
    {
        private readonly ILogger logger;
        private readonly IServiceProvider serviceProvider;
        private SynchronizedCollection<NotificationRequest> requestCollection;
        private readonly IList<Task> tasks;
        private readonly IDBRepository dataAccess;
        public PollingLoop(ILogger<PollingLoop> logger,
            SynchronizedCollection<NotificationRequest> requestCollection,
            IList<Task> tasks,
            IServiceProvider serviceProvider,
            IHostApplicationLifetime applicationLifetime,
            IDBRepository dataAccess)
        {
            this.logger = logger;
            this.requestCollection = requestCollection;
            this.tasks = tasks;
            this.serviceProvider = serviceProvider;
            this.dataAccess = dataAccess;
            applicationLifetime.ApplicationStarted.Register(LoadNotificationRequests);
        }
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            logger.LogInformation("Main loop background service starting.");
            while (!stoppingToken.IsCancellationRequested)
            {
                if (requestCollection.Count>0)
                {
                    foreach (NotificationRequest request in requestCollection)
                    {
                        tasks.Add(Task.Run(() => serviceProvider.GetRequiredService<ICapacityManager>().CheckCapacityAndAlert(request, requestCollection)));
                    }
                    Task requestTasks = Task.WhenAll(tasks);
                    try
                    {
                        await requestTasks;
                    }
                    catch (Exception e)
                    {
                        logger.LogError($"Error occured while awaiting {nameof(requestTasks)}: {e.Message}");
                    }
                    if (requestTasks.Status == TaskStatus.Faulted)
                    {
                        logger.LogError($"{nameof(requestTasks)} has completed due to an unhandled exception.");
                    }
                    tasks.Clear();
                }
                await Task.Delay(10000);
            }
        }
        private void LoadNotificationRequests()
        {
            logger.LogInformation("Loading NotificationRequestCollection from db.");
            int NumberOfRetrievedDocuments =0;
            try
            {
                IEnumerable<NotificationRequest> dbcollection = dataAccess.GetAllNotificationRequests();
                NumberOfRetrievedDocuments = dbcollection.Count();
                foreach(NotificationRequest request in dbcollection)
                {
                    requestCollection.Add(request);
                }
            }
            catch(Exception e)
            {
                logger.LogError("Possible failure to load requests from database: " + e.Message);
            }
            logger.LogInformation($"Loaded {NumberOfRetrievedDocuments} document(s) from db.");
        }
    }
}

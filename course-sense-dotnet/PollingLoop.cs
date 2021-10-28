using course_sense_dotnet.Application.CapacityManager;
using course_sense_dotnet.Models;
using course_sense_dotnet.Repository;
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
        private readonly IDBRepository repository;

        public PollingLoop(ILogger<PollingLoop> logger,
            SynchronizedCollection<NotificationRequest> requestCollection,
            IServiceProvider serviceProvider,
            IDBRepository repository)
        {
            // Get dependencies from dependency injection.
            this.logger = logger;
            this.requestCollection = requestCollection;
            this.repository = repository;
            this.serviceProvider = serviceProvider;

            // Load notification requests before main polling loop starts.
            LoadNotificationRequests();
        }

        // This is the background task that is started upon application start.
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            logger.LogInformation("Main loop background service starting.");

            var tasks = new List<Task>();

            // Stop if Cancellation is requested via the token. Ex. application shutdown
            while (!stoppingToken.IsCancellationRequested)
            {
                // Only continue if there are requests in the collection to process.
                if (requestCollection.Count > 0)
                {
                    // Loop through all requests, and spawn a task to check the course's capacity and alert the user if open space is found.
                    foreach (NotificationRequest request in requestCollection)
                    {
                        tasks.Add(Task.Run(() => {
                            using (var scope = serviceProvider.CreateScope())
                            {
                                scope.ServiceProvider.GetRequiredService<ICapacityManager>().NotifyIfSpaceFound(request, requestCollection);
                            }
                        }));
                    }

                    // Wait for all the tasks to complete, log any errors.
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
                        logger.LogError($"{nameof(requestTasks)} has faulted.");
                        
                    }

                    // Clear the task list in preparation for next iteration.
                    tasks.Clear();
                }

                // A primitive rate-limit to avoid spamming the WebAdvisor servers.
                await Task.Delay(10000);
            }
        }

        //This method loads saved NotificationRequests from the repository.
        private void LoadNotificationRequests()
        {
            logger.LogInformation("Loading existing NotificationRequests from repository, if any.");
            int requestCount = 0;
            try
            {
                // Get requests from the repository, then add each to the service's request collection.
                IEnumerable<NotificationRequest> requests = repository.GetAllNotificationRequests();
                requestCount = requests.Count();
                foreach(NotificationRequest request in requests)
                {
                    requestCollection.Add(request);
                }
            }
            catch(Exception e)
            {
                logger.LogError("Failed to load requests from database: " + e.Message);
            }
            logger.LogInformation($"Loaded {requestCount} NotificationRequest(s) from repository.");
        }
    }
}

using course_sense_dotnet.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace course_sense_dotnet.NotificationManager
{
    public interface INotificationManager
    {
        Task CheckCapacityAndAlert(NotificationRequest notificationRequest,
            SynchronizedCollection<NotificationRequest> notificationRequests);
    }
}
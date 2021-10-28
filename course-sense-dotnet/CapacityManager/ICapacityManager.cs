using course_sense_dotnet.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace course_sense_dotnet.CapacityManager
{
    public interface ICapacityManager
    {
        Task CheckCapacityAndAlert(NotificationRequest notificationRequest,
            SynchronizedCollection<NotificationRequest> notificationRequests);
    }
}
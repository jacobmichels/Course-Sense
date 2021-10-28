using course_sense_dotnet.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace course_sense_dotnet.Application.CapacityManager
{
    public interface ICapacityManager
    {
        Task NotifyIfSpaceFound(NotificationRequest notificationRequest,
            SynchronizedCollection<NotificationRequest> notificationRequests);
    }
}
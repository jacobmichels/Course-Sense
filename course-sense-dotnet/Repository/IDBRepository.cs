using course_sense_dotnet.Models;
using System.Collections.Generic;

namespace course_sense_dotnet.Repository
{
    public interface IDBRepository
    {
        IEnumerable<NotificationRequest> GetAllNotificationRequests();
        bool AddRequest(NotificationRequest request);
        bool RemoveRequest(NotificationRequest request);
    }
}
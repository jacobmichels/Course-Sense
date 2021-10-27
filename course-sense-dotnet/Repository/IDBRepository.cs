using course_sense_dotnet.Models;
using System.Collections.Generic;

namespace course_sense_dotnet.Repository
{
    public interface IDBRepository
    {
        void Dispose();
        IEnumerable<NotificationRequest> GetAllNotificationRequests();
        bool InsertRequest(NotificationRequest request);
        bool RemoveRequest(NotificationRequest request);
    }
}
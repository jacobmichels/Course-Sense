using course_sense_dotnet.Utility;
using System.Collections.Generic;

namespace course_sense_dotnet.DataAccessLayer
{
    public interface IDataAccess
    {
        void Dispose();
        IEnumerable<NotificationRequest> GetCollectionFromDB();
        bool InsertRequestIntoDB(NotificationRequest request);
        bool RemoveRequestFromDB(NotificationRequest request);
    }
}
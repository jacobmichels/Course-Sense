using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LiteDB;
using Microsoft.Extensions.Configuration;
using course_sense_dotnet.Models;

namespace course_sense_dotnet.Repository
{
    // Repository for accessing LiteDB to persist NotificationRequests on disk.
    public class DBRepository : IDisposable, IDBRepository
    {
        private readonly ILogger logger;
        private readonly LiteDatabase db;
        private bool disposedValue;

        public DBRepository(ILogger<DBRepository> logger, IConfiguration configuration)
        {
            this.logger = logger;
            db = new LiteDatabase(configuration["LiteDB:ConnectionString"]);
        }

        // This method will add the NotificationRequest to litedb, returning if the insert was successful and logging any errors.
        public bool AddRequest(NotificationRequest request)
        {
            try
            {
                ILiteCollection<NotificationRequest> collection = db.GetCollection<NotificationRequest>("notification_requests");
                collection.Insert(request);
                return true;
            }
            catch (Exception e)
            {
                logger.LogError("Unable to save document into LiteDB: " + e.Message);
                return false;
            }
        }

        // This method will remove the NotificationRequest from litedb, returning if the deletion was successful and logging any errors.
        public bool RemoveRequest(NotificationRequest request)
        {
            try
            {
                ILiteCollection<NotificationRequest> collection = db.GetCollection<NotificationRequest>("notification_requests");
                return collection.Delete(request._id);
            }
            catch (Exception e)
            {
                logger.LogError("Unable to delete document from LiteDB: " + e.Message);
                return false;
            }
        }

        // This method will fetch all the NotificationRequests from litedb and return them, or null if none are found.
        public IEnumerable<NotificationRequest> GetAllNotificationRequests()
        {
            try
            {
                ILiteCollection<NotificationRequest> collection = db.GetCollection<NotificationRequest>("notification_requests");
                return collection.FindAll();
            }
            catch (Exception e)
            {
                logger.LogError("Could not read from LiteDB: " + e.Message);
                return null;
            }
        }

        //The LiteDatabase object needs to be disposed, this method and the DI container disposes of it properly.
        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    db.Dispose();
                }
                disposedValue = true;
            }
        }

        public void Dispose()
        {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }
}

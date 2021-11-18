using Domain.Entities;
using Infrastructure.Persistance;
using System.Collections.Generic;

namespace Application.Interfaces
{
    public interface INotificationManager
    {
        public ICollection<NotificationDTO> GetCurrentUserNotifications(long? lastNotifyDateTicks);
        bool ReadCurrentUserNotification(int id);
        public void AddAdminNotification(string notificationMessage, string actionPath, int? reviewId = null, int? pendingAuthorId = null, int? extendId = null);
        void ClearCurrentUserNotifications();
        void RemoveBy(Extend extend);
        public void AddExtendNotification(string notificationMessage, string actionPath, User assignee);
        void AddWishedBookNotification(string notificationMessage, string actionPath, WishBook wishedBook);
        public void AddAssignNotification(string notificationMessage, string actionPath, User assignee);
    }
}
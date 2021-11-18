using Infrastructure.Persistance;
using System.Collections.Generic;

namespace Infrastructure.Repository.Interfaces
{
    public interface INotificationRepository : IRepository<Notification>
    {
        ICollection<Notification> GetUserNotifications(User currentUser);
        ICollection<Notification> GetUserNotifications(User currentUser, long lastNotifyDate);
        Notification GetUserNotification(User user, int id);
        void ReadUserNotification(User user, Notification notification);
        public void AddUsersNotification(Notification notification, string userRoleScope);
        public void RemoveBy(Extend extend);
        public void RemoveBy(Author author);
        void ClearNotifications(User user);
        void AddWishedBookNotifications(Notification notification, WishBook wishedBook);
    }
}
using Infrastructure.Persistance;
using Infrastructure.Repository.Abstract;
using Infrastructure.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace Infrastructure.Repository.Implementations
{
    public class NotificationRepository : AbstractRepository<Notification>, INotificationRepository
    {
        public NotificationRepository(BookSharingContext context) : base(context) { }

        public ICollection<Notification> GetUserNotifications(User currentUser)
        {
            return DataBaseContext.Users.Where(u => u == currentUser).Include(u => u.Notifications).FirstOrDefault().Notifications.OrderByDescending(n => n.ReceivedDate).ToList();
        }

        public ICollection<Notification> GetUserNotifications(User currentUser, long lastNotifyDate)
        {
            return DataBaseContext.Users.Where(u => u == currentUser).Include(u => u.Notifications).FirstOrDefault().Notifications.Where(n => n.ReceivedDate.Ticks > lastNotifyDate).OrderByDescending(n => n.ReceivedDate).ToList();
        }

        public Notification GetUserNotification(User user, int notificationId)
        {
            return DataBaseContext.Users.Where(u => u == user).Include(u => u.Notifications).FirstOrDefault().Notifications.FirstOrDefault(n => n.Id == notificationId);
        }

        public void ReadUserNotification(User user, Notification notification)
        {
            DataBaseContext.Users.Where(u => u == user).Include(u => u.Notifications).FirstOrDefault().Notifications.Remove(notification);
        }

        public void AddUsersNotification(Notification notification, string userRoleScope)
        {
            DataBaseContext.Users.Include(a => a.UserRoles).ThenInclude(ur => ur.Role).Where(u => u.UserRoles.Any(r => r.Role.Name.Equals(userRoleScope))).ForEachAsync(i => i.Notifications.Add(notification)).GetAwaiter().GetResult();
        }

        public void RemoveBy(Extend extend)
        {
            Remove(DataBaseContext.Notifications.Where(n => n.PendingExtend == extend).FirstOrDefault());
        }

        public void RemoveBy(Author author)
        {
            Remove(DataBaseContext.Notifications.Where(n => n.PendingAuthor == author).FirstOrDefault());
        }

        public void ClearNotifications(User user)
        {
            DataBaseContext.Users.Where(u => u == user).Include(u => u.Notifications).FirstOrDefault().Notifications.Clear();
        }

        public void AddWishedBookNotifications(Notification notification, WishBook wishedBook)
        {
            DataBaseContext.Users.Include(u => u.WishBooks).Where(u => u.WishBooks.Any(w => w == wishedBook)).ForEachAsync(i => i.Notifications.Add(notification)).GetAwaiter().GetResult();
        }
    }
}

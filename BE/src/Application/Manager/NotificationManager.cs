using Application.Interfaces;
using AutoMapper;
using Domain.Entities;
using Domain.Enums;
using Infrastructure.Persistance;
using Infrastructure.Repository.Interfaces;
using System;
using System.Collections.Generic;

namespace Application.Manager
{
    public class NotificationManager : INotificationManager
    {
        private readonly IMapper _mapper;
        private readonly INotificationRepository _notificationRepository;
        private readonly IUserManager _userManager;

        public NotificationManager(IMapper mapper, INotificationRepository notificationRepository, IUserManager userManager)
        {
            _mapper = mapper;
            _notificationRepository = notificationRepository;
            _userManager = userManager;
        }

        public ICollection<NotificationDTO> GetCurrentUserNotifications(long? lastNotifyDateTicks)
        {
            var currentUser = _userManager.GetCurrentUser();
            var model = _mapper.Map<ICollection<NotificationDTO>>
            (
                (lastNotifyDateTicks == null) ?
                _notificationRepository.GetUserNotifications(currentUser) :
                _notificationRepository.GetUserNotifications(currentUser, (long)lastNotifyDateTicks)
            );
            return model;
        }

        public bool ReadCurrentUserNotification(int id)
        {
            var notification = _notificationRepository.Get(id);
            if(notification != null)
            {
                _notificationRepository.ReadUserNotification(_userManager.GetCurrentUser(), notification);
                _notificationRepository.SaveChanges();
                return true;
            }
            return false;
        }

        public static Notification BaseNotification(string messgage, string actionPath)
        {
            return new Notification
            {
                Message = messgage,
                ReceivedDate = DateTime.Now,
                ActionPath = actionPath,
            };
        }

        public void AddAdminNotification(string notificationMessage, string actionPath, int? reviewId = null, int? pendingAuthorId = null, int? extendId = null)
        {
            var notification = BaseNotification(notificationMessage, actionPath);
            notification.AdminScope = true;
            notification.ReviewId = reviewId;
            notification.AuthorId = pendingAuthorId;
            notification.ExtendId = extendId;
            _notificationRepository.Add(notification);
            _notificationRepository.AddUsersNotification(notification, AccessRole.Admin);
            _notificationRepository.SaveChanges();
        }

        public void AddExtendNotification(string notificationMessage, string actionPath, User assignee)
        {
            var notification = BaseNotification(notificationMessage, actionPath);
            _notificationRepository.Add(notification);
            assignee.Notifications.Add(notification);
            _notificationRepository.SaveChanges();
        }

        public void AddWishedBookNotification(string notificationMessage, string actionPath, WishBook wishedBook)
        {
            var notification = BaseNotification(notificationMessage, actionPath);
            _notificationRepository.AddWishedBookNotifications(notification, wishedBook);
            _notificationRepository.SaveChanges();
        }
        public void AddAssignNotification(string notificationMessage, string actionPath, User assignee)
        {
            var notification = BaseNotification(notificationMessage, actionPath);
            _notificationRepository.Add(notification);
            assignee.Notifications.Add(notification);
        }

        public void ClearCurrentUserNotifications()
        {
            _notificationRepository.ClearNotifications(_userManager.GetCurrentUser());
            _notificationRepository.SaveChanges();
        }

        public void RemoveBy(Extend extend)
        {
            _notificationRepository.RemoveBy(extend);
        }
    }
}

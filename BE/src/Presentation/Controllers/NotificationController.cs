using Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Presentation.Controllers
{
    [Authorize]
    public class NotificationController : Controller
    {
        private readonly INotificationManager _notificationManager;

        public NotificationController(INotificationManager notificationManager)
        {
            _notificationManager = notificationManager;
        }

        [HttpGet]
        public IActionResult GetAll(long? lastNotifyDate)
        {
            var model = _notificationManager.GetCurrentUserNotifications(lastNotifyDate);
            return model.Count == 0 ? NoContent() : PartialView(model);
        }

        [HttpDelete]
        public IActionResult ReadNotification(int id)
        {
            return _notificationManager.ReadCurrentUserNotification(id) ? Ok() : BadRequest();
        }

        public IActionResult ClearNotifications()
        {
            _notificationManager.ClearCurrentUserNotifications();
            return NoContent();
        }
    }
}

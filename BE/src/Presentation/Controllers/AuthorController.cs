using Application.Interfaces;
using Domain.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Presentation.Controllers
{
    [Authorize]
    public class AuthorController : Controller
    {
        private readonly IAuthorManager _authorManager;
        public AuthorController(IAuthorManager authorManager)
        {
            _authorManager = authorManager;
        }

        [HttpPost, ActionName("UploadAuthor")]
        [Authorize(Roles = AccessRole.Admin)]
        public IActionResult UploadAuthor(string data)
        {
            if(data != null)
            {
                string authorName = _authorManager.AddAuthorNotPending(data);
                RedirectToAction("Admin", "Account");
                return Json(new { Name = authorName });
            }
            else
                return BadRequest();
        }
    }
}

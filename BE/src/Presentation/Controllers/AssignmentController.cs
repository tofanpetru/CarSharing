using Application.Interfaces;
using Domain.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Presentation.Controllers
{
    [Authorize(Roles = AccessRole.Admin)]
    public class AssignmentController : Controller
    {
        private readonly IExtendManager _extendManager;
        public AssignmentController(IExtendManager extendManager)
        {
            _extendManager = extendManager;
        }

        public ActionResult RejectAssignment(int id, string section)
        {
            _extendManager.RejectExtend(id);
            return RedirectToAction("Admin", "Account", new { section });
        }

        public ActionResult ApproveAssignment(int id, string section)
        {
            _extendManager.ApproveExtend(id);
            return RedirectToAction("Admin", "Account", new { section });
        }
    }
}

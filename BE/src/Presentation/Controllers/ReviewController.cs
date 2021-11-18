using Application.Interfaces;
using Domain.Entities;
using Domain.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Presentation.Controllers
{
    [Authorize]
    public class ReviewController : Controller
    {
        private readonly IReviewManager _reviewManager;

        public ReviewController(IReviewManager reviewManager)
        {
            _reviewManager = reviewManager;
        }

        [HttpPost]
        public IActionResult AddBookReview([FromBody] AddReviewDTO addReview)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    if(_reviewManager.AddReview(addReview))
                        return StatusCode(201);
                }
                catch 
                { 
                    return BadRequest();
                }
            }
            return BadRequest();
        }

        [HttpPost]
        [Authorize(Roles = AccessRole.Admin)]
        public IActionResult DeleteReview(int id, string section)
        {
            _reviewManager.DeleteReview(id);
            return RedirectToAction("Admin", "Account", new { section });
        }
    }
}

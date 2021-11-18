using Application;
using Application.Interfaces;
using BookSharing.Models;
using Domain.Entities;
using Domain.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Presentation.Controllers
{
    [Authorize]
    public class BookController : Controller
    {
        private readonly IBookManager _bookManager;
        private readonly IAuthorManager _authorManager;
        private readonly ILanguageManager _languageManager;
        private readonly IAssignmentManager _assignmentManager;
        private readonly IUserManager _userManager;
        private readonly IWishBookManager _wishListManager;
        private readonly IReviewManager _reviewManager;
        private readonly IExtendManager _extendManager;
        private readonly IGenreManager _genreManager;

        public BookController(IGenreManager genreManager, IBookManager bookManager, IAuthorManager authorManager, ILanguageManager languageManager,
                              IAssignmentManager assignmentManager, IUserManager userManager, IWishBookManager wishListManager, IReviewManager reviewManager,
                              IExtendManager extendManager)
        {
            _bookManager = bookManager;
            _authorManager = authorManager;
            _languageManager = languageManager;
            _assignmentManager = assignmentManager;
            _userManager = userManager;
            _wishListManager = wishListManager;
            _reviewManager = reviewManager;
            _extendManager = extendManager;
            _genreManager = genreManager;
        }

        public IActionResult Details(int id)
        {
            var book = _bookManager.GetBookDetails(id);
            if(book == null)
            {
                var errorModel = new ErrorViewModel() { Code = 404, Status = "Not Found", Message = "Book with such id doesn't exist." };
                return View("Error", errorModel);
            }
            var user = _userManager.GetCurrentUser();
            string currentUserName = user.UserName;
            var hasLeftQueueAssignments = _userManager.HasQueueAssignmentsLeft(user);
            ViewBag.currentUserName = currentUserName;
            ViewBag.hasAssignmentsLeft = _userManager.HasAssignmentsLeft(user);
            ViewBag.hasQueueAssignmentsLeft = hasLeftQueueAssignments;
            ViewBag.isBookAssignedToCurrentUser = _userManager.IsBookAssignedToCurrentUser(id);
            ViewBag.bookTotalRating = _reviewManager.GetRatingsByBookId(id);
            ViewBag.genres = _genreManager.GetAll();
            ViewBag.languages = _languageManager.GetAll();
            ViewBag.authors = _authorManager.GetAll();
            ViewBag.hasReviews = _reviewManager.HasUserBookReview(currentUserName, book.Id);
            if (ViewBag.hasReviews)
                ViewBag.UserReview = _reviewManager.GetByUsernameAndBookId(currentUserName, book.Id);
            ViewBag.hasPendingExtend = _assignmentManager.HasPendingExtend(id);
            if (!hasLeftQueueAssignments)
                ViewBag.queueEndDate = _assignmentManager.GetUserQueueAssignmentsLastEndDate(user).Value.ToString(AppSettings.Instance.DateDisplayFormat);
            if (book.Owner != currentUserName && book.IsPending)
                return NoContent();
            return View("Details", book);
        }

        public IActionResult Update(int id)
        {
            var book = _bookManager.GetBookToUpdate(id);
            var authors = _authorManager.GetAllNonPendingAuthors();
            var languages = _languageManager.GetAll();
            ViewBag.authors = authors;
            ViewBag.languages = languages;
            return View(book);
        }

        [HttpPost]
        public async Task<IActionResult> Update(ResponseEditBookDTO updateBookDTO)
        {
            if (ModelState.IsValid && await _bookManager.Update(updateBookDTO))
                return Ok();
            else
                return BadRequest();
        }

        [HttpPost]
        public IActionResult DeleteBook(int id, [FromQuery] string referrer)
        {
            _bookManager.DeleteBook(id);
            return Redirect(referrer);
        }

        [HttpPost]
        [RequestSizeLimit(1_500_000)]
        public async Task<IActionResult> AddNewBook(AddBookDTO addBookDTO)
        {
            if (ModelState.IsValid && await _bookManager.UploadBook(addBookDTO))
                return Ok();
            else
                return BadRequest();
        }

        [HttpPost]
        public JsonResult AssignBook(int bookId)
        {
            return Json(_userManager.AssignBookToCurrentUser(bookId));
        }

        [HttpPost]
        public IActionResult AddToQueue(int bookId)
        {
            return Ok(_userManager.AssignBookToUserQueue(bookId));
        }

        [HttpGet]
        public IActionResult WishBookList(string sortOrder, string searchString, bool userChecked, int firstItem = 0)
        {
            var userFilter = userChecked ? _userManager.GetCurrentUser() : null;
            var model = _wishListManager.GetWishedBooks(firstItem, AppSettings.Instance.WishedBooksPageListSize, sortOrder, searchString, userFilter);
            ViewBag.UserName = _userManager.GetCurrentUserProfileData().UserName;
            if (model.Count == 0)
                return StatusCode(204);
            return PartialView(model);
        }

        [HttpPost, ActionName("AddBookFromPending")]
        [Authorize(Roles = AccessRole.Admin)]
        public ActionResult AddBookFromPending(ResponsePendingRequestDTO responsePendingRequestDTO)
        {
            if (responsePendingRequestDTO != null && ModelState.IsValid)
            {
                _bookManager.AddBookFromPending(responsePendingRequestDTO.Id, responsePendingRequestDTO.Author);
                return RedirectToAction("Admin", "Account");
            }
            else
                return NoContent();
        }

        [HttpPost, ActionName("UpdatePendingBook")]
        [Authorize(Roles = AccessRole.Admin)]
        public ActionResult UpdatePendingBook(ResponsePendingRequestDTO responsePendingRequestDTO)
        {
            if (ModelState.IsValid)
            {
                _bookManager.UpdateAuthorInPendingBook(responsePendingRequestDTO.Id, responsePendingRequestDTO.Author);
                return RedirectToAction("Admin", "Account");
            }
            else
                return NoContent();
        }

        [HttpPost]
        public ActionResult ExtendAssignment(ExtendAssignmentDTO extendAssignmentDTO)
        {
            return (ModelState.IsValid && _extendManager.ExtendAssignment(extendAssignmentDTO)) ? Ok() : BadRequest();
        }

        public IActionResult BookReviews(int bookId, int firstItem = 0)
        {
            var model = _reviewManager.GetByBookId(bookId, firstItem, AppSettings.Instance.ReviewsPerPage);
            if (model.Count == 0)
                return StatusCode(204);
            return PartialView(model);
        }

        [HttpPost]
        public IActionResult AddWishBook(WishBookDTO wishBook)
        {
            return Ok(_wishListManager.AddWishBook(wishBook, _userManager.GetCurrentUser()));
        }

        [HttpPut]
        public IActionResult EditWishBook(WishBookDTO wishBookDTO)
        {
            return Ok(_wishListManager.UpdateWishBook(wishBookDTO, _userManager.GetCurrentUser()));
        }

        [HttpDelete]
        public IActionResult DeleteWishBook(int id)
        {
            if (id == 0)
            {
                return BadRequest();
            }

            _wishListManager.DeleteWishBook(_userManager.GetCurrentUser(), id);
            return Ok();
        }

        [Authorize(Roles = AccessRole.Admin)]
        public IActionResult AdminReviews(int firstItem = 0)
        {
            var model = _reviewManager.GetAllReviewsList(firstItem, AppSettings.Instance.AdminReviewsPerPage);
            if (model.Count == 0)
                return StatusCode(204);
            return PartialView(model);
        }
    }
}

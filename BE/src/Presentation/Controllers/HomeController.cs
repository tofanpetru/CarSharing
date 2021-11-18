using Application.Interfaces;
using BookSharing.Models;
using Domain.Entities;
using Domain.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace BookSharing.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly IBookManager _bookManager;
        private readonly IAuthorManager _authorManager;
        private readonly ILanguageManager _languageManager;
        private readonly IGenreManager _genreManager;

        public HomeController(IGenreManager genreManager, IBookManager bookManager, IAuthorManager authorManager, ILanguageManager languageManager)
        {
            _bookManager = bookManager;
            _authorManager = authorManager;
            _languageManager = languageManager;
            _genreManager = genreManager;
        }

        public IActionResult Index(BookSearch bookSearch, CheckBoxFilter checkBoxFilter, BookRating bookRating, int page = 1)
        {
            var books = _bookManager.GetPagedFilteredBooks(bookSearch, checkBoxFilter, bookRating, page);
            ViewBag.BookSearch = bookSearch;
            ViewBag.checkBoxFilter = checkBoxFilter;
            ViewBag.bookRating = bookRating;
            ViewBag.authors = _authorManager.GetAllNonPendingAuthors();
            ViewBag.languages = _languageManager.GetAll();
            ViewBag.genres = _genreManager.GetAll();
            return View(books);
        }
        [HttpGet("GetAuthors")]
        public IActionResult GetAuthors()
        {
            try
            {
                string term = HttpContext.Request.Query["term"].ToString();
                var authors = _authorManager.GetAuthorsByName(term);
                return Ok(authors);
            }
            catch
            {
                return BadRequest();
            }
        }

        [HttpGet("GetGenres")]
        public IActionResult GetGenres()
        {
            try
            {
                string term = HttpContext.Request.Query["term"].ToString();
                var genres = _genreManager.GetGenresByName(term);
                return Ok(genres);
            }
            catch
            {
                return BadRequest();
            }
        }

        [HttpGet("GetLanguages")]
        public IActionResult GetLanguages()
        {
            try
            {
                string term = HttpContext.Request.Query["term"].ToString();
                var languages = _languageManager.GetLanguagesByName(term);

                return Ok(languages);
            }
            catch
            {
                return BadRequest();
            }
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { Code = 500, Status="Internal Error", Message = "Unexpected Server Error Occured. Request ID: " + Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}

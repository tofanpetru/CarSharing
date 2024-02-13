using Application;
using Application.Interfaces;
using BookSharing.Models;
using Domain.Entities;
using Domain.Enums;
using Infrastructure.Persistance;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Presentation.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {
        private readonly ILogger<AccountController> _logger;
        private readonly IUserManager _userManager;
        private readonly IAuthorManager _authorManager;
        private readonly IGenreManager _genreManager;
        private readonly ILanguageManager _languageManager;
        private readonly IAssignmentManager _assignmentManager;
        private readonly IBookManager _bookManager;
        private readonly UserManager<User> _identityManager;
        private readonly SignInManager<User> _signInManager;
        public AccountController(IAuthorManager authorManager, IGenreManager genreManager, ILanguageManager languageManager, IAssignmentManager assignmentManager,
                                 ILogger<AccountController> logger, IUserManager userManager, SignInManager<User> signInManager, IBookManager bookManager,
                                 UserManager<User> identityManager)
        {
            _logger = logger;
            _userManager = userManager;
            _authorManager = authorManager;
            _genreManager = genreManager;
            _languageManager = languageManager;
            _assignmentManager = assignmentManager;
            _signInManager = signInManager;
            _bookManager = bookManager;
            _identityManager = identityManager;
        }

        [HttpGet, AllowAnonymous]
        public IActionResult Register()
        {
            if (_signInManager.IsSignedIn(User))
                return NoContent();
            return View(new UserRegistrationDTO());
        }

        [HttpPost, AllowAnonymous]
        public async Task<IActionResult> Register(UserRegistrationDTO userDTO)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var result = await _userManager.AddUser(userDTO);

                    if (result.Succeeded)
                    {
                        return RedirectToAction("Login");
                    }
                    else
                    {
                        if (result.Errors.Any())
                        {
                            ModelState.AddModelError(CustomValidation.RegisterError, result.Errors.ElementAt(0).Description);
                        }
                        else
                        {
                            var errorModel = new ErrorViewModel() { Code = 500, Status = "Internal Error", Message = "Unknown error occured while registering the user." };
                            _logger.LogWarning(errorModel.Message);
                            return View("Error", errorModel);
                        }
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex.Message);
                    return View("Error", new ErrorViewModel() { Code = 500, Status = "Internal Error", Message = "Database request error occured while registering user." });
                }
            }
            return View(userDTO);
        }

        [HttpGet, AllowAnonymous]
        public IActionResult Login()
        {
            if (_signInManager.IsSignedIn(User))
                return NoContent();
            return View(new UserLoginDTO());
        }

        [HttpPost, AllowAnonymous]
        [RequestSizeLimit(500)]
        public async Task<IActionResult> Login(UserLoginDTO userLoginDTO)
        {
            if (ModelState.IsValid)
            {
                var result = await _signInManager.PasswordSignInAsync(userLoginDTO.UserName, userLoginDTO.Password, false, false);
                if (result.Succeeded)
                    return RedirectToAction("Index", "Home");
                else
                {
                    ViewData["Error"] = "LogIn failed! UserName or Password is invalid!";
                    return View(userLoginDTO);
                }
            }
            return View(userLoginDTO);
        }

        public IActionResult UserProfile(string section, int page = 1)
        {
            var currentUser = _userManager.GetCurrentUser();
            var authors = _authorManager.GetAllNonPendingAuthors();
            var genres = _genreManager.GetAll();
            var languages = _languageManager.GetAll();
            var userAssignments = _assignmentManager.GetUserAssignments(currentUser);
            var userProfileData = _userManager.GetCurrentUserProfileData();
            var allUserBooks = _bookManager.GetAllPagedUserBooks(currentUser, page);
            ViewBag.Section = section;
            return View((authors, genres, languages, userProfileData, allUserBooks, userAssignments));
        }

        [Authorize(Roles = AccessRole.Admin)]
        public IActionResult Admin(string section)
        {
            var extendAssignments = _assignmentManager.GetUnapprovedExtendedAsignments();
            ViewBag.Assignments = extendAssignments;
            ViewBag.Section = section;
            return View();
        }

        public async Task<IActionResult> LogOut()
        {
            await _signInManager.SignOutAsync();
            return View("Login");
        }

        [HttpPatch, RequestSizeLimit(1_200_000)]
        public async Task<IActionResult> AddAccountAvatar(IFormFile image)
        {
            if (ModelState.IsValid && await _userManager.UpdateUserAvatar(image))
                return NoContent();
            else
                return BadRequest();
        }

        [Authorize(Roles = AccessRole.Admin)]
        public IActionResult AdminManageRoles(int firstItem = 0, string userNameSearch = null)
        {
            var model = _userManager.GetUsers(firstItem, AppSettings.Instance.UsersPerPage, userNameSearch);
            return PartialView(model);
        }

        [Authorize(Roles = AccessRole.Admin)]
        public IActionResult AssignAdmin(string id)
        {
            return _userManager.AssignAdmin(id) ? Ok() : BadRequest();
        }

        [Authorize(Roles = AccessRole.Admin)]
        public IActionResult RemoveAdmin(string id)
        {
            return _userManager.RemoveAdmin(id) ? Ok() : BadRequest();
        }

        [HttpGet, AllowAnonymous]
        public IActionResult ForgotPassword()
        {
            return View();
        }

        [HttpPost, AllowAnonymous]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordDTO forgotPassword)
        {
            if (ModelState.IsValid)
            {
                var user = await _identityManager.FindByEmailAsync(forgotPassword.Email);
                if (user != null && await _identityManager.IsEmailConfirmedAsync(user))
                {
                    var token = await _identityManager.GeneratePasswordResetTokenAsync(user);
                    var passwordResetLink = Url.Action("ResetPassword", "Account", new { email = forgotPassword.Email, token }, Request.Scheme);
                    _userManager.SendEmail(passwordResetLink, AppSettings.Instance.PasswordResetEmailSubject, user.Email, AppSettings.Instance.EmailSettings, user.UserName);
                    ViewData["Success"] = "You'll receive an email with a link to reset your password";
                }
                else
                    ViewData["Error"] = "The provided email doesn't exist in Game Sharing System";
            }
            return View(forgotPassword);
        }

        [AllowAnonymous]
        public IActionResult ResetPassword(string token, string email)
        {
            if (string.IsNullOrEmpty(token) || string.IsNullOrEmpty(email))
                ViewData["Error"] = "Invalid token or email. Password can not be reset.";
            return View();
        }

        [HttpPost, AllowAnonymous]
        public async Task<IActionResult> ResetPassword(ResetPasswordDTO resetPassword)
        {
            if (ModelState.IsValid)
            {
                var user = await _identityManager.FindByEmailAsync(resetPassword.Email);
                if (user != null)
                {
                    var result = await _identityManager.ResetPasswordAsync(user, resetPassword.Token, resetPassword.Password);
                    if (result.Succeeded)
                        return RedirectToAction("Login");
                }
                ViewData["Error"] = "Invalid Token. Please recheck the link in the email.";
                return View(resetPassword);
            }
            else
                return View(resetPassword);
        }

        public IActionResult AdminPendingAuthor(int firstItem = 0)
        {
            var pendingAuthors = _bookManager.GetPagedPendingBooksAuthors(firstItem, AppSettings.Instance.PendingAuthorsPageSize);
            if (pendingAuthors.Count == 0)
                return StatusCode(204);
            return PartialView(pendingAuthors);
        }
    }
}

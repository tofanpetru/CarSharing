using Application.Interfaces;
using AutoMapper;
using Domain.Entities;
using Domain.Enums;
using Infrastructure.Persistance;
using Infrastructure.Repository.Implementations;
using Infrastructure.Repository.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Hosting;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Application.Manager
{
    public class UserManager : IUserManager
    {
        private readonly IMapper _mapper;
        private readonly UserStoreRepository _userStoreRepository;
        private readonly UserManager<User> _identityManager;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IHostEnvironment _hostEnvironment;
        private readonly IUserRepository _userRepository;
        private readonly IAssignmentManager _assignmentManager;
        private readonly IBookRepository _bookRepository;
        private readonly IAssignmentRepository _assignmentRepository;

        public UserManager(IMapper mapper,
                           UserManager<User> identityManager,
                           UserStoreRepository userStoreRepository,
                           IHttpContextAccessor httpContextAccessor,
                           IHostEnvironment hostEnvironment,
                           IUserRepository userRepository,
                           IAssignmentManager assignmentManager,
                           IBookRepository bookRepository,
                           IAssignmentRepository assignmentRepository)
        {
            _mapper = mapper;
            _identityManager = identityManager;
            _userStoreRepository = userStoreRepository;
            _httpContextAccessor = httpContextAccessor;
            _hostEnvironment = hostEnvironment;
            _userRepository = userRepository;
            _assignmentManager = assignmentManager;
            _bookRepository = bookRepository;
            _assignmentRepository = assignmentRepository;
        }

        public async Task<IdentityResult> AddUser(UserRegistrationDTO userDTO)
        {
            var user = _mapper.Map<User>(userDTO);
            user.LockoutEnabled = false;
            user.EmailConfirmed = true;
            var result = await _identityManager.CreateAsync(user, userDTO.Password);
            if (result.Succeeded)
            {
                await _userStoreRepository.AddToRoleAsync(user, AccessRole.User);
                _userRepository.SaveChanges();
            }

            return result;
        }

        public User GetCurrentUser()
        {
            var userId = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
            return _identityManager.Users.FirstOrDefault(i => i.Id == userId);
        }

        public UserProfileDTO GetCurrentUserProfileData()
        {
            return _mapper.Map<UserProfileDTO>(GetCurrentUser());
        }

        public async Task<bool> UpdateUserAvatar(IFormFile image)
        {
            var partialPath = Settings.GetAuthorAvatarsPath(_hostEnvironment);
            var avatarPath = await ServerUtils.UploadImage(image, partialPath);
            if (avatarPath != null)
            {
                var currentUser = GetCurrentUser();
                var userImagePath = currentUser.ProfileImagePath;
                if (userImagePath != null)
                {
                    var fullOldAvatarPath = Path.Combine(partialPath, userImagePath);
                    ServerUtils.DeleteFile(fullOldAvatarPath);
                }
                currentUser.ProfileImagePath = avatarPath;
                _userRepository.SaveChanges();
                return true;
            }
            return false;
        }

        public bool HasAssignmentsLeft(User user)
        {
            return _userRepository.GetActiveAssignments(user).Count() < AppSettings.Instance.MaxAllowedAssignments;
        }

        public AssignResultDTO AssignBookToCurrentUser(int bookId)
        {
            var result = new AssignResultDTO() { Error = true };
            var user = GetCurrentUser();
            var book = _bookRepository.Get(bookId);

            if (book == null)
            {
                result.Message = "The book that is tried to be assigned doesn't exist.";
            }
            else if (!book.IsAvailable)
            {
                result.Message = "This book is not available at the moment.";
            }
            else if (!HasAssignmentsLeft(user))
            {
                result.Message = "You can't have more than " + AppSettings.Instance.MaxAllowedAssignments.ToString() + " active assignments.";
            }
            else
            {
                result.Error = false;
                result.Message = "Success";
                result.Date = _assignmentManager.AddAssignment(book, user).Value.ToString(AppSettings.Instance.DateDisplayFormat);
            }
            return result;
        }

        public bool HasQueueAssignmentsLeft(User user)
        {
            return _userRepository.GetUsersBooksQueue(user).Count < AppSettings.Instance.MaxAllowedBooksInUsersQueue;
        }

        public AssignResultDTO AssignBookToUserQueue(int bookId)
        {
            var result = new AssignResultDTO();
            var user = GetCurrentUser();

            if (HasQueueAssignmentsLeft(user) && !_assignmentManager.HasPendingExtend(bookId))
            {
                var assignmentBook = _bookRepository.Get(bookId);
                var startDate = _assignmentManager.AddBookToQueue(user, assignmentBook);
                result.Message = "Success";
                result.Date = startDate.Value.ToString(AppSettings.Instance.DateDisplayFormat);
            }
            return result;
        }

        public bool IsBookAssignedToCurrentUser(int bookId)
        {
            var dbBook = _bookRepository.Get(bookId);
            return (dbBook != null) && _assignmentRepository.IsBookAssignedToUser(GetCurrentUser(), dbBook);
        }

        public ICollection<UserManageDTO> GetUsers(int firstItem, int usersPerPage, string userNameSearch)
        {
            var users = _userRepository.GetUsers(firstItem, usersPerPage, userNameSearch);
            return _mapper.Map<ICollection<UserManageDTO>>(users);
        }

        public bool AssignAdmin(string id)
        {
            var user = _userRepository.Get(id);
            if (user != null)
            {
                _userStoreRepository.AddToRoleAsync(user, AccessRole.Admin).GetAwaiter().GetResult();
                _userRepository.SaveChanges();
                return true;
            }
            return false;
        }

        public bool RemoveAdmin(string id)
        {
            var user = _userRepository.Get(id);
            if (user != null && !_userStoreRepository.IsInRoleAsync(user, AccessRole.SuperAdmin).GetAwaiter().GetResult())
            {
                _userStoreRepository.RemoveFromRoleAsync(user, AccessRole.Admin).GetAwaiter().GetResult();
                _userRepository.SaveChanges();
                return true;
            }
            return false;
        }

        public void SendEmail(string passwordResetLink, string subject, string receiverEmail, EmailSettings emailSettings, string userName)
        {
            var htmlString = ServerUtils.GetForgotPasswordBody(passwordResetLink, userName);
            ServerUtils.SendEmail(htmlString, subject, receiverEmail, emailSettings);
        }
    }
}

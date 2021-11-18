using Domain.Entities;
using Domain.Enums;
using Infrastructure.Persistance;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface IUserManager
    {
        public Task<IdentityResult> AddUser(UserRegistrationDTO userDTO);
        public User GetCurrentUser();
        public UserProfileDTO GetCurrentUserProfileData();
        public Task<bool> UpdateUserAvatar(IFormFile image);
        bool HasAssignmentsLeft(User user);
        AssignResultDTO AssignBookToCurrentUser(int bookId);
        bool HasQueueAssignmentsLeft(User user);
        AssignResultDTO AssignBookToUserQueue(int bookId);
        bool IsBookAssignedToCurrentUser(int bookId);
        ICollection<UserManageDTO> GetUsers(int firstItem, int usersPerPage, string userNameSearch);
        bool AssignAdmin(string id);
        bool RemoveAdmin(string id);
        public void SendEmail(string passwordResetLink, string subject, string receiverEmail, EmailSettings emailSettings, string userName);
    }
}

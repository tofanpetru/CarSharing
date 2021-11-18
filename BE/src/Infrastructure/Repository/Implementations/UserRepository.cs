using Infrastructure.Persistance;
using Infrastructure.Repository.Abstract;
using Infrastructure.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Infrastructure.Repository.Implementations
{
    public class UserRepository : AbstractRepository<User>, IUserRepository
    {
        public UserRepository(BookSharingContext context) : base(context) { }

        public IEnumerable<Assignment> GetActiveAssignments(User user)
        {
            return DataBaseContext.Assignments.Where(a => a.IsActive && a.Assignee == user);
        }

        public ICollection<User> GetUsers(int firstItem, int usersPerPage, string userNameSearch)
        {
            return String.IsNullOrWhiteSpace(userNameSearch) ?
                    DataBaseContext.Users.Include(u => u.UserRoles).ThenInclude(ur => ur.Role).OrderByDescending(u => u.UserRoles.Count).ThenBy(u => u.UserName).Skip(firstItem).Take(usersPerPage).ToList() :
                    DataBaseContext.Users.Where(u => u.UserName.ToLower().Contains(userNameSearch.ToLower())).Include(u => u.UserRoles).ThenInclude(ur => ur.Role).OrderByDescending(u => u.UserRoles.Count).ThenBy(u => u.UserName).Skip(firstItem).Take(usersPerPage).ToList();
        }

        public List<Assignment> GetUsersBooksQueue(User user)
        {
            return DataBaseContext.Assignments.Include(x => x.Assignee).Where(x => x.IsActive == false && x.Assignee == user).ToList();
        }
    }
}

using Infrastructure.Persistance;
using System.Collections.Generic;

namespace Infrastructure.Repository.Interfaces
{
    public interface IUserRepository : IRepository<User>
    {
        public IEnumerable<Assignment> GetActiveAssignments(User user);
        List<Assignment> GetUsersBooksQueue(User user);
        ICollection<User> GetUsers(int firstItem, int usersPerPage, string userNameSearch);
    }
}

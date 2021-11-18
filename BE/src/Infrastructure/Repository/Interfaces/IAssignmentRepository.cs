using Infrastructure.Persistance;
using System.Collections.Generic;
using System;

namespace Infrastructure.Repository.Interfaces
{
    public interface IAssignmentRepository : IRepository<Assignment>
    {
        DateTime? GetLastQueueAssignmentEndDate(int bookId);
        DateTime? GetUserLastQueueAssignmentEndDate(User user);
        bool IsBookAssignedToUser(User user, Book book);
        public IEnumerable<Assignment> GetUserAssignments(string userId);
        bool IsAssignedBookQueued(int assignmentId);
        Assignment GetWithExtends(int assignmentId);
        bool HasPendingExtend(int bookId);
        public IEnumerable<Assignment> GetUnapprovedExtendedAssignments();
    }
}

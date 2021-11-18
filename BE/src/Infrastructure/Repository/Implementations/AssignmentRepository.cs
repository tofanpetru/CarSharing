using Infrastructure.Persistance;
using Infrastructure.Repository.Abstract;
using Infrastructure.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Infrastructure.Repository.Implementations
{
    public class AssignmentRepository : AbstractRepository<Assignment>, IAssignmentRepository
    {
        public AssignmentRepository(BookSharingContext context) : base(context) { }

        public DateTime? GetLastQueueAssignmentEndDate(int bookId)
        {
            return DataBaseContext.Assignments.Include(x => x.Book).Where(x => x.Book.Id == bookId).Max(x => (DateTime?)x.EndDate);
        }

        public DateTime? GetUserLastQueueAssignmentEndDate(User user)
        {
            return DataBaseContext.Assignments.Where(x => x.Assignee == user && x.IsActive == false).Max(x => (DateTime?)x.EndDate);
        }

        public bool IsBookAssignedToUser(User user, Book book)
        {
            return DataBaseContext.Assignments.Where(x => x.Assignee == user && x.Book == book && x.IsActive == true).Any();
        }

        public IEnumerable<Assignment> GetUserAssignments(string userId)
        {
            return DataBaseContext.Assignments.Where(i => i.Assignee.Id == userId).Include(i => i.Book).Include(i => i.Extend);
        }

        public bool IsAssignedBookQueued(int assignmentId)
        {
            return DataBaseContext.Assignments.Where(i => i.Id == assignmentId)
                                              .Include(i => i.Book)
                                              .ThenInclude(b => b.Assignments)
                                              .FirstOrDefault().Book.Assignments
                                              .Any(a => !a.IsActive);
        }

        public Assignment GetWithExtends(int id)
        {
            return DataBaseContext.Assignments.Where(i => i.Id == id).Include(i => i.Extend).Include(i => i.Assignee).Include(i => i.Book).FirstOrDefault();
        }

        public bool HasPendingExtend(int bookId)
        {
            return DataBaseContext.Assignments.Where(a => a.Book.Id == bookId).Include(a => a.Extend).Any(a => a.Extend != null && !a.Extend.Approved);
        }

        public IEnumerable<Assignment> GetUnapprovedExtendedAssignments()
        {
            return DataBaseContext.Assignments.Where(a => a.ExtendId != null && a.Extend.Approved==false).Include(a => a.Extend).Include(a=>a.Book).Include(a=>a.Assignee);
        }
    }
}

using Domain.Entities;
using Infrastructure.Persistance;
using System;
using System.Collections.Generic;

namespace Application.Interfaces
{
    public interface IAssignmentManager
    {
        DateTime? AddAssignment(Book book, User user);
        DateTime? AddBookToQueue(User user, Book book);
        DateTime? GetUserQueueAssignmentsLastEndDate(User user);
        public IEnumerable<UserAssignmentsDTO> GetUserAssignments(User user);
        bool HasPendingExtend(int bookId);
        public IEnumerable<ExtendAssignmentRqDTO> GetUnapprovedExtendedAsignments();
    }
}

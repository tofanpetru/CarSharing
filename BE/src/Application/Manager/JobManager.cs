using Application.Interfaces;
using Infrastructure.Repository.Interfaces;
using System;

namespace Application.Manager
{
    public class JobManager : IJobManager
    {
        private readonly IBookRepository _bookRepository;
        private readonly IAssignmentRepository _assignmentRepository;
        private readonly IExtendRepository _extendRepository;
        private readonly INotificationManager _notificationManager;

        public JobManager(IBookRepository bookRepository, IAssignmentRepository assignmentRepository, IExtendRepository extendRepository, INotificationManager notificationManager)
        {
            _bookRepository = bookRepository;
            _assignmentRepository = assignmentRepository;
            _extendRepository = extendRepository;
            _notificationManager = notificationManager;
        }

        public void CheckBooksAvailability()
        {
            var books = _bookRepository.GetValidBooksWithAssignments();
            foreach (var book in books)
            {
                bool hasActiveAssignments = false;
                foreach (var assignment in book.Assignments)
                {
                    if (DateTime.Compare(assignment.StartDate.Date, DateTime.Today.Date) == 0)
                    {
                        assignment.IsActive = true;
                        hasActiveAssignments = true;
                        _notificationManager.AddAssignNotification(String.Format(AppSettings.Instance.Notifications.User.BookAssigned, book.Title),
                                       AppSettings.Instance.Notifications.User.BookAssignedActionPath, assignment.Assignee);
                    }
                    else if (DateTime.Compare(assignment.EndDate.Date, DateTime.Today.Date) < 0)
                    {
                        if (assignment.Extend != null)
                        {
                            _extendRepository.Remove(assignment.Extend);
                        }
                        _assignmentRepository.Remove(assignment);
                    }
                    else
                    {
                        hasActiveAssignments = true;
                    }
                }
                if (!hasActiveAssignments)
                {
                    book.IsAvailable = true;
                }
            }
            _bookRepository.SaveChanges();
        }
    }
}

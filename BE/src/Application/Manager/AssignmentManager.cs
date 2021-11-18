using Application.Interfaces;
using AutoMapper;
using Domain.Entities;
using Infrastructure.Persistance;
using Infrastructure.Repository.Interfaces;
using System;
using System.Collections.Generic;

namespace Application.Manager
{
    public class AssignmentManager : IAssignmentManager
    {
        private readonly IAssignmentRepository _assignmentRepository;
        private readonly IMapper _mapper;

        public AssignmentManager(IAssignmentRepository assignmentRepository, IMapper mapper)
        {
            _assignmentRepository = assignmentRepository;
            _mapper = mapper;
        }

        public DateTime? AddAssignment(Book book, User user)
        {
            var assignment = new Assignment()
            {
                StartDate = DateTime.Today,
                EndDate = DateTime.Today.AddDays(AppSettings.Instance.AssignmentDays),
                Assignee = user,
                Book = book,
                IsActive = true
            };
            book.IsAvailable = false;
            _assignmentRepository.Add(assignment);
            _assignmentRepository.SaveChanges();
            return assignment.EndDate;
        }

        public DateTime? AddBookToQueue(User user, Book book)
        {
            DateTime? startDate;
            if (book.IsAvailable == true)
            {
                startDate = _assignmentRepository.GetUserLastQueueAssignmentEndDate(user);
            }
            else
            {
                startDate = _assignmentRepository.GetLastQueueAssignmentEndDate(book.Id);
            }

            if (startDate != null)
            {
                var endDate = startDate.Value.AddDays(AppSettings.Instance.AssignmentDays);
                var bookQueueAssignment = new Assignment { Assignee = user, Book = book, EndDate = endDate, StartDate = startDate.Value };
                _assignmentRepository.Add(bookQueueAssignment);
                _assignmentRepository.SaveChanges();
            }
            return startDate;
        }

        public DateTime? GetUserQueueAssignmentsLastEndDate(User user)
        {
            return _assignmentRepository.GetUserLastQueueAssignmentEndDate(user);
        }

        public IEnumerable<UserAssignmentsDTO> GetUserAssignments(User user)
        {
            var model = _mapper.Map<ICollection<UserAssignmentsDTO>>(_assignmentRepository.GetUserAssignments(user.Id));
            foreach (var assignment in model)
                assignment.CanBeExtended = !_assignmentRepository.IsAssignedBookQueued(assignment.Id);
            return model;
        }

        public bool HasPendingExtend(int bookId)
        {
            return _assignmentRepository.HasPendingExtend(bookId);
        }

        public IEnumerable<ExtendAssignmentRqDTO> GetUnapprovedExtendedAsignments()
        {
            return _mapper.Map<ICollection<ExtendAssignmentRqDTO>>(_assignmentRepository.GetUnapprovedExtendedAssignments());
        }
    }
}

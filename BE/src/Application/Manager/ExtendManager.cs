using Application.Interfaces;
using AutoMapper;
using Domain.Entities;
using Infrastructure.Persistance;
using Infrastructure.Repository.Interfaces;
using System;

namespace Application.Manager
{
    public class ExtendManager : IExtendManager
    {
        private readonly IMapper _mapper;
        private readonly IAssignmentRepository _assignmentRepository;
        private readonly INotificationManager _notificationManager;
        private readonly IExtendRepository _extendRepository;
        public ExtendManager(IExtendRepository extendRepository, IMapper mapper, IAssignmentRepository assignmentRepository, INotificationManager notificationManager)
        {
            _extendRepository = extendRepository;
            _mapper = mapper;
            _assignmentRepository = assignmentRepository;
            _notificationManager = notificationManager;
        }

        public bool ExtendAssignment(ExtendAssignmentDTO extendAssignmentDTO)
        {
            var assignment = _assignmentRepository.GetWithExtends(extendAssignmentDTO.AssignmentId);
            if (assignment != null && !_assignmentRepository.IsAssignedBookQueued(assignment.Id) && assignment.Extend == null)
            {
                var extendedEndDate = extendAssignmentDTO.EndDate.Date;

                if ((DateTime.Compare(DateTime.Today.Date, extendedEndDate) <= 0) &&
                    (DateTime.Compare(extendedEndDate, assignment.EndDate.AddDays(AppSettings.Instance.MaxExtendDays).Date) <= 0))
                {
                    var extend = _mapper.Map<Extend>(extendAssignmentDTO);
                    _extendRepository.Add(extend);
                    assignment.Extend = extend;
                    assignment.Extend.EndDate = extendedEndDate;
                    _assignmentRepository.SaveChanges();
                    _notificationManager.AddAdminNotification(String.Format(AppSettings.Instance.Notifications.Admin.PendingAssignment, assignment.Assignee.UserName, assignment.Book.Title),
                                                              AppSettings.Instance.Notifications.Admin.PendingAssignmentActionPath, extendId: extend.Id );
                    return true;
                }
            }
            return false;
        }

        public void RejectExtend(int id)
        {
            var assignment = _assignmentRepository.GetWithExtends(id);
            if (assignment != null)
            {
                assignment.ExtendId = null;
                _extendRepository.Remove(assignment.Extend);
                _assignmentRepository.SaveChanges();
            }
        }

        public void ApproveExtend(int id)
        {
            var assignment = _assignmentRepository.GetWithExtends(id);
            assignment.Extend.Approved = true;
            assignment.EndDate = assignment.Extend.EndDate;
            _notificationManager.AddExtendNotification(String.Format(AppSettings.Instance.Notifications.User.AssignmentExtended, assignment.Book.Title),
                                                       AppSettings.Instance.Notifications.User.AssignmentExtendedActionPath, assignment.Assignee);
            _notificationManager.RemoveBy(assignment.Extend);
            _assignmentRepository.SaveChanges();
        }
    }
}

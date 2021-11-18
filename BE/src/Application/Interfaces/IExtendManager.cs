using Domain.Entities;

namespace Application.Interfaces
{
    public interface IExtendManager
    {
        public bool ExtendAssignment(ExtendAssignmentDTO extendAssignmentDTO);
        public void ApproveExtend(int id);
        public void RejectExtend(int id);
    }
}

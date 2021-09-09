using Domain.Entities;
using System.Collections.Generic;

namespace Application.Interfaces
{
    public interface ICarManager
    {
        public ICollection<AllCarsDTO> GetAll();
    }
}

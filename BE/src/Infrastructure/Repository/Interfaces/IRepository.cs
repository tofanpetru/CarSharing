using System.Collections.Generic;

namespace Infrastructure.Repository.Interfaces
{
    public interface IRepository<TModel> where TModel : class
    {
        TModel Get<T>(T id);
        ICollection<TModel> GetAll();
        void Add(TModel entity);
        void Remove(TModel entity);
        void SaveChanges();
    }
}

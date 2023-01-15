using System.Collections.Generic;

namespace Infrastructure.Repository.Interfaces
{
    public interface IRepository<TModel> where TModel : class
    {
        void Add(TModel entity);
        TModel Get<T>(T id);
        ICollection<TModel> GetAll();
        void Remove(TModel entity);
        void SaveChanges();
    }
}

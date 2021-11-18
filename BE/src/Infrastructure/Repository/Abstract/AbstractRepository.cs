using Infrastructure.Repository.Interfaces;
using System.Collections.Generic;
using System.Linq;

namespace Infrastructure.Repository.Abstract
{
    public abstract class AbstractRepository<TModel> : IRepository<TModel> where TModel : class
    {
        protected readonly BookSharingContext DataBaseContext;

        public AbstractRepository(BookSharingContext context)
        {
            this.DataBaseContext = context;
        }

        public void Add(TModel entity)
        {
            DataBaseContext.Set<TModel>().Add(entity);
        }

        public TModel Get<T>(T id)
        {
            return DataBaseContext.Set<TModel>().Find(id);
        }

        public ICollection<TModel> GetAll()
        {
            return DataBaseContext.Set<TModel>().ToList();
        }

        public void Remove(TModel entity)
        {
            DataBaseContext.Set<TModel>().Remove(entity);
        }

        public void SaveChanges()
        {
            DataBaseContext.SaveChanges();
        }
    }
}

using Infrastructure.Repository.Interfaces;
using System.Collections.Generic;
using System.Linq;

namespace Infrastructure.Repository.Abstract
{
    public abstract class AbstractRepository<TModel> : IRepository<TModel> where TModel : class
    {
        protected readonly CarSharingContext CarSharingContext;

        public AbstractRepository(CarSharingContext context)
        {
            this.CarSharingContext = context;
        }

        public void Add(TModel entity)
        {
            CarSharingContext.Set<TModel>().Add(entity);
        }

        public TModel Get<T>(T id)
        {
            return CarSharingContext.Set<TModel>().Find(id);
        }

        public ICollection<TModel> GetAll()
        {
            return CarSharingContext.Set<TModel>().ToList();
        }

        public void Remove(TModel entity)
        {
            CarSharingContext.Set<TModel>().Remove(entity);
        }

        public void SaveChanges()
        {
            CarSharingContext.SaveChanges();
        }
    }
}

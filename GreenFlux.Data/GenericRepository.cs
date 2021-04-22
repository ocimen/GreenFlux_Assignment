using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GreenFlux.Data
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        protected readonly GreenFluxContext Context;

        public GenericRepository(GreenFluxContext context)
        {
            Context = context;
        }

        public int Add(T entity)
        {
            Context.Set<T>().Add(entity);
            return 1;
        }

        public IEnumerable<T> GetAll()
        {
            return Context.Set<T>().ToList();
        }

        public async Task<T> GetByIdAsync(Guid id)
        {
            return await Context.Set<T>().FindAsync(id);
        }

        public T Remove(T entity)
        {
           var result = Context.Set<T>().Remove(entity);
           return result.Entity;
        }

        public async Task<int> SaveChangesAsync()
        {
            return await Context.SaveChangesAsync();
        }
    }
}

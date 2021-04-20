using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GreenFlux.Data
{
    public interface IGenericRepository<T> where T : class
    {
        Task<T> GetByIdAsync(Guid id);
        IEnumerable<T> GetAll();
        int Add(T entity);
        T Remove(T entity);
        Task<int> SaveChangesAsync();
    }
}

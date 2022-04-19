using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace core.Repositories.Abstractions
{
    public interface IRepository<T>
    {
        public Task<IEnumerable<T>> GetAll();
        public Task<T> Get(int id);
        public Task Create(T entity);
        public Task Update(int id, T entity);
        public Task Delete(int id);
    }

    public class EntityNotFoundException : Exception
    {
        public EntityNotFoundException() : base("The given entity was not found")
        {
        }
    }
}

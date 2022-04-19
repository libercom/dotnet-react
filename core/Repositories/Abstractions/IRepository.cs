using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace core.Repositories.Abstractions
{
    public interface IRepository<T>
    {
        public IList<T> GetAll();
        public IList<T> Get(int id);
        public void Create(T entity);
        public void Update(T entity);
        public void Delete(T entity);
    }
}

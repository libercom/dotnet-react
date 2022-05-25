namespace core.Repositories.Abstractions
{
    public interface IRepository<T, S>
    {
        public abstract Task<IEnumerable<T>> GetAll();
        public Task<T> Get(int id);
        public Task Create(S entity);
        public Task Update(int id, S entity);
        public Task Delete(int id);
    }
}

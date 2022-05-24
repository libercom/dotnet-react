using core.Dtos;
using core.Models;

namespace core.Repositories.Abstractions
{
    public interface IOrdersRepository: IRepository<OrderDto, OrderCreationDto>
    {
        public Task<PagedResponse> GetPagedData(PagedRequest request);
    }
}

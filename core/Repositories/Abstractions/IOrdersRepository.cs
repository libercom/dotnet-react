using common.Dtos;
using common.Models;

namespace core.Repositories.Abstractions
{
    public interface IOrdersRepository: IRepository<OrderDto, OrderCreationDto>
    {
        public Task<PagedResponse> GetPagedData(PagedRequest request);
    }
}

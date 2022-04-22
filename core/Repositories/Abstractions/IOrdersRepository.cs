using core.Dtos;

namespace core.Repositories.Abstractions
{
    public interface IOrdersRepository: IRepository<OrderDto, OrderCreationDto>
    {
    }
}

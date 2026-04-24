using AutoriaStore.Domain.Entities;
using AutoriaStore.Domain.Interfaces.Repositories;
using AutoriaStore.Infrastructure.Context;

namespace AutoriaStore.Infrastructure.Repositories;

public class OrderRepository(ApplicationDbContext context) : BaseRepository<Order>(context), IOrderRepository
{
}

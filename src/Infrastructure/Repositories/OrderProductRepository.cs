using AutoriaStore.Domain.Entities;
using AutoriaStore.Domain.Interfaces.Repositories;
using AutoriaStore.Infrastructure.Context;

namespace AutoriaStore.Infrastructure.Repositories;

public class OrderProductRepository(ApplicationDbContext context) : BaseRepository<OrderProduct>(context), IOrderProductRepository
{
    
}
using AutoriaStore.Domain.Entities;
using AutoriaStore.Domain.Interfaces;
using AutoriaStore.Infrastructure.Context;

namespace AutoriaStore.Infrastructure.Repositories;

public class OrderProductRepository(ApplicationDbContext context) : BaseRepository<OrderProduct>(context), IOrderProductRepository
{
    
}
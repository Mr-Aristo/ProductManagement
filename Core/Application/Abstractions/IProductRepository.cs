using Domain.Entities;

namespace Application.Abstractions;

public interface IProductRepository:IRepository<Product>
{
     Task<Product> CreateProduct (Product product);
}

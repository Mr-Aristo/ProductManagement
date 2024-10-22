using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Abstractions;
using Domain.Entities;
using Infrastructure.Context;

namespace Infrastructure.Concrete;

public class ProductRepository : Repository<Product>,IProductRepository
{
    public ProductRepository(ProductContext context):base(context)
    {
        
    }

    public Task<Product> CreateProduct(Product product)
    {
        throw new NotImplementedException();
    }
}

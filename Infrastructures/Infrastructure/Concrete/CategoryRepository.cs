
using Application.Abstractions;
using Domain.Entities;
using Infrastructure.Context;

namespace Infrastructure.Concrete;

public class CategoryRepository : Repository<Category>, ICategoryRepository
{
    public CategoryRepository(ProductContext context) : base(context)
    {

    }
}

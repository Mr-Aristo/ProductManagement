using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Abstractions;
using Infrastructure.Context;

namespace Infrastructure.Concrete;

public class UnitOfWork : IUnitOfWork
{
    private readonly ProductContext context;
    private ProductRepository productRepository;
    private CategoryRepository categoryRepository;
    public UnitOfWork(ProductContext context)
    {
        this.context = context;
    }

    public IProductRepository Product
    {
        get => productRepository ??= new ProductRepository(context);
        set => productRepository = (ProductRepository)value;
    }
    public ICategoryRepository Category
    {
        get => categoryRepository ??= new CategoryRepository(context);
        set => categoryRepository = (CategoryRepository)value;
    }

    public void Dispose()
    {
        context.Dispose();
    }

    public async Task<int> SaveAsync()
    {
        return await context.SaveChangesAsync();
    }
}

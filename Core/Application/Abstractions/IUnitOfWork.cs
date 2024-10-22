using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Abstractions;

public interface IUnitOfWork : IDisposable
{
    IProductRepository Product { get; set; }
    ICategoryRepository Category { get; set; }

    Task<int> SaveAsync();
}

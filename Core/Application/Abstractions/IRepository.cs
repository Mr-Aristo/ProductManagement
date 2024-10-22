using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Domain.Entities.Common;

namespace Application.Abstractions;

public interface IRepository<T> where T : BaseEntity
{
    /*Write*/
    Task<bool> AddAsync(T model);

    Task<bool> AddRangeAsync(List<T> datas);
    bool Remove(T model);
    Task<bool> RemoveAsync(Guid id);

    bool RemoveRange(List<T> datas);

    bool Update(T model);



    /*Read*/
    IQueryable<T> GetAll(bool tracking = true);
    Task<T> GetByIdAsync(Guid id, bool tracking = true);
    IQueryable<T> GetWhere(Expression<Func<T, bool>> expression, bool tracking = true);

    Task<int> SaveAsync();
}

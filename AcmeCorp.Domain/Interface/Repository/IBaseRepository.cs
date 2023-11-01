using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace AcmeCorp.Domain.Interface.Repository
{
    public interface IBaseRepository<T> where T : class
    {
        Task<IEnumerable<T>> GetAll();
        Task Add(T entity);
        Task<T> GetById(int id);
        Task<IEnumerable<T>> GetByData(Expression<Func<T, bool>> express);
        void Delete(T model);
        Task Update(T entity);
    }
}

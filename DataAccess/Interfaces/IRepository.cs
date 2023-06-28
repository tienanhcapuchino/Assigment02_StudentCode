using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Interfaces
{
    public interface IRepository<T>
    {
        Task <T> GetById(int id);
        Task<List<T>> GetAll();
        Task<bool> Update(T entity, bool saveChange = true);
        Task<bool> Delete(T entity, bool saveChange = true);
        Task<bool> Add(T entity, bool saveChange = true);
    }
}

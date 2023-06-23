using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Interfaces
{
    public interface IRepository<T>
    {
        T GetById(int id);
        List<T> GetAll();
        bool Update(T entity, bool saveChange = true);
        bool Delete(T entity, bool saveChange = true);
        bool Add(T entity, bool saveChange = true);
    }
}

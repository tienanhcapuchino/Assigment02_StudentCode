using BussinessObject.Entities;
using BussinessObject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Interfaces
{
    public interface IUserService
    {
        Task<APIResponeModel> Login(UserLoginModel model);
        Task<APIResponeModel> Register(UserRegisterModel model);
    }
}

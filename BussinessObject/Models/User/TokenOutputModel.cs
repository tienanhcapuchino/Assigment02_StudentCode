using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BussinessObject.Models.User
{
    public class TokenOutputModel
    {
        public string Email { get; set; }
        public string Username { get; set; }
        public long ExpiredTime { get; set; }
        public string RoleName { get; set; }
        public string UserId { get; set; }
    }
}

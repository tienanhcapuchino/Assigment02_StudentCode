using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BussinessObject.Models
{
    public class APIResponeModel
    {
        public int Code { get; set; }
        public string Message { get; set; }
        public bool IsSuccess { get; set; }
        public object Data { get; set; }
    }
    public enum MethodAPI : byte
    {
        GET = 0,
        POST = 1,
        PUT = 2,
        DELETE = 3,
    }
}

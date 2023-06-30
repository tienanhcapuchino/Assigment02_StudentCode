using BussinessObject.Models.User;

namespace EStoreWeb.Services
{
    public interface ICommonService
    {
        Task<TokenOutputModel> GetTokenData();
    }
}

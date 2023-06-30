using BussinessObject.Models.User;
using System.IdentityModel.Tokens.Jwt;

namespace EStoreWeb.Services
{
    public class CommonService : ICommonService
    {
        private readonly IHttpContextAccessor _contextAccessor;
        public CommonService(IHttpContextAccessor contextAccessor)
        {
            _contextAccessor = contextAccessor;
        }
        public async Task<TokenOutputModel> GetTokenData()
        {
            var token = _contextAccessor.HttpContext.Request.Cookies["token"];
            var tokenHandler = new JwtSecurityTokenHandler();
            if (token != null)
            {
                var jwtToken = tokenHandler.ReadToken(token) as JwtSecurityToken;
                if (jwtToken != null)
                {
                    var claims = jwtToken.Claims.ToList();
                    var expTime = claims.Where(c => c.Type.Equals("exp")).FirstOrDefault().Value;
                    var roleClaim = claims.Where(c => c.Type.Equals("role")).FirstOrDefault().Value;
                    var claimMail = claims.Where(c => c.Type.Equals("email")).FirstOrDefault().Value;
                    long expDate = long.Parse(expTime);
                    TokenOutputModel result = new TokenOutputModel()
                    {
                        Email = claimMail,
                        ExpiredTime = expDate,
                        RoleName = roleClaim,
                    };
                    return await Task.FromResult(result);
                }
            }
            return null;
        }
    }
}

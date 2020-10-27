using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SSCMS.Models;

namespace SSCMS.Login.Controllers
{
    public partial class LoginController
    {
        [HttpGet, Route(Route)]
        public async Task<ActionResult<GetResult>> Get()
        {
            User user = null;
            string token = null;

            if (_authManager.IsUser)
            {
                user = await _authManager.GetUserAsync();
                token = _authManager.AuthenticateUser(user, true);
            }

            return new GetResult
            {
                User = user,
                Token = token
            };
        }
    }
}

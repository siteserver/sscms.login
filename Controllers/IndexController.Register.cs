using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SSCMS.Dto;
using SSCMS.Models;
using SSCMS.Utils;

namespace SSCMS.Login.Controllers
{
    public partial class IndexController
    {
        [HttpPost, Route(RouteRegister)]
        public async Task<ActionResult<BoolResult>> Register([FromBody] RegisterRequest request)
        {
            var user = new User
            {
                UserName = request.UserName,
                DisplayName = request.DisplayName,
                Email = request.Email,
                Mobile = request.Mobile
            };

            var config = await _configRepository.GetAsync();
            var (newUser, errorMessage) =
                await _userRepository.InsertAsync(user, request.Password, config.IsUserRegistrationChecked, PageUtils.GetIpAddress(Request));
            if (newUser == null)
            {
                return this.Error(errorMessage);
            }

            return new BoolResult
            {
                Value = true
            };
        }
    }
}

using BitirmeEntity.Models;
using BitirmeService.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BitirmeWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        readonly IAuthService authService;

        public AuthController(IAuthService authService)
        {
            this.authService = authService;
        }

        [HttpPost("LoginUser")]
        [AllowAnonymous]
        public async Task<ActionResult<UserLoginResponse>> LoginUserAsync([FromBody] UserLoginRequest request)
        {
            var result = await authService.LoginUserAsync(request);

            return result;
        }

        [HttpGet]
        [Authorize]
        [Route("[action]")]
        public bool IsLogged()
        {
            return true;
        }

        [HttpPost]
        [AllowAnonymous]
        [Route("[action]")]
        public bool RegisterUser([FromBody]UserLoginRequest request)
        {
            var result = authService.RegisterUser(request);
            return result;
        }
    }
}

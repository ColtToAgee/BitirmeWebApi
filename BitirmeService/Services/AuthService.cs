using BitirmeEntity.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BitirmeService.Services
{
    public class AuthService : IAuthService
    {
        readonly ITokenService tokenService;

        public AuthService(ITokenService tokenService)
        {
            this.tokenService = tokenService;
        }

        public async Task<UserLoginResponse> LoginUserAsync(UserLoginRequest request)
        {
            UserLoginResponse response = new();

            if (string.IsNullOrEmpty(request.Username) || string.IsNullOrEmpty(request.Password))
            {
                throw new ArgumentNullException(nameof(request));
            }
            var isLogged = false;
            using (var db = new DbService())
            {
                isLogged = db.FirstOrDefault<UserLoginRequest>($"{nameof(UserLoginRequest.Username)} = '{request.Username}' and {nameof(UserLoginRequest.Password)} = '{request.Password}'") == null ? false : true;
            }
            if (isLogged)
            {
                var generatedTokenInformation = await tokenService.GenerateToken(new GenerateTokenRequest { Username = request.Username });

                response.AuthenticateResult = true;
                response.AuthToken = generatedTokenInformation.Token;
                response.AccessTokenExpireDate = generatedTokenInformation.TokenExpireDate;
                response.Username = request.Username;
                response.Password = request.Password;
                response.Email = request.Email;
                response.PhoneNumber = request.PhoneNumber;
            }
            else
            {
                response.AuthenticateResult = false;
            }
            return response;
        }

        public bool RegisterUser(UserLoginRequest request)
        {
            bool response = false;
            if (string.IsNullOrEmpty(request.Username) || string.IsNullOrEmpty(request.Password))
            {
                return response;
            }
            using (var db = new DbService())
            {
                var dbUser = db.FirstOrDefault<UserLoginRequest>($"{nameof(UserLoginRequest.Username)} = '{request.Username}' and {nameof(UserLoginRequest.Password)} = '{request.Password}'");
                if (dbUser != null)
                {
                    dbUser.Username = request.Username;
                    dbUser.Password = request.Password;
                    dbUser.PhoneNumber = request.PhoneNumber;
                    dbUser.Email = request.Email;
                }
                db.AddOrUpdateEntity<UserLoginRequest>(dbUser);
                response = true;
            }
            
            return response;
        }
    }
}

using AlunosApi.Models;
using AlunosApi.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace AlunosApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly IAuthenticate _authentication;

        public AccountController(IConfiguration configuration, IAuthenticate authenticate)
        {
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
            _authentication = authenticate ?? throw new ArgumentNullException(nameof(authenticate));
        }

        [HttpPost("CreateUser")]
        public async Task<ActionResult<UserToken>> CreateUser([FromBody] RegisterModel model)
        {
            if (model.Password.Equals(model.ConfirmPassword) is false)
            {
                ModelState.AddModelError(nameof(model.ConfirmPassword), "Senhas não conferem");
                return BadRequest(ModelState);
            }

            var result = await _authentication.RegisterUser(model.Email, model.Password);
            if (result)
            {
                return Ok($"Usuário {model.Email} criado com sucesso.");
            }
            else
            {
                ModelState.AddModelError(nameof(CreateUser), "Registro inválido");
                return BadRequest(ModelState);
            }
        }

        [HttpPost("LoginUser")]
        public async Task<ActionResult<UserToken>> Login([FromBody] LoginModel model)
        {
            var result = await _authentication.AuthenticateUser(model.Email, model.Password);
            if (result)
            {
                return GenerateToken(model);
            }
            else
            {
                ModelState.AddModelError(nameof(Login), "Login inválido.");
                return BadRequest(ModelState);
            }
        }

        private ActionResult<UserToken> GenerateToken(LoginModel userInfo)
        {
            var claims = new[]
            {
                new Claim(ClaimTypes.Email, userInfo.Email),
                new Claim("MeuToken", "Token JWT da aplicação"),
                new Claim(JwtRegisteredClaimNames.Jti,Guid.NewGuid().ToString())
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var expiration = DateTime.Now.AddMinutes(20);

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: expiration,
                signingCredentials: credentials);

            return new UserToken()
            {
                Token = new JwtSecurityTokenHandler().WriteToken(token),
                Expiration = expiration
            };
        }
    }
}

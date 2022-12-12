using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
//using models
using ApiControleDespesas.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace ApiControleDespesas.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AutorizaController : ControllerBase
    {
        //usermanager
        private readonly UserManager<IdentityUser> _userManager;
        //signmanager
        private readonly SignInManager<IdentityUser> _signInManager;
        //configuration
        private readonly IConfiguration _configuration;


        //construtor
        public AutorizaController(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager,
            IConfiguration configuration)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _configuration = configuration;
        }
     
        

        //metodo para verificar se api ta atendendo
        [HttpGet]
        public ActionResult<string> Get()
        {
            return Ok("Api Controle de Despesas");
        }

        //metodo para listar todos os usuarios
        [HttpGet("usuarios")]
        public ActionResult<IEnumerable<IdentityUser>> GetUsuarios()
        {
            var lista = _userManager.Users.ToList();
            return Ok(lista);
        }
        


        //metodo para registrar usuario
        [HttpPost("registrar")]
        public async Task<ActionResult> Registrar(Usuario usuario)
        {
            //verifica se o usuario ja existe
            var user = await _userManager.FindByNameAsync(usuario.Email);
            if (user != null)
            {
                return BadRequest("Usuario ja existe");
            }
            //cria usuario
            user = new IdentityUser
            {
                UserName = usuario.Email,
                Email = usuario.Email,
                EmailConfirmed = true
            };
            //cria usuario
            var result = await _userManager.CreateAsync(user, usuario.Senha);
            if (result.Succeeded)
            {
                return Ok(GeraToken(usuario));
            }
            else
            {
                return BadRequest(result.Errors);
            }
        }

        //metodo para logar usuario
        [HttpPost("logar")]
        public async Task<ActionResult> Logar(Usuario usuario)
        {
               
            //verifica se o usuario existe
            var user = await _userManager.FindByNameAsync(usuario.Email);
            if (user == null)
            {
                return BadRequest("Usuario nao existe");
            }
            //verifica se a senha esta correta
            var result = await _signInManager.PasswordSignInAsync(user, usuario.Senha, false, true);
            if (result.Succeeded)
            {
                return Ok(GeraToken(usuario));
            }
            else
            {
                return BadRequest("Senha incorreta");
            }
        }

        private UsuarioToken GeraToken(Usuario userInfo)
        {
            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.UniqueName, userInfo.Email),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),

            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var exparation = _configuration["TokenConfiguration:ExpireHours"];

            var expiration = DateTime.UtcNow.AddHours(double.Parse(exparation));

            JwtSecurityToken token = new JwtSecurityToken(
                issuer: _configuration["TokenConfiguration:Issuer"],
                audience: _configuration["TokenConfiguration:Audience"],
                claims: claims,
                expires: expiration,
                signingCredentials: creds
            );

            return new UsuarioToken()
            {
                Authenticated = true,
                Expiration = expiration,
                Token = new JwtSecurityTokenHandler().WriteToken(token),
                Message = "OK"
            };
        }

    }
}

using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Api.PontoDigital.Class;
using Api.PontoDigital.Models.API;
using Api.PontoDigital.Repository.PessoaFisica;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace Api.PontoDigital.Controllers
{
    /// <summary>
    /// TokenController
    /// </summary>
    [Route("[controller]")]
    [ApiController]
    public class TokenController : Controller
    {

        /// <summary>
        /// _configuration
        /// </summary>
        public IConfiguration _configuration;
        /// <summary>
        /// _pessoaFisicaRepository
        /// </summary>
        private readonly IPessoaFisicaRepository _pessoaFisicaRepository;


        /// <summary>
        /// TokenController
        /// </summary>
        /// <param name="config"></param>
        /// <param name="pessoaFisicaRepository"></param>
        public TokenController(IConfiguration config, IPessoaFisicaRepository pessoaFisicaRepository)
        {
            _configuration = config;
            _pessoaFisicaRepository = pessoaFisicaRepository;
        }

        /// <summary>
        /// GerarToken
        /// </summary>
        /// <param name="CPF"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> GerarToken(string CPF)
        {
            CPF = CPF?.Trim().Replace(".", "")?.Replace("-", "");
            var ValidarCPF = FUNCOES_UTEIS.ValidarCpf(CPF);
            if (!ValidarCPF)
                return BadRequest("CPF Inválido");

            //Verifica se existe no banco
            var pessoa = await _pessoaFisicaRepository.SelecionarPorCPF(CPF);
            if (pessoa == null)
            {
                return BadRequest("Pessoa com CPF " + FUNCOES_UTEIS.FormatString(CPF, FUNCOES_UTEIS.MASCARA_FORMATO.CPF) + " não existe");
            }
            else
            {
                //cria claims baseado nas informações do usuário
                var claims = new[] {
                    new Claim(JwtRegisteredClaimNames.Sub, _configuration["Jwt:Subject"]),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                    new Claim(JwtRegisteredClaimNames.Iat, DateTime.UtcNow.ToString()),
                    new Claim("cpf", CPF)
                   };
                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
                var signIn = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
                var token = new JwtSecurityToken(_configuration["Jwt:Issuer"],
                             _configuration["Jwt:Audience"], claims,
                             expires: DateTime.UtcNow.AddHours(24), signingCredentials: signIn);
                var tokenGerado = new JwtSecurityTokenHandler().WriteToken(token);
                var result = new TokenJWT { Token = "Bearer " + tokenGerado };
                return Ok(result);
            }
        }
    }
}

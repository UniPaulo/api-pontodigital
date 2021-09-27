using System;
using System.ComponentModel;
using System.Threading.Tasks;
using Api.PontoDigital.Class;
using Api.PontoDigital.Models.API;
using Api.PontoDigital.Repository.PessoaFisica;
using Api.PontoDigital.Repository.PessoaFisicaLogin;
using Api.PontoDigital.Repository.PessoaJuridica;
using Api.PontoDigital.Repository.PessoaJuridicaFisica;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using static Api.PontoDigital.Models.SQL.PESSOA_JURIDICA_FISICA;

namespace Api.PontoDigital.Controllers
{
    /// <summary>
    /// API de Autenticação
    /// </summary>
    [Route("[controller]")]
    [Produces("application/json")]
    [ApiVersion("1.0")]
    [ApiController]
    [Description("Autenticação")]
    public class AutenticarController : ControllerBase
    {
        private readonly ILogger<AutenticarController> _logger;
        private readonly IPessoaFisicaRepository _pessoaFisicaRepository;
        private readonly IPessoaJuridicaRepository _pessoaJuridicaRepository;
        private readonly IPessoaFisicaLoginRepository _pessoaFisicaLoginRepository;
        private readonly IPessoaJuridicaFisicaRepository _pessoaJuridicaFisicaRepository;

        /// <summary>
        /// AutenticarController
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="pessoaFisicaRepository"></param>
        /// <param name="pessoaJuridicaRepository"></param>
        /// <param name="pessoaFisicaLoginRepository"></param>
        /// <param name="pessoaJuridicaFisicaRepository"></param>
        public AutenticarController(ILogger<AutenticarController> logger, IPessoaFisicaRepository pessoaFisicaRepository, IPessoaJuridicaRepository pessoaJuridicaRepository, IPessoaFisicaLoginRepository pessoaFisicaLoginRepository, IPessoaJuridicaFisicaRepository pessoaJuridicaFisicaRepository)
        {
            _logger = logger;
            _pessoaFisicaRepository = pessoaFisicaRepository;
            _pessoaFisicaLoginRepository = pessoaFisicaLoginRepository;
            _pessoaJuridicaRepository = pessoaJuridicaRepository;
            _pessoaJuridicaFisicaRepository = pessoaJuridicaFisicaRepository;
        }

        /// <summary>
        /// Método que autentica o Usuário
        /// </summary>
        /// <param name="autenticar"></param>
        /// <returns>Resultado da Autenticação</returns>
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Autenticar autenticar)
        {
            try
            {

                if (autenticar.IdPessoaJuridica == 0)
                    return BadRequest("Obrigatório informar ID  da Pessoa Juridica");

                var ExisteEmpresa = await _pessoaJuridicaRepository.SelecionarPorId(autenticar.IdPessoaJuridica);
                if (ExisteEmpresa == null)
                    return BadRequest("Empresa não encontrada");

                var ValidarCPF = FUNCOES_UTEIS.ValidarCpf(autenticar.CPF);
                if (!ValidarCPF)
                    return BadRequest("CPF Inválido");

                autenticar.CPF = autenticar?.CPF?.Trim().Replace(".", "")?.Replace("-", "");

                //Verifica se existe no banco
                var pessoa = await _pessoaFisicaRepository.SelecionarPorCPF(autenticar.CPF);
                if (pessoa == null)
                    return BadRequest("Pessoa com CPF " + FUNCOES_UTEIS.FormatString(autenticar.CPF, FUNCOES_UTEIS.MASCARA_FORMATO.CPF) + " não existe");

                var result = await _pessoaFisicaLoginRepository.SelecionarPorIdPessoaFisica(pessoa.IdPessoaFisica);
                if (result != null)
                {
                    var EmpresaUsuario = await _pessoaJuridicaFisicaRepository.SelecionarPorId(ExisteEmpresa.IdPessoaJuridica, result.IdPessoaFisica);
                    if(EmpresaUsuario == null)
                    {
                        return BadRequest("Usuário não está associado com a Empresa");
                    }
                    if(EmpresaUsuario?.Status  == StatusValores.Inativo)
                    {
                        return BadRequest("Usuário está Inativo");
                    }
                    if (result.Senha == autenticar.Senha)
                    {
                        PessoaFisica pessoaFisica = new PessoaFisica
                        {
                            IdPessoaFisica = pessoa.IdPessoaFisica,
                            Nome = pessoa.Nome,
                            CPF = FUNCOES_UTEIS.FormatString(pessoa.CPF, FUNCOES_UTEIS.MASCARA_FORMATO.CPF),
                            IdPessoaJuridica = autenticar.IdPessoaJuridica,
                            CodigoPerfil = result.IdPerfil,
                            Hoje = DateTime.Now.ToString("dd/MM/yyyy")
                        };
                        return Ok(pessoaFisica);
                    }
                    else
                    {
                        return BadRequest("Senha inválida");
                    }
                }
                else
                {
                    return BadRequest("Não foi encontrado cadastro de Login para o CPF " + FUNCOES_UTEIS.FormatString(autenticar.CPF, FUNCOES_UTEIS.MASCARA_FORMATO.CPF));
                }
            }
            catch (Exception ex)
            {
                string msg = "Erro ao buscar Pessoa Fisica: ";
                _logger.LogError(ex, msg);
                return BadRequest(msg + ex);
            }
        }
    }
}

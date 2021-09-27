using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;
using Api.PontoDigital.Class;
using Api.PontoDigital.Models.API;
using Api.PontoDigital.Models.SQL;
using Api.PontoDigital.Repository.PessoaFisica;
using Api.PontoDigital.Repository.PessoaJuridica;
using Api.PontoDigital.Repository.PessoaJuridicaFisica;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Api.PontoDigital.Controllers
{
    /// <summary>
    /// API de Pessoa Jurídica
    /// </summary>
    [Route("[controller]")]
    [Produces("application/json")]
    [ApiVersion("1.0")]
    [ApiController]
    [Description("Pessoa Jurídica")]
    public class PessoaJuridicaController : ControllerBase
    {
        private readonly ILogger<PessoaJuridicaController> _logger;
        private readonly IPessoaJuridicaRepository _pessoaJuridicaRepository;
        private readonly IPessoaFisicaRepository _pessoaFisicaRepository;
        private readonly IPessoaJuridicaFisicaRepository _pessoaJuridicaFisicaRepository;


        /// <summary>
        /// PessoaJuridicaController
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="pessoaJuridicaRepository"></param>
        /// <param name="pessoaFisicaRepository"></param>
        /// <param name="pessoaJuridicaFisicaRepository"></param>
        public PessoaJuridicaController(ILogger<PessoaJuridicaController> logger, IPessoaJuridicaRepository pessoaJuridicaRepository, IPessoaFisicaRepository pessoaFisicaRepository, IPessoaJuridicaFisicaRepository pessoaJuridicaFisicaRepository)
        {
            _logger = logger;
            _pessoaJuridicaRepository = pessoaJuridicaRepository;
            _pessoaFisicaRepository = pessoaFisicaRepository;
            _pessoaJuridicaFisicaRepository = pessoaJuridicaFisicaRepository;
        }

        /// <summary>
        /// Método que Busca a Pessoa Jurídica pelo CPF
        /// </summary>
        /// <param name="CPF"></param>
        /// <returns></returns>
        [HttpGet("ListarEmpresas/{CPF}")]
        public async Task<IActionResult> GetByPessoaFisica(string CPF)
        {
            try
            {

                //Verifica se existe no banco
                var pessoa = await _pessoaFisicaRepository.SelecionarPorCPF(CPF);
                if (pessoa == null)
                    return BadRequest("Pessoa com CPF " + FUNCOES_UTEIS.FormatString(CPF, FUNCOES_UTEIS.MASCARA_FORMATO.CPF) + " não existe");

                var empresas = await _pessoaJuridicaFisicaRepository.SelecionarPorPessoaFisica((long)(pessoa?.IdPessoaFisica));
                if (empresas == null || empresas.Count == 0)
                    return BadRequest("Não foi encontrada empresas associadas ao CPF informado");

                List<Empresa> lstEmpresas = new List<Empresa>();
                foreach (var emp in empresas)
                {
                    Empresa empresa = new Empresa
                    {
                        Id = emp.IdPessoaJuridica
                    };
                    empresa.Nome = _pessoaJuridicaRepository.SelecionarPorId(empresa.Id).Result.RazaoSocial;
                    lstEmpresas.Add(empresa);
                }
                return Ok(lstEmpresas);
            }
            catch (Exception ex)
            {
                string msg = "Erro ao buscar Pessoa Juridica: ";
                _logger.LogError(ex, msg);
                return BadRequest(msg + ex);
            }
        }

        /// <summary>
        /// Método que Busca a Pessoa Jurídica pelo Id
        /// </summary>
        /// <param name="IdPessoaJuridica"></param>
        /// <returns></returns>
        [HttpGet("{IdPessoaJuridica:long}")]
        public async Task<IActionResult> GetById([FromRoute] long IdPessoaJuridica)
        {
            try
            {
                var result = await _pessoaJuridicaRepository.SelecionarPorId(IdPessoaJuridica);
                if (result != null)
                    return Ok(result);
                else
                    return NotFound(result);
            }
            catch (Exception ex)
            {
                string msg = "Erro ao buscar Pessoa Juridica: ";
                _logger.LogError(ex, msg);
                return BadRequest(msg + ex);
            }
        }

        /// <summary>
        /// Método que Insere a Pessoa Jurídica
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] PessoaJuridica pessoaJuridica)
        {
            try
            {

                var ValidarCNPJ = FUNCOES_UTEIS.ValidaCNPJ(pessoaJuridica.CNPJ);
                if (!ValidarCNPJ)
                    return BadRequest("CNPJ Inválido");

                pessoaJuridica.CNPJ = pessoaJuridica?.CNPJ?.Trim().Replace(".", "")?.Replace("/", "")?.Replace("-", "");

                //Verifica se existe no banco
                var existe = await _pessoaJuridicaRepository.SelecionarPorCNPJ(pessoaJuridica.CNPJ);
                if (existe != null)
                    return BadRequest("Empresa com CNPJ " + FUNCOES_UTEIS.FormatString(pessoaJuridica.CNPJ, FUNCOES_UTEIS.MASCARA_FORMATO.CNPJ) + " já existe");

                PESSOA_JURIDICA objPJ = new PESSOA_JURIDICA()
                {
                    RazaoSocial = pessoaJuridica?.RazaoSocial,
                    CNPJ = pessoaJuridica?.CNPJ,
                    DataHoraCadastro = DateTime.Now,
                };

                var result = await _pessoaJuridicaRepository.Inserir(objPJ);
                if (result != null)
                    return Ok(result);
                else
                    return NotFound(result);
            }
            catch (Exception ex)
            {
                string msg = "Erro ao buscar Pessoa Juridica: ";
                _logger.LogError(ex, msg);
                return BadRequest(msg + ex);
            }
        }

        /// <summary>
        /// Método que Atualiza a Pessoa Jurídica
        /// </summary>
        /// <returns></returns>
        [HttpPut]
        public async Task<IActionResult> Put([FromBody] PessoaJuridica pessoaJuridica)
        {
            try
            {

                if (pessoaJuridica.IdPessoaJuridica == 0)
                    return BadRequest("Obrigatório informar ID");

                var ValidarCNPJ = FUNCOES_UTEIS.ValidaCNPJ(pessoaJuridica.CNPJ);
                if (!ValidarCNPJ)
                    return BadRequest("CNPJ Inválido");

                //Verifica se existe no banco
                var existe = await _pessoaJuridicaRepository.SelecionarPorId(pessoaJuridica.IdPessoaJuridica);
                if (existe == null)
                    return BadRequest("Empresa não encontrada");

                PESSOA_JURIDICA objPJ = new PESSOA_JURIDICA()
                {
                    IdPessoaJuridica = pessoaJuridica.IdPessoaJuridica,
                    RazaoSocial = pessoaJuridica?.RazaoSocial,
                    CNPJ = pessoaJuridica?.CNPJ?.Trim().Replace(".", "")?.Replace("/", "")?.Replace("-", ""),
                    DataHoraCadastro = existe?.DataHoraCadastro
                };



                var result = await _pessoaJuridicaRepository.Atualizar(objPJ);
                if (result != null)
                    return Ok(result);
                else
                    return NotFound(result);
            }
            catch (Exception ex)
            {
                string msg = "Erro ao atualizar Pessoa Juridica: ";
                _logger.LogError(ex, msg);
                return BadRequest(msg + ex);
            }
        }
    }
}

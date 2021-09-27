using System;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using Api.PontoDigital.Class;
using Api.PontoDigital.Models.API;
using Api.PontoDigital.Models.SQL;
using Api.PontoDigital.Repository.PessoaFisica;
using Api.PontoDigital.Repository.PessoaFisicaLogin;
using Api.PontoDigital.Repository.PessoaJuridica;
using Api.PontoDigital.Repository.PessoaJuridicaFisica;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Api.PontoDigital.Controllers
{
    /// <summary>
    /// API de Pessoa Física
    /// </summary>
    [Route("[controller]")]
    [Produces("application/json")]
    [ApiVersion("1.0")]
    [ApiController]
    [Description("Pessoa Física")]
    public class PessoaFisicaController : ControllerBase
    {
        private readonly ILogger<PessoaFisicaController> _logger;
        private readonly IPessoaFisicaRepository _pessoaFisicaRepository;
        private readonly IPessoaJuridicaFisicaRepository _pessoaJuridicaFisicaRepository;
        private readonly IPessoaJuridicaRepository _pessoaJuridicaRepository;
        private readonly IPessoaFisicaLoginRepository _pessoaFisicaLoginRepository;

        /// <summary>
        /// PessoaFisicaController
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="pessoaFisicaRepository"></param>
        /// <param name="pessoaJuridicaFisicaRepository"></param>
        /// <param name="pessoaJuridicaRepository"></param>
        /// <param name="pessoaFisicaLoginRepository"></param>
        public PessoaFisicaController(ILogger<PessoaFisicaController> logger, IPessoaFisicaRepository pessoaFisicaRepository, IPessoaJuridicaFisicaRepository pessoaJuridicaFisicaRepository, IPessoaJuridicaRepository pessoaJuridicaRepository, IPessoaFisicaLoginRepository pessoaFisicaLoginRepository)
        {
            _logger = logger;
            _pessoaFisicaRepository = pessoaFisicaRepository;
            _pessoaJuridicaFisicaRepository = pessoaJuridicaFisicaRepository;
            _pessoaJuridicaRepository = pessoaJuridicaRepository;
            _pessoaFisicaLoginRepository = pessoaFisicaLoginRepository;
        }

        /// <summary>
        /// Método que Busca a Pessoa Física pelo Id
        /// </summary>
        /// <param name="IdPessoaFisica"></param>
        /// <returns>Objeto da Pessoa Física</returns>
        [HttpGet("{IdPessoaFisica:long}")]
        public async Task<IActionResult> GetById([FromRoute] long IdPessoaFisica)
        {
            try
            {
                var result = await _pessoaFisicaRepository.SelecionarPorId(IdPessoaFisica);
                if (result != null)
                    return Ok(result);
                else
                    return NotFound(result);
            }
            catch (Exception ex)
            {
                string msg = "Erro ao buscar Pessoa Fisica: ";
                _logger.LogError(ex, msg);
                return BadRequest(msg + ex);
            }
        }


        /// <summary>
        /// Método que Busca a Pessoa Física pelo Nome
        /// </summary>
        /// <param name="nome"></param>
        /// <returns>Objeto da Pessoa Física</returns>
        [HttpGet("Nome/{nome}")]
        public async Task<IActionResult> GetByNome([FromRoute] string nome)
        {
            try
            {
                var result = await _pessoaFisicaRepository.SelecionarPorNome(nome);
                if (result != null)
                    return Ok(result);
                else
                    return NotFound("Pessoa não encontrada");
            }
            catch (Exception ex)
            {
                string msg = "Erro ao buscar Pessoa Fisica: ";
                _logger.LogError(ex, msg);
                return BadRequest(msg + ex);
            }
        }

        /// <summary>
        /// Método que Busca a Pessoa Física pelo CPF
        /// </summary>
        /// <param name="cpf"></param>
        /// <returns>Objeto da Pessoa Física</returns>
        [HttpGet("CPF/{cpf}")]
        public async Task<IActionResult> GetByCPF([FromRoute] string cpf)
        {
            try
            {
                var result = await _pessoaFisicaRepository.SelecionarPorCPF(cpf);
                if (result != null)
                {
                    PessoaFisica PessoaFisica = new PessoaFisica
                    {
                        IdPessoaFisica = result.IdPessoaFisica,
                        Nome = result.Nome,
                        CPF = result.CPF,
                        Ocupacao = result.Ocupacao,
                        DataHoraInicioExpediente = result.DataHoraInicioExpediente?.ToShortTimeString(),
                        DataHoraInicioIntervalo = result.DataHoraInicioExpediente?.ToShortTimeString(),
                        DataHoraFimIntervalo = result.DataHoraInicioIntervalo?.ToShortTimeString(),
                        DataHoraFimExpediente = result.DataHoraFimIntervalo?.ToShortTimeString()
                    };

                    var perfil = await _pessoaFisicaLoginRepository.SelecionarPorIdPessoaFisica(PessoaFisica.IdPessoaFisica);
                    if (perfil != null)
                    {
                        PessoaFisica.CodigoPerfil = perfil.IdPerfil;
                        PessoaFisica.Senha = perfil.Senha;
                    }

                    var empresa = await _pessoaJuridicaFisicaRepository.SelecionarPorPessoaFisica(PessoaFisica.IdPessoaFisica);
                    if (empresa != null && empresa?.Count > 0)
                    {
                        PessoaFisica.Status = empresa?.FirstOrDefault()?.Status;
                    }


                    return Ok(PessoaFisica);
                }
                else
                    return NotFound("CPF não encontrado");
            }
            catch (Exception ex)
            {
                string msg = "Erro ao buscar Pessoa Fisica pelo CPF: ";
                _logger.LogError(ex, msg);
                return BadRequest(msg + ex);
            }
        }

        /// <summary>
        /// Método que Insere a Pessoa Física
        /// </summary>
        /// <returns>Objeto da Pessoa Física</returns>
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] PessoaFisica pessoaFisica)
        {
            try
            {

                #region Converter Horarios e Validar
                var inicioExpediente = TimeSpan.Parse(pessoaFisica.DataHoraInicioExpediente);
                var inicioExpedienteConvertido = DateTime.Today.Add(inicioExpediente);
                var inicioIntervalo = TimeSpan.Parse(pessoaFisica.DataHoraInicioIntervalo);
                var inicioIntervaloConvertido = DateTime.Today.Add(inicioIntervalo);
                var fimIntervalo = TimeSpan.Parse(pessoaFisica.DataHoraFimIntervalo);
                var fimIntervaloConvertido = DateTime.Today.Add(fimIntervalo);
                var fimExpediente = TimeSpan.Parse(pessoaFisica.DataHoraFimExpediente);
                var fimExpedienteConvertido = DateTime.Today.Add(fimExpediente);
                #endregion

                if (pessoaFisica.IdPessoaJuridica == 0)
                    return BadRequest("Obrigatório informar ID  da Pessoa Juridica");

                var ExisteEmpresa = await _pessoaJuridicaRepository.SelecionarPorId(pessoaFisica.IdPessoaJuridica);
                if (ExisteEmpresa == null)
                    return BadRequest("Empresa não encontrada");

                var ValidarCPF = FUNCOES_UTEIS.ValidarCpf(pessoaFisica.CPF);
                if (!ValidarCPF)
                    return BadRequest("CPF Inválido");

                pessoaFisica.CPF = pessoaFisica?.CPF?.Trim().Replace(".", "")?.Replace("-", "");

                //Verifica se existe no banco
                var existe = await _pessoaFisicaRepository.SelecionarPorCPF(pessoaFisica.CPF);

                PESSOA_FISICA objPF = new PESSOA_FISICA()
                {
                    IdPessoaFisica = existe != null ? existe.IdPessoaFisica : 0,
                    Nome = pessoaFisica?.Nome,
                    CPF = pessoaFisica?.CPF,
                    Ocupacao = pessoaFisica?.Ocupacao,
                    DataHoraCadastro = DateTime.Now,
                    DataHoraInicioExpediente = inicioExpedienteConvertido,
                    DataHoraInicioIntervalo = inicioIntervaloConvertido,
                    DataHoraFimIntervalo = fimIntervaloConvertido,
                    DataHoraFimExpediente = fimExpedienteConvertido
                };

                var result = existe != null ? await _pessoaFisicaRepository.Atualizar(objPF) : await _pessoaFisicaRepository.Inserir(objPF);
                if (result != null)
                {
                    #region Vincular Pessoa na empresa
                    PESSOA_JURIDICA_FISICA objPJF = new PESSOA_JURIDICA_FISICA()
                    {
                        IdPessoaFisica = result.IdPessoaFisica,
                        IdPessoaJuridica = (long)(pessoaFisica?.IdPessoaJuridica),
                        Status = pessoaFisica?.Status == null ? PESSOA_JURIDICA_FISICA.StatusValores.Ativo : pessoaFisica.Status
                    };
                    var resultPJF = await _pessoaJuridicaFisicaRepository.SelecionarPorId(objPJF.IdPessoaJuridica, objPJF.IdPessoaFisica);
                    resultPJF = existe != null ? await _pessoaJuridicaFisicaRepository.Atualizar(objPJF) : await _pessoaJuridicaFisicaRepository.Inserir(objPJF);
                    #endregion

                    #region Cadastrar Senha e associar ao perfil
                    PESSOA_FISICA_LOGIN objPFL = new PESSOA_FISICA_LOGIN()
                    {
                        IdPessoaFisica = result.IdPessoaFisica,
                        Senha = pessoaFisica?.Senha,
                        IdPerfil = (pessoaFisica?.CodigoPerfil == null || pessoaFisica?.CodigoPerfil == 0) ? 1 : (int)(pessoaFisica?.CodigoPerfil)
                    };
                    var resultPFL = await _pessoaFisicaLoginRepository.SelecionarPorIdPessoaFisica(objPFL.IdPessoaFisica);
                    resultPFL = resultPFL != null ? await _pessoaFisicaLoginRepository.Atualizar(objPFL) : await _pessoaFisicaLoginRepository.Inserir(objPFL);
                    #endregion
                    return Ok(result);
                }
                else
                {
                    return NotFound(result);
                }
            }
            catch (Exception ex)
            {
                string msg = "Erro ao buscar Pessoa Fisica: ";
                _logger.LogError(ex, msg);
                return BadRequest(msg + ex);
            }
        }


        /// <summary>
        /// Método que Atualiza a Pessoa Física
        /// </summary>
        /// <returns>Objeto da Pessoa Física</returns>
        [HttpPut]
        public async Task<IActionResult> Put([FromBody] PessoaFisica pessoaFisica)
        {
            try
            {
                #region Converter Horarios
                var inicioExpediente = TimeSpan.Parse(pessoaFisica.DataHoraInicioExpediente);
                var inicioExpedienteConvertido = DateTime.Today.Add(inicioExpediente);
                var inicioIntervalo = TimeSpan.Parse(pessoaFisica.DataHoraInicioIntervalo);
                var inicioIntervaloConvertido = DateTime.Today.Add(inicioIntervalo);
                var fimIntervalo = TimeSpan.Parse(pessoaFisica.DataHoraFimIntervalo);
                var fimIntervaloConvertido = DateTime.Today.Add(fimIntervalo);
                var fimExpediente = TimeSpan.Parse(pessoaFisica.DataHoraFimExpediente);
                var fimExpedienteConvertido = DateTime.Today.Add(fimExpediente);
                #endregion

                if (pessoaFisica.IdPessoaFisica == 0)
                    return BadRequest("Obrigatório informar ID da Pessoa Fisica");
                if (pessoaFisica.IdPessoaJuridica == 0)
                    return BadRequest("Obrigatório informar ID  da Pessoa Juridica");
                var ExisteEmpresa = await _pessoaJuridicaRepository.SelecionarPorId(pessoaFisica.IdPessoaJuridica);
                if (ExisteEmpresa == null)
                    return BadRequest("Empresa não encontrada");

                var ValidarCPF = FUNCOES_UTEIS.ValidarCpf(pessoaFisica.CPF);
                if (!ValidarCPF)
                    return BadRequest("CPF Inválido");

                pessoaFisica.CPF = pessoaFisica?.CPF?.Trim().Replace(".", "")?.Replace("-", "");

                //Verifica se existe no banco
                var existe = await _pessoaFisicaRepository.SelecionarPorId(pessoaFisica.IdPessoaFisica);
                if (existe == null)
                    return BadRequest("Pessoa não encontrada");

                PESSOA_FISICA objPF = new PESSOA_FISICA()
                {
                    IdPessoaFisica = pessoaFisica.IdPessoaFisica,
                    Nome = pessoaFisica?.Nome,
                    CPF = pessoaFisica?.CPF,
                    Ocupacao = pessoaFisica?.Ocupacao,
                    DataHoraCadastro = DateTime.Now,
                    DataHoraInicioExpediente = inicioExpedienteConvertido,
                    DataHoraInicioIntervalo = inicioIntervaloConvertido,
                    DataHoraFimIntervalo = fimIntervaloConvertido,
                    DataHoraFimExpediente = fimExpedienteConvertido
                };

                var result = await _pessoaFisicaRepository.Atualizar(objPF);
                if (result != null)
                {
                    #region Desvincular/Vincular Pessoa na empresa
                    PESSOA_JURIDICA_FISICA objPJF = new PESSOA_JURIDICA_FISICA()
                    {
                        IdPessoaFisica = result.IdPessoaFisica,
                        IdPessoaJuridica = (long)(pessoaFisica?.IdPessoaJuridica),
                        Status = pessoaFisica?.Status == null ? PESSOA_JURIDICA_FISICA.StatusValores.Ativo : pessoaFisica.Status
                    };
                    var resultPJF = await _pessoaJuridicaFisicaRepository.Atualizar(objPJF);
                    #endregion

                    #region Cadastrar/Alterar Senha e associar ao perfil
                    PESSOA_FISICA_LOGIN objPFL = new PESSOA_FISICA_LOGIN()
                    {
                        IdPessoaFisica = result.IdPessoaFisica,
                        Senha = pessoaFisica?.Senha,
                        IdPerfil = (pessoaFisica?.CodigoPerfil == null || pessoaFisica?.CodigoPerfil == 0) ? 1 : (int)(pessoaFisica?.CodigoPerfil)
                    };
                    var resultPFL = await _pessoaFisicaLoginRepository.Atualizar(objPFL);
                    #endregion
                    return Ok(result);
                }
                else
                {
                    return NotFound(result);
                }
            }
            catch (Exception ex)
            {
                string msg = "Erro ao atualizar Pessoa Fisica: ";
                _logger.LogError(ex, msg);
                return BadRequest(msg + ex);
            }
        }
    }
}

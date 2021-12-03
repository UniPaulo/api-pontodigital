using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Api.PontoDigital.Models.API;
using Api.PontoDigital.Models.SQL;
using Api.PontoDigital.Repository.OperacaoPonto;
using Api.PontoDigital.Repository.PessoaFisica;
using Api.PontoDigital.Repository.PessoaJuridica;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Api.PontoDigital.Controllers
{

    /// <summary>
    /// PontoController
    /// </summary>
    [Route("[controller]")]
    [ApiController]
    public class PontoController : ControllerBase
    {
        private readonly ILogger<PontoController> _logger;
        private readonly IPessoaFisicaRepository _pessoaFisicaRepository;
        private readonly IPessoaJuridicaRepository _pessoaJuridicaRepository;
        private readonly IOperacaoPontoRepository _operacaoPontoRepository;

        /// <summary>
        /// PontoController
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="pessoaFisicaRepository"></param>
        /// <param name="pessoaJuridicaRepository"></param>
        /// <param name="operacaoPontoRepository"></param>
        public PontoController(ILogger<PontoController> logger, IPessoaFisicaRepository pessoaFisicaRepository, IPessoaJuridicaRepository pessoaJuridicaRepository, IOperacaoPontoRepository operacaoPontoRepository)
        {
            _logger = logger;
            _pessoaFisicaRepository = pessoaFisicaRepository;
            _pessoaJuridicaRepository = pessoaJuridicaRepository;
            _operacaoPontoRepository = operacaoPontoRepository;
        }
        /// <summary>
        /// Método que Realiza o Ponto
        /// </summary>
        /// <returns>Objeto da Pessoa Física</returns>
        [Authorize]
        [HttpPost("RealizarPonto")]
        public async Task<IActionResult> RealizarPonto([FromBody] Ponto ponto)
        {
            try
            {
                Retorno retorno = new Retorno();
                var PessoaFisica = await _pessoaFisicaRepository.SelecionarPorId(ponto.IdPessoaFisica);
                if (PessoaFisica != null)
                {
                    var PessoaJuridica = await _pessoaJuridicaRepository.SelecionarPorId(ponto.IdPessoaJuridica);
                    if (PessoaJuridica != null)
                    {
                        var ExistePonto = await _operacaoPontoRepository.BuscarRelatorio(ponto.IdPessoaFisica, DateTime.Now, DateTime.Now);
                        if(ExistePonto == null || ExistePonto.Count == 0)
                        {
                            //Primeiro Ponto do dia (Inicio do Expediente)
                            OPERACAO_PONTO Ponto = new OPERACAO_PONTO
                            {
                                IdPessoaFisica = ponto.IdPessoaFisica,
                                IdPessoaJuridica = ponto.IdPessoaJuridica,
                                DataHoraInicioExpediente = DateTime.Now
                            };
                            var PrimeiroPonto = await _operacaoPontoRepository.InserirPonto(Ponto);
                            retorno.Mensagem = "Primeiro Ponto do Dia Realizado com Sucesso, tenha um ótimo expediente hoje.";
                            return Ok(retorno);
                        }
                        else
                        {
                            OPERACAO_PONTO Ponto = ExistePonto?.FirstOrDefault();
                            if(Ponto != null)
                            {
                                //Segundo Ponto do dia  (Inicio do Intervalo)
                                if (Ponto?.DataHoraInicioIntervalo == null)
                                {
                                    Ponto.DataHoraInicioIntervalo = DateTime.Now;
                                    DateTime CargaHoraria = new DateTime();
                                    CargaHoraria = CargaHoraria.AddDays(Ponto.DataHoraInicioIntervalo.Value.Day - 1);
                                    CargaHoraria = CargaHoraria.AddMonths(Ponto.DataHoraInicioIntervalo.Value.Month - 1);
                                    CargaHoraria = CargaHoraria.AddYears(Ponto.DataHoraInicioIntervalo.Value.Year - 1);

                                    TimeSpan AntesIntervalo = (TimeSpan)(Ponto.DataHoraInicioIntervalo - Ponto.DataHoraInicioExpediente);

                                    CargaHoraria = CargaHoraria.Add(AntesIntervalo);
                                    Ponto.CargaHoraria = CargaHoraria;

                                    await _operacaoPontoRepository.AtualizarPonto(Ponto);
                                    retorno.Mensagem = "Segundo Ponto do Dia Realizado com Sucesso, tenha um ótimo intervalo hoje.";
                                    return Ok(retorno);
                                }
                                //Terceiro Ponto do dia  (Fim do Intervalo)
                                else if (Ponto?.DataHoraFimIntervalo == null)
                                {
                                    Ponto.DataHoraFimIntervalo = DateTime.Now;
                                    await _operacaoPontoRepository.AtualizarPonto(Ponto);
                                    retorno.Mensagem = "Terceiro Ponto do Dia Realizado com Sucesso, seja bem-vindo do intervalo.";
                                    return Ok(retorno);
                                }
                                //Quarto Ponto do dia  (Fim do Expediente)
                                else if (Ponto?.DataHoraFimExpediente == null)
                                {
                                    Ponto.DataHoraFimExpediente = DateTime.Now;
                                    #region Carga Horária
                                    DateTime CargaHoraria = new DateTime();
                                    CargaHoraria = CargaHoraria.AddDays(DateTime.Now.Day - 1);
                                    CargaHoraria = CargaHoraria.AddMonths(DateTime.Now.Month - 1);
                                    CargaHoraria = CargaHoraria.AddYears(DateTime.Now.Year - 1);

                                    TimeSpan AntesIntervalo = (TimeSpan)(Ponto.DataHoraInicioIntervalo - Ponto.DataHoraInicioExpediente);
                                    TimeSpan DepoisIntervalo = (TimeSpan)(Ponto.DataHoraFimExpediente - Ponto.DataHoraFimIntervalo);
                                    TimeSpan Total = (TimeSpan)(DepoisIntervalo + AntesIntervalo);

                                    CargaHoraria = CargaHoraria.Add(Total);

                                    Ponto.CargaHoraria = CargaHoraria;
                                    #endregion
                                    await _operacaoPontoRepository.AtualizarPonto(Ponto);
                                    retorno.Mensagem = "Quarto Ponto do Dia Realizado com Sucesso, tenha um ótimo descanso.";
                                    return Ok(retorno);
                                }
                                else
                                {
                                    return BadRequest("Você já bateu o ponto na Data de Hoje.");
                                }
                            }
                            else
                            {
                                return BadRequest("Não foi encontrada informações do Ponto com a Data de Hoje.");
                            }
                        }
                    }
                    else
                    {
                        return BadRequest("Pessoa Jurídica não encontrada.");
                    }
                }
                else
                {
                    return BadRequest("Pessoa Física não encontrada.");
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

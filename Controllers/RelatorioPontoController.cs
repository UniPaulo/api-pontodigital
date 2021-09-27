using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;
using Api.PontoDigital.Repository.OperacaoPonto;
using Api.PontoDigital.Repository.PessoaFisica;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Api.PontoDigital.Models.API;
using Api.PontoDigital.Repository.PessoaJuridicaFisica;
using System.Linq;
using Api.PontoDigital.Class;

namespace Api.PontoDigital.Controllers
{
    /// <summary>
    /// API de Relatório
    /// </summary>
    [Route("[controller]")]
    [Produces("application/json")]
    [ApiVersion("1.0")]
    [ApiController]
    [Description("Relatório de Ponto")]
    public class RelatorioPontoController : ControllerBase
    {
        private readonly ILogger<RelatorioPontoController> _logger;
        private readonly IPessoaFisicaRepository _pessoaFisicaRepository;
        private readonly IOperacaoPontoRepository _operacaoPontoRepository;
        private readonly IPessoaJuridicaFisicaRepository _pessoaJuridicaFisicaRepository;
        private readonly IExportarExcel _exportarExcel;
        /// <summary>
        /// RelatorioPontoController
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="pessoaFisicaRepository"></param>
        /// <param name="operacaoPontoRepository"></param>
        /// <param name="pessoaJuridicaFisicaRepository"></param>
        /// <param name="exportarExcel"></param>
        public RelatorioPontoController(ILogger<RelatorioPontoController> logger, IPessoaFisicaRepository pessoaFisicaRepository, IOperacaoPontoRepository operacaoPontoRepository, IPessoaJuridicaFisicaRepository pessoaJuridicaFisicaRepository, IExportarExcel exportarExcel)
        {
            _logger = logger;
            _pessoaFisicaRepository = pessoaFisicaRepository;
            _operacaoPontoRepository = operacaoPontoRepository;
            _pessoaJuridicaFisicaRepository = pessoaJuridicaFisicaRepository;
            _exportarExcel = exportarExcel;
        }

        /// <summary>
        /// Método que Busca o Relatório de Histórico de Ponto
        /// </summary>
        /// <returns></returns>
        [HttpGet("{Filtro}/{DataInicio?}/{DataFim?}/{IdPessoaJuridica?}")]
        public async Task<IActionResult> GetRelatorio([FromRoute] string Filtro, [FromRoute] DateTime? DataInicio = null, [FromRoute] DateTime? DataFim = null, [FromRoute] long? IdPessoaJuridica = null)
        {
            try
            {
                #region Validações
                if (!DataInicio.HasValue || !DataFim.HasValue)
                {
                    if (DataInicio.HasValue && !DataFim.HasValue)
                    {
                        return BadRequest("Obrigatório informar valor para Data Final");
                    }
                    else if (!DataInicio.HasValue && DataFim.HasValue)
                    {
                        return BadRequest("Obrigatório informar valor para Data Inicial");
                    }
                }
                if (DataInicio.HasValue && DataFim.HasValue)
                {
                    if (DateTime.Compare(DataInicio.Value, DataFim.Value) > 0)
                    {
                        return BadRequest("Data de Inicio não pode ser maior que Data Final");
                    }
                    if (DateTime.Compare(DataFim.Value, DataInicio.Value) < 0)
                    {
                        return BadRequest("Data de Inicio não pode ser maior que Data Final");
                    }
                }
                #endregion

                var Pessoa = await _pessoaFisicaRepository.SelecionarPorCPF(Filtro);
                if (Pessoa != null)
                {
                    var empresa = await _pessoaJuridicaFisicaRepository.SelecionarPorId((long)IdPessoaJuridica, Pessoa.IdPessoaFisica);
                    if (empresa == null)
                    {
                        return BadRequest("Não foi encontrado informações com os filtros informados");
                    }

                    var Relatorio = await _operacaoPontoRepository.BuscarRelatorio(Pessoa.IdPessoaFisica, DataInicio, DataFim);
                    if(Relatorio != null && Relatorio.Count > 0)
                    {
                        List<Relatorio> relatorios = new List<Relatorio>();
                        foreach (var item in Relatorio)
                        {
                            Relatorio relatorio = new Relatorio
                            {
                                CPF = Pessoa?.CPF,
                                Nome = Pessoa?.Nome,
                                Data = item.DataHoraInicioExpediente?.ToString("dd/MM/yyy"),
                                DataInicioExpediente = item.DataHoraInicioExpediente?.ToShortTimeString(),
                                DataInicioIntervalo = item.DataHoraInicioIntervalo?.ToShortTimeString(),
                                DataFimIntervalo = item.DataHoraFimIntervalo?.ToShortTimeString(),
                                DataFimExpediente = item.DataHoraFimExpediente?.ToShortTimeString(),
                                CargaHoraria = item.CargaHoraria?.ToShortTimeString()
                            };
                            #region Hora Extra
                            #region Carga Horaria

                            var result = await _pessoaFisicaRepository.SelecionarPorCPF(relatorio.CPF);
                            if (result != null)
                            {
                                DateTime CargaHoraria = Convert.ToDateTime(relatorio.Data);

                                TimeSpan AntesIntervalo = (TimeSpan)(result.DataHoraInicioIntervalo - result.DataHoraInicioExpediente);
                                TimeSpan DepoisIntervalo = (TimeSpan)(result.DataHoraFimExpediente - result.DataHoraFimIntervalo);
                                TimeSpan TotalCargaraHoraria = DepoisIntervalo + AntesIntervalo;
                                CargaHoraria = CargaHoraria.Add(TotalCargaraHoraria);
                                #endregion
                                if(item.CargaHoraria != null)
                                {
                                    DateTime HoraExtra = (DateTime)item.DataHoraInicioExpediente;
                                    TimeSpan Total = (TimeSpan)(item.CargaHoraria - CargaHoraria);

                                    HoraExtra = HoraExtra.Add(Total);
                                    string extra = $"{((bool)(Total.Hours.ToString()?.Contains("-")) ? "00:00" : (Total.Hours.ToString().Length == 2 ? "-0" + Total.Hours.ToString()?.Replace("-", "") : Total.Hours.ToString()))}:{Total.Minutes.ToString()?.Replace("-", "")}";
                                    if ((bool)(Total.Hours.ToString()?.Contains("-")))
                                    {
                                        extra = "00:00";
                                    }
                                    else if (Total.Hours.ToString().Length == 2)
                                    {
                                        extra = "-0" + Total.Hours.ToString()?.Replace("-", "");
                                    }
                                    else
                                    {
                                        extra = $"{Total.Hours}:{Total.Minutes.ToString()?.Replace("-", "")}";
                                    }
                                    relatorio.HoraExtra = extra;
                                }                               
                            }

                            #endregion
                            relatorios.Add(relatorio);
                        }

                        #region Excel
                        relatorios.FirstOrDefault().Excel = await _exportarExcel.ExportarExcelAsync(relatorios);
                        #endregion

                        return Ok(relatorios);
                    }
                    else
                    {
                        return BadRequest("Não foi encontrado informações com os filtros informados");
                    }
                }                    
                else
                {
                    var Pessoas = await _pessoaFisicaRepository.SelecionarPorNome(Filtro);
                    if (Pessoas != null && Pessoas.Count > 0)
                    {
                        List<Relatorio> relatorios = new List<Relatorio>();
                        foreach (var pes in Pessoas)
                        {
                            var empresa = await _pessoaJuridicaFisicaRepository.SelecionarPorId((long)IdPessoaJuridica,pes.IdPessoaFisica);
                            if (empresa == null)
                            {
                                continue;
                            }

                            var Relatorio = await _operacaoPontoRepository.BuscarRelatorio(pes.IdPessoaFisica, DataInicio, DataFim);
                            if (Relatorio != null && Relatorio.Count > 0)
                            {
                                foreach (var item in Relatorio)
                                {
                                    Relatorio relatorio = new Relatorio
                                    {
                                        CPF = pes?.CPF,
                                        Nome = pes?.Nome,
                                        Data = item.DataHoraInicioExpediente?.ToString("dd/MM/yyy"),
                                        DataInicioExpediente = item.DataHoraInicioExpediente?.ToShortTimeString(),
                                        DataInicioIntervalo = item.DataHoraInicioIntervalo?.ToShortTimeString(),
                                        DataFimIntervalo = item.DataHoraFimIntervalo?.ToShortTimeString(),
                                        DataFimExpediente = item.DataHoraFimExpediente?.ToShortTimeString(),
                                        CargaHoraria = item.CargaHoraria?.ToShortTimeString()
                                    };

                                    #region Hora Extra
                                    #region Carga Horaria

                                    var result = await _pessoaFisicaRepository.SelecionarPorCPF(relatorio.CPF);
                                    if (result != null)
                                    {
                                        DateTime CargaHoraria = Convert.ToDateTime(relatorio.Data);

                                        TimeSpan AntesIntervalo = (TimeSpan)(result.DataHoraInicioIntervalo - result.DataHoraInicioExpediente);
                                        TimeSpan DepoisIntervalo = (TimeSpan)(result.DataHoraFimExpediente - result.DataHoraFimIntervalo);
                                        TimeSpan TotalCargaraHoraria = DepoisIntervalo + AntesIntervalo;
                                        CargaHoraria = CargaHoraria.Add(TotalCargaraHoraria);
                                        #endregion
                                        if (item.CargaHoraria != null)
                                        {
                                            DateTime HoraExtra = (DateTime)item.DataHoraInicioExpediente;
                                            TimeSpan Total = (TimeSpan)(item.CargaHoraria - CargaHoraria);

                                            HoraExtra = HoraExtra.Add(Total);
                                            string extra = $"{((bool)(Total.Hours.ToString()?.Contains("-")) ? "00:00" : (Total.Hours.ToString().Length == 2 ? "-0" + Total.Hours.ToString()?.Replace("-", "") : Total.Hours.ToString()))}:{Total.Minutes.ToString()?.Replace("-", "")}";
                                            if ((bool)(Total.Hours.ToString()?.Contains("-")))
                                            {
                                                extra = "00:00";
                                            }
                                            else if(Total.Hours.ToString().Length == 2)
                                            {
                                                extra = "-0" + Total.Hours.ToString()?.Replace("-", "");
                                            }
                                            else
                                            {
                                                extra = $"{Total.Hours}:{Total.Minutes.ToString()?.Replace("-", "")}";
                                            }
                                            relatorio.HoraExtra = extra;
                                        }
                                    }

                                    #endregion
                                    relatorios.Add(relatorio);
                                }
                            }
                        }
                        if(relatorios != null && relatorios?.Count > 0)
                        {
                            #region Excel
                            relatorios.FirstOrDefault().Excel = await _exportarExcel.ExportarExcelAsync(relatorios);
                            #endregion
                            return Ok(relatorios);
                        }
                        else
                        {
                            return BadRequest("Não foi encontrado informaçoes com os filtros informados");
                        }
                    }
                    else
                    {
                        return BadRequest("Não foi encontrado informaçoes com os filtros informados");
                    }
                }             
            }
            catch (Exception ex)
            {
                string msg = "Erro ao buscar Relatório: ";
                _logger.LogError(ex, msg);
                return BadRequest(msg + ex);
            }
        }
    }
}

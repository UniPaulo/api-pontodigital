using Api.PontoDigital.Models.SQL;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Api.PontoDigital.Repository.OperacaoPonto
{
    /// <summary>
    /// Interface do Operacao Ponto
    /// </summary>
    public interface IOperacaoPontoRepository
    {
        /// <summary>
        /// Injeção de Dependencia do Relatório
        /// </summary>
        /// <param name="IdPessoaFisica"></param>
        /// <param name="DataInicio"></param>
        /// <param name="DataFim"></param>
        /// <returns></returns>
        Task<List<OPERACAO_PONTO>> BuscarRelatorio(long IdPessoaFisica, DateTime? DataInicio, DateTime? DataFim);
        /// <summary>
        /// Injeção de Dependencia do Inserir Ponto
        /// </summary>
        /// <param name="objPonto"></param>
        /// <returns></returns>
        Task<OPERACAO_PONTO> InserirPonto(OPERACAO_PONTO objPonto);
        /// <summary>
        /// Injeção de Dependencia do Atualizar Ponto
        /// </summary>
        /// <param name="objPonto"></param>
        /// <returns></returns>
        Task<OPERACAO_PONTO> AtualizarPonto(OPERACAO_PONTO objPonto);
        /// <summary>
        ///  Injeção de Dependencia do Buscar Ponto Por Id
        /// </summary>
        /// <param name="IdOperacao"></param>
        /// <returns></returns>
        Task<OPERACAO_PONTO> BuscarPontoPorId(long IdOperacao);

    }
}

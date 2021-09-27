using Api.PontoDigital.Models.SQL;
using Dapper;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace Api.PontoDigital.Repository.OperacaoPonto
{
    /// <summary>
    /// Repositório do Operação Ponto
    /// </summary>
    public class OperacaoPontoRepository : IOperacaoPontoRepository
    {
        private readonly string _connectionString;
        /// <summary>
        /// Configuração da connectionString
        /// </summary>
        /// <param name="configuration"></param>
        public OperacaoPontoRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString(configuration?.GetValue<string>("Enviroment"));
        }
        /// <summary>
        /// Query para buscar relatório
        /// </summary>
        /// <param name="IdPessoaFisica"></param>
        /// <param name="DataInicio"></param>
        /// <param name="DataFim"></param>
        /// <returns></returns>
        public async Task<List<OPERACAO_PONTO>> BuscarRelatorio(long IdPessoaFisica, DateTime? DataInicio, DateTime? DataFim)
        {
            using var connection = new SqlConnection(_connectionString);
            var result = await connection?.QueryAsync<OPERACAO_PONTO>(OPERACAO_PONTO.Query.SelectIdPessoaFisica, new { @IdPessoaFisica = IdPessoaFisica, @DataInicio = DataInicio, @DataFim = DataFim }, commandType: CommandType.StoredProcedure);
            return result.ToList();
        }
        /// <summary>
        /// Inserir Ponto pela primeira vez no dia
        /// </summary>
        /// <param name="objPonto"></param>
        /// <returns></returns>
        public async Task<OPERACAO_PONTO> InserirPonto(OPERACAO_PONTO objPonto)
        {
            using var connection = new SqlConnection(_connectionString);
            var result = await connection?.QueryAsync<OPERACAO_PONTO>(OPERACAO_PONTO.Query.Insert, objPonto, commandType: CommandType.StoredProcedure);
            return result.FirstOrDefault();
        }
        /// <summary>
        /// Atualizar Ponto do dia
        /// </summary>
        /// <param name="objPonto"></param>
        /// <returns></returns>
        public async Task<OPERACAO_PONTO> AtualizarPonto(OPERACAO_PONTO objPonto)
        {
            using var connection = new SqlConnection(_connectionString);
            var result = await connection?.QueryAsync<OPERACAO_PONTO>(OPERACAO_PONTO.Query.Update, objPonto, commandType: CommandType.StoredProcedure);
            return result.FirstOrDefault();
        }

        /// <summary>
        /// Buscar Ponto Por Id
        /// </summary>
        /// <param name="IdOperacao"></param>
        /// <returns></returns>
        public async Task<OPERACAO_PONTO> BuscarPontoPorId(long IdOperacao)
        {
            using var connection = new SqlConnection(_connectionString);
            var result = await connection?.QueryAsync<OPERACAO_PONTO>(OPERACAO_PONTO.Query.SelectPorId, new { @IdOperacao = IdOperacao }, commandType: CommandType.StoredProcedure);
            return result.FirstOrDefault();
        }
    }
}

using Api.PontoDigital.Models.SQL;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using Microsoft.Extensions.Configuration;
using System.Data;
using System.Collections.Generic;

namespace Api.PontoDigital.Repository.PessoaJuridica
{
    /// <summary>
    /// PessoaJuridicaRepository
    /// </summary>
    public sealed class PessoaJuridicaRepository : IPessoaJuridicaRepository
    {
        private readonly string _connectionString;
        /// <summary>
        /// PessoaJuridicaRepository
        /// </summary>
        /// <param name="configuration"></param>
        public PessoaJuridicaRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString(configuration?.GetValue<string>("Enviroment"));
        }
        /// <summary>
        /// SelecionarTodos
        /// </summary>
        /// <returns></returns>
        public async Task<List<PESSOA_JURIDICA>> SelecionarTodos()
        {
            using var connection = new SqlConnection(_connectionString);
            var result = await connection?.QueryAsync<PESSOA_JURIDICA>(PESSOA_JURIDICA.Query.SelectAll, null, commandType: CommandType.StoredProcedure);
            return result.ToList();
        }
        /// <summary>
        /// SelecionarPorId
        /// </summary>
        /// <param name="IdPessoaJuridica"></param>
        /// <returns></returns>
        public async Task<PESSOA_JURIDICA> SelecionarPorId(long IdPessoaJuridica)
        {
            using var connection = new SqlConnection(_connectionString);
            var result = await connection?.QueryAsync<PESSOA_JURIDICA>(PESSOA_JURIDICA.Query.Select, new { IdPessoaJuridica }, commandType: CommandType.StoredProcedure);
            return result?.FirstOrDefault();
        }
        /// <summary>
        /// SelecionarPorCNPJ
        /// </summary>
        /// <param name="CNPJ"></param>
        /// <returns></returns>
        public async Task<PESSOA_JURIDICA> SelecionarPorCNPJ(string CNPJ)
        {
            using var connection = new SqlConnection(_connectionString);
            var result = await connection?.QueryAsync<PESSOA_JURIDICA>(PESSOA_JURIDICA.Query.CNPJ, new { CNPJ }, commandType: CommandType.StoredProcedure);
            return result?.FirstOrDefault();
        }
        /// <summary>
        /// Inserir
        /// </summary>
        /// <param name="PessoaJuridica"></param>
        /// <returns></returns>
        public async Task<PESSOA_JURIDICA> Inserir(PESSOA_JURIDICA PessoaJuridica)
        {
            using var connection = new SqlConnection(_connectionString);
            var result = await connection?.QueryAsync<PESSOA_JURIDICA>(PESSOA_JURIDICA.Query.Insert, PessoaJuridica, commandType: CommandType.StoredProcedure);
            return result?.FirstOrDefault();
        }
        /// <summary>
        /// Atualizar
        /// </summary>
        /// <param name="PessoaJuridica"></param>
        /// <returns></returns>
        public async Task<PESSOA_JURIDICA> Atualizar(PESSOA_JURIDICA PessoaJuridica)
        {
            using var connection = new SqlConnection(_connectionString);
            var result = await connection?.QueryAsync<PESSOA_JURIDICA>(PESSOA_JURIDICA.Query.Update, PessoaJuridica, commandType: CommandType.StoredProcedure);
            return result?.FirstOrDefault();
        }
    }
}

using Api.PontoDigital.Models.SQL;
using Dapper;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace Api.PontoDigital.Repository.PessoaFisica
{
    /// <summary>
    /// Repositório do Pessoa Fisíca
    /// </summary>
    public class PessoaFisicaRepository : IPessoaFisicaRepository
    {
        private readonly string _connectionString;
        /// <summary>
        /// PessoaFisicaRepository
        /// </summary>
        public PessoaFisicaRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString(configuration?.GetValue<string>("Enviroment"));
        }
        /// <summary>
        /// Query Selecionar Por Id
        /// </summary>
        /// <param name="IdPessoaFisica"></param>
        /// <returns></returns>
        public async Task<PESSOA_FISICA> SelecionarPorId(long IdPessoaFisica)
        {
            using var connection = new SqlConnection(_connectionString);
            var result = await connection?.QueryAsync<PESSOA_FISICA>(PESSOA_FISICA.Query.Select, new { @IdPessoaFisica = IdPessoaFisica }, commandType: CommandType.StoredProcedure);
            return result.FirstOrDefault();
        }
        /// <summary>
        /// Query Selecionar Por Nome
        /// </summary>
        /// <param name="Nome"></param>
        /// <returns></returns>
        public async Task<List<PESSOA_FISICA>> SelecionarPorNome(string Nome)
        {
            using var connection = new SqlConnection(_connectionString);
            var result = await connection?.QueryAsync<PESSOA_FISICA>(PESSOA_FISICA.Query.Nome, new { @Nome = Nome }, commandType: CommandType.StoredProcedure);
            return result.ToList();
        }
        /// <summary>
        /// Query Selecionar Por CPF
        /// </summary>
        /// <param name="CPF"></param>
        /// <returns></returns>
        public async Task<PESSOA_FISICA> SelecionarPorCPF(string CPF)
        {
            using var connection = new SqlConnection(_connectionString);
            var result = await connection?.QueryAsync<PESSOA_FISICA>(PESSOA_FISICA.Query.CPF, new { @CPF = CPF }, commandType: CommandType.StoredProcedure);
            return result.FirstOrDefault();
        }
        /// <summary>
        /// Insert
        /// </summary>
        /// <param name="PessoaFisica"></param>
        /// <returns></returns>
        public async Task<PESSOA_FISICA> Inserir(PESSOA_FISICA PessoaFisica)
        {
            using var connection = new SqlConnection(_connectionString);
            var result = await connection?.QueryAsync<PESSOA_FISICA>(PESSOA_FISICA.Query.Insert, PessoaFisica, commandType: CommandType.StoredProcedure);
            return result.FirstOrDefault();
        }
        /// <summary>
        /// Update
        /// </summary>
        /// <param name="PessoaFisica"></param>
        /// <returns></returns>
        public async Task<PESSOA_FISICA> Atualizar(PESSOA_FISICA PessoaFisica)
        {
            using var connection = new SqlConnection(_connectionString);
            var result = await connection?.QueryAsync<PESSOA_FISICA>(PESSOA_FISICA.Query.Update, PessoaFisica, commandType: CommandType.StoredProcedure);
            return result.FirstOrDefault();
        }
    }
}

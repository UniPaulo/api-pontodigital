using Api.PontoDigital.Models.SQL;
using Dapper;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace Api.PontoDigital.Repository.PessoaJuridicaFisica
{
    /// <summary>
    /// PessoaJuridicaFisicaRepository
    /// </summary>
    public class PessoaJuridicaFisicaRepository : IPessoaJuridicaFisicaRepository
    {
        private readonly string _connectionString;
        /// <summary>
        /// PessoaFisicaRepository
        /// </summary>
        public PessoaJuridicaFisicaRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString(configuration?.GetValue<string>("Enviroment"));
        }
        /// <summary>
        /// Query Selecionar Por Id
        /// </summary>
        /// <param name="IdPessoaJuridica"></param>
        /// <param name="IdPessoaFisica"></param>
        /// <returns></returns>
        public async Task<PESSOA_JURIDICA_FISICA> SelecionarPorId(long IdPessoaJuridica, long IdPessoaFisica)
        {
            using var connection = new SqlConnection(_connectionString);
            var result = await connection?.QueryAsync<PESSOA_JURIDICA_FISICA>(PESSOA_JURIDICA_FISICA.Query.Select, new { IdPessoaJuridica, IdPessoaFisica }, commandType: CommandType.StoredProcedure);
            return result.FirstOrDefault();
        }
        /// <summary>
        /// SelecionarPorPessoaFisica
        /// </summary>
        /// <param name="IdPessoaFisica"></param>
        /// <returns></returns>
        public async Task<List<PESSOA_JURIDICA_FISICA>> SelecionarPorPessoaFisica(long IdPessoaFisica)
        {
            using var connection = new SqlConnection(_connectionString);
            var result = await connection?.QueryAsync<PESSOA_JURIDICA_FISICA>(PESSOA_JURIDICA_FISICA.Query.SelectPessoaFisica, new { IdPessoaFisica }, commandType: CommandType.StoredProcedure);
            return result.ToList();
        }
        /// <summary>
        /// Insert
        /// </summary>
        /// <param name="PessoaJuridicaFisica"></param>
        /// <returns></returns>
        public async Task<PESSOA_JURIDICA_FISICA> Inserir(PESSOA_JURIDICA_FISICA PessoaJuridicaFisica)
        {
            using var connection = new SqlConnection(_connectionString);
            var result = await connection?.QueryAsync<PESSOA_JURIDICA_FISICA>(PESSOA_JURIDICA_FISICA.Query.Insert, PessoaJuridicaFisica, commandType: CommandType.StoredProcedure);
            return result.FirstOrDefault();
        }
        /// <summary>
        /// Update
        /// </summary>
        /// <param name="PessoaJuridicaFisica"></param>
        /// <returns></returns>
        public async Task<PESSOA_JURIDICA_FISICA> Atualizar(PESSOA_JURIDICA_FISICA PessoaJuridicaFisica)
        {
            using var connection = new SqlConnection(_connectionString);
            var result = await connection?.QueryAsync<PESSOA_JURIDICA_FISICA>(PESSOA_JURIDICA_FISICA.Query.Update, PessoaJuridicaFisica, commandType: CommandType.StoredProcedure);
            return result.FirstOrDefault();
        }
    }
}

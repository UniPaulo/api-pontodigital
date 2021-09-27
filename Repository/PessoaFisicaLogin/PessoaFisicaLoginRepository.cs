using Api.PontoDigital.Models.SQL;
using Dapper;
using Microsoft.Extensions.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace Api.PontoDigital.Repository.PessoaFisicaLogin
{
    /// <summary>
    /// PessoaFisicaLoginRepository
    /// </summary>
    public class PessoaFisicaLoginRepository : IPessoaFisicaLoginRepository
    {
        private readonly string _connectionString;
        /// <summary>
        /// Configuração da connectionString
        /// </summary>
        /// <param name="configuration"></param>
        public PessoaFisicaLoginRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString(configuration?.GetValue<string>("Enviroment"));
        }
        /// <summary>
        /// Query de Selecionar Por Id Pessoa Fisica
        /// </summary>
        /// <param name="IdPessoaFisica"></param>
        /// <returns></returns>
        public async Task<PESSOA_FISICA_LOGIN> SelecionarPorIdPessoaFisica(long IdPessoaFisica)
        {
            using var connection = new SqlConnection(_connectionString);
            var result = await connection?.QueryAsync<PESSOA_FISICA_LOGIN>(PESSOA_FISICA_LOGIN.Query.SelectByIdPessoaFisica, new { IdPessoaFisica }, commandType: CommandType.StoredProcedure);
            return result.FirstOrDefault();
        }
        /// <summary>
        /// Insert
        /// </summary>
        /// <param name="objPFL"></param>
        /// <returns></returns>
        public async Task<PESSOA_FISICA_LOGIN> Inserir(PESSOA_FISICA_LOGIN objPFL)
        {
            using var connection = new SqlConnection(_connectionString);
            var result = await connection?.QueryAsync<PESSOA_FISICA_LOGIN>(PESSOA_FISICA_LOGIN.Query.Insert, objPFL, commandType: CommandType.StoredProcedure);
            return result.FirstOrDefault();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="objPFL"></param>
        /// <returns></returns>
        public async Task<PESSOA_FISICA_LOGIN> Atualizar(PESSOA_FISICA_LOGIN objPFL)
        {
            using var connection = new SqlConnection(_connectionString);
            var result = await connection?.QueryAsync<PESSOA_FISICA_LOGIN>(PESSOA_FISICA_LOGIN.Query.Update, objPFL, commandType: CommandType.StoredProcedure);
            return result.FirstOrDefault();
        }
    }
}

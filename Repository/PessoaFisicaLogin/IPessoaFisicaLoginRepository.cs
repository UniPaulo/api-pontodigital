using Api.PontoDigital.Models.SQL;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Api.PontoDigital.Repository.PessoaFisicaLogin
{
    /// <summary>
    /// IPessoaFisicaLoginRepository
    /// </summary>
    public interface IPessoaFisicaLoginRepository
    {
        /// <summary>
        /// SelecionarPorIdPessoaFisica
        /// </summary>
        /// <param name="IdPessoaFisica"></param>
        /// <returns></returns>
        Task<PESSOA_FISICA_LOGIN> SelecionarPorIdPessoaFisica(long IdPessoaFisica);
        /// <summary>
        /// Inserir
        /// </summary>
        /// <param name="objPFL"></param>
        /// <returns></returns>
        Task<PESSOA_FISICA_LOGIN> Inserir(PESSOA_FISICA_LOGIN objPFL);
        /// <summary>
        /// Atualizar
        /// </summary>
        /// <param name="objPFL"></param>
        /// <returns></returns>
        Task<PESSOA_FISICA_LOGIN> Atualizar(PESSOA_FISICA_LOGIN objPFL);
    }
}

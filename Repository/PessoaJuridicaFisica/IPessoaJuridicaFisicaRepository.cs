using Api.PontoDigital.Models.SQL;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Api.PontoDigital.Repository.PessoaJuridicaFisica
{
    /// <summary>
    /// IPessoaJuridicaFisicaRepository
    /// </summary>
    public interface IPessoaJuridicaFisicaRepository
    {
        /// <summary>
        /// SelecionarPorId
        /// </summary>
        /// <param name="IdPessoaJuridica"></param>
        /// <param name="IdPessoaFisica"></param>
        /// <returns></returns>
        Task<PESSOA_JURIDICA_FISICA> SelecionarPorId(long IdPessoaJuridica, long IdPessoaFisica);
        /// <summary>
        /// SelecionarPorPessoaFisica
        /// </summary>
        /// <param name="IdPessoaFisica"></param>
        /// <returns></returns>
        Task<List<PESSOA_JURIDICA_FISICA>> SelecionarPorPessoaFisica(long IdPessoaFisica);
        /// <summary>
        /// Inserir
        /// </summary>
        /// <param name="PessoaFisica"></param>
        /// <returns></returns>
        Task<PESSOA_JURIDICA_FISICA> Inserir(PESSOA_JURIDICA_FISICA PessoaFisica);
        /// <summary>
        /// Atualizar
        /// </summary>
        /// <param name="PessoaFisica"></param>
        /// <returns></returns>
        Task<PESSOA_JURIDICA_FISICA> Atualizar(PESSOA_JURIDICA_FISICA PessoaFisica);
    }
}

using Api.PontoDigital.Models.SQL;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Api.PontoDigital.Repository.PessoaFisica
{
    /// <summary>
    /// IPessoaFisicaRepository
    /// </summary>
    public interface IPessoaFisicaRepository
    {
        /// <summary>
        /// SelecionarPorId
        /// </summary>
        /// <param name="IdPessoaFisica"></param>
        /// <returns></returns>
        Task<PESSOA_FISICA> SelecionarPorId(long IdPessoaFisica);
        /// <summary>
        /// SelecionarPorNome
        /// </summary>
        /// <param name="Nome"></param>
        /// <returns></returns>
        Task<List<PESSOA_FISICA>> SelecionarPorNome(string Nome);
        /// <summary>
        /// SelecionarPorCPF
        /// </summary>
        /// <param name="CPF"></param>
        /// <returns></returns>
        Task<PESSOA_FISICA> SelecionarPorCPF(string CPF);
        /// <summary>
        /// Inserir
        /// </summary>
        /// <param name="PessoaFisica"></param>
        /// <returns></returns>
        Task<PESSOA_FISICA> Inserir(PESSOA_FISICA PessoaFisica);
        /// <summary>
        /// Atualizar
        /// </summary>
        /// <param name="PessoaFisica"></param>
        /// <returns></returns>
        Task<PESSOA_FISICA> Atualizar(PESSOA_FISICA PessoaFisica);
    }
}

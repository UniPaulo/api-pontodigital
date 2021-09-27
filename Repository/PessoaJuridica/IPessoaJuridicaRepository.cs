using Api.PontoDigital.Models.SQL;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Api.PontoDigital.Repository.PessoaJuridica
{
    /// <summary>
    /// IPessoaJuridicaRepository
    /// </summary>
    public interface IPessoaJuridicaRepository
    {
        /// <summary>
        /// SelecionarTodos
        /// </summary>
        /// <returns></returns>
        Task<List<PESSOA_JURIDICA>> SelecionarTodos();
        /// <summary>
        /// SelecionarPorId
        /// </summary>
        /// <param name="IdPessoaJuridica"></param>
        /// <returns></returns>
        Task<PESSOA_JURIDICA> SelecionarPorId(long IdPessoaJuridica);
        /// <summary>
        /// SelecionarPorCNPJ
        /// </summary>
        /// <param name="CNPJ"></param>
        /// <returns></returns>
        Task<PESSOA_JURIDICA> SelecionarPorCNPJ(string CNPJ);
        /// <summary>
        /// Inserir
        /// </summary>
        /// <param name="PessoaJuridica"></param>
        /// <returns></returns>
        Task<PESSOA_JURIDICA> Inserir(PESSOA_JURIDICA PessoaJuridica);
        /// <summary>
        /// Atualizar
        /// </summary>
        /// <param name="PessoaJuridica"></param>
        /// <returns></returns>
        Task<PESSOA_JURIDICA> Atualizar(PESSOA_JURIDICA PessoaJuridica);
    }
}

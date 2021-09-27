using Newtonsoft.Json;
using System;
using System.ComponentModel.DataAnnotations;

namespace Api.PontoDigital.Models.API
{
    /// <summary>
    /// PessoaJuridica
    /// </summary>
    [JsonObject]
    [Serializable]
    public class PessoaJuridica
    {
        /// <summary>
        /// IdPessoaJuridica
        /// </summary>
        [Display(Name = "Id da Pessoa Jurídica"), Required(ErrorMessage = "Obrigatório informar dados em {0}.")]
        public int IdPessoaJuridica { get; set; }
        /// <summary>
        /// Razão Social
        /// </summary>
        [Display(Name = "Razão Social"), Required(ErrorMessage = "Obrigatório informar dados em {0}.")]
        public string RazaoSocial { get; set; }
        /// <summary>
        /// CNPJ
        /// </summary>
        [Display(Name = "CNPJ"), Required(ErrorMessage = "Obrigatório informar dados em {0}.")]
        public string CNPJ { get; set; }
    }
}

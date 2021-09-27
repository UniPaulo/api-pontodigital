using Newtonsoft.Json;
using System;
using System.ComponentModel.DataAnnotations;

namespace Api.PontoDigital.Models.API
{
    /// <summary>
    /// Ponto
    /// </summary>
    [JsonObject]
    [Serializable]
    public class Ponto
    {
        /// <summary>
        /// Id da Pessoa Fisica
        /// </summary>
        [Display(Name = "Id da Pessoa Fisíca"), Required(ErrorMessage = "Obrigatório informar dados em {0}.")]
        public long IdPessoaFisica { get; set; }
        /// <summary>
        /// Id da Pessoa Juridica
        /// </summary>
        [Display(Name = "Id da Pessoa Jurídica"), Required(ErrorMessage = "Obrigatório informar dados em {0}.")]
        public long IdPessoaJuridica { get; set; }
    }
}

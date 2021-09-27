using Newtonsoft.Json;
using System;
using System.ComponentModel.DataAnnotations;

namespace Api.PontoDigital.Models.API
{
    /// <summary>
    /// Autenticar
    /// </summary>
    [JsonObject]
    [Serializable]
    public class Autenticar
    {
        /// <summary>
        /// CPF
        /// </summary>
        [Display(Name = "CPF"), Required(ErrorMessage = "Obrigatório informar dados em {0}.")]
        public string CPF { get; set; }
        /// <summary>
        /// Senha
        /// </summary>
        [Display(Name = "Senha"), Required(ErrorMessage = "Obrigatório informar dados em {0}.")]
        public string Senha { get; set; }
        /// <summary>
        /// Id da Pessoa Juridica
        /// </summary>
        [Display(Name = "Id da Pessoa Jurídica"), Required(ErrorMessage = "Obrigatório informar dados em {0}.")]
        public long IdPessoaJuridica { get; set; }

    }
}

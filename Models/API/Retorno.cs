using Newtonsoft.Json;
using System;
using System.ComponentModel.DataAnnotations;

namespace Api.PontoDigital.Models.API
{
    /// <summary>
    /// Sucesso
    /// </summary>
    [JsonObject]
    [Serializable]
    public class Retorno
    {
        /// <summary>
        /// Mensagem de Retorno
        /// </summary>
        [Display(Name = "Mensagem"), Required(ErrorMessage = "Obrigatório informar dados em {0}.")]
        public string Mensagem { get; set; }
    }
}

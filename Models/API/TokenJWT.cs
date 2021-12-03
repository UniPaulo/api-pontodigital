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
    public class TokenJWT
    {
        /// <summary>
        /// CPF
        /// </summary>
        [Display(Name = "Token"), Required(ErrorMessage = "Obrigatório informar dados em {0}.")]
        public string Token { get; set; }
    }
}

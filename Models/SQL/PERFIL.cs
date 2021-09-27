using Newtonsoft.Json;
using System;
using System.ComponentModel.DataAnnotations;

namespace Api.PontoDigital.Models.SQL
{
    /// <summary>
    /// PERFIL
    /// </summary>
    [JsonObject]
    [Serializable]
    public class PERFIL
    {
        /// <summary>
        /// IdPerfil
        /// </summary>
        [Display(Name = "Id do Perfil"), Required(ErrorMessage = "Obrigatório informar dados em {0}.")]
        public int IdPerfil { get; set; }
        /// <summary>
        /// Status
        /// </summary>
        [Display(Name = "Status"), Required(ErrorMessage = "Obrigatório informar dados em {0}.")]
        public string Status { get; set; }
    }
}

using Newtonsoft.Json;
using System;
using System.ComponentModel.DataAnnotations;

namespace Api.PontoDigital.Models.API
{
    /// <summary>
    /// Empresa
    /// </summary>
    [JsonObject]
    [Serializable]
    public class Empresa
    {
        /// <summary>
        /// Id da Empresa
        /// </summary>
        [Display(Name = "Id da Empresa"), Required(ErrorMessage = "Obrigatório informar dados em {0}.")]
        public long Id { get; set; }
        /// <summary>
        /// Nome da Empresa
        /// </summary>
        [Display(Name = "Nome"), Required(ErrorMessage = "Obrigatório informar dados em {0}.")]
        public string Nome { get; set; }
    }
}

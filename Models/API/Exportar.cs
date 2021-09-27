using Newtonsoft.Json;
using System;
using System.ComponentModel.DataAnnotations;

namespace Api.PontoDigital.Models.API
{
    /// <summary>
    /// Relatorio
    /// </summary>
    [JsonObject]
    [Serializable]
    public class Exportar
    {
        /// <summary>
        /// CPF
        /// </summary>
        [Display(Name = "CPF"), Required(ErrorMessage = "Obrigatório informar dados em {0}.")]
        public string CPF { get; set; }
        /// <summary>
        /// Nome
        /// </summary>
        [Display(Name = "Nome"), Required(ErrorMessage = "Obrigatório informar dados em {0}.")]
        public string NOME { get; set; }
        /// <summary>
        /// Data de Hoje
        /// </summary>
        [Display(Name = "Data de Hoje"), Required(ErrorMessage = "Obrigatório informar dados em {0}.")]
        public string DATA_INICIO { get; set; }
        /// <summary>
        /// Data Inicio Expediente
        /// </summary>
        [Display(Name = "Data Inicio Expediente"), Required(ErrorMessage = "Obrigatório informar dados em {0}.")]
        public string DATA_INICIO_EXPEDIENTE { get; set; }
        /// <summary>
        /// Data Inicio do Intervalo
        /// </summary>
        [Display(Name = "Data Inicio do Intervalo"), Required(ErrorMessage = "Obrigatório informar dados em {0}.")]
        public string DATA_INICIO_INTERVALO { get; set; }
        /// <summary>
        /// Data Fim do Intervalo
        /// </summary>
        [Display(Name = "Data Fim do Intervalo"), Required(ErrorMessage = "Obrigatório informar dados em {0}.")]
        public string DATA_FIM_INTERVALO { get; set; }
        /// <summary>
        /// Data Fim do Intervalo
        /// </summary>
        [Display(Name = "Data Fim do Intervalo"), Required(ErrorMessage = "Obrigatório informar dados em {0}.")]
        public string DATA_FIM_EXPEDIENTE { get; set; }
        /// <summary>
        /// Carga Horaria
        /// </summary>
        [Display(Name = "Carga Horaria"), Required(ErrorMessage = "Obrigatório informar dados em {0}.")]
        public string CARGA_HORARIA { get; set; }
        /// <summary>
        /// Hora Extra
        /// </summary>
        [Display(Name = "Hora Extra"), Required(ErrorMessage = "Obrigatório informar dados em {0}.")]
        public string HORA_EXTRA { get; set; }
    }
}

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
    public class Relatorio
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
        public string Nome { get; set; }
        /// <summary>
        /// Data de Hoje
        /// </summary>
        [Display(Name = "Data de Hoje"), Required(ErrorMessage = "Obrigatório informar dados em {0}.")]
        public string Data { get; set; }
        /// <summary>
        /// Data Inicio Expediente
        /// </summary>
        [Display(Name = "Data Inicio Expediente"), Required(ErrorMessage = "Obrigatório informar dados em {0}.")]
        public string DataInicioExpediente { get; set; }
        /// <summary>
        /// Data Inicio do Intervalo
        /// </summary>
        [Display(Name = "Data Inicio do Intervalo"), Required(ErrorMessage = "Obrigatório informar dados em {0}.")]
        public string DataInicioIntervalo { get; set; }
        /// <summary>
        /// Data Fim do Intervalo
        /// </summary>
        [Display(Name = "Data Fim do Intervalo"), Required(ErrorMessage = "Obrigatório informar dados em {0}.")]
        public string DataFimIntervalo { get; set; }
        /// <summary>
        /// Data Fim do Intervalo
        /// </summary>
        [Display(Name = "Data Fim do Intervalo"), Required(ErrorMessage = "Obrigatório informar dados em {0}.")]
        public string DataFimExpediente { get; set; }
        /// <summary>
        /// Carga Horaria
        /// </summary>
        [Display(Name = "Carga Horaria"), Required(ErrorMessage = "Obrigatório informar dados em {0}.")]
        public string CargaHoraria { get; set; }
        /// <summary>
        /// Hora Extra
        /// </summary>
        [Display(Name = "Hora Extra"), Required(ErrorMessage = "Obrigatório informar dados em {0}.")]
        public string HoraExtra { get; set; }
        /// <summary>
        /// Link do Excel
        /// </summary>
        [Display(Name = "Link do Excel"), Required(ErrorMessage = "Obrigatório informar dados em {0}.")]
        public string Excel { get; set; }
    }
}

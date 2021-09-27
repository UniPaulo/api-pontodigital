using Newtonsoft.Json;
using System;
using System.ComponentModel.DataAnnotations;

namespace Api.PontoDigital.Models.API
{
    /// <summary>
    /// PessoaFisica
    /// </summary>
    [JsonObject]
    [Serializable]
    public class PessoaFisica
    {
        /// <summary>
        /// Id da Pessoa Fisica
        /// </summary>
        [Display(Name = "Id da Pessoa Fisíca")]
        public long IdPessoaFisica { get; set; }
        /// <summary>
        /// Nome
        /// </summary>
        [Display(Name = "Nome"), Required(ErrorMessage = "Obrigatório informar dados em {0}.")]
        public string Nome { get; set; }
        /// <summary>
        /// CPF
        /// </summary>
        [Display(Name = "CPF"), Required(ErrorMessage = "Obrigatório informar dados em {0}.")]
        public string CPF { get; set; }
        /// <summary>
        /// Ocupacao
        /// </summary>
        [Display(Name = "Ocupacao")]
        public string Ocupacao { get; set; }
        /// <summary>
        /// DataHoraInicioExpediente
        /// </summary>
        [Display(Name = "Data e Hora do Inicio do Expediente")]
        public string DataHoraInicioExpediente { get; set; }
        /// <summary>
        /// DataHoraInicioIntervalo
        /// </summary>
        [Display(Name = "Data e Hora do Inicio do Intervalo (Almoço)")]
        public string DataHoraInicioIntervalo { get; set; }
        /// <summary>
        /// DataHoraFimIntervalo
        /// </summary>
        [Display(Name = "Data e Hora do Fim do Intervalo (Almoço)")]
        public string DataHoraFimIntervalo { get; set; }
        /// <summary>
        /// DataHoraFimExpediente
        /// </summary>
        [Display(Name = "Data e Hora do Fim do Expediente")]
        public string DataHoraFimExpediente { get; set; }
        /// <summary>
        /// IdPessoaJuridica
        /// </summary>
        [Display(Name = "Id da Pessoa Jurídica"), Required(ErrorMessage = "Obrigatório informar dados em {0}.")]
        public long IdPessoaJuridica { get; set; }
        /// <summary>
        /// Status
        /// </summary>
        [Display(Name = "Status"), Required(ErrorMessage = "Obrigatório informar dados em {0}.")]
        public string Status { get; set; }
        /// <summary>
        /// CodigoPerfil
        /// </summary>
        [Display(Name = "CodigoPerfil"), Required(ErrorMessage = "Obrigatório informar dados em {0}.")]
        public int? CodigoPerfil { get; set; }
        /// <summary>
        /// Senha
        /// </summary>
        [Display(Name = "Senha"), Required(ErrorMessage = "Obrigatório informar dados em {0}.")]
        public string Senha { get; set; }
        /// <summary>
        /// Hoje
        /// </summary>
        [Display(Name = "Hoje")]
        public string Hoje { get; set; }

    }
}

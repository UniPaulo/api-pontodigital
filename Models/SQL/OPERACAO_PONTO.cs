using Newtonsoft.Json;
using System;
using System.ComponentModel.DataAnnotations;

namespace Api.PontoDigital.Models.SQL
{
    /// <summary>
    /// OPERACAO_PONTO
    /// </summary>
    [JsonObject]
    [Serializable]
    public class OPERACAO_PONTO
    {
        /// <summary>
        /// Id da Operacao
        /// </summary>
        [Display(Name = "Id da Operacao"), Required(ErrorMessage = "Obrigatório informar dados em {0}.")]
        public long IdOperacao { get; set; }
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
        /// <summary>
        /// DataHoraInicioExpediente
        /// </summary>
        [Display(Name = "Data e Hora do Inicio do Expediente")]
        public DateTime? DataHoraInicioExpediente { get; set; }
        /// <summary>
        /// DataHoraInicioIntervalo
        /// </summary>
        [Display(Name = "Data e Hora do Inicio do Intervalo (Almoço)")]
        public DateTime? DataHoraInicioIntervalo { get; set; }
        /// <summary>
        /// DataHoraFimIntervalo
        /// </summary>
        [Display(Name = "Data e Hora do Fim do Intervalo (Almoço)")]
        public DateTime? DataHoraFimIntervalo { get; set; }
        /// <summary>
        /// DataHoraFimExpediente
        /// </summary>
        [Display(Name = "Data e Hora do Fim do Expediente")]
        public DateTime? DataHoraFimExpediente { get; set; }
        /// <summary>
        /// CargaHoraria
        /// </summary>
        [Display(Name = "Carga Horaria")]
        public DateTime? CargaHoraria { get; set; }
        /// <summary>
        /// StoredProcedures
        /// </summary>
        public struct Query
        {
            /// <summary>
            /// SelectIdPessoaFisica
            /// </summary>
            public const string SelectIdPessoaFisica = "prQ_OP";
            /// <summary>
            /// SelectPorId
            /// </summary>
            public const string SelectPorId = "prQ_OP_Id";
            /// <summary>
            /// Insert
            /// </summary>
            public const string Insert = "prI_OP";
            /// <summary>
            /// Update
            /// </summary>
            public const string Update = "prU_OP";
        }
    }
}

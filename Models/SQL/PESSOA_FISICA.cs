using Newtonsoft.Json;
using System;
using System.ComponentModel.DataAnnotations;

namespace Api.PontoDigital.Models.SQL
{
    /// <summary>
    /// PESSOA_FISICA
    /// </summary>
    [JsonObject]
    [Serializable]
    public class PESSOA_FISICA
    {
        /// <summary>
        /// Id da Pessoa Fisica
        /// </summary>
        [Display(Name = "Id da Pessoa Fisíca"), Required(ErrorMessage = "Obrigatório informar dados em {0}.")]
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
        /// Data e Hora do Cadastro
        /// </summary>
        [Display(Name = "Data e Hora do Cadastro")]
        public DateTime? DataHoraCadastro { get; set; }
        /// <summary>
        /// Data e Hora do Inicio do Expediente
        /// </summary>
        [Display(Name = "Data e Hora do Inicio do Expediente")]
        public DateTime? DataHoraInicioExpediente { get; set; }
        /// <summary>
        /// Data e Hora do Inicio do Intervalo
        /// </summary>
        [Display(Name = "Data e Hora do Inicio do Intervalo (Almoço)")]
        public DateTime? DataHoraInicioIntervalo { get; set; }
        /// <summary>
        /// Data e Hora do Fim do Intervalo
        /// </summary>
        [Display(Name = "Data e Hora do Fim do Intervalo (Almoço)")]
        public DateTime? DataHoraFimIntervalo { get; set; }
        /// <summary>
        /// Data e Hora do Fim do Expediente
        /// </summary>
        [Display(Name = "Data e Hora do Fim do Expediente")]
        public DateTime? DataHoraFimExpediente { get; set; }
        /// <summary>
        /// Procs
        /// </summary>
        public struct Query
        {
            /// <summary>
            /// Select
            /// </summary>
            public const string Select = "prQ_PF_Id";
            /// <summary>
            /// Insert
            /// </summary>
            public const string Insert = "prI_PF";
            /// <summary>
            /// Update
            /// </summary>
            public const string Update = "prU_PF";
            /// <summary>
            /// CPF
            /// </summary>
            public const string CPF = "prQ_PF_CPF";
            /// <summary>
            /// Nome
            /// </summary>
            public const string Nome = "prQ_PF_Nome";
        }
    }
}

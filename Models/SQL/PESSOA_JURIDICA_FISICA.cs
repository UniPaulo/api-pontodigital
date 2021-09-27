using Newtonsoft.Json;
using System;
using System.ComponentModel.DataAnnotations;

namespace Api.PontoDigital.Models.SQL
{
    /// <summary>
    /// PESSOA_JURIDICA_FISICA
    /// </summary>
    [JsonObject]
    [Serializable]
    public class PESSOA_JURIDICA_FISICA
    {
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
        /// Status
        /// </summary>
        [Display(Name = "Status"), Required(ErrorMessage = "Obrigatório informar dados em {0}.")]
        public string Status { get; set; }
        /// <summary>
        /// Procs
        /// </summary>
        public struct Query
        {
            /// <summary>
            /// Select
            /// </summary>
            public const string SelectPessoaFisica = "prQ_PJF_PF";
            /// <summary>
            /// Select
            /// </summary>
            public const string Select = "prQ_PJF";
            /// <summary>
            /// Insert
            /// </summary>
            public const string Insert = "prI_PJF";
            /// <summary>
            /// Update
            /// </summary>
            public const string Update = "prU_PJF";
        }
        /// <summary>
        /// Status
        /// </summary>
        public struct StatusValores
        {
            /// <summary>
            /// Ativo
            /// </summary>
            public const string Ativo = "A";
            /// <summary>
            /// Inativo
            /// </summary>
            public const string Inativo = "I";
        }
    }
}

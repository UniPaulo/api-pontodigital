using Newtonsoft.Json;
using System;
using System.ComponentModel.DataAnnotations;

namespace Api.PontoDigital.Models.SQL
{
    /// <summary>
    /// PESSOA_JURIDICA
    /// </summary>
    [JsonObject]
    [Serializable]
    public class PESSOA_JURIDICA
    {
        /// <summary>
        /// Id da Pessoa Juridica
        /// </summary>
        [Display(Name = "Id da Pessoa Jurídica"), Required(ErrorMessage = "Obrigatório informar dados em {0}.")]
        public int IdPessoaJuridica { get; set; }
        /// <summary>
        /// Razao Social
        /// </summary>
        [Display(Name = "Razão Social"), Required(ErrorMessage = "Obrigatório informar dados em {0}.")]
        public string RazaoSocial { get; set; }
        /// <summary>
        /// CNPJ
        /// </summary>
        [Display(Name = "CNPJ"), Required(ErrorMessage = "Obrigatório informar dados em {0}.")]
        public string CNPJ { get; set; }
        /// <summary>
        /// DataHoraCadastro
        /// </summary>
        [Display(Name = "Data e Hora do Cadastro")]
        public DateTime? DataHoraCadastro { get; set; }
        /// <summary>
        /// Procs
        /// </summary>
        public struct Query
        {
            /// <summary>
            /// Select
            /// </summary>
            public const string SelectAll = "prQ_PJ";
            /// <summary>
            /// Select
            /// </summary>
            public const string Select = "prQ_PJ_Id";
            /// <summary>
            /// Insert
            /// </summary>
            public const string Insert = "prI_PJ";
            /// <summary>
            /// Update
            /// </summary>
            public const string Update = "prU_PJ";
            /// <summary>
            /// CNPJ
            /// </summary>
            public const string CNPJ = "prQ_PJ_CNPJ";
        }
    }

}

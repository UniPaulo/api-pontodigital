using Newtonsoft.Json;
using System;
using System.ComponentModel.DataAnnotations;

namespace Api.PontoDigital.Models.SQL
{
    /// <summary>
    /// PESSOA_FISICA_LOGIN
    /// </summary>
    [JsonObject]
    [Serializable]
    public class PESSOA_FISICA_LOGIN
    {
        /// <summary>
        /// Id da Pessoa Fisica
        /// </summary>
        [Display(Name = "Id da Pessoa Fisíca"), Required(ErrorMessage = "Obrigatório informar dados em {0}.")]
        public long IdPessoaFisica { get; set; }
        /// <summary>
        /// Id do Perfil
        /// </summary>
        [Display(Name = "Id do Perfil"), Required(ErrorMessage = "Obrigatório informar dados em {0}.")]
        public int IdPerfil { get; set; }
        /// <summary>
        /// Senha
        /// </summary>
        [Display(Name = "Senha"), Required(ErrorMessage = "Obrigatório informar dados em {0}.")]
        public string Senha { get; set; }
        /// <summary>
        /// Procs
        /// </summary>
        public struct Query
        {
            /// <summary>
            /// SelectByIdPessoaFisica
            /// </summary>
            public const string SelectByIdPessoaFisica = "prQ_PFL_IdPessoaFisica";
            /// <summary>
            /// Insert
            /// </summary>
            public const string Insert = "prI_PFL";
            /// <summary>
            /// Update
            /// </summary>
            public const string Update = "prU_PFL";
        }
    }
}

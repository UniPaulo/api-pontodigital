using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static Api.PontoDigital.Class.FUNCOES_UTEIS;

namespace Api.PontoDigital.Class
{
	/// <summary>
	/// IFUNCOES_UTEIS
	/// </summary>
	public interface IFUNCOES_UTEIS
    {
		/// <summary>
		/// FormatString
		/// </summary>
		/// <param name="strValorFormatar"></param>
		/// <param name="strFormato"></param>
		/// <returns></returns>
		string FormatString(string strValorFormatar, MASCARA_FORMATO strFormato);
		/// <summary>
		/// ValidarCpf
		/// </summary>
		/// <param name="strCPF"></param>
		/// <returns></returns>
		bool ValidarCpf(string strCPF);
		/// <summary>
		/// ValidaCNPJ
		/// </summary>
		/// <param name="cnpj"></param>
		/// <returns></returns>
		bool ValidaCNPJ(string cnpj);
	}
}

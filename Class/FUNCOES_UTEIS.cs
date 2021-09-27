using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Amazon;
using Amazon.S3;
using Amazon.S3.Model;
using Amazon.S3.Transfer;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;

namespace Api.PontoDigital.Class
{
	/// <summary>
	/// FUNCOES_UTEIS
	/// </summary>
	public class FUNCOES_UTEIS
    {
		/// <summary>
		/// MASCARA_FORMATO
		/// </summary>
		public enum MASCARA_FORMATO
        {
			/// <summary>
			/// Data_DD_MM_YYYY
			/// </summary>
			Data_DD_MM_YYYY,
			/// <summary>
			/// Data_YYYY_MM_DDM
			/// </summary>
			Data_YYYY_MM_DDM,
			/// <summary>
			/// CNPJ
			/// </summary>
			CNPJ,
			/// <summary>
			/// CPF
			/// </summary>
			CPF
		}
		/// <summary>
		/// FormatString
		/// </summary>
		/// <param name="strValorFormatar"></param>
		/// <param name="strFormato"></param>
		/// <returns></returns>
		public static string FormatString(string strValorFormatar, MASCARA_FORMATO strFormato)
        {
            try
            {
                if (String.IsNullOrWhiteSpace(strValorFormatar))
                {
                    return strValorFormatar;
                }
                switch (strFormato)
                {
                    case MASCARA_FORMATO.CNPJ:
                        return string.Format("{0}.{1}.{2}/{3}-{4}", strValorFormatar.Substring(0, 2), strValorFormatar.Substring(2, 3), strValorFormatar.Substring(5, 3), strValorFormatar.Substring(8, 4), strValorFormatar.Substring(12, 2));
                    case MASCARA_FORMATO.CPF:
                        return string.Format("{0}.{1}.{2}-{3}", strValorFormatar.Substring(0, 3), strValorFormatar.Substring(3, 3), strValorFormatar.Substring(6, 3), strValorFormatar.Substring(9, 2));
                    case MASCARA_FORMATO.Data_DD_MM_YYYY:
                        if (Convert.ToDateTime(strValorFormatar) == Convert.ToDateTime("1/1/1900"))
                            return string.Empty;
                        else
                            return Convert.ToDateTime(strValorFormatar).ToString("dd/MM/yyyy");
                    case MASCARA_FORMATO.Data_YYYY_MM_DDM:
                        if (Convert.ToDateTime(strValorFormatar) == Convert.ToDateTime("1/1/1900"))
                            return string.Empty;
                        else
                            return Convert.ToDateTime(strValorFormatar).ToString("yyyyMMdd");              
                    default:
                        return strValorFormatar;
                }
            }
            catch
            {
                return strValorFormatar;
            }
        }
		/// <summary>
		/// ValidarCpf
		/// </summary>
		/// <param name="strCPF"></param>
		/// <returns></returns>
		public static bool ValidarCpf(string strCPF)
		{
            strCPF = strCPF.Trim();
            string strValor = strCPF.Replace(".", "").Replace("-", "");
            if (strValor.Length != 11)
				return false;

			bool igual = true;
			for (int i = 1; i < 11 && igual; i++)
			{
				if (strValor[i] != strValor[0])
					igual = false;
			}

			if (igual || strValor == "12345678909")
				return false;

			int[] numeros = new int[11];
			for (int i = 0; i < 11; i++)
			{
				numeros[i] = int.Parse(strValor[i].ToString());
			}

			int soma = 0;
			for (int i = 0; i < 9; i++)
			{
				soma += (10 - i) * numeros[i];
			}

			int resultado = soma % 11;
			if (resultado == 1 || resultado == 0)
			{
				if (numeros[9] != 0)
					return false;
			}
			else if (numeros[9] != 11 - resultado)
				return false;

			soma = 0;
			for (int i = 0; i < 10; i++)
			{
				soma += (11 - i) * numeros[i];
			}

			resultado = soma % 11;
			if (resultado == 1 || resultado == 0)
			{
				if (numeros[10] != 0)
					return false;
			}
			else if (numeros[10] != 11 - resultado)
				return false;

			return true;
		}
		/// <summary>
		/// ValidaCNPJ
		/// </summary>
		/// <param name="cnpj"></param>
		/// <returns></returns>
		public static bool ValidaCNPJ(string cnpj)
		{
			string CNPJ = cnpj.Trim().Replace(".", "");
			CNPJ = CNPJ.Replace("/", "");
			CNPJ = CNPJ.Replace("-", "");
			int[] digitos, soma, resultado; int nrDig; string ftmt; bool[] CNPJOk;
			ftmt = "6543298765432";
			digitos = new int[14];
			soma = new int[2];
			soma[0] = 0;
			soma[1] = 0;
			resultado = new int[2];
			resultado[0] = 0;
			resultado[1] = 0;
			CNPJOk = new bool[2];
			CNPJOk[0] = false;
			CNPJOk[1] = false;

			try
			{
				for (nrDig = 0; nrDig < 14; nrDig++)
				{
					digitos[nrDig] = int.Parse(CNPJ.Substring(nrDig, 1));
					if (nrDig <= 11) soma[0] += (digitos[nrDig] * int.Parse(ftmt.Substring(nrDig + 1, 1)));
					if (nrDig <= 12) soma[1] += (digitos[nrDig] * int.Parse(ftmt.Substring(nrDig, 1)));
				}
				for (nrDig = 0; nrDig < 2; nrDig++)
				{
					resultado[nrDig] = (soma[nrDig] % 11);
					if ((resultado[nrDig] == 0) || (resultado[nrDig] == 1))
						CNPJOk[nrDig] = (digitos[12 + nrDig] == 0);
					else
						CNPJOk[nrDig] = (digitos[12 + nrDig] == (11 - resultado[nrDig]));
				}
				return (CNPJOk[0] && CNPJOk[1]);
			}
			catch
			{
				return false;
			}
		}		
	}
}

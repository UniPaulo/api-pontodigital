using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Api.PontoDigital.Class
{
	/// <summary>
	/// IExportarExcel
	/// </summary>
	public interface IExportarExcel
    {
		/// <summary>
		/// ExportarExcelAsync
		/// </summary>
		/// <param name="json"></param>
		/// <returns></returns>
		Task<string> ExportarExcelAsync(object json);
		/// <summary>
		/// GetNomeColunaByIndex
		/// </summary>
		/// <param name="numColuna"></param>
		/// <returns></returns>
		string GetNomeColunaByIndex(int numColuna);
		/// <summary>
		/// UploadFile
		/// </summary>
		/// <param name="filePath"></param>
		/// <returns></returns>
		Task<string> UploadFile(string filePath);
	}
}

using Amazon;
using Amazon.S3;
using Amazon.S3.Model;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Data;
using System.IO;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;

namespace Api.PontoDigital.Class
{
	/// <summary>
	/// ExportarExcel
	/// </summary>
	public class ExportarExcel : IExportarExcel
	{
		private readonly string _bucketName;
		private readonly string _keyName;
		private readonly string _secretName;
		/// <summary>
		/// ExportarExcel
		/// </summary>
		/// <param name="configuration"></param>
		public ExportarExcel(IConfiguration configuration)
		{
			_bucketName = Encoding.UTF8.GetString(Convert.FromBase64String(configuration?.GetValue<string>("Bucket")));
			_keyName = Encoding.UTF8.GetString(Convert.FromBase64String(configuration?.GetValue<string>("Key")));
			_secretName = Encoding.UTF8.GetString(Convert.FromBase64String(configuration?.GetValue<string>("Secret")));
		}
		/// <summary>
		/// ExportarExcelAsync
		/// </summary>
		/// <param name="json"></param>
		/// <returns></returns>
		public async Task<string> ExportarExcelAsync(object json)
		{
			string strJSON = JsonConvert.SerializeObject(json);
			strJSON = strJSON.Replace("_", " ");

			var dt = (DataTable)JsonConvert.DeserializeObject(strJSON, (typeof(DataTable)));
			dt.TableName = $"RelatorioPonto{DateTime.Now:dd-MM-yyyy HH-mm}";
			dt.Columns.Remove("Excel");

			MemoryStream sMemoryStream = new MemoryStream();

			var wb = new ClosedXML.Excel.XLWorkbook();
			var ws = wb.Worksheets.Add(dt);

			if (dt.Columns != null)
			{
				int StartIndexData = 2;
				int linha = 1;


				for (int i = 0; i < dt.Rows.Count; i++)
				{

					for (int j = 0; j < dt.Columns.Count; j++)
					{

						string DataCell = GetNomeColunaByIndex(linha).ToString() + StartIndexData;
						var b = dt.Rows[i][j].GetType().ToString();
						var a = dt.Rows[i][j].ToString();
						a = a.Replace("&nbsp;", " ");
						a = a.Replace("&amp;", "&");
						a = a.Replace("R$", "");
						a = a.Replace("BRL", "");

                        if (b != "System.String")
                        {
                            if (b == "System.Int64" || b == "System.Int32" || b == "System.Int16")
                            {
                                int.TryParse(a, out int val);
                                ws.Cell(DataCell).Style.NumberFormat.NumberFormatId = 1;
                                ws.Cell(DataCell).Value = val;
                            }
                            else if (b == "System.Double" || b == "System.Decimal")
                            {

                                decimal.TryParse(a, out decimal valor);
                                int count = BitConverter.GetBytes(decimal.GetBits(valor)[3])[2];
                                if (count > 2)
                                {
                                    string PrimeiroFormato = "#,".PadRight(count + 2, '#');
                                    string SegundoFormato = "0.".PadRight(count + 2, '0');
                                    string Formato = PrimeiroFormato + SegundoFormato;
                                    ws.Cell(DataCell).Style.NumberFormat.Format = Formato;
                                    ws.Cell(DataCell).Value = valor;
                                }
                                else
                                {
                                    ws.Cell(DataCell).Style.NumberFormat.Format = "#,##0.00";
                                    ws.Cell(DataCell).Value = valor;
                                }
                            }
                            else if (b == "System.DateTime")
                            {
                                DateTime.TryParse(a, out DateTime date);
                                if (date.Hour == 0 && date.Minute == 0 && date.Second == 0)
                                {
                                    ws.Cell(DataCell).Style.DateFormat.Format = "dd/MM/yyyy";
                                    ws.Cell(DataCell).Style.NumberFormat.Format = "dd/MM/yyyy";
                                    ws.Cell(DataCell).Value = date;
                                }
                                else
                                {
                                    ws.Cell(DataCell).Style.DateFormat.Format = "dd/MM/yyyy HH:mm:ss";
                                    ws.Cell(DataCell).Style.NumberFormat.Format = "dd/MM/yyyy HH:mm:ss";
                                    ws.Cell(DataCell).Value = date;
                                }
                            }
                        }
                        linha++;
					}
					linha = 1;
					StartIndexData++;
				}
			}

			wb.SaveAs(sMemoryStream);
			sMemoryStream.Position = 0;

			string nome_arquivo = $"Relatório de Ponto - {DateTime.Now:dd-MM-yyyy}";

            string CaminhoFisicoExport;
            if (Environment.MachineName.StartsWith("DESKTOP"))
            {
				CaminhoFisicoExport = System.Reflection.Assembly.GetExecutingAssembly().Location.Replace("bin\\Debug\\netcoreapp3.1\\Api.PontoDigital.dll", "Excel\\");
			}
            else
            {
				CaminhoFisicoExport = Directory.Exists("C:\\Excel\\") ? "C:\\Excel\\" : Directory.CreateDirectory("C:\\Excel\\").FullName;
			}
			string Arquivo = CaminhoFisicoExport + nome_arquivo + ".xlsx";

			FileStream file = new FileStream(Arquivo, FileMode.Create, FileAccess.Write);
			sMemoryStream.WriteTo(file);
			file.Close();

			sMemoryStream.Close();

			System.Threading.Thread.Sleep(2000);

			string Link = await UploadFile(Arquivo);
			File.Delete(Arquivo);
			return Link;
		}
		/// <summary>
		/// GetNomeColunaByIndex
		/// </summary>
		/// <param name="numColuna"></param>
		/// <returns></returns>
		public string GetNomeColunaByIndex(int numColuna)
		{
			int dividend = numColuna;
			string nomeColuna = string.Empty;
			int modulo;

			while (dividend > 0)
			{
				modulo = (dividend - 1) % 26;
				nomeColuna = Convert.ToChar(65 + modulo).ToString() + nomeColuna;
				dividend = (dividend - modulo) / 26;
			}

			return nomeColuna;
		}
		/// <summary>
		/// UploadFile
		/// </summary>
		/// <param name="filePath"></param>
		/// <returns></returns>
		public async Task<string> UploadFile(string filePath)
		{
			IAmazonS3 client = new AmazonS3Client(_keyName, _secretName, RegionEndpoint.SAEast1);

			try
			{
				FileInfo file = new FileInfo(filePath);
				PutObjectRequest putRequest = new PutObjectRequest
				{
					InputStream = file.OpenRead(),
					BucketName = _bucketName,
					Key = "excel/" + Path.GetFileName(filePath),
					ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
				};

				PutObjectResponse response = await client.PutObjectAsync(putRequest);
                GetPreSignedUrlRequest request = new GetPreSignedUrlRequest
                {
                    BucketName = _bucketName,
                    Key = "excel/" + Path.GetFileName(filePath),
                    Expires = DateTime.Now.AddHours(1),
                    Protocol = Protocol.HTTP
                };
                string url = client.GetPreSignedURL(request);
				return url;
			}
			catch (AmazonS3Exception amazonS3Exception)
			{
				if (amazonS3Exception.ErrorCode != null &&
					(amazonS3Exception.ErrorCode.Equals("InvalidAccessKeyId")
					||
					amazonS3Exception.ErrorCode.Equals("InvalidSecurity")))
				{
					throw new Exception("Check the provided AWS Credentials.");
				}
				else
				{
					throw new Exception("Error occurred: " + amazonS3Exception.Message);
				}
			}
		}
	}
}

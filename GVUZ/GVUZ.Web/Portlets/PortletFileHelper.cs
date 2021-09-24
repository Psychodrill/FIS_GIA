using System;
using System.Text;
using FogSoft.WSRP;

namespace GVUZ.Web.Portlets
{
	public class PortletFileHelper
	{
		public static byte[] GetPortletFile(UploadContext[] uploadContexts, out string contentDisposition, out string contentType)
		{
			contentDisposition = "";
			contentType = "";
			if (uploadContexts == null || uploadContexts.Length == 0 || uploadContexts[0].uploadData.Length == 0)
				return null;

			byte[] fileUploadData = uploadContexts[0].uploadData;
			string fileUploadString = System.Text.Encoding.GetEncoding(1251).GetString(fileUploadData);
			string[] lineSeparators = new string[] { "\r\n" };
			var uploadArray = fileUploadString.Split(lineSeparators, StringSplitOptions.None);

			if (uploadArray.Length < 7)
				return null;

			string pattern = uploadArray[0];
			//contentDisposition = uploadArray[1]; //Content-Disposition
			//take content-disposition as UTF8 to correctly get file name
			contentDisposition = Encoding.UTF8.GetString(fileUploadData, uploadArray[0].Length + lineSeparators[0].Length, uploadArray[1].Length);
			contentType = uploadArray[2]; //Content-Type
			int fileBeginDataPos = pattern.Length + lineSeparators[0].Length + uploadArray[1].Length + lineSeparators[0].Length
				+ contentType.Length + lineSeparators[0].Length + lineSeparators[0].Length;
			string fileEndData = lineSeparators[0] + pattern + "--" + lineSeparators[0];
			int fileLength = fileUploadData.Length - fileBeginDataPos - fileEndData.Length;

			if (fileLength < 0)
				return null;

			byte[] file = new byte[fileLength];

			Array.Copy(fileUploadData, fileBeginDataPos, file, 0, fileLength);

//			string uploadFileString = System.Text.Encoding.GetEncoding(1251).GetString(file);
			//			System.IO.File.WriteAllBytes("c:\\weblog\\uploadFile.txt", fileUploadData);
			//			System.IO.File.WriteAllBytes("c:\\weblog\\file.txt", file);

			return file;
		}

		public class UploadFile
		{
			public byte[] Content { get; private set; }
			public string FormDataName { get; private set; }
			public string FileName { get; private set; }
			public string ContentType { get; private set; }

			public UploadFile(UploadContext[] uploadContexts)
			{
				string contentDisposition;
				string contentType;
				byte[] file = GetPortletFile(uploadContexts, out contentDisposition, out contentType);
				Content = file;
				ContentType = contentType.Replace("Content-Type: ", "");
				var contentDispositionArray = contentDisposition.Replace("Content-Disposition: ", "").Split(';');
				if (contentDispositionArray.Length > 2)
				{
					FormDataName = contentDispositionArray[1].Replace("name=\"", "").Replace("\"", "");
					FileName = contentDispositionArray[2].Replace("filename=\"", "").Replace("\"", "");
				}
			}
		}
	}
}
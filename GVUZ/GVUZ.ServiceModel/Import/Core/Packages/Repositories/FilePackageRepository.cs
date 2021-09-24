using System;
using System.IO;
using System.Xml;
using FogSoft.Helpers;

namespace GVUZ.ServiceModel.Import.Core.Packages.Repositories
{
	/// <summary>
	/// Настройки сервиса.
	/// </summary>
	public static class FileServiceSettings
	{
		public static string ImportFolderPath
		{
			get { return RootPath(AppSettings.Get("ImportFolder", string.Empty)); }
		}

		public static string ProcessingFolderPath
		{
			get { return RootPath(AppSettings.Get("ProcessingFolder", string.Empty)); }
		}

		public static string ResultFolderPath
		{
			get { return RootPath(AppSettings.Get("ResultFolder", string.Empty)); }
		}

		private static string RootPath(string path)
		{
			string resultPath = path;
			if (!Path.IsPathRooted(resultPath))
				resultPath += "\\";

			return resultPath;
		}
	}

	/// <summary>
	/// Менеджер для работы с пакетами. Файловый. Не используется в данный момент (изначально планировался для тестов и возможности хранения пакетов на диске)
	/// </summary>
	public class FilePackageRepository : IPackageRepository
	{
		public ImportPackage GetPackageAsLIFO()
		{
			string[] files = Directory.GetFiles(FileServiceSettings.ImportFolderPath);
			if(files.Length > 0)
			{
				string srcPath = files[0];
				string fileName = Path.GetFileName(srcPath);
				string destPath = FileServiceSettings.ProcessingFolderPath + fileName;
				if(File.Exists(destPath))
				{
					LogHelper.Log.Error(string.Format(
						"Processing directory already contains file with the same name. {0} will be deleted. No actions will be applied.", srcPath));
					File.Delete(srcPath);
					return GetPackageAsLIFO();
				}

				File.Move(srcPath, destPath);
				LogHelper.Log.Info(string.Format(
					"File from import directory moved to processing directory: {0}", destPath));
				return LoadPackageData(destPath);
			}

			return null;
		}

		public ImportPackage GetUnprocessedPackage(int[] excludedInstitutions)
		{
			throw new NotImplementedException();
		}

		public ImportPackage GetPackage(int packageID)
		{
			throw new NotImplementedException();
		}

		public void SetPackageChecked(int packageID, string checkResult)
		{
			throw new NotImplementedException();
		}

		public void SavePackage(ImportPackage importPackage)
		{
			throw new NotImplementedException();
		}

	    public bool IsPackageProcessed(int packageId)
	    {
	        throw new NotImplementedException();
	    }

	    private static ImportPackage LoadPackageData(string packageFile)
		{
			if (String.IsNullOrEmpty(packageFile)) return null;
			PackageType packageType = DefinePackageType(packageFile);
			switch (packageType)
			{
				case PackageType.Import:
					return null;
				default:
					return null;
			}
		}

		private static PackageType DefinePackageType(string packageFile)
		{
			using (XmlTextReader xmlReader = new XmlTextReader(packageFile))
			{
				if (!xmlReader.Read())
				{
					throw new ImportException("Can't read from xml file: " + packageFile);
				}

				// пропускаем тег xml
				if (xmlReader.Name.ToLower() == "xml")
					xmlReader.Read();
				while (xmlReader.Name.Trim().Length == 0)
					xmlReader.Read();

				switch (xmlReader.Name.ToLower())
				{
					case "packagedata":
						return PackageType.Import;
					default:
						throw new ImportException(String.Format("Unsupported package type. File: {0}. Xml tag name: {1}.", packageFile, xmlReader.Name));
				}
			}
		}

        public ImportPackage GetUncheckedPackage(int[] excludedInstitutions)
		{
			throw new NotImplementedException();
		}
	}
}

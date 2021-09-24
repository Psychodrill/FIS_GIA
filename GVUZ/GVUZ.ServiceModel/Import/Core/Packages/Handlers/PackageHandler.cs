using System;
using GVUZ.ServiceModel.Import.Schemas;

namespace GVUZ.ServiceModel.Import.Core.Packages.Handlers
{
	/// <summary>
	/// Базовый класс обработки пакетов
	/// </summary>
	public abstract class PackageHandler : IDisposable
	{
        public abstract string ValidatePackage(string packageData, PackageType packageType);
		public abstract string Process();
		public abstract void AddExtraInfoToPackage(ImportPackage importPackage);
		public abstract void Dispose();

        /// <summary>
        /// Валидация поступающего XML по XSD
        /// </summary>
        public static string ValidatePackage(string packageData, XsdManager.XsdName xsdName)
        {
            return new XsdManager().ValidateXmlBySheme(packageData, xsdName);
		}
	}
}

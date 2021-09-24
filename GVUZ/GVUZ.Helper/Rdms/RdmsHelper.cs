using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.ServiceModel;
using System.ServiceModel.Security;
using System.Xml;
using System.Xml.Linq;
using FogSoft.Helpers;
using Ionic.Zip;
using Rdms.Communication.Entities;
using log4net;

namespace GVUZ.Helper.Rdms
{
    public static class RdmsHelper
    {
        private const string NamespacePrefix = "{http://schemas.datacontract.org/2004/07/Rdms.Communication.Entities}";
        private const string VersionDescriptionXName = NamespacePrefix + "VersionDescription";
        private static readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        ///     Возвращает содержимое справочника из НСИ (если требуется, то есть более свежий идентификатор версии).
        /// </summary>
        public static DictionaryContent GetDictionaryContent(Dictionary dictionaryId, int currentVersionId)
        {
            try
            {
                byte[] archive;
                string result = LoadArchive(dictionaryId, out archive);
                if (!string.IsNullOrEmpty(result))
                    return new DictionaryContent(result);

                using (ZipFile zip = ZipFile.Read(archive))
                {
                    XDocument meta = LoadXml(zip["meta.xml"]);

                    if (meta.Root == null)
                        return new DictionaryContent(Messages.RdmsHelper_InvalidFormat);
                    XElement description = meta.Root.Elements(VersionDescriptionXName)
                                               .FirstOrDefault();
                    if (description == null)
                        return new DictionaryContent(Messages.RdmsHelper_InvalidFormat);

                    var versionDescription
                        = new VersionDescription
                            {
                                ActivationDate = GetValue<DateTime>(description, "ActivationDate"),
                                DirectoryId = GetValue<int>(description, "DirectoryId"),
                                Id = GetValue<int>(description, "Id"),
                                Name = GetValue<string>(description, "Name"),
                                Note = GetValue<string>(description, "Note"),
                                Number = GetValue<int>(description, "Number"),
                                State = GetValue<VersionStateEnum>(description, "State"),
                            };


                    if (currentVersionId >= versionDescription.Id)
                        return new DictionaryContent(versionDescription);

                    return new DictionaryContent(versionDescription, LoadXml(zip["data.xml"]));
                }
            }
            catch (StackOverflowException)
            {
                throw;
            }
            catch (OutOfMemoryException)
            {
                throw;
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message, ex);
                return new DictionaryContent(Messages.RdmsHelper_DataProcessingError);
            }
        }

        private static T GetValue<T>(XElement description, string name)
        {
            XElement element = description.Element(NamespacePrefix + name);
            if (element == null)
                throw new ArgumentException("Не найден элемент {0} в метаданных справочника.".FormatWith(name));
            return element.Value.To<T>(ensureExists: true);
        }

        private static XDocument LoadXml(ZipEntry meta)
        {
            using (var memoryStream = new MemoryStream(meta.UncompressedSize.To(-1)))
            {
                meta.Extract(memoryStream);
                memoryStream.Seek(0, SeekOrigin.Begin);
                using (XmlReader reader = XmlReader.Create(memoryStream))
                {
                    XDocument document = XDocument.Load(reader);
                    // namespaceManager = reader.NameTable == null ? null : new XmlNamespaceManager(reader.NameTable);
                    return document;
                }
            }
        }

        private static string LoadArchive(Dictionary dictionaryId, out byte[] archive)
        {
            // настройка аутентификации на прокси (код от РБК)
#pragma warning disable 612,618
// ReSharper disable PossibleNullReferenceException
            if ((GlobalProxySelection.Select as WebProxy).Address != null)
// ReSharper restore PossibleNullReferenceException
#pragma warning restore 612,618
            {
                WebRequest.DefaultWebProxy.Credentials = new NetworkCredential
                    (AppSettings.Get("ConnectionFactory.ProxyUsername", ""),
                     AppSettings.Get("ConnectionFactory.ProxyPassword", ""));
            }

            // аутентификация на сервере
            ConnectionFactory.Username = AppSettings.Get("ConnectionFactory.Username", "");
            ConnectionFactory.Password = AppSettings.Get("ConnectionFactory.Password", "");
            archive = null;
            try
            {
                // TODO: discuss using VersionDescription to avoid unnececcary data loading (отложим до реализации НСИ)
                // VersionDescription description = ConnectionFactory.GetVersionService().GetVersionDescription((int) dictionary);
                archive = ConnectionFactory.GetExportService().GetByDate((int) dictionaryId, DateTime.Now.Date);
                return "";
            }
            catch (StackOverflowException)
            {
                throw;
            }
            catch (OutOfMemoryException)
            {
                throw;
            }
            catch (MessageSecurityException)
            {
                return Messages.RdmsHelper_InvalidLoginOrPassport;
            }
            catch (SecurityNegotiationException)
            {
                return Messages.RdmsHelper_InvalidLoginOrPassport;
            }
            catch (SecurityAccessDeniedException)
            {
                return Messages.RdmsHelper_AccessDenied;
            }
            catch (ProtocolException ex)
            {
                LogHelper.Log.Error(ex.Message, ex);
                var webEx = ex.InnerException as WebException;
                if (webEx != null &&
                    ((HttpWebResponse) webEx.Response).StatusCode == HttpStatusCode.ProxyAuthenticationRequired)
                    return Messages.RdmsHelper_ProxyAuthenticationError;
                return Messages.RdmsHelper_AuthorizationError;
            }
            catch (Exception ex)
            {
                LogHelper.Log.Error(ex.Message, ex);
                return Messages.RdmsHelper_AuthorizationError;
            }
        }
    }
}
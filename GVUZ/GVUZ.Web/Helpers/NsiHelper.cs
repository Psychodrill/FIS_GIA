using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Reflection;
using System.Xml.Linq;
using FogSoft.Import;
using GVUZ.Helper.Rdms;
using GVUZ.Model.NormativeDictionaries;
using GVUZ.ServiceModel.Import.NormativeDictionaries;
using log4net;

namespace GVUZ.Web.Helpers
{
	/// <summary>
	/// Хелпер для работы со справочниками НСИ. Справочники сейчас не используются
	/// </summary>
	public static class NsiHelper
	{
		private static readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
		private static readonly Dictionary<Dictionary, Type> Types;

		static NsiHelper()
		{
			Types = new Dictionary<Dictionary, Type>
			{
				{ Dictionary.FormOfLaw, typeof(FormOfLaw) },
				{ Dictionary.Country, typeof(Country) },
				{ Dictionary.Profession, typeof(Profession) },
				{ Dictionary.RussianFederationSubject, typeof(Okato) }
			};
		}

		public static string Import(Dictionary dictionary, DictionaryContent content = null)
		{
			try
			{
				using (NormativeDictionaryEntities entities = new NormativeDictionaryEntities())
				{
					int currentVersion = entities.GetCurrentVersion(dictionary);
					if (content != null)
					{
						if (content.VersionDescription.Id.HasValue && content.VersionDescription.Id.Value <= currentVersion)
							return Messages.DictionaryAlreadyActual;
					}
					else
						content = RdmsHelper.GetDictionaryContent(dictionary, currentVersion);
					if (content.HasErrors)
						return content.ErrorMessage;

					if (content.ShouldImportContent)
					{
						Type recordType;
						if (!Types.TryGetValue(dictionary, out recordType))
							return Messages.DictionaryNotRegistered;

						using (var reader = GetReader(recordType, content.Content))
						{
							using (SqlConnection connection = new SqlConnection(ConnectionString))
							{
								connection.Open();
								using (SqlTransaction transaction = connection.BeginTransaction())
								{
									new SqlClientCsvImporter().Import(reader, transaction, recordType);

									transaction.Commit();
								}
							}
						}

						entities.AddOrEditVersion(dictionary, content.VersionDescription);
						return Messages.DictionaryRefreshed;
					}

					if (currentVersion >= content.VersionDescription.Id)
						return Messages.DictionaryAlreadyActual;
				}

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
			catch (SqlException ex)
			{
				Log.Error(ex.Message, ex);
				return Messages.DatabaseError;
			}
			catch (Exception ex)
			{
				Log.Error(ex.Message, ex);
				return Messages.UnknownError;
			}
		}

		private static AbstractDataReader GetReader(Type type, XDocument document)
		{
			Type readerType = typeof(NormativeXmlReader<>).MakeGenericType(type);
			return (AbstractDataReader)Activator.CreateInstance(readerType, document);
		}

		private static string ConnectionString
		{
			get { return ConfigurationManager.ConnectionStrings["Main"].ConnectionString; }
		}
	}
}
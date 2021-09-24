namespace Esrp.Services
{
    using System;
    using System.Collections.Generic;
    using Esrp.Web.ViewModel.Documents;

    /// <summary>
    /// Сервис для работы с документами
    /// </summary>
    public class DocumentsService
    {
        #region Public Methods and Operators

        /// <summary>
        /// Выбрать все документы
        /// </summary>
        /// <returns>
        /// список новостей
        /// </returns>
        public List<DocumentView> SelectDocuments()
        {
            var result = new List<DocumentView>();

            using (var cmd = new Command("dbo.SearchDocument"))
            {
                cmd.Parameters.AddWithValue("@IsActive", 1);
                cmd.Parameters.AddWithValue("@contextCodes", "other");

                for (var reader = cmd.ExecuteReader(); reader.Read();)
                {
                    var news = new DocumentView
                        {
                            Id = (long)reader["Id"],
                            CreateDate = (DateTime)reader["Date"],
                            Name = (string)reader["Name"],
                            Description = (string)reader["Description"],
                            RelativeUrl = reader["RelativeUrl"] as string
                        };

                    result.Add(news);
                }
            }

            return result;
        }

        #endregion
    }
}
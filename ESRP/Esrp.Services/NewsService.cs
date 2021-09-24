namespace Esrp.Services
{
    using System;
    using System.Collections.Generic;
    using Esrp.Web.ViewModel.News;

    /// <summary>
    /// Сервис для работы с новостями
    /// </summary>
    public class NewsService
    {
        #region Public Methods and Operators

        /// <summary>
        /// Выбрать все новости
        /// </summary>
        /// <returns>
        /// список новостей
        /// </returns>
        public List<NewsView> SelectNews()
        {
            var result = new List<NewsView>();

            using (var cmd = new Command("dbo.SearchNews"))
            {
                cmd.Parameters.AddWithValue("@IsActive", 1);
                for (var reader = cmd.ExecuteReader(); reader.Read();)
                {
                    var news = new NewsView
                        {
                            Id = (long)reader["Id"], 
                            CreateDate = (DateTime)reader["Date"], 
                            Name = (string)reader["Name"], 
                            Description = (string)reader["Description"]
                        };

                    result.Add(news);
                }
            }

            return result;
        }

        #endregion
    }
}
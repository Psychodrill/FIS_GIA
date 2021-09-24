namespace Esrp.Services
{
    using System.Collections.Generic;

    using Esrp.Web.ViewModel.Requests;

    /// <summary>
    /// Сервис для работы с заявками
    /// </summary>
    public class RequestsService
    {
        /// <summary>
        /// Получить существующие года из таблицы с заявками
        /// </summary>
        /// <returns>
        /// Список годов
        /// </returns>
        public List<YearView> GetYearsInRequests()
        {
            var result = new List<YearView>();

            using (var cmd = new Command("dbo.GetYearsInRequests"))
            {
                for (var reader = cmd.ExecuteReader(); reader.Read();)
                {
                    var year = new YearView
                        {
                            Year = reader["Year"].ToString()
                        };

                    result.Add(year);
                }
            }

            return result;
        }
    }
}

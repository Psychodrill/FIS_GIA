namespace Ege.Check.App.Web.Common.Auth
{
    using System;
    using System.Globalization;
    using Newtonsoft.Json;

    public class StaffCookieModel
    {
        [JsonProperty("Id")]
        public int Id { get; set; }

        public string Serialize()
        {
            return Id.ToString(CultureInfo.InvariantCulture);
        }

        public static StaffCookieModel Deserialize(string serialized)
        {
            int id;
            if (!int.TryParse(serialized, out id))
            {
                throw new ArgumentException("Invalid cookie");
            }
            return new StaffCookieModel {Id = id};
        }
    }
}

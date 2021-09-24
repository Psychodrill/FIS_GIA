using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using GVUZ.Web.Helpers;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace GVUZ.Web.Infrastructure
{
    public class CookieFilterStateManager : IFilterStateManager
    {
        private const string FilterCookieNamePrefix = "fis_flt_";
        
        private static readonly IsoDateTimeConverter IsoDates = new IsoDateTimeConverter
            {
                DateTimeFormat = "dd.MM.yyyy"
            };

        private static readonly JsonSerializerSettings SerializationSettings = new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore,
                Converters = new List<JsonConverter> {IsoDates}
            };
        
        public void Update<TFilter>(TFilter instance) where TFilter : class, IFilterState<TFilter>, new()
        {
            string key = GetFilterCookieKey(typeof (TFilter));
            SetCookie(key, instance.CloneInputFields());
        }

        public void Remove<TFilter>()
        {
            RemoveCookie(GetFilterCookieKey(typeof(TFilter)));
        }

        public TFilter GetOrCreate<TFilter>() where TFilter : class, IFilterState<TFilter>, new()
        {
            string key = GetFilterCookieKey(typeof (TFilter));
            string serialized = GetCookieValue(key);

            if (serialized != null)
            {
                return JsonConvert.DeserializeObject<TFilter>(serialized, SerializationSettings);
            }

            TFilter instance = new TFilter();
            SetCookie(key, instance.CloneInputFields());
            return instance;
        }

        public void RemoveAll()
        {
            RemoveAllFilterCookies();
        }

        private void SetCookie(string key, object serializable)
        {
            string serialized = JsonConvert.SerializeObject(serializable, SerializationSettings);

            var cookie = new HttpCookie(key, ZipCompressionHelper.ZipToBase64String(serialized))
            {
                Expires = DateTime.UtcNow.AddYears(1),
            };

            HttpContext.Current.Response.Cookies.Set(cookie);
        }

        private string GetCookieValue(string key)
        {
            var cookie = HttpContext.Current.Request.Cookies[key];

            if (cookie == null || string.IsNullOrEmpty(cookie.Value))
            {
                return null;
            }

            return ZipCompressionHelper.UnzipFromBase64String(cookie.Value);

        }

        private void RemoveCookie(string key)
        {
            HttpContext.Current.Request.Cookies.Remove(key);
            HttpContext.Current.Response.Cookies.Remove(key);
            HttpContext.Current.Request.Cookies.Set(new HttpCookie(key, null) { Expires = DateTime.UtcNow.AddYears(-1) });
            HttpContext.Current.Response.Cookies.Set(new HttpCookie(key, null) { Expires = DateTime.UtcNow.AddYears(-1) });
        }

        private void RemoveAllFilterCookies()
        {
            var keys = HttpContext.Current.Request.Cookies.AllKeys.Where(x => x.StartsWith(FilterCookieNamePrefix, StringComparison.OrdinalIgnoreCase));
            foreach (string key in keys)
            {
                RemoveCookie(key);
                //HttpContext.Current.Request.Cookies.Set(new HttpCookie(key, null) { Expires = DateTime.UtcNow.AddYears(-1) });
            }
        }

        private static string GetFilterCookieKey(Type filterType)
        {
            return string.Format("{0}{1}", FilterCookieNamePrefix, filterType.Name.Replace("ViewModel", string.Empty).ToLowerInvariant());
        }
    }
}
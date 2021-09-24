using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Newtonsoft.Json;

namespace Ege.Check.App.Web.Common.ReCapture
{
    public class GoogleRecaptchaService : IRecaptchaService
    {
        private readonly HttpClient _httpClient;
        private readonly string _result; 

        public GoogleRecaptchaService()
        {
            _httpClient = new HttpClient();
            _httpClient.BaseAddress = new Uri("https://www.google.com");
            _result = System.Configuration.ConfigurationManager.AppSettings["ReCaptchaPrivateKey"];
        }

        public async Task<RecaptchaResponse> Validate(string token)
        {
            try
            {
                var content = new FormUrlEncodedContent(new[]
                {
                    new KeyValuePair<string, string>("secret", _result),
                    new KeyValuePair<string, string>("response", token)
                });

                var response = await _httpClient.PostAsync("/recaptcha/api/siteverify", content);
                if (response.IsSuccessStatusCode)
                {
                    var resultContent = await response.Content.ReadAsStringAsync();
                    var captchaResponse = JsonConvert.DeserializeObject<RecaptchaResponse>(resultContent);

                    return captchaResponse;
                }
            }
            catch (Exception e)
            {
                return null;
            }
            return null;
        }
    }
}
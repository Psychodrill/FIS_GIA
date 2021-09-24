using System.Collections.Generic;
using Newtonsoft.Json;

namespace Ege.Check.App.Web.Common.ReCapture
{
    public class RecaptchaResponse
    {
        [JsonProperty("success")]
        public bool Success { get; set; }

        [JsonProperty("error-codes")]
        public List<string> ErrorCodes { get; set; }
    }
}
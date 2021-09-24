namespace Ege.Check.Logic.Models
{
    using System;
    using System.Runtime.Serialization;
    using JetBrains.Annotations;
    using Newtonsoft.Json;

    [DataContract]
    public class Captcha
    {
        public Captcha([NotNull] string image, [NotNull] string token)
        {
            Image = image;
            Token = token;
        }

        /// <summary>
        ///     base64 картинки в формате JPEG
        /// </summary>
        [NotNull]
        [DataMember]
        public string Image { get; set; }

        /// <summary>
        ///     Проверочный код
        /// </summary>
        [NotNull]
        [DataMember]
        public string Token { get; set; }
    }

    [DataContract]
    //[JsonConverter(typeof(CaptchaJsonConverter))]
    public class CachedCaptcha
    {
        [NotNull]
        [DataMember]
        public string Image { get; set; }

        [NotNull]
        [DataMember]
        public string Number { get; set; }
    }

    public class CaptchaJsonConverter : JsonConverter
    {
        public override void WriteJson([NotNull]JsonWriter writer, object value, [NotNull]JsonSerializer serializer)
        {
            if (value == null)
            {
                throw new ArgumentNullException("value");
            }
            var captcha = (CachedCaptcha)value;
            serializer.Serialize(writer, string.Format("{0}{1}{2}", captcha.Number.Length, captcha.Number, captcha.Image));
        }

        public override object ReadJson([NotNull]JsonReader reader, Type objectType, object existingValue, [NotNull]JsonSerializer serializer)
        {
            var serialized = serializer.Deserialize<string>(reader);
            var numberLength = serialized[0] - '0';
            return new CachedCaptcha
            {
                Number = serialized.Substring(1, numberLength),
                Image = serialized.Substring(1 + numberLength),
            };
        }

        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(CachedCaptcha);
        }
    }
}

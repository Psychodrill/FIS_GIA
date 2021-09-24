namespace Ege.Check.Logic.Models.Cache
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using JetBrains.Annotations;
    using Newtonsoft.Json;

    /// <summary>
    ///     Кэш-модель участника экзамена
    ///     Ключ в кэше - Hash
    /// </summary>
    public class ParticipantCacheModel
    {
        /// <summary>
        ///     Хэш ФИО
        /// </summary>
        [JsonIgnore]
        public string Hash { get; set; }

        /// <summary>
        ///     Код регистрации
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        ///     Номер документа (без серии)
        /// </summary>
        public string Document { get; set; }

        /// <summary>
        ///     Идентификатор региона
        /// </summary>
        public int RegionId { get; set; }

        public string Serialize()
        {
            return (char)RegionId + Code;
        }

        public static ParticipantCacheModel Deserialize(string input)
        {
            if (string.IsNullOrEmpty(input))
            {
                throw new ArgumentException("Invalid participant cookie");
            }
            return new ParticipantCacheModel
            {
                RegionId = input[0],
                Code = input.Substring(1),
            };
        }

        public override string ToString()
        {
            return string.Format("Hash: {0}, Code: {1}, Document: {2}, RegionId: {3}", Hash, Code, Document, RegionId);
        }
    }

    public class ParticipantCacheModelEqualityComparer : IEqualityComparer<ParticipantCacheModel>
    {
        public static ParticipantCacheModelEqualityComparer Instance = new ParticipantCacheModelEqualityComparer();

        public bool Equals(ParticipantCacheModel x, ParticipantCacheModel y)
        {
            if (x == null || y == null)
            {
                return x == null && y == null;
            }
            if (x.Code == null || y.Code == null)
            {
                return x.Code == null && y.Code == null;
            }
            return x.Code.Equals(y.Code, StringComparison.Ordinal) && x.RegionId == y.RegionId;
        }

        public int GetHashCode(ParticipantCacheModel obj)
        {
            if (obj == null)
            {
                throw new ArgumentNullException("obj");
            }
            return (obj.Code != null ? obj.Code.GetHashCode() : 0) ^ obj.RegionId.GetHashCode();
        }
    }

    public class ParticipantCookieConverter : JsonConverter
    {
        public static JsonSerializerSettings ConvertForCookie = new JsonSerializerSettings
        {
            Converters = new JsonConverter[] {new ParticipantCookieConverter(), }
        };

        public override void WriteJson([NotNull]JsonWriter writer, object value, [NotNull]JsonSerializer serializer)
        {
            if (value == null)
            {
                throw new ArgumentNullException("value");
            }
            var participant = (ParticipantCacheModel)value;
            writer.WriteStartObject();
            writer.WritePropertyName("Code");
            serializer.Serialize(writer, participant.Code);
            writer.WritePropertyName("RegionId");
            serializer.Serialize(writer, participant.RegionId);
            writer.WriteEndObject();
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, [NotNull]JsonSerializer serializer)
        {
            return serializer.Deserialize<ParticipantCacheModel>(reader);
        }

        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(ParticipantCacheModel);
        }
    }
}

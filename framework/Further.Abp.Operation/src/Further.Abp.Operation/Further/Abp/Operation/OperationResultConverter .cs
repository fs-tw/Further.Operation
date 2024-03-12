using FluentResults;
using Further.Operation.Operations;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Formats.Asn1;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Further.Abp.Operation
{
    public class FluentResultConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(Result);
        }

        public override object? ReadJson(JsonReader reader, Type objectType, object? existingValue, JsonSerializer serializer)
        {
            JObject jsonObject = JObject.Load(reader);

            var result = new Result();

            if (jsonObject["Errors"] is JArray errors)
            {
                foreach (var item in errors)
                {
                    var error = new Error(item["message"].ToString());
                    error = (Error)CopyMetadataReason(item, error);
                    result.WithError(error);
                }
            }

            if (jsonObject["Successes"] is JArray successes)
            {
                foreach (var item in successes)
                {
                    var success = new Success(item["message"].ToString());
                    success = (Success)CopyMetadataReason(item, success);
                    result.WithSuccess(success);
                }
            }

            return result;
        }

        public override void WriteJson(JsonWriter writer, object? value, JsonSerializer serializer)
        {
            writer.WriteStartObject();

            var properties = value.GetType().GetProperties(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance);
            foreach (var property in properties)
            {
                var propertyValue = property.GetValue(value);

                writer.WritePropertyName(property.Name);
                serializer.Serialize(writer, propertyValue);
            }

            writer.WriteEndObject();
        }

        private IReason CopyMetadataReason(JToken json, IReason reason)
        {
            reason.Metadata.Clear();

            if (json["metadata"] is JObject metaJObject)
            {
                var metadataDictionary = metaJObject.ToObject<Dictionary<string, object>>();
                foreach (var kvp in metadataDictionary)
                {
                    reason.Metadata.Add(kvp.Key, kvp.Value);
                }
            }

            return reason;
        }
    }
}

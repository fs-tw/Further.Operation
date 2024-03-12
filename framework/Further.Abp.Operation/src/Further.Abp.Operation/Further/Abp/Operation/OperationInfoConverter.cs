using FluentResults;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Further.Abp.Operation
{
    public class OperationInfoConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(OperationInfo);
        }

        public override object? ReadJson(JsonReader reader, Type objectType, object? existingValue, JsonSerializer serializer)
        {
            JObject jsonObject = JObject.Load(reader);

            var id = Guid.Parse(jsonObject["id"].ToString());
            var operationId = jsonObject["operationId"].Value<string>();
            var operationName = jsonObject["operationName"].Value<string>();
            var result = (Result)serializer.Deserialize(jsonObject["result"].CreateReader(), typeof(Result));
            var owners = jsonObject["owners"].ToObject<List<OperationOwnerInfo>>();
            var executionDuration = jsonObject["executionDuration"].Value<int>();

            var operationInfo = new OperationInfo(id, operationId, operationName, result, owners, executionDuration);

            return operationInfo;
        }

        public override void WriteJson(JsonWriter writer, object? value, JsonSerializer serializer)
        {
            writer.WriteStartObject();

            var properties = value.GetType().GetProperties(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance);
            foreach (var property in properties)
            {
                var propertyValue = property.GetValue(value);

                writer.WritePropertyName(ConvertToCamelCase(property.Name));
                serializer.Serialize(writer, propertyValue);
            }

            writer.WriteEndObject();
        }

        private string ConvertToCamelCase(string s)
        {
            if (!string.IsNullOrEmpty(s) && char.IsUpper(s[0]))
            {
                s = char.ToLower(s[0]) + s.Substring(1);
            }
            return s;
        }
    }
}

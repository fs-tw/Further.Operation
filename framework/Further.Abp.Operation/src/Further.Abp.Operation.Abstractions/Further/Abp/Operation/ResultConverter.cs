using FluentResults;
using System;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Further.Abp.Operation
{
    public class ResultConverter : JsonConverter<IResultBase>
    {
        private readonly static JsonConverter<ResultDto> defaultConverter =
            (JsonConverter<ResultDto>)JsonSerializerOptions.Default.GetConverter(typeof(ResultDto));
        public override IResultBase Read(
            ref Utf8JsonReader reader,
            Type typeToConvert,
            JsonSerializerOptions options)
        {
            var obj = defaultConverter.Read(ref reader, typeof(ResultDto), options)!;

            var result = Result.Ok();

            obj.Errors?.ForEach(e =>
            {
                var error = new Error(e.Message);
                e.Metadata.ToList().ForEach(m => error.WithMetadata(m.Key, m.Value));
                result.WithError(error);
            });
            obj.Successes?.ForEach(s =>
            {
                var success = new Success(s.Message);
                s.Metadata.ToList().ForEach(m => success.WithMetadata(m.Key, m.Value));
                result.WithSuccess(success);
            });

            return result;
        }

        public override void Write(
            Utf8JsonWriter writer,
            IResultBase value,
            JsonSerializerOptions options)
        {
            JsonSerializer.Serialize(writer, (object)value, options);
        }
    }
}

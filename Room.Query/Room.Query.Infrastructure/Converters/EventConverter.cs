using CQRS.Core.Event;
using Room.Common.Events;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Room.Query.Infrastructure.Converters
{
    public class EventConverter : JsonConverter<BaseEvent>
    {
        public override bool CanConvert(Type typeToConvert)
        {
            return typeToConvert.IsAssignableFrom(typeof(BaseEvent));
        }
        public override BaseEvent? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            if(!JsonDocument.TryParseValue(ref reader, out JsonDocument document))
            {
                throw new JsonException($"faild to parse {nameof(JsonDocument)}!");
            }
            if(!document.RootElement.TryGetProperty("Type", out var type))
            {
                throw new JsonException($"faild to get the Type Property of this {nameof(document)}");
            }
            var typeDiscriminator = type.GetString();
            var json = document.RootElement.GetRawText();
            return typeDiscriminator switch
            {
                nameof(RoomCreatedEvent) => JsonSerializer.Deserialize<RoomCreatedEvent>(json,options),
                nameof(RoomUpdatedEvent) => JsonSerializer.Deserialize<RoomUpdatedEvent>(json,options),
                nameof(RoomDeletedEvent) => JsonSerializer.Deserialize<RoomDeletedEvent>(json, options),
                _ => throw new JsonException($"this Type {typeDiscriminator} is not supported yet ")
            };
        }

        public override void Write(Utf8JsonWriter writer, BaseEvent value, JsonSerializerOptions options)
        {
            throw new NotImplementedException();
        }
    }
}

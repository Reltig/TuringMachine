/*using Newtonsoft.Json;

namespace Turing;

public class CustomConverter : JsonConverter
{
    public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
    {
        var rules = (Rules)value;

        writer.WriteStartObject();
        writer.WritePropertyName("Rules");
        serializer.Serialize(writer, rules.Data);
        writer.WriteEndObject();
    }

    public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
    {
        List<Rule> rules = new();
        while (reader.Read())
        {
            if (reader.TokenType != JsonToken.PropertyName)
                break;

            var propertyName = (string)reader.Value;
            if (!reader.Read())
                continue;

            if (propertyName == "Rules")
            {
                rules = serializer.Deserialize<List<Rule>>(reader);
                break;
            }
        }
        return new Rules(rules);
    }

    public override bool CanConvert(Type objectType)
    {
        return objectType == typeof(Rules);
    }
}*/
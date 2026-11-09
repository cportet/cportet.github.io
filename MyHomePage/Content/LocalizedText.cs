using System.Text.Json;
using System.Text.Json.Serialization;

namespace MyHomePage.Content;

/// <summary>
/// A text available in several languages. In JSON, either a plain string
/// (same text for every language) or an object such as <c>{ "fr": "…", "en": "…" }</c>.
/// </summary>
[JsonConverter(typeof(LocalizedTextConverter))]
public sealed class LocalizedText
{
    private readonly IReadOnlyDictionary<string, string> _values;

    public LocalizedText(IReadOnlyDictionary<string, string> values) =>
        _values = values;

    public LocalizedText(string invariant) =>
        _values = new Dictionary<string, string> { [Languages.Invariant] = invariant };

    public IReadOnlyDictionary<string, string> Values => _values;

    public bool IsInvariant => _values.ContainsKey(Languages.Invariant);

    public string Get(string language)
    {
        if (_values.TryGetValue(language, out var value) || _values.TryGetValue(Languages.Invariant, out value))
            return value;

        if (_values.TryGetValue(Languages.Default, out value))
            return value;

        return _values.Values.FirstOrDefault() ?? string.Empty;
    }

    public override string ToString() => Get(Languages.Default);
}

public sealed class LocalizedTextConverter : JsonConverter<LocalizedText>
{
    public override LocalizedText? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        switch (reader.TokenType)
        {
            case JsonTokenType.Null:
                return null;

            case JsonTokenType.String:
                return new LocalizedText(reader.GetString() ?? string.Empty);

            case JsonTokenType.StartObject:
                var values = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
                while (reader.Read() && reader.TokenType != JsonTokenType.EndObject)
                {
                    if (reader.TokenType != JsonTokenType.PropertyName)
                        throw new JsonException("Expected a language code.");

                    var language = reader.GetString()!;
                    reader.Read();
                    values[language] = reader.GetString() ?? string.Empty;
                }
                return new LocalizedText(values);

            default:
                throw new JsonException($"Unexpected token {reader.TokenType} for a localized text.");
        }
    }

    public override void Write(Utf8JsonWriter writer, LocalizedText value, JsonSerializerOptions options)
    {
        if (value.IsInvariant)
        {
            writer.WriteStringValue(value.Get(Languages.Invariant));
            return;
        }

        writer.WriteStartObject();
        foreach (var (language, text) in value.Values)
            writer.WriteString(language, text);
        writer.WriteEndObject();
    }
}

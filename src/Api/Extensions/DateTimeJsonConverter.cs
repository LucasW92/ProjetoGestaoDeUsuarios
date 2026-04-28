using System.Globalization;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace GestaoDeUsuarios.Api.Extensions;

public sealed class DateTimeJsonConverter : JsonConverter<DateTime>
{
    private const string DateFormat = "dd/MM/yyyy HH:mm:ss";

    public override DateTime Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        var value = reader.GetString();

        if (string.IsNullOrWhiteSpace(value))
            return default;

        return DateTime.ParseExact(value, DateFormat, CultureInfo.InvariantCulture);
    }

    public override void Write(Utf8JsonWriter writer, DateTime value, JsonSerializerOptions options)
    {
        writer.WriteStringValue(value.ToString(DateFormat, CultureInfo.InvariantCulture));
    }
}
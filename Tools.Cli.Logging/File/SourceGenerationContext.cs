using System.Text.Json.Serialization;

namespace Tools.Cli.Logging.File;

[JsonSourceGenerationOptions(WriteIndented = false, DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull)]
[JsonSerializable(typeof(SummaryEntry))]
[JsonSerializable(typeof(LogJsonEntry))]
[JsonSerializable(typeof(bool))]
[JsonSerializable(typeof(int))]
[JsonSerializable(typeof(string))]
[JsonSerializable(typeof(DateTimeOffset))]
[JsonSerializable(typeof(TimeSpan))]
[JsonSerializable(typeof(Exception))]
[JsonSerializable(typeof(Dictionary<string, object>))]
[JsonSerializable(typeof(List<string>))]
[JsonSerializable(typeof(List<Dictionary<string, object>>))]
[JsonSerializable(typeof(List<SummaryEntry>))]
[JsonSerializable(typeof(List<Exception>))]
[JsonSerializable(typeof(List<KeyValuePair<string, object>>))]
public partial class SourceGenerationContext : JsonSerializerContext;
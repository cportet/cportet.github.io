using System.Text.Json.Serialization;

namespace MyHomePage.Content;

[JsonSourceGenerationOptions(
    PropertyNamingPolicy = JsonKnownNamingPolicy.CamelCase,
    PropertyNameCaseInsensitive = true,
    ReadCommentHandling = System.Text.Json.JsonCommentHandling.Skip,
    AllowTrailingCommas = true)]
[JsonSerializable(typeof(SiteData))]
[JsonSerializable(typeof(Profile))]
[JsonSerializable(typeof(IReadOnlyList<Experience>))]
[JsonSerializable(typeof(ResumeData))]
[JsonSerializable(typeof(PersoData))]
[JsonSerializable(typeof(ProjectsData))]
[JsonSerializable(typeof(AboutData))]
[JsonSerializable(typeof(Dictionary<string, PageMeta>))]
[JsonSerializable(typeof(Dictionary<string, LocalizedText>))]
[JsonSerializable(typeof(AppConfig))]
public sealed partial class ContentJsonContext : JsonSerializerContext;

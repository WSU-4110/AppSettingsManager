using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace AppSettingsManagerBff.Infrastructure.ApiRepositories;

public static class Constants
{
    private const string RequestMediaType = "application/json";

    private static readonly JsonSerializerOptions JsonSerializerOptions = new JsonSerializerOptions
    {
        PropertyNameCaseInsensitive = true,
        Converters = { new JsonStringEnumConverter(JsonNamingPolicy.CamelCase) }
    };

    public static StringContent GetStringContent(object request) =>
        new StringContent(JsonSerializer.Serialize(request), Encoding.UTF8, RequestMediaType);

    public static async Task<T> DeserializeResponse<T>(HttpResponseMessage response) =>
        JsonSerializer.Deserialize<T>(
            await response.Content.ReadAsStringAsync(),
            JsonSerializerOptions
        ) ?? throw new HttpRequestException("Method returned null object");

    public static string GetRequestUri(
        string userId,
        string password,
        string? settingGroupId = null
    ) =>
        string.IsNullOrEmpty(settingGroupId)
            ? $"userId/{userId}/password/{password}"
            : $"userId/{userId}/password/{password}/settingGroupId/{settingGroupId}";
}

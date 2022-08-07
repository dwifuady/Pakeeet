using System.Text.Json;
using System.Text.Json.Serialization;

namespace Pakeeet.Web.Dtos;

public class GetContentResponse
{
    [JsonPropertyName("name")] public string Name { get; set; }
    [JsonPropertyName("sha")] public string Sha { get; set; }
    [JsonPropertyName("content")] public string Content { get; set; }

    [JsonIgnore]
    public Awbs? CurrentAwbs
    {
        get
        {
            var base64EncodedBytes = Convert.FromBase64String(Content);
            var encoded = System.Text.Encoding.UTF8.GetString(base64EncodedBytes);
            return JsonSerializer.Deserialize<Awbs>(encoded);
        }
    }

}
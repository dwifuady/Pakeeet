using System.Text.Json;
using System.Text.Json.Serialization;

namespace Pakeeet.Web.Dtos;

public class Awbs
{
    [JsonPropertyName("awbNo")]
    public List<string> AwbNumbers { get; set; } = null!;
}

public static class AwbsExtension
{
    public static string ToGithubContent(this Awbs awbs)
    {
        var plainText = JsonSerializer.Serialize(awbs);
        return Base64Encode(plainText);
    }
    
    private static string Base64Encode(string plainText) {
        var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(plainText);
        return System.Convert.ToBase64String(plainTextBytes);
    }
}
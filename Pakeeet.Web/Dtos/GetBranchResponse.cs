using System.Text.Json.Serialization;

namespace Pakeeet.Web.Dtos;

public class GetBranchResponse
{
    [JsonPropertyName("name")] public string Name { get; set; }
    
    [JsonPropertyName("commit")] public Commit Commit { get; set; }
    [JsonPropertyName("message")] public string Message { get; set; }
}

public class Commit
{
    [JsonPropertyName("sha")] public string Sha { get; set; }
}
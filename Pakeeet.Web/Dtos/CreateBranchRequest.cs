using System.Text.Json.Serialization;

namespace Pakeeet.Web.Dtos;

public class CreateBranchRequest
{
    public CreateBranchRequest(string @ref, string sha)
    {
        Ref = @ref;
        Sha = sha;
    }

    [JsonPropertyName("ref")] public string Ref { get; }
    [JsonPropertyName("sha")] public string Sha { get; }
}
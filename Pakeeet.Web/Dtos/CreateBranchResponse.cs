using System.Text.Json.Serialization;

namespace Pakeeet.Web.Dtos;

public class CreateBranchResponse
{
    [JsonPropertyName("ref")] public string Ref { get; set; }

    [JsonPropertyName("node_id")] public string NodeId { get; set; }

    [JsonPropertyName("url")] public string Url { get; set; }

    [JsonPropertyName("object")] public CreateBranchResponseObjectDetail Object { get; set; }
}

public class CreateBranchResponseObjectDetail
{
    [JsonPropertyName("sha")] public string Sha { get; set; }

    [JsonPropertyName("commit")] public string Commit { get; set; }

    [JsonPropertyName("url")] public string Url { get; set; }
}
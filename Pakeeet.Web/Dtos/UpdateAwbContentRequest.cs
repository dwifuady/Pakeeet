using System.Text.Json;
using System.Text.Json.Serialization;

namespace Pakeeet.Web.Dtos;

public class UpdateAwbContentRequest
{
    public UpdateAwbContentRequest(string awb, Awbs awbs, string sha)
    {
        Message = $"track {awb}";
        Branch = $"req-{awb}";
        Sha = sha;
        Content = awbs.ToGithubContent();
    }

    [JsonPropertyName("owner")] public string Owner => "dwifuady";
    [JsonPropertyName("repo")] public string Repo => "Pakeeet";
    [JsonPropertyName("path")] public string Path => "Public/AWBs/Awb.json";
    [JsonPropertyName("message")] public string Message { get; }

    [JsonPropertyName("committer")] public Committer Committer => new();
    [JsonPropertyName("content")] public string Content { get; }
    [JsonPropertyName("branch")] public string Branch { get; }
    [JsonPropertyName("sha")] public string Sha { get; }
}

public class Committer
{
    [JsonPropertyName("name")] public string Name => "Pakeeet Web Request";
    [JsonPropertyName("email")] public string Email => "pakeeet@github.com";
}
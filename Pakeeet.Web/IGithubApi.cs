using System.Runtime.CompilerServices;
using Pakeeet.Web.Dtos;
using Refit;

namespace Pakeeet.Web;

public interface IGithubApi
{
    [Get("/repos/dwifuady/Pakeeet/branches/main")]
    Task<GetBranchResponse> GetMainBranch([Authorize("token")] string token);

    [Post("/repos/dwifuady/Pakeeet/git/refs")]
    Task<CreateBranchResponse> CreateBranch(CreateBranchRequest request, [Authorize("token")] string token);

    [Get("/repos/dwifuady/Pakeeet/contents/Public/AWBs/Awb.json")]
    Task<GetContentResponse> GetAwbList([Authorize(("token"))] string token);

    [Put("/repos/dwifuady/Pakeeet/contents/Public/AWBs/Awb.json")]
    Task UpdateContent(UpdateAwbContentRequest request, [Authorize("token")] string token);

    [Post("/repos/dwifuady/Pakeeet/pulls")]
    Task CreatePullRequest(CreatePRRequest request, [Authorize("token")] string token);
    
    [Get("/repos/dwifuady/Pakeeet/branches/{branch}")]
    Task<GetBranchResponse> GetBranchByName(string branch, [Authorize("token")] string token);
}
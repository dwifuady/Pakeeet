namespace Pakeeet.Web.Dtos;

public class CreatePRRequest
{
    public CreatePRRequest(string awb, string branchName)
    {
        Title = $"Track {awb}";
        Head = branchName;
    }

    public string Owner => "dwifuady";
    public string Repo => "Pakeeet";
    public string Title { get; }
    public string Body { get; set; }
    public string Head { get; }
    public string Base => "main";
}
using System.Text.Json;
using Pakeeet;
using Refit;

const string siCepatApiUrl = "https://content-main-api-production.sicepat.com";

var awbs = GetAwbs();

if (awbs is not null)
{
    foreach (var awb in awbs.AwbNumbers)
    {
        await Process(awb);
    }
}

async Task Process(string awb)
{
    // Check Previous Track Status
    SiCepatDto previousTrackResult = null;
    var resultFileName = $"../../../../Public/Results/data_{awb}.json";
    if (File.Exists(resultFileName))
    {
        var jsonString = File.ReadAllText(resultFileName);
        previousTrackResult = JsonSerializer.Deserialize<SiCepatDto>(jsonString);
    }

    if ((previousTrackResult is not null && !previousTrackResult.Sicepat.Result.HasReceived) ||
        previousTrackResult is null)
    {
        var result = await GetLatestStatus(awb);

        if (result.Sicepat.Status.Code == 200)
        {
            bool isStatusUpdated;
            var newStatus = result.Sicepat.Result.LastStatus;

            if (previousTrackResult is null)
            {
                Console.WriteLine($"First track. {newStatus.DateTime} - {newStatus.Status} - {newStatus.City}");
                isStatusUpdated = true;
            }
            else
            {
                var prevStatus = previousTrackResult.Sicepat.Result.LastStatus;
                if (
                    newStatus.DateTime != prevStatus.DateTime &&
                    newStatus.City != prevStatus.City &&
                    newStatus.Status != prevStatus.Status
                )
                {
                    Console.WriteLine($"Status updated. {newStatus.DateTime} - {newStatus.Status} - {newStatus.City}");
                    isStatusUpdated = true;
                }
                else
                {
                    Console.WriteLine($"No update for this awb.");
                    isStatusUpdated = false;
                }
            }

            if (isStatusUpdated)
            {
                await SaveResult(result, resultFileName);
                //send telegram message
            }
        }
    }
    else
    {
        Console.WriteLine("Already received");
    }
}

Awbs GetAwbs()
{
    const string awbNoFileName = $"../../../../Public/AWBs/Awb.json";
    if (!File.Exists(awbNoFileName)) return null;
    var jsonString = File.ReadAllText(awbNoFileName);
    return JsonSerializer.Deserialize<Awbs>(jsonString);
}

async Task<SiCepatDto> GetLatestStatus(string awb)
{
    var siCepatApi = RestService.For<ISiCepatApi>(siCepatApiUrl);
    return await siCepatApi.CheckAwbAsync(awb);
}

async Task SaveResult(SiCepatDto result, string resultFileName)
{
    await using var streamWriter = File.CreateText(resultFileName);
    await streamWriter.WriteAsync(JsonSerializer.Serialize(result));
}
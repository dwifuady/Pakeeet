using System.Runtime.InteropServices;
using System.Text.Json;
using Pakeeet;
using Refit;

if(args is null || args.Length < 2)
{
    Console.WriteLine("Telegram token or chatId is empty. Use 'dotnet run --project Pakeeet/Pakeeet.csproj -- \"token\" \"chatid\"' command to run");
    return;
}

var telegramToken = args[0];
var chatId = args[1];

var telegramApiUrl = $"https://api.telegram.org/bot{telegramToken}";

const string siCepatApiUrl = "https://content-main-api-production.sicepat.com";

var awbs = GetAwbs();

if (awbs is not null && awbs.AwbNumbers.Count > 0)
{
    foreach (var awb in awbs.AwbNumbers)
    {
        await Process(awb);
    }
}
else
{
    Console.WriteLine("No awb to track");
}

async Task Process(string awb)
{
    if (string.IsNullOrWhiteSpace(awb))
        return;
    
    // Check Previous Track Status
    SiCepatDto previousTrackResult = null;
    var resultFileName = $"../../../../Public/Results/data_{awb}.json";
    if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
    {
        resultFileName = Path.Combine(Directory.GetCurrentDirectory(), "Public", "Results", $"data_{awb}.json");
    }
    if (File.Exists(resultFileName))
    {
        var jsonString = File.ReadAllText(resultFileName);
        previousTrackResult = JsonSerializer.Deserialize<SiCepatDto>(jsonString);
    }

    if ((previousTrackResult is not null && !previousTrackResult.Sicepat.Result.Delivered) ||
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
                await SendTelegramMessage(result);
            }
        }
    }
    else
    {
        Console.WriteLine("Already delivered");
    }
}

Awbs GetAwbs()
{
    var awbNoFileName = $"../../../../Public/AWBs/Awb.json";
    if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
    {
        awbNoFileName = Path.Combine(Directory.GetCurrentDirectory(), "Public", "AWBs", "Awb.json");
    }

    if (!File.Exists(awbNoFileName)) 
    {
        Console.WriteLine($"File not exists - {awbNoFileName}");
        return null;
    }
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

async Task SendTelegramMessage(SiCepatDto result)
{
    Console.WriteLine("Sending telegram message..");
    var telegramApi = RestService.For<ITelegramBotApi>(telegramApiUrl);
    await telegramApi.SendTelegramMessage(chatId, await GetMessage(result.Sicepat.Result));
}

async Task<string> GetMessage(Result result)
{
    if (result.Delivered)
    {
        return await Task.Run(() => 
            @$"<strong>{result.WaybillNumber}</strong>
From {result.Sender} - {result.SenderAddress} at {result.SendDate} has been Delivered.

{result.PODReceiver} : {result.PODReceiverTime}"
        );    
    }
    
    return await Task.Run(() => 
        @$"<strong>{result.WaybillNumber}</strong>
From {result.Sender} - {result.SenderAddress} at {result.SendDate} 

Status updated
{result.LastStatus.DateTime}: {result.LastStatus.ReceiverName ?? result.LastStatus.City}"
    );
}
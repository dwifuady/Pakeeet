![Update Tracking Data](https://github.com/dwifuady/Pakeeet/actions/workflows/refresh-data.yml/badge.svg)

# Pakeeet
> :information_source: Currently only for Sicepat

## Prerequisites
- NET 6 (https://dotnet.microsoft.com/en-us/download/dotnet/6.0)
- A Telegram bot (https://core.telegram.org/bots#3-how-do-i-create-a-bot)

Currently, the AWB ids need to be stored in Public/AWBs/Awb.json, so put the AWB you want to track on that file.

## Running the app locally
``` 
dotnet run --project Pakeeet/Pakeeet.csproj -- "telegram bot token" "chatid"
```

## Create the scheduler with Github action
- Download or fork this to your github repo
- Add your TELEGRAM_API_TOKEN and TELEGRAM_CHAT_ID to your github repo secrets

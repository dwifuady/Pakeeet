using Refit;

namespace Pakeeet;

public interface ITelegramBotApi
{
    [Get("/sendMessage?chat_id={chatId}&text={text}&parse_mode=HTML")]
    Task<string> SendTelegramMessage(string chatId, string text);
}

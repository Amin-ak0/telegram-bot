using Newtonsoft.Json.Linq;
using Telegram.Bot;
using Telegram.Bot.Types.ReplyMarkups;

namespace telegram_bot
{
    public class BotSetting
    {
        public TelegramBotClient client;


        public void Start()
        {
            client = new TelegramBotClient("6821094342:AAGvgCM2wShyw4zcPxs6I6Cc8IoLYBC1jQU");
            client.OnMessage += Client_OnMessage;
            client.OnCallbackQuery += Client_OnCallbackQuery;
            client.StartReceiving();
            Thread.Sleep(-1);
        }

        private async void Client_OnCallbackQuery(object? sender, Telegram.Bot.Args.CallbackQueryEventArgs e)
        {
            var id = e.CallbackQuery.From.Id;
            var text = e.CallbackQuery.Data.ToString();
            var res = DateTime.Now.ToString();

            switch (text)
            {
                case "Now":
                    client.EditMessageTextAsync(id, e.CallbackQuery.Message.MessageId,
                        "\n<b>PROJECT BOT</b>\n" +
                       $"\n {res} ",
                        Telegram.Bot.Types.Enums.ParseMode.Html);
                    break;
                case "BtcPrice":
                    using (HttpClient httpClient = new HttpClient())
                    {
                        string apiUrl = "https://api.kucoin.com/api/v1/market/stats?symbol=BTC-USDT";

                        HttpResponseMessage response = await httpClient.GetAsync(apiUrl);
                        if (response.IsSuccessStatusCode)
                        {
                            string apiresponse = await response.Content.ReadAsStringAsync();

                            string parametervalue = ParseParameterValue(apiresponse);

                            await client.SendTextMessageAsync(id, $"Btcprice is {parametervalue}");
                        }
                        else
                        {
                            await client.SendTextMessageAsync(id, "API request failed.");
                        }
                    }
                    break;
                case "Sunset":
                    using (HttpClient httpClient = new HttpClient())
                    {
                        string apiUrl = "https://api.sunrise-sunset.org/json?lat=35.807579&lng=50.987419&date=today";

                        HttpResponseMessage response = await httpClient.GetAsync(apiUrl);
                        if (response.IsSuccessStatusCode)
                        {
                            string apiresponse = await response.Content.ReadAsStringAsync();

                            string parametervalue = ParseParameterValue1(apiresponse);

                            await client.SendTextMessageAsync(id, $"Sunset is {parametervalue}");
                        }
                        else
                        {
                            await client.SendTextMessageAsync(id, "API request failed.");
                        }
                    }
                    break;


            }

        }

        private async void Client_OnMessage(object? sender, Telegram.Bot.Args.MessageEventArgs e)
        {
            var id = e.Message.From.Id;
            var text = e.Message.Text;
            if (text.ToLower() == "/start")
            {
                client.SendTextMessageAsync(id,
                        "\n<b>SALAM</b>\n" +
                        "\n gozine mored nazar ra entekhab konid!!!",
                        Telegram.Bot.Types.Enums.ParseMode.Html, replyMarkup: CreateButton());
            }


        }
        static string ParseParameterValue(string responseBody)
        {
            JObject json = JObject.Parse(responseBody);
            string desiredValue = json["data"]["buy"].ToString();
            return desiredValue;
        }
        static string ParseParameterValue1(string responseBody)
        {
            JObject json = JObject.Parse(responseBody);
            string desiredValue = json["results"]["sunset"].ToString();
            return desiredValue;
        }
        public InlineKeyboardMarkup CreateButton()
        {
            List<InlineKeyboardButton> btn = new List<InlineKeyboardButton>();
            btn.Add(new InlineKeyboardButton { Text = "Now", CallbackData = "Now" });
            btn.Add(new InlineKeyboardButton { Text = "BtcPrice", CallbackData = "BtcPrice" });
            btn.Add(new InlineKeyboardButton { Text = "Sunset", CallbackData = "Sunset" });


            var menu = new List<InlineKeyboardButton[]>();
            menu.Add(new[] { btn[0], btn[1], btn[2] });

            var main = new InlineKeyboardMarkup(menu.ToArray());
            return main;


        }
    }



}


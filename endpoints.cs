using Discord;
using Discord.WebSocket;
using System;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;


public static class Endpoints
{
    static HttpClient endpointClient = new HttpClient();

    static Endpoints()
    {
        endpointClient.DefaultRequestHeaders.Add("user-agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/136.0.0.0 Safari/537.36");
    }
    public static async Task HandleEchoCommand(SocketSlashCommand command)
    {
        var messageOption = command.Data.Options.First(option => option.Name == "message");
        var response = messageOption.Value.ToString();
        await command.RespondAsync(response);

    }
    public static async Task HandleFirstGlobalCommand(SocketSlashCommand command)
    {
        var response = command.Data.ToString();
        await command.RespondAsync(response);
    }

    public static async Task HandleHelpCommand(SocketSlashCommand command)
    {
        //var embedBuilder = new EmbedBuilder();
        //embedBuilder.WithAuthor("Help Command");

        await command.RespondAsync("ASDAWDAWWD");
    }

    internal static async Task HandleGetRandomMangaCommand(SocketSlashCommand command)
    {
        HttpResponseMessage response = await endpointClient.GetAsync("https://api.mangadex.org/manga/random?contentRating%5B%5D=safe&contentRating%5B%5D=suggestive&contentRating%5B%5D=erotica&includedTagsMode=AND&excludedTagsMode=OR");

        string responseBody = await response.Content.ReadAsStringAsync();

        JsonDocument doc = JsonDocument.Parse(responseBody);

        JsonElement root = doc.RootElement;

        string mangaName = "Name: ";
        string mangaTags = "tags: ";

        mangaName += root.GetProperty("data").GetProperty("attributes").GetProperty("title").GetProperty("en").ToString();

        try
        {
            for (int i = 0; true; i++)
            {
                mangaTags += root.GetProperty("data").GetProperty("attributes").GetProperty("tags")[i].GetProperty("attributes").GetProperty("name").GetProperty("en").ToString() + ", ";
            }
        }
        catch
        {
            //loop ends when out of index
        }

        if (mangaTags == "tags: ")
        {
            mangaTags = "tags: not found";
        }

        string mangaInfo = mangaName + "\n" + mangaTags;

        var embedBuilder = new EmbedBuilder()
        .WithTitle(mangaInfo);

        await command.RespondAsync(embed: embedBuilder.Build());
    }

}
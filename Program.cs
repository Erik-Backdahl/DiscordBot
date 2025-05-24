using System;
using System.IO;
using System.Threading.Tasks;
using Discord;
using Discord.WebSocket;
using Discord.Commands;
using Discord.Net;
using Newtonsoft.Json;
using Microsoft.VisualBasic;
using System.Linq;

public class Program
{
    private static DiscordSocketClient client;

    public static async Task Main()
    {
        client = new DiscordSocketClient();

        client.Log += Log;

        //  You can assign your bot token to a string, and pass that in to connect.
        //  This is, however, insecure, particularly if you plan to have your code hosted in a public repository.


        var token = File.ReadAllText(@"C:\Users\Erik\VSC\PROJECTS\DiscordBotTest\token\token.txt");


        

        client.Ready += Client_Ready;

        client.SlashCommandExecuted += SlashCommandHandler;

        await client.LoginAsync(TokenType.Bot, token);

        await client.StartAsync();

        // Block this task until the program is closed.
        await Task.Delay(-1);


    }

    public static async Task Client_Ready()
    {
        var firstGlobalCommand = new SlashCommandBuilder()
        .WithName("first-global-command")
        .WithDescription("First Test Command");
        await client.CreateGlobalApplicationCommandAsync(firstGlobalCommand.Build());

        var globalCommandHelp = new SlashCommandBuilder()
        .WithName("help")
        .WithDescription("displays all commands and how to use them")
        .AddOption("specific-command", ApplicationCommandOptionType.String, "OPTIONAL displays a more advanced description of a commmand", isRequired: false);

        await client.CreateGlobalApplicationCommandAsync(globalCommandHelp.Build());

        var globalcommandEcho = new SlashCommandBuilder()
        .WithName("echo")
        .WithDescription("repeates your input back")
        .AddOption("message", ApplicationCommandOptionType.String, "input the message you want repeated", isRequired: true, minLength: 1, maxLength: 500);

        await client.CreateGlobalApplicationCommandAsync(globalcommandEcho.Build());

        var globalcommandGetRandomManga = new SlashCommandBuilder()
        .WithName("random-manga")
        .WithDescription("returns a random manga");

        await client.CreateGlobalApplicationCommandAsync(globalcommandGetRandomManga.Build());

        var globalcommandLookUp = new SlashCommandBuilder()
        .WithName("look-up")
        .WithDescription("gives info on a manga of your choice loop with : placeholder or use mangadex.org to find id")
        .AddOption("id", ApplicationCommandOptionType.String, "enter id of manga", isRequired: false);

        await client.CreateGlobalApplicationCommandAsync(globalcommandLookUp.Build());
    }
    private static async Task SlashCommandHandler(SocketSlashCommand command)
    {
        switch (command.Data.Name)
        {
            case "first-global-command":
                await Endpoints.HandleFirstGlobalCommand(command);
                break;
            case "help":
                await Endpoints.HandleHelpCommand(command);
                break;
            case "echo":
                await Endpoints.HandleEchoCommand(command);
                break;
            case "random-manga":
                await Endpoints.HandleGetRandomMangaCommand(command);
                break;
            case "look-up":
                await Endpoints.HandleLookUpCommand(command);
                break;
            default:
                await command.RespondAsync("error finding command THIS SHOULD NEVER HAPPEN");
                break;
        }

    }
    private static Task Log(LogMessage msg)
    {
        Console.WriteLine(msg.ToString());
        return Task.CompletedTask;
    }
}


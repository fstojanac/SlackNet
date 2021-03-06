﻿using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

namespace SlackNet.SocketModeExample
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var settings = ReadSettings();

            var slackServices = new SlackServiceFactory()
                .UseApiToken(settings.ApiToken)
                .UseAppLevelToken(settings.AppLevelToken)
                .RegisterEventHandler(ctx => new PingHandler(ctx.ServiceFactory.GetApiClient()));

            using var socketModeClient = slackServices.GetSocketModeClient();

            await socketModeClient.Connect();

            Console.WriteLine("Connected. Press any key to exit...");
            await Task.Run(Console.ReadKey);
        }

        private static AppSettings ReadSettings()
        {
            return new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .AddUserSecrets<Program>()
                .Build()
                .Get<AppSettings>();
        }
    }
}
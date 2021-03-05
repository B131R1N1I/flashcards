using System;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;

namespace flashcards_server
{
    internal static class Program
    {
        public static readonly DatabaseManagement.DatabaseManagement db = new DatabaseManagement.DatabaseManagement("localhost", "flashcards_app", "fc_app", "flashcards");

        private static void Main(string[] args)
        {
            db.OpenConnection();
            CreateHostBuilder(args).Build().Run();
        }

        private static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}

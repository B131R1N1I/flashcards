using System;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;

namespace flashcards_server
{
    static class Program
    {
        public static DatabaseManagement.DatabaseManagement db = new DatabaseManagement.DatabaseManagement("localhost", "flashcards_app", "fc_app", "flashcards");

        static void Main(string[] args)
        {
            db.OpenConnection();
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<flashcards_server.API.Startup>();
                });


        static void AddUserEvents(object obj, EventArgs e)
        {
            ((User.User)obj).NameChangedEventHandler += db.UpdateUserName;
            ((User.User)obj).EmailChangedEventHandler += db.UpdateUserEmail;
            ((User.User)obj).SurnameChangedEventHandler += db.UpdateUserSurname;
            ((User.User)obj).PasswordChangedEventHandler += db.UpdateUserPassword;
        }
    }
}

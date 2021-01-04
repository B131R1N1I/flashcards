using System;



namespace flashcards_server
{
    class Program
    {

        static void Main()
        {
            Console.WriteLine("Hello World!");
            

            var u = new User.User("aaab1234", "aaaaa2@wp.pl", "jan", "kowalski", "haslo", 4);
            System.Console.WriteLine(u.email);
            System.Console.WriteLine(u.surname);
            System.Console.WriteLine(u.id);
            var db = new DatabaseManagement.DatabaseManagement("localhost", "flashcards_app", "fc_app", "flashcards");
            db.OpenConnection();
            db.AddUserToDatabase(u);
            db.UpdateUserEmail(u, "żółć@wąż→→.com");
            // System.Console.WriteLine($"{us[0]} \t {us[1]}");
            // System.Console.WriteLine(i[]);
            System.Console.WriteLine("end");
        }
    }
}
// var connString = "Host=myserver;Username=mylogin;Password=mypass;Database=mydatabase";

// var conn = new NpgsqlConnection("Server=localhost;User Id=flashcards_app; Password=fc_app;Database=flashcards");
// conn.Open();
// var cmd = new NpgsqlCommand("SELECT * FROM users;", conn);
// var us = cmd.ExecuteReader();

// while (us.Read())
//     for (int i = 0; i > -5; i++)
//     {
//         try
//         {
//             System.Console.WriteLine(us[i]);
//         }
//         catch
//         {
//             break;
//         }
//     }
// conn.Close();
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
            System.Console.WriteLine(db.IsUsernameUnique("aaab1234"));
            var getuser = db.GetUser(4);
            System.Console.WriteLine(getuser.email);


            var getu = db.GetUser("aaab1234");
            System.Console.WriteLine(getu.email);
            System.Console.WriteLine("end");
        }
    }
}

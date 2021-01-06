using System;



namespace flashcards_server
{
    class Program
    {

        static void Main()
        {
            Console.WriteLine("Hello World!");


            var u = new User.User("Usernr2", "userek@wp.pl", "Paweł", "Kowalski", "trudne_hasło");
            var db = new DatabaseManagement.DatabaseManagement("localhost", "flashcards_app", "fc_app", "flashcards");

            System.Console.WriteLine(u.email);
            System.Console.WriteLine(u.surname);
            // System.Console.WriteLine(u.id);
            db.OpenConnection();

            try
            {
                db.AddUserToDatabase(u);
            }
            catch (Exception e)
            {
                System.Console.WriteLine(e.Message);
            }
            System.Console.WriteLine(db.IsUsernameUnique("aaab1234"));

            try
            {
                var getuser = db.GetUserById(1);
                System.Console.WriteLine(getuser);
            }
            catch (Exception e)
            {
                System.Console.WriteLine(e.Message);
            }

            try
            {
                var getu = db.GetUserByUsername("Usernr2");
                System.Console.WriteLine(getu);
            }
            catch (Exception e)
            {
                System.Console.WriteLine(e.Message);
            }

            try
            {
                var getu = db.GetUserByEmail("email@werw.pl");
                System.Console.WriteLine(getu);
            }
            catch (Exception e)
            {
                System.Console.WriteLine(e.Message);
            }
            System.Console.WriteLine("end");
        }
    }
}

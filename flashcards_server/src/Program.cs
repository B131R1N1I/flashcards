using System;

namespace flashcards_server
{
    class Program
    {
        static void Main()
        {
            Console.WriteLine("Hello World!");
            var u = new User.User("aaa123", "aaa@wp.pl", "jan", "kowalski", "haslo");
            System.Console.WriteLine(u.email);
            System.Console.WriteLine(u.id);
        }
    }
}

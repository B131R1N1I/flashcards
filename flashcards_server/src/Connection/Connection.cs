using System;
using System.Linq;
using System.Text.Json;
using System.Net.Sockets;
using System.Net;


namespace flashcards_server.Connection
{

    public class Connection
    {
        public Connection(string iPAddress, Int32 port)
        {
            server = new TcpListener(IPAddress.Parse(iPAddress), port);
            server.Start();
            System.Console.WriteLine($"Server: ip {iPAddress}, port: {port}");
        }

        public string ReciveData()
        {
            var cl = server.AcceptTcpClient();
            var stream = cl.GetStream();
            var buffer = new Byte[256];
            var data = stream.Read(buffer);
            stream.Close();
            cl.Close();
            return System.Text.Encoding.UTF8.GetString(buffer, 0, data);
        }

        TcpListener server = null;

    }
}
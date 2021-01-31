using System;
using Npgsql;

namespace flashcards_server.DatabaseManagement
{
    partial class DatabaseManagement
    {
        public Card.Card GetCardByID(uint id)
        {
            using (var cmd = new NpgsqlCommand($"SELECT id, question, answer, picture, in_set, picture is null AS picture_is_null FROM cards WHERE id = {id};", conn))
            {
                using (var reader = cmd.ExecuteReader())
                {
                    reader.Read();
                    // if (reader.)
                    if (!(bool)reader.GetValue(5))
                    {
                        var len = reader.GetBytes(3, 0, null, 0, 0);
                        var buffer = new Byte[len];
                        reader.GetBytes(3, 0, buffer, 0, (int)len);
                        return new Card.Card(reader.GetString(1), reader.GetString(2), buffer, (uint)reader.GetInt32(4), (uint)reader.GetInt32(0));
                    }
                    else
                        return new Card.Card(reader.GetString(1), reader.GetString(2), null, (uint)reader.GetInt32(4), (uint)reader.GetInt32(0));


                }
            }
        }
    }
}
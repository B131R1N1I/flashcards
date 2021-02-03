using System;
using Npgsql;

namespace flashcards_server.DatabaseManagement
{
    partial class DatabaseManagement
    {

        public void AddCardToDatabase(Card.Card card)
        {
            using (var cmd = new NpgsqlCommand("INSERT INTO cards (question, answer, picture, in_set)" +
            // $"VALUES ('{card.question}', '{card.answer}', @Image, {card.inSet});", conn))
            $"VALUES ('{card.question}', '{card.answer}', @Image, {card.inSet});", conn))
                
            {
                System.Console.WriteLine(cmd.Parameters);
                NpgsqlParameter parameter = cmd.CreateParameter();
                parameter.ParameterName = "@Image";
                parameter.NpgsqlDbType = NpgsqlTypes.NpgsqlDbType.Bytea;
                parameter.Value = card.image;
                cmd.Parameters.Add(parameter);

                cmd.ExecuteNonQuery();
            }
        }
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
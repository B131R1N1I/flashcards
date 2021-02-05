using System;
using Npgsql;
using System.Collections.Generic;

namespace flashcards_server.DatabaseManagement
{
    partial class DatabaseManagement
    {

        public void AddCardToDatabase(Card.Card card)
        {
            using (var cmd = new NpgsqlCommand("INSERT INTO cards (question, answer, picture, in_set)" +
            $"VALUES ('{card.question}', '{card.answer}', @Image, {card.inSet});", conn))
            {
                NpgsqlParameter parameter = cmd.CreateParameter();
                parameter.ParameterName = "@Image";
                parameter.NpgsqlDbType = NpgsqlTypes.NpgsqlDbType.Bytea;
                parameter.Value = card.image;
                cmd.Parameters.Add(parameter);

                cmd.ExecuteNonQuery();
            }
        }

        public void UpdateCardQuestion(Card.Card card, string question)
        {
            using (var cmd = new NpgsqlCommand($"UPDATE cards SET question = '{question}' WHERE id={card.id};", conn))
            {
                cmd.ExecuteNonQuery();
            }
        }

        public void UpdateCardAnswer(Card.Card card, string answer)
        {
            using (var cmd = new NpgsqlCommand($"UPDATE cards SET answer = '{answer}' WHERE id={card.id};", conn))
            {
                cmd.ExecuteNonQuery();
            }
        }

        public void UpdateCardPicture(Card.Card card, byte[] picture)
        {
            throw new NotImplementedException();
        }

        public Card.Card GetCardByID(uint id)
        {
            using (var cmd = new NpgsqlCommand($"SELECT id, question, answer, picture, in_set, picture FROM cards WHERE id = {id};", conn))
            {
                using (var reader = cmd.ExecuteReader())
                {
                    reader.Read();
                    if (reader.GetValue(5) != null)
                    {
                        var len = reader.GetBytes(3, 0, null, 0, 0);
                        var buffer = new Byte[len];
                        reader.GetBytes(3, 0, buffer, 0, (int)len);
                        return new Card.Card(reader.GetString(2), reader.GetString(1), buffer, (uint)reader.GetInt32(4), (uint)reader.GetInt32(0));
                    }
                    else
                        return new Card.Card(reader.GetString(2), reader.GetString(1), null, (uint)reader.GetInt32(4), (uint)reader.GetInt32(0));


                }
            }
        }

        public List<Card.Card> GetCardsBySet(Set.Set set)
        {
            var listOfCards = new List<Card.Card>();
            using (var cmd = new NpgsqlCommand($"SELECT * FROM cards WHERE in_set = {set.id};", conn))
            {
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())

                    {
                        var len = reader.GetBytes(3, 0, null, 0, 0);
                        var buffer = new Byte[len];
                        reader.GetBytes(3, 0, buffer, 0, (int)len);
                        listOfCards.Add(new Card.Card(reader.GetString(2), reader.GetString(1), buffer, (uint)reader.GetInt32(4), (uint)reader.GetInt32(0)));
                    }
                    return listOfCards;
                }
            }
        }
    }
}
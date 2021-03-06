using System;
using Npgsql;
using System.Collections.Generic;
using System.IO;
using System.Drawing;

namespace flashcards_server.DatabaseManagement
{
    partial class DatabaseManagement
    {

        public void AddCardToDatabase(Card.Card card)
        {
            using (var cmd = new NpgsqlCommand("INSERT INTO cards (question, answer, picture, in_set)" +
            $"VALUES ('{card.question}', '{card.answer}', @Image, {card.inSet});", Conn))
            {
                NpgsqlParameter parameter = cmd.CreateParameter();
                parameter.ParameterName = "@Image";
                parameter.NpgsqlDbType = NpgsqlTypes.NpgsqlDbType.Bytea;

                parameter.Value = ConvertImageToBytes(card.image);
                cmd.Parameters.Add(parameter);

                cmd.ExecuteNonQuery();
            }
        }

        protected void UpdateCardQuestion(Card.Card card, string question)
        {
            using (var cmd = new NpgsqlCommand($"UPDATE cards SET question = '{question}' WHERE id={card.id};", Conn))
                cmd.ExecuteNonQuery();
        }

        public void UpdateCardQuestion(object source, Card.Card.QuestionEventArgs args)
        {
            UpdateCardQuestion((Card.Card)source, args.question);
        }

        public void UpdateCardAnswer(Card.Card card, string answer)
        {
            using (var cmd = new NpgsqlCommand($"UPDATE cards SET answer = '{answer}' WHERE id={card.id};", Conn))
                cmd.ExecuteNonQuery();
        }

        public void UpdateCardAnswer(object source, Card.Card.AnswerEventArgs args)
        {
            UpdateCardAnswer((Card.Card)source, args.answer);
        }

        public void UpdateCardPicture(Card.Card card, Bitmap img)
        {
            using (var cmd = new NpgsqlCommand($"UPDATE cards SET picture = @Image WHERE id = {card.id};", Conn))
            {
                NpgsqlParameter parameter = cmd.CreateParameter();
                parameter.ParameterName = "@Image";
                parameter.NpgsqlDbType = NpgsqlTypes.NpgsqlDbType.Bytea;
                parameter.Value = ConvertImageToBytes(img);
                cmd.Parameters.Add(parameter);

                cmd.ExecuteNonQuery();
            }
        }

        public void UpdateCardPicture(object source, Card.Card.ImageEventArgs args)
        {
            UpdateCardPicture((Card.Card)source, args.image);
        }


        public Card.Card GetCardById(uint id)
        {
            using (var cmd = new NpgsqlCommand($"SELECT id, question, answer, picture, in_set FROM cards WHERE id = {id};", Conn))
            {
                using (var reader = cmd.ExecuteReader())
                {
                    reader.Read();
                    if (!reader.HasRows)
                        throw new NpgsqlException("No Card found by ID");
                    try
                    {
                        var imageBytes = (byte[])reader.GetValue(3);
                        var ms = new MemoryStream(imageBytes);
                        var bmap = new Bitmap(ms);
                        return new Card.Card(reader.GetString(2), reader.GetString(1), bmap, (uint)reader.GetInt32(4), (uint)reader.GetInt32(0));

                    }
                    catch (Exception)
                    {
                        return new Card.Card(reader.GetString(2), reader.GetString(1), null, (uint)reader.GetInt32(4), (uint)reader.GetInt32(0));
                    }


                }
            }
        }

        public List<Card.Card> GetCardsBySet(Set.Set set)
        {
            var listOfCards = new List<Card.Card>();
            using (var cmd = new NpgsqlCommand($"SELECT id, question, answer, picture, in_set FROM cards WHERE in_set = {set.id};", Conn))
            {
                using (var reader = cmd.ExecuteReader())
                {
                    if (!reader.HasRows)
                        throw new NpgsqlException("No Card found by setID");
                    while (reader.Read())
                    {
                        try
                        {
                            var imageBytes = (byte[])reader.GetValue(3);
                            var ms = new MemoryStream(imageBytes);
                            var bmap = new Bitmap(ms);
                            listOfCards.Add(new Card.Card(reader.GetString(2), reader.GetString(1), bmap, (uint)reader.GetInt32(4), (uint)reader.GetInt32(0)));
                        }
                        catch (Exception)
                        {
                            listOfCards.Add(new Card.Card(reader.GetString(2), reader.GetString(1), null, (uint)reader.GetInt32(4), (uint)reader.GetInt32(0)));
                        }
                    }
                    return listOfCards;
                }
            }
        }

        private Byte[] ConvertImageToBytes(Bitmap img)
        {
            var converter = new ImageConverter();

            return (byte[])converter.ConvertTo(img, typeof(byte[]));
        }
    }
}
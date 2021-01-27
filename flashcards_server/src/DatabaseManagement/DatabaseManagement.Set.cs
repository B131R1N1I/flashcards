using Npgsql;
using System;
using NpgsqlTypes;
using System.Collections.Generic;

namespace flashcards_server.DatabaseManagement
{
    partial class DatabaseManagement
    {
        public void AddSetToDatabase(Set.Set set)
        {
            using (var cmd = new NpgsqlCommand($"INSERT INTO sets (name, creator_id, owner_id, created_date, last_modification, is_public) " +
            $"VALUES ('{set.name}', {set.creator.id}, {set.owner.id}, '{(NpgsqlTypes.NpgsqlDateTime)set.createdDate}', " +
            $"'{(NpgsqlTypes.NpgsqlDateTime)set.lastModificationDate}', {set.isPublic});", conn))
            {
                try
                {
                    cmd.ExecuteNonQuery();
                }
                catch (PostgresException e)
                {
                    System.Console.WriteLine("Cannot add user to DB");
                    System.Console.WriteLine(">>>" + e.MessageText);
                }
            }
        }
        public List<Set.Set> GetSetsByOwner(User.User user)
        {
            var listOfSets = new List<Set.Set>();
            var listOfSetsTemp = new List<Object[]>();
            using (var cmd = new NpgsqlCommand($"SELECT * FROM sets WHERE owner_id = {user.id};", conn))
            {
                try
                {

                    using (var output = cmd.ExecuteReader())
                    {

                        while (output.Read())
                        {
                            var temp = new Object[7];
                            output.GetValues(temp);
                            listOfSetsTemp.Add(temp);
                        }
                    }
                    foreach (var i in listOfSetsTemp)
                    {
                        listOfSets.Add(new Set.Set((string)i[1], GetUserById((int)i[2]), GetUserById((int)i[3]), (DateTime)i[4], (DateTime)i[5], (bool)i[6], (int)i[0]));
                    }

                }
                catch (PostgresException e)
                {
                    System.Console.WriteLine("Cannot read any user from DB");
                    System.Console.WriteLine(">>>" + e.MessageText);
                }

            }
            return listOfSets;
        }

        public List<Set.Set> GetSetsByCreator(User.User user)
        {
            var listOfSets = new List<Set.Set>();
            var listOfSetsTemp = new List<Object[]>();
            using (var cmd = new NpgsqlCommand($"SELECT * FROM sets WHERE creator_id = {user.id};", conn))
            {
                try
                {

                    using (var output = cmd.ExecuteReader())
                    {
                        if (!output.HasRows)
                            return listOfSets;
                        while (output.Read())
                        {
                            var temp = new Object[7];
                            output.GetValues(temp);
                            listOfSetsTemp.Add(temp);
                        }
                    }
                    foreach (var i in listOfSetsTemp)
                    {
                        listOfSets.Add(new Set.Set((string)i[1], GetUserById((int)i[2]), GetUserById((int)i[3]), DateTime.Parse(i[4].ToString()), DateTime.Parse(i[5].ToString()), (bool)i[6], (int)i[0]));
                    }

                }
                catch (PostgresException e)
                {
                    System.Console.WriteLine("Cannot read any user from DB");
                    System.Console.WriteLine(">>>" + e.MessageText);
                }

            }
            return listOfSets;
        }

        public Set.Set GetSetByName(String name)
        {
            Set.Set set;
            using (var cmd = new NpgsqlCommand($"SELECT * FROM sets where name='{name}' LIMIT 1;", conn))
            {
                Object[] setTemp = new Object[7];
                try
                {
                    using (var reader = cmd.ExecuteReader())
                    {
                        if (!reader.HasRows)
                            throw new NpgsqlException($"(set)No row found by name: {name}");
                        reader.Read();
                        for (int i = 0; i < 7; i++)
                            setTemp[i] = reader[i];
                    }
                    set = new Set.Set((String)setTemp[1], GetUserById((Int32)setTemp[2]), GetUserById((Int32)setTemp[3]), DateTime.Parse(setTemp[4].ToString()), DateTime.Parse(setTemp[5].ToString()), (bool)setTemp[6], (Int32)setTemp[0]);
                }
                catch (NpgsqlException)
                {
                    throw;
                }
            }
            return set;
        }

        public Set.Set GetSetById(int id)
        {
            var setTemp = new Object[7];
            using (var cmd = new NpgsqlCommand($"SELECT * FROM sets WHERE id = {id};", conn))
            {
                using (var reader = cmd.ExecuteReader())
                {
                    if (!reader.HasRows)
                        throw new NpgsqlException($"No set found by id {id}");
                    reader.Read();
                    for (int i = 0; i < 7; i++)
                        setTemp[i] = reader[i];
                }
            }
            return new Set.Set((String)setTemp[1], GetUserById((Int32)setTemp[2]), GetUserById((Int32)setTemp[3]), DateTime.Parse(setTemp[4].ToString()), DateTime.Parse(setTemp[5].ToString()), (bool)setTemp[6], (Int32)setTemp[0]);
        }

        public void TransferOwnership(Set.Set set, User.User user)
        {
            using (var cmd = new NpgsqlCommand($"UPDATE sets SET owner_id = {user.id} WHERE id={set.id};", conn))
            {
                cmd.ExecuteNonQuery();
            }
        }

        public bool IsSetNameUnique(String name)
        {
            using (var cmd = new NpgsqlCommand($"SELECT COUNT(*) FROM sets WHERE LOWER(sets.name) = LOWER('{name}');", conn))
            {
                return (Int64)cmd.ExecuteScalar() == 0;
            }
        }
    }
}
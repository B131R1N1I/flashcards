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
                                               $"VALUES ('{set.name}', {set.creator}, {set.owner}, '{(NpgsqlTypes.NpgsqlDateTime)set.createdDate}', " +
                                               $"'{(NpgsqlTypes.NpgsqlDateTime)set.lastModificationDate}', {set.isPublic});", conn))
            {
                try
                {
                    if (IsSetNameUnique(set.name))
                        cmd.ExecuteNonQuery();
                    else
                        throw new NpgsqlException($"'{set.name}' is already used set name");
                }
                catch (Exception)
                {
                    throw;
                }
            }
        }

        public List<Set.Set> GetSetsByOwner(User.User user)
        {
            using (var cmd = new NpgsqlCommand($"SELECT * FROM sets WHERE owner_id = {user.id};", conn))
                return _GetListOfSetsByCmd(cmd);
        }

        public List<Set.Set> GetSetsByCreator(User.User user)
        {
            using (var cmd = new NpgsqlCommand($"SELECT * FROM sets WHERE creator_id = {user.id};", conn))
                return _GetListOfSetsByCmd(cmd);
        }

        public List<Set.Set> GetActiveSetsforUser(User.User user)
        {
            using (var cmd = new NpgsqlCommand($"SELECT sets.* FROM sets JOIN active_sets AS acts ON sets.id = acts.set_id " +
                                               $"WHERE acts.user_id = {user.id};", conn))
                return _GetListOfSetsByCmd(cmd);
        }

        public List<Set.Set> GetPublicSets()
        {
            using (var cmd = new NpgsqlCommand($"SELECT * FROM sets WHERE is_public = true;", conn))
                return _GetListOfSetsByCmd(cmd);
        }

        public void MakeSetActive(User.User user, Set.Set set)
        {
            using (var cmd = new NpgsqlCommand($"DELETE FROM active_sets WHERE user_id={user.id} AND set_id={set.id}; " +
                                               $"INSERT INTO active_sets (user_id, set_id) VALUES ({user.id}, {set.id});", conn))
                cmd.ExecuteNonQuery();
        }

        public void MakeSetInactive(User.User user, Set.Set set)
        {
            using (var cmd = new NpgsqlCommand($"DELETE FROM active_sets WHERE user_id={user.id} AND set_id={set.id}; ", conn))
                cmd.ExecuteNonQuery();
        }

        public List<Set.Set> GetPublicSetsByNameLike(String name)
        {
            using (var cmd = new NpgsqlCommand($"SELECT * FROM sets where name LIKE '%{name}%' AND is_public = true;", conn))
                return _GetListOfSetsByCmd(cmd);
        }

        public Set.Set GetSetByName(String name)
        {
            using (var cmd = new NpgsqlCommand($"SELECT * FROM sets where name='{name}' LIMIT 1;", conn))
                return _GetSetByCmd(cmd);
        }

        public Set.Set GetSetById(uint id)
        {
            using (var cmd = new NpgsqlCommand($"SELECT * FROM sets WHERE id = {id};", conn))
                return _GetSetByCmd(cmd);
        }

        public void TransferOwnership(Set.Set set, User.User user)
        {
            using (var cmd = new NpgsqlCommand($"UPDATE sets SET owner_id = {user.id} WHERE id={set.id};", conn))
                cmd.ExecuteNonQuery();
        }

        public bool IsSetNameUnique(String name)
        {
            using (var cmd = new NpgsqlCommand($"SELECT COUNT(*) FROM sets WHERE LOWER(sets.name) = LOWER('{name}');", conn))
                return (Int64)cmd.ExecuteScalar() == 0;
        }

        private List<Set.Set> _GetListOfSetsByCmd(NpgsqlCommand cmd)
        {
            var listOfSets = new List<Set.Set>();
            using (var reader = cmd.ExecuteReader())
            {
                if (!reader.HasRows)
                    return listOfSets;
                while (reader.Read())
                {
                    var temp = new Object[7];
                    reader.GetValues(temp);
                    listOfSets.Add(new Set.Set(reader.GetString(1), Convert.ToUInt32(reader.GetInt32(2)),
                                               Convert.ToUInt32(reader.GetInt32(3)), reader.GetDateTime(4),
                                               reader.GetDateTime(5), reader.GetBoolean(6), reader.GetInt32(0)));
                }
            }
            return listOfSets;
        }

        private Set.Set _GetSetByCmd(NpgsqlCommand cmd)
        {
            using (var reader = cmd.ExecuteReader())
            {
                if (!reader.HasRows)
                    throw new NpgsqlException("No set found");
                reader.Read();
                return new Set.Set(reader.GetString(1), Convert.ToUInt32(reader.GetInt32(2)),
                                   Convert.ToUInt32(reader.GetInt32(3)), reader.GetDateTime(4),
                                   reader.GetDateTime(5), reader.GetBoolean(6), reader.GetInt32(0));
            }
        }

        public void UpdateSetName(Set.Set set, String name)
        {
            using (var cmd = new NpgsqlCommand($"UPDATE sets SET name = '{name}' WHERE id = {set.id};", conn))
            {
                cmd.ExecuteNonQuery();
            }
        }

        public void UpdateSetIsPublic(Set.Set set, bool isPublic)
        {
            using (var cmd = new NpgsqlCommand($"UPDATE sets SET is_public = {isPublic} WHERE id = {set.id};", conn))
            {
                cmd.ExecuteNonQuery();
            }
        }
    }
}

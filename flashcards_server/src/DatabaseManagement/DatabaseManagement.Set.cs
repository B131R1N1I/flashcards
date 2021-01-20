using Npgsql;

namespace flashcards_server.DatabaseManagement
{
    partial class DatabaseManagement
    {
        public void AddSetToDatabase(Set.Set set)
        {
            using (var cmd = new NpgsqlCommand($"INSERT INTO sets (name, creator_id, owner_id, created_date, last_modification, is_public) VALUES ('{set.name}', {set.creator.id}, {set.owner.id}, {set.lastModificationDate}, {set.isPublic});", conn))
            {
                cmd.ExecuteNonQuery();
            }
        }
    }
}
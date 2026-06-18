using Filmoteka.Models;
using Npgsql;

namespace Filmoteka.Repositories;

public sealed class WatchStatusRepository(NpgsqlDataSource dataSource) : IWatchStatusRepository
{
    public IReadOnlyList<WatchStatus> GetAll()
    {
        const string sql = "SELECT id, name FROM watch_statuses ORDER BY id";

        using NpgsqlCommand command = dataSource.CreateCommand(sql);
        using NpgsqlDataReader reader = command.ExecuteReader();

        List<WatchStatus> statuses = new List<WatchStatus>();
        while (reader.Read())
        {
            statuses.Add(new WatchStatus
            {
                Id = reader.GetInt32(0),
                Name = reader.GetString(1)
            });
        }

        return statuses;
    }
}

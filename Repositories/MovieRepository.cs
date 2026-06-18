using Filmoteka.Models;
using Npgsql;

namespace Filmoteka.Repositories;

public sealed class MovieRepository(NpgsqlDataSource dataSource) : IMovieRepository
{
    public IReadOnlyList<Movie> GetAll()
    {
        const string sql = """
            SELECT m.id, m.title, m.director, m.release_year, m.status_id, s.name
            FROM movies m
            JOIN watch_statuses s ON s.id = m.status_id
            ORDER BY m.title
            """;

        using NpgsqlCommand command = dataSource.CreateCommand(sql);
        using NpgsqlDataReader reader = command.ExecuteReader();

        List<Movie> movies = new List<Movie>();
        while (reader.Read())
        {
            movies.Add(ReadMovie(reader));
        }

        return movies;
    }

    public Movie? GetById(int id)
    {
        const string sql = """
            SELECT m.id, m.title, m.director, m.release_year, m.status_id, s.name
            FROM movies m
            JOIN watch_statuses s ON s.id = m.status_id
            WHERE m.id = @id
            """;

        using NpgsqlCommand command = dataSource.CreateCommand(sql);
        command.Parameters.AddWithValue("id", id);

        using NpgsqlDataReader reader = command.ExecuteReader();
        return reader.Read() ? ReadMovie(reader) : null;
    }

    public int Create(Movie movie)
    {
        const string sql = """
            INSERT INTO movies (title, director, release_year, status_id)
            VALUES (@title, @director, @releaseYear, @statusId)
            RETURNING id
            """;

        using NpgsqlCommand command = dataSource.CreateCommand(sql);
        AddMovieParameters(command, movie);
        object? id = command.ExecuteScalar();
        return Convert.ToInt32(id);
    }

    public void Update(Movie movie)
    {
        const string sql = """
            UPDATE movies
            SET title = @title,
                director = @director,
                release_year = @releaseYear,
                status_id = @statusId
            WHERE id = @id
            """;

        using NpgsqlCommand command = dataSource.CreateCommand(sql);
        AddMovieParameters(command, movie);
        command.Parameters.AddWithValue("id", movie.Id);
        command.ExecuteNonQuery();
    }

    public void Delete(int id)
    {
        const string sql = "DELETE FROM movies WHERE id = @id";

        using NpgsqlCommand command = dataSource.CreateCommand(sql);
        command.Parameters.AddWithValue("id", id);
        command.ExecuteNonQuery();
    }

    private static Movie ReadMovie(NpgsqlDataReader reader)
    {
        return new Movie
        {
            Id = reader.GetInt32(0),
            Title = reader.GetString(1),
            Director = reader.GetString(2),
            ReleaseYear = reader.GetInt32(3),
            StatusId = reader.GetInt32(4),
            StatusName = reader.GetString(5)
        };
    }

    private static void AddMovieParameters(NpgsqlCommand command, Movie movie)
    {
        command.Parameters.AddWithValue("title", movie.Title);
        command.Parameters.AddWithValue("director", movie.Director);
        command.Parameters.AddWithValue("releaseYear", movie.ReleaseYear);
        command.Parameters.AddWithValue("statusId", movie.StatusId);
    }
}

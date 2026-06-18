using Filmoteka.Models;
using Npgsql;

namespace Filmoteka.Repositories;

public sealed class ReviewRepository(NpgsqlDataSource dataSource) : IReviewRepository
{
    public IReadOnlyList<Review> GetByMovieId(int movieId)
    {
        const string sql = """
            SELECT id, movie_id, author, rating, text, reviewed_at
            FROM reviews
            WHERE movie_id = @movieId
            ORDER BY reviewed_at DESC, id DESC
            """;

        using NpgsqlCommand command = dataSource.CreateCommand(sql);
        command.Parameters.AddWithValue("movieId", movieId);
        using NpgsqlDataReader reader = command.ExecuteReader();

        List<Review> reviews = new List<Review>();
        while (reader.Read())
        {
            reviews.Add(ReadReview(reader));
        }

        return reviews;
    }

    public int Create(Review review)
    {
        const string sql = """
            INSERT INTO reviews (movie_id, author, rating, text, reviewed_at)
            VALUES (@movieId, @author, @rating, @text, @reviewedAt)
            RETURNING id
            """;

        using NpgsqlCommand command = dataSource.CreateCommand(sql);
        AddReviewParameters(command, review);
        object? id = command.ExecuteScalar();
        return Convert.ToInt32(id);
    }

    public void Update(Review review)
    {
        const string sql = """
            UPDATE reviews
            SET author = @author,
                rating = @rating,
                text = @text,
                reviewed_at = @reviewedAt
            WHERE id = @id
            """;

        using NpgsqlCommand command = dataSource.CreateCommand(sql);
        AddReviewParameters(command, review);
        command.Parameters.AddWithValue("id", review.Id);
        command.ExecuteNonQuery();
    }

    public void Delete(int id)
    {
        const string sql = "DELETE FROM reviews WHERE id = @id";

        using NpgsqlCommand command = dataSource.CreateCommand(sql);
        command.Parameters.AddWithValue("id", id);
        command.ExecuteNonQuery();
    }

    private static Review ReadReview(NpgsqlDataReader reader)
    {
        return new Review
        {
            Id = reader.GetInt32(0),
            MovieId = reader.GetInt32(1),
            Author = reader.GetString(2),
            Rating = reader.GetInt32(3),
            Text = reader.GetString(4),
            ReviewedAt = reader.GetDateTime(5)
        };
    }

    private static void AddReviewParameters(NpgsqlCommand command, Review review)
    {
        command.Parameters.AddWithValue("movieId", review.MovieId);
        command.Parameters.AddWithValue("author", review.Author);
        command.Parameters.AddWithValue("rating", review.Rating);
        command.Parameters.AddWithValue("text", review.Text);
        command.Parameters.AddWithValue("reviewedAt", review.ReviewedAt.Date);
    }
}

using Filmoteka.Models;

namespace Filmoteka.Repositories;

public interface IReviewRepository
{
    IReadOnlyList<Review> GetByMovieId(int movieId);
    int Create(Review review);
    void Update(Review review);
    void Delete(int id);
}

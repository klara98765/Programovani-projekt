using Filmoteka.Models;

namespace Filmoteka.Repositories;

public interface IMovieRepository
{
    IReadOnlyList<Movie> GetAll();
    Movie? GetById(int id);
    int Create(Movie movie);
    void Update(Movie movie);
    void Delete(int id);
}

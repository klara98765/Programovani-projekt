using System.Collections.ObjectModel;
using Filmoteka.Models;
using Filmoteka.Repositories;

namespace Filmoteka.ViewModels;

public sealed class MovieListViewModel : ViewModelBase
{
    private readonly IMovieRepository _movieRepository;
    private MainWindowViewModel? _main;

    public MovieListViewModel(IMovieRepository movieRepository)
    {
        _movieRepository = movieRepository;
        AddMovieCommand = new ButtonCommand(_ => AddMovie());
        OpenDetailCommand = new ButtonCommand(OpenDetailFromCommand);
        EditMovieCommand = new ButtonCommand(EditMovieFromCommand);
        DeleteMovieCommand = new ButtonCommand(DeleteMovieFromCommand);
    }

    public ObservableCollection<Movie> Movies { get; } = [];
    public ButtonCommand AddMovieCommand { get; }
    public ButtonCommand OpenDetailCommand { get; }
    public ButtonCommand EditMovieCommand { get; }
    public ButtonCommand DeleteMovieCommand { get; }

    public void Initialize(MainWindowViewModel main)
    {
        _main = main;
    }

    public void Load()
    {
        Movies.Clear();
        foreach (Movie movie in _movieRepository.GetAll())
        {
            Movies.Add(movie);
        }
    }

    public void AddMovie()
    {
        if (_main == null) return;
        _main.ShowMovieForm();
    }

    public void OpenDetail(Movie movie)
    {
        if (_main == null) return;
        _main.ShowMovieDetail(movie.Id);
    }

    public void EditMovie(Movie movie)
    {
        if (_main == null) return;
        _main.ShowMovieForm(movie.Id);
    }

    public void DeleteMovie(Movie movie)
    {
        _movieRepository.Delete(movie.Id);
        Movies.Remove(movie);
    }

    private void OpenDetailFromCommand(object? parameter)
    {
        Movie? movie = parameter as Movie;
        if (movie != null)
        {
            OpenDetail(movie);
        }
    }

    private void EditMovieFromCommand(object? parameter)
    {
        Movie? movie = parameter as Movie;
        if (movie != null)
        {
            EditMovie(movie);
        }
    }

    private void DeleteMovieFromCommand(object? parameter)
    {
        Movie? movie = parameter as Movie;
        if (movie != null)
        {
            DeleteMovie(movie);
        }
    }
}

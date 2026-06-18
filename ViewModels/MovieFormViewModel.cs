using System.Collections.ObjectModel;
using Filmoteka.Models;
using Filmoteka.Repositories;

namespace Filmoteka.ViewModels;

public sealed class MovieFormViewModel : ViewModelBase
{
    private readonly IMovieRepository _movieRepository;
    private readonly IWatchStatusRepository _statusRepository;
    private MainWindowViewModel? _main;
    private int? _movieId;
    private Movie _movie = new() { ReleaseYear = DateTime.Today.Year };
    private WatchStatus? _selectedStatus;

    public MovieFormViewModel(IMovieRepository movieRepository, IWatchStatusRepository statusRepository)
    {
        _movieRepository = movieRepository;
        _statusRepository = statusRepository;
        SaveCommand = new ButtonCommand(_ => Save());
        CancelCommand = new ButtonCommand(_ => Cancel());
    }

    public ObservableCollection<WatchStatus> Statuses { get; } = [];
    public ButtonCommand SaveCommand { get; }
    public ButtonCommand CancelCommand { get; }
    public string PageTitle => _movieId is null ? "Přidat film" : "Upravit film";
    public string ErrorMessage { get; set; } = string.Empty;

    public Movie Movie
    {
        get => _movie;
        set => SetProperty(ref _movie, value);
    }

    public WatchStatus? SelectedStatus
    {
        get => _selectedStatus;
        set => SetProperty(ref _selectedStatus, value);
    }

    public void Initialize(MainWindowViewModel main, int? movieId)
    {
        _main = main;
        _movieId = movieId;
        OnPropertyChanged(nameof(PageTitle));
    }

    public void Load()
    {
        Statuses.Clear();
        foreach (WatchStatus status in _statusRepository.GetAll())
        {
            Statuses.Add(status);
        }

        if (_movieId is null)
        {
            SelectedStatus = Statuses.FirstOrDefault();
            return;
        }

        Movie = _movieRepository.GetById(_movieId.Value) ?? new Movie();
        SelectedStatus = Statuses.FirstOrDefault(status => status.Id == Movie.StatusId);
    }

    public void Save()
    {
        if (string.IsNullOrWhiteSpace(Movie.Title) ||
            string.IsNullOrWhiteSpace(Movie.Director) ||
            SelectedStatus is null)
        {
            ErrorMessage = "Vyplň název, režiséra a status.";
            OnPropertyChanged(nameof(ErrorMessage));
            return;
        }

        Movie.StatusId = SelectedStatus.Id;

        if (_movieId == null)
        {
            int newId = _movieRepository.Create(Movie);
            if (_main != null)
            {
                _main.ShowMovieDetail(newId);
            }
        }
        else
        {
            _movieRepository.Update(Movie);
            if (_main != null)
            {
                _main.ShowMovieDetail(Movie.Id);
            }
        }
    }

    public void Cancel()
    {
        if (_movieId == null)
        {
            if (_main != null)
            {
                _main.ShowMovieList();
            }
        }
        else
        {
            if (_main != null)
            {
                _main.ShowMovieDetail(_movieId.Value);
            }
        }
    }
}

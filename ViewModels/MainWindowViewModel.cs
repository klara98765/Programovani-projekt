using Microsoft.Extensions.DependencyInjection;

namespace Filmoteka.ViewModels;

public sealed class MainWindowViewModel(IServiceProvider serviceProvider) : ViewModelBase
{
    private ViewModelBase? _currentViewModel;

    public ViewModelBase? CurrentViewModel
    {
        get => _currentViewModel;
        private set => SetProperty(ref _currentViewModel, value);
    }

    public void ShowMovieList()
    {
        MovieListViewModel viewModel = serviceProvider.GetRequiredService<MovieListViewModel>();
        viewModel.Initialize(this);
        CurrentViewModel = viewModel;
        viewModel.Load();
    }

    public void ShowMovieDetail(int movieId)
    {
        MovieDetailViewModel viewModel = serviceProvider.GetRequiredService<MovieDetailViewModel>();
        viewModel.Initialize(this, movieId);
        CurrentViewModel = viewModel;
        viewModel.Load();
    }

    public void ShowMovieForm(int? movieId = null)
    {
        MovieFormViewModel viewModel = serviceProvider.GetRequiredService<MovieFormViewModel>();
        viewModel.Initialize(this, movieId);
        CurrentViewModel = viewModel;
        viewModel.Load();
    }

    public void Start() => ShowMovieList();
}

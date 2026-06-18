using Filmoteka.Repositories;
using Filmoteka.ViewModels;
using Microsoft.Extensions.DependencyInjection;
using Npgsql;

namespace Filmoteka;

public static class Services
{
    public static ServiceProvider Configure()
    {
        EnvLoader.Load();

        string? connectionString = Environment.GetEnvironmentVariable("CONNECTION_STRING");

        ServiceCollection services = new ServiceCollection();

        services.AddSingleton(new NpgsqlDataSourceBuilder(connectionString).Build());
        services.AddSingleton<IWatchStatusRepository, WatchStatusRepository>();
        services.AddSingleton<IMovieRepository, MovieRepository>();
        services.AddSingleton<IReviewRepository, ReviewRepository>();

        services.AddTransient<MovieListViewModel>();
        services.AddTransient<MovieDetailViewModel>();
        services.AddTransient<MovieFormViewModel>();
        services.AddSingleton<MainWindowViewModel>();

        return services.BuildServiceProvider();
    }
}

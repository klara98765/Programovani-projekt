using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using Filmoteka.ViewModels;
using Filmoteka.Views;
using Microsoft.Extensions.DependencyInjection;

namespace Filmoteka;

public partial class App : Application
{
    public override void Initialize()
    {
        AvaloniaXamlLoader.Load(this);
    }

    public override void OnFrameworkInitializationCompleted()
    {
        ServiceProvider services = Services.Configure();

        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            MainWindowViewModel mainViewModel = services.GetRequiredService<MainWindowViewModel>();
            desktop.MainWindow = new MainWindow
            {
                DataContext = mainViewModel
            };

            mainViewModel.Start();
        }

        base.OnFrameworkInitializationCompleted();
    }
}

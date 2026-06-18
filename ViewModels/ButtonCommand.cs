using System;
using System.Windows.Input;

namespace Filmoteka.ViewModels;

public sealed class ButtonCommand : ICommand
{
    private readonly Action<object?> _action;

    public ButtonCommand(Action<object?> action)
    {
        _action = action;
    }

    public event EventHandler? CanExecuteChanged
    {
        add { }
        remove { }
    }

    public bool CanExecute(object? parameter)
    {
        return true;
    }

    public void Execute(object? parameter)
    {
        _action(parameter);
    }
}
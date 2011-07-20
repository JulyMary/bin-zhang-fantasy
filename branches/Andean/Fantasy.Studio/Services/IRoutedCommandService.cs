using System;
namespace Fantasy.Studio.Services
{
    public interface IRoutedCommandService
    {
        System.Windows.Input.ICommand FindCommand(string name);
    }
}

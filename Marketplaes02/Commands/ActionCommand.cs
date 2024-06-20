
using System.Windows.Input;

namespace Marketplaes02.Commands
{
    /// <summary>
    /// Класс для реализации команды
    /// </summary>
    public class ActionCommand : ICommand
    {
        Action<object> action;


        public ActionCommand(Action<object> action)
        {
            this.action = action;
        }

        public event EventHandler? CanExecuteChanged;

        public bool CanExecute(object? parameter) 
        {
            return  parameter != null;
        } 

        public void Execute(object? parameter) 
        {
            action?.Invoke(parameter);
        } 
    }
}

using System.Windows;
using System.Windows.Input;

namespace Shared.Control
{
    public static class WindowHelp
    {
        public static void SetWindowSystemButton(this Window window)
        {
            SetWindowSystemButton(window, (target, e) => { SystemCommands.CloseWindow(window); });
        }


        public static void SetWindowSystemButton(this Window window, ExecutedRoutedEventHandler closeCommand)
        {
            window.CommandBindings.Add(new CommandBinding(SystemCommands.CloseWindowCommand, closeCommand));
            window.CommandBindings.Add(new CommandBinding(SystemCommands.MaximizeWindowCommand, (target, e) => { SystemCommands.MaximizeWindow(window); }, (target, e) => { e.CanExecute = window.ResizeMode == ResizeMode.CanResize || window.ResizeMode == ResizeMode.CanResizeWithGrip; }));
            window.CommandBindings.Add(new CommandBinding(SystemCommands.MinimizeWindowCommand, (target, e) => { SystemCommands.MinimizeWindow(window); }, (target, e) => { e.CanExecute = window.ResizeMode != ResizeMode.NoResize; }));
            window.CommandBindings.Add(new CommandBinding(SystemCommands.RestoreWindowCommand, (target, e) => { SystemCommands.RestoreWindow(window); }, (target, e) => { e.CanExecute = window.ResizeMode == ResizeMode.CanResize || window.ResizeMode == ResizeMode.CanResizeWithGrip; }));
        }
    }
}

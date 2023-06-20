using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Retros.Program.Workstation {
    public class UndoCommand : ICommand {
        public event EventHandler? CanExecuteChanged;

        public bool CanExecute(object? parameter) {
            return true;
        }

        public void Execute(object? parameter) {
            WindowManager.MainWindow?.SelectedWorkstation.ImageElement.GetHistoryManager.Undo();
        }
    }

    public class RedoCommand : ICommand {
        public event EventHandler? CanExecuteChanged;

        public bool CanExecute(object? parameter) {
            return true;
        }

        public void Execute(object? parameter) {
            WindowManager.MainWindow?.SelectedWorkstation.ImageElement.GetHistoryManager.Redo();
        }
    }
}

namespace Aiphw.ViewModels;
using Avalonia.Controls;
using ReactiveUI;

public class MainViewModel : ViewModelBase {

    private string _description = "so hard";
    public string Description {
        get => _description;
        set => this.RaiseAndSetIfChanged(ref _description, value);
    }
    public void OpenFile() {
        var diaglog = new OpenFileDialog();
    }
    public void SaveFile() {
        Description = "FuckYea";
    }
}

using System.Windows;
using AmongUsModManager.ViewModels;

namespace AmongUsModManager;

public partial class ModBrowserWindow : Window
{
    public ModBrowserWindow(ModBrowserViewModel viewModel)
    {
        InitializeComponent();
        DataContext = viewModel;
    }
}

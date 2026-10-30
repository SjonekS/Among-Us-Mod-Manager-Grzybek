using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Windows;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using AmongUsModManager.Models;
using AmongUsModManager.Services.Interfaces;

namespace AmongUsModManager.ViewModels;

public class TargetEnvironmentItem
{
    public string Name { get; set; } = "";
    public string Path { get; set; } = "";
    public bool IsInstance { get; set; }
}

public partial class ModBrowserViewModel : ObservableObject
{
    private readonly IModCatalogService _catalogService;
    private readonly IInstanceManagerService _instanceManager;
    private readonly IModInstallerService _modInstaller;
    private readonly IProcessChecker _processChecker;
    private readonly IGitHubReleaseService _gitHubService;

    [ObservableProperty]
    private ObservableCollection<TargetEnvironmentItem> _availableTargets = new();

    [ObservableProperty]
    private TargetEnvironmentItem? _selectedTarget;

    [ObservableProperty]
    private ObservableCollection<ModCatalogItem> _allMods = new();

    [ObservableProperty]
    private ObservableCollection<ModCatalogItem> _filteredMods = new();

    [ObservableProperty]
    private ModCatalogItem? _selectedMod;

    [ObservableProperty]
    private ObservableCollection<GitHubReleaseInfo> _availableReleases = new();

    [ObservableProperty]
    private GitHubReleaseInfo? _selectedRelease;

    [ObservableProperty]
    private bool _isLoadingReleases;

    [ObservableProperty]
    private string _searchQuery = "";

    [ObservableProperty]
    private string _filterStatus = "All"; // All, Installed, NotInstalled

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(IsNotBusy))]
    private bool _isBusy;

    public bool IsNotBusy => !IsBusy;

    [ObservableProperty]
    private string _statusMessage = "Wybierz mod z listy, aby zobaczyć szczegóły.";

    [ObservableProperty]
    private double _progressPercentage;

    [ObservableProperty]
    private bool _isSideTrayOpen = true;

    [ObservableProperty]
    private bool _isDetailsOpen;

    [RelayCommand]
    private async Task SelectModAsync(ModCatalogItem? mod)
    {
        if (mod != null)
        {
            SelectedMod = mod;
            IsDetailsOpen = true;
            await LoadReleasesForSelectedModAsync();
        }
    }

    private async Task LoadReleasesForSelectedModAsync()
    {
        if (SelectedMod == null) return;
        AvailableReleases.Clear();
        SelectedRelease = null;
        IsLoadingReleases = true;

        try
        {
            var releases = await _gitHubService.GetReleasesListForRepoAsync(
                SelectedMod.RepoOwner, 
                SelectedMod.RepoName, 
                SelectedMod.AssetFileNameFilter);

            foreach (var rel in releases)
            {
                AvailableReleases.Add(rel);
            }

            SelectedRelease = AvailableReleases.FirstOrDefault() ?? SelectedMod.LatestRelease;
        }
        catch
        {
            if (SelectedMod.LatestRelease != null)
            {
                AvailableReleases.Add(SelectedMod.LatestRelease);
                SelectedRelease = SelectedMod.LatestRelease;
            }
        }
        finally
        {
            IsLoadingReleases = false;
        }
    }

    [RelayCommand]
    private void CloseDetails()
    {
        IsDetailsOpen = false;
    }

    public ModBrowserViewModel(
        IModCatalogService catalogService,
        IInstanceManagerService instanceManager,
        IModInstallerService modInstaller,
        IProcessChecker processChecker,
        IGitHubReleaseService gitHubService)
    {
        _catalogService = catalogService;
        _instanceManager = instanceManager;
        _modInstaller = modInstaller;
        _processChecker = processChecker;
        _gitHubService = gitHubService;

        LoadInitialData();
    }

    private async void LoadInitialData()
    {
        // 1. Załaduj listę środowisk (Steam + Kapsuły)
        var steamPath = _modInstaller.ResolveGamePath() ?? "";
        AvailableTargets.Clear();
        AvailableTargets.Add(new TargetEnvironmentItem
        {
            Name = "🎮 Główna gra (Steam)",
            Path = steamPath,
            IsInstance = false
        });

        var instances = await _instanceManager.GetAllInstancesAsync();
        foreach (var inst in instances)
        {
            AvailableTargets.Add(new TargetEnvironmentItem
            {
                Name = $"📦 Instancja: {inst.Name}",
                Path = inst.Path,
                IsInstance = true
            });
        }

        SelectedTarget = AvailableTargets.FirstOrDefault();

        // 2. Załaduj katalog modów
        var items = _catalogService.GetCatalogItems();
        AllMods = new ObservableCollection<ModCatalogItem>(items);
        SelectedMod = AllMods.FirstOrDefault();

        await RefreshModsStatusAsync();
    }

    partial void OnSelectedTargetChanged(TargetEnvironmentItem? value)
    {
        if (value != null)
        {
            _ = RefreshModsStatusAsync();
        }
    }

    partial void OnSearchQueryChanged(string value)
    {
        ApplyFilters();
    }

    [RelayCommand]
    private void ToggleSideTray()
    {
        IsSideTrayOpen = !IsSideTrayOpen;
    }

    [RelayCommand]
    private void SetFilter(string filter)
    {
        FilterStatus = filter;
        ApplyFilters();
    }

    private void ApplyFilters()
    {
        var query = SearchQuery?.Trim().ToLowerInvariant() ?? "";
        var list = AllMods.AsEnumerable();

        if (!string.IsNullOrEmpty(query))
        {
            list = list.Where(m => m.Name.ToLowerInvariant().Contains(query) ||
                                  m.Author.ToLowerInvariant().Contains(query) ||
                                  m.Description.ToLowerInvariant().Contains(query));
        }

        if (FilterStatus == "Installed")
        {
            list = list.Where(m => m.Status == ModInstallationStatus.Installed || m.Status == ModInstallationStatus.UpdateAvailable);
        }
        else if (FilterStatus == "NotInstalled")
        {
            list = list.Where(m => m.Status == ModInstallationStatus.NotInstalled || m.Status == ModInstallationStatus.MissingCoreDependency);
        }

        FilteredMods = new ObservableCollection<ModCatalogItem>(list);
    }

    [RelayCommand]
    public async Task RefreshModsStatusAsync()
    {
        if (SelectedTarget == null || string.IsNullOrWhiteSpace(SelectedTarget.Path)) return;

        IsBusy = true;
        StatusMessage = "Odświeżanie stanu modyfikacji...";
        try
        {
            await _catalogService.RefreshStatusesAsync(SelectedTarget.Path, AllMods.ToList());
            ApplyFilters();
            StatusMessage = "Gotowy do działania.";
        }
        finally
        {
            IsBusy = false;
        }
    }

    [RelayCommand]
    private async Task InstallModAsync(ModCatalogItem? mod)
    {
        var target = mod ?? SelectedMod;
        if (target == null || SelectedTarget == null || IsBusy) return;

        if (_processChecker.IsGameRunning())
        {
            MessageBox.Show("Gra Among Us jest uruchomiona! Zamknij grę przed instalacją modyfikacji.", "Gra aktywna", MessageBoxButton.OK, MessageBoxImage.Warning);
            return;
        }

        var chosenRelease = (target == SelectedMod) ? SelectedRelease : null;
        var versionLabel = chosenRelease != null ? chosenRelease.VersionString : target.LatestRemoteVersion;

        IsBusy = true;
        StatusMessage = $"Pobieranie i instalacja {target.Name} ({versionLabel})...";
        ProgressPercentage = 0;

        try
        {
            var prog = new Progress<OperationProgress>(p =>
            {
                ProgressPercentage = p.Percentage;
                StatusMessage = p.Status;
            });

            await _catalogService.InstallModAsync(SelectedTarget.Path, target, prog, null, default, chosenRelease);
            await RefreshModsStatusAsync();

            MessageBox.Show($"Pomyślnie zainstalowano {target.Name} ({versionLabel}) w {SelectedTarget.Name}!", "Instalacja zakończona", MessageBoxButton.OK, MessageBoxImage.Information);
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Błąd instalacji:\n\n{ex.Message}", "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
        }
        finally
        {
            IsBusy = false;
            StatusMessage = "Gotowy.";
        }
    }

    [RelayCommand]
    private async Task UninstallModAsync(ModCatalogItem? mod)
    {
        var target = mod ?? SelectedMod;
        if (target == null || SelectedTarget == null || IsBusy) return;

        if (_processChecker.IsGameRunning())
        {
            MessageBox.Show("Gra Among Us jest uruchomiona! Zamknij grę przed odinstalowaniem modyfikacji.", "Gra aktywna", MessageBoxButton.OK, MessageBoxImage.Warning);
            return;
        }

        if (target.IsCore)
        {
            MessageBox.Show("Pakiet bazowy TouMira nie może zostać usunięty z tego poziomu. Użyj opcji resetu w oknie Pomoc głównej aplikacji.", "Chroniony pakiet", MessageBoxButton.OK, MessageBoxImage.Information);
            return;
        }

        var confirm = MessageBox.Show($"Czy na pewno chcesz odinstalować {target.Name} z wybranego środowiska ({SelectedTarget.Name})?", "Potwierdzenie", MessageBoxButton.YesNo, MessageBoxImage.Question);
        if (confirm != MessageBoxResult.Yes) return;

        IsBusy = true;
        StatusMessage = $"Usuwanie {target.Name}...";

        try
        {
            await _catalogService.UninstallModAsync(SelectedTarget.Path, target);
            await RefreshModsStatusAsync();
            MessageBox.Show($"Odinstalowano modyfikację {target.Name}.", "Sukces", MessageBoxButton.OK, MessageBoxImage.Information);
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Błąd podczas odinstalowywania:\n\n{ex.Message}", "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
        }
        finally
        {
            IsBusy = false;
            StatusMessage = "Gotowy.";
        }
    }

    [RelayCommand]
    private void OpenGameFolder()
    {
        if (SelectedTarget != null && Directory.Exists(SelectedTarget.Path))
        {
            Process.Start(new ProcessStartInfo { FileName = SelectedTarget.Path, UseShellExecute = true });
        }
    }

    [RelayCommand]
    private void OpenProjectPage()
    {
        if (SelectedMod != null && !string.IsNullOrEmpty(SelectedMod.ProjectUrl))
        {
            Process.Start(new ProcessStartInfo { FileName = SelectedMod.ProjectUrl, UseShellExecute = true });
        }
    }
}

namespace AmongUsModManager.Models;

public enum ModInstallationStatus
{
    NotInstalled,
    Installed,
    UpdateAvailable,
    MissingCoreDependency,
    Unknown
}

public class ModCatalogItem
{
    public string Id { get; set; } = "";
    public string Name { get; set; } = "";
    public string Author { get; set; } = "Nieznany";
    public string Description { get; set; } = "";
    public string Category { get; set; } = "Inne";
    public string RepoOwner { get; set; } = "";
    public string RepoName { get; set; } = "";
    public string AssetFileNameFilter { get; set; } = "";
    public string TargetPluginFileName { get; set; } = "";
    public bool RequiresCore { get; set; } = true;
    public bool IsCore { get; set; } = false;
    public string? ProjectUrl { get; set; }
    public string? ChangelogUrl { get; set; }
    public string? IconUrl { get; set; }

    /// <summary>
    /// Zwraca zdefiniowaną ikonę lub domyślny avatar z repozytorium GitHub autora
    /// </summary>
    public string DisplayIcon => !string.IsNullOrEmpty(IconUrl)
        ? IconUrl
        : (!string.IsNullOrEmpty(RepoOwner) ? $"https://github.com/{RepoOwner}.png?size=128" : "");

    // Stan runtime dla wybranej instancji
    public string? InstalledVersion { get; set; }
    public string? LatestRemoteVersion { get; set; }
    public ModInstallationStatus Status { get; set; } = ModInstallationStatus.NotInstalled;
    public GitHubReleaseInfo? LatestRelease { get; set; }
}

using AmongUsModManager.Models;
using AmongUsModManager.Services.Interfaces;

namespace AmongUsModManager.Services.Interfaces;

public interface IModCatalogService
{
    List<ModCatalogItem> GetCatalogItems();
    Task<ModManifest> LoadManifestAsync(string gamePath);
    Task SaveManifestAsync(string gamePath, ModManifest manifest);
    Task RefreshStatusesAsync(string gamePath, List<ModCatalogItem> items, Action<string>? log = null);
    Task<bool> InstallModAsync(string gamePath, ModCatalogItem item, IProgress<OperationProgress>? progress = null, Action<string>? log = null, CancellationToken ct = default, GitHubReleaseInfo? customRelease = null);
    Task<bool> UninstallModAsync(string gamePath, ModCatalogItem item, Action<string>? log = null);
}

using AmongUsModManager.Models;

namespace AmongUsModManager.Services.Interfaces;

public interface IGitHubReleaseService
{
    Task<GitHubReleaseInfo?> GetLatestTouMiraReleaseAsync(Action<string>? logCallback = null, CancellationToken ct = default);
    Task<GitHubReleaseInfo?> GetLatestReleaseForRepoAsync(string owner, string repo, string? assetFilter = null, Action<string>? logCallback = null, CancellationToken ct = default);
    Task<List<GitHubReleaseInfo>> GetReleasesListForRepoAsync(string owner, string repo, string? assetFilter = null, Action<string>? logCallback = null, CancellationToken ct = default);
    Task<string> DownloadAssetAsync(GitHubReleaseInfo release, string destinationDirectory, IProgress<OperationProgress>? progress = null, Action<string>? logCallback = null, CancellationToken ct = default);
}

using System.IO;
using System.IO.Compression;
using System.Text.Json;
using AmongUsModManager.Models;
using AmongUsModManager.Services.Interfaces;

namespace AmongUsModManager.Services;

public class ModCatalogService : IModCatalogService
{
    private static readonly JsonSerializerOptions JsonOpts = new()
    {
        WriteIndented = true,
        PropertyNameCaseInsensitive = true
    };

    private readonly IGitHubReleaseService _gitHubService;
    private readonly IConfigService _configService;
    private readonly IBackupService _backupService;

    public ModCatalogService(
        IGitHubReleaseService gitHubService,
        IConfigService configService,
        IBackupService backupService)
    {
        _gitHubService = gitHubService;
        _configService = configService;
        _backupService = backupService;
    }

    public List<ModCatalogItem> GetCatalogItems()
    {
        return new List<ModCatalogItem>
        {
            new ModCatalogItem
            {
                Id = "tou-mira",
                Name = "Town Of Us: Mira (Core Pack)",
                Author = "AU-Avengers / MiraAPI Team",
                Description = "Wymagany pakiet bazowy dla Among Us: zawiera framework BepInEx, Reactor oraz bibliotekę MiraAPI. Niezbędny do działania pozostałych modyfikacji.",
                Category = "Pakiet Bazowy",
                RepoOwner = "AU-Avengers",
                RepoName = "TOU-Mira",
                AssetFileNameFilter = ".zip",
                IsCore = true,
                RequiresCore = false,
                ProjectUrl = "https://github.com/AU-Avengers/TOU-Mira",
                IconUrl = "https://raw.githubusercontent.com/AU-Avengers/TOU-Mira/main/icon.png"
            },
            new ModCatalogItem
            {
                Id = "submerged",
                Name = "Submerged (Nowa Mapa)",
                Author = "Submerged Team",
                Description = "Dodaje w pełni grywalną, ogromną mapę podwodnej łodzi podwodnej z windami, nowymi zadaniami i unikalnymi mechanikami.",
                Category = "Mapy",
                RepoOwner = "SubmergedAmongUs",
                RepoName = "Submerged",
                AssetFileNameFilter = ".dll",
                TargetPluginFileName = "Submerged.dll",
                IsCore = false,
                RequiresCore = true,
                ProjectUrl = "https://github.com/SubmergedAmongUs/Submerged",
                IconUrl = "https://raw.githubusercontent.com/SubmergedAmongUs/Submerged/main/Submerged/Resources/SubmergedIcon.png"
            },
            new ModCatalogItem
            {
                Id = "better-polus",
                Name = "BetterPolus (Ulepszenia Mapy)",
                Author = "Galster-dev",
                Description = "Znaczące usprawnienia balansu i oprawione oświetlenie dla klasycznej mapy Polus.",
                Category = "Ulepszenia",
                RepoOwner = "Galster-dev",
                RepoName = "BetterPolus",
                AssetFileNameFilter = ".dll",
                TargetPluginFileName = "BetterPolus.dll",
                IsCore = false,
                RequiresCore = true,
                ProjectUrl = "https://github.com/Galster-dev/BetterPolus"
            },
            new ModCatalogItem
            {
                Id = "level-imposter",
                Name = "LevelImposter",
                Author = "DigiWorm0",
                Description = "Pozwala tworzyć i ładować niestandardowe mapy Among Us za pomocą edytora webowego. Ogromna biblioteka map tworzonych przez społeczność.",
                Category = "Mapy",
                RepoOwner = "DigiWorm0",
                RepoName = "LevelImposter",
                AssetFileNameFilter = ".dll",
                TargetPluginFileName = "LevelImposter.dll",
                IsCore = false,
                RequiresCore = true,
                ProjectUrl = "https://github.com/DigiWorm0/LevelImposter",
                IconUrl = "https://levelimposter.net/favicon.ico"
            },
            new ModCatalogItem
            {
                Id = "localize-us",
                Name = "LocalizeUs",
                Author = "AU-Avengers",
                Description = "Moduł lokalizacyjny dodający wsparcie dla niestandardowych tłumaczeń i języków w Among Us.",
                Category = "Narzędzia",
                RepoOwner = "AU-Avengers",
                RepoName = "LocalizeUs",
                AssetFileNameFilter = ".dll",
                TargetPluginFileName = "LocalizeUs.dll",
                IsCore = false,
                RequiresCore = true,
                ProjectUrl = "https://github.com/AU-Avengers/LocalizeUs"
            },
            new ModCatalogItem
            {
                Id = "chaos-tokens",
                Name = "ChaosTokens",
                Author = "xChipseq",
                Description = "Dodaje system tokenów chaosu wprowadzających losowe modyfikatory rozgrywki każdej rundy.",
                Category = "Rozgrywka",
                RepoOwner = "xChipseq",
                RepoName = "ChaosTokens",
                AssetFileNameFilter = ".dll",
                TargetPluginFileName = "ChaosTokens.dll",
                IsCore = false,
                RequiresCore = true,
                ProjectUrl = "https://github.com/xChipseq/ChaosTokens"
            },
            new ModCatalogItem
            {
                Id = "game-tweaks",
                Name = "GameTweaks",
                Author = "xChipseq",
                Description = "Zestaw drobnych usprawnień i poprawek rozgrywki w Among Us – tweaki mechanik, UI i balansu.",
                Category = "Ulepszenia",
                RepoOwner = "xChipseq",
                RepoName = "GameTweaks",
                AssetFileNameFilter = ".dll",
                TargetPluginFileName = "GameTweaks.dll",
                IsCore = false,
                RequiresCore = true,
                ProjectUrl = "https://github.com/xChipseq/GameTweaks"
            },
            new ModCatalogItem
            {
                Id = "tou-mira-divani-addon",
                Name = "TouMira Divani Mods AddOn",
                Author = "DivaniNL",
                Description = "Dodatek do TownOfUs Mira – nowe role, umiejętności i mechaniki od Divani.",
                Category = "Role / AddOn",
                RepoOwner = "DivaniNL",
                RepoName = "TownOfUsMiraDivaniModsAddOn",
                AssetFileNameFilter = ".dll",
                TargetPluginFileName = "TownOfUsMiraDivaniModsAddOn.dll",
                IsCore = false,
                RequiresCore = true,
                ProjectUrl = "https://github.com/DivaniNL/TownOfUsMiraDivaniModsAddOn"
            },
            new ModCatalogItem
            {
                Id = "tou-mira-unleashed",
                Name = "TouMira Unleashed",
                Author = "rewalo",
                Description = "Rozszerzenie TownOfUs Mira – odblokowane dodatkowe funkcje, role i ustawienia rozgrywki.",
                Category = "Role / AddOn",
                RepoOwner = "rewalo",
                RepoName = "TownOfUsMiraUnleashed",
                AssetFileNameFilter = ".dll",
                TargetPluginFileName = "TownOfUsMiraUnleashed.dll",
                IsCore = false,
                RequiresCore = true,
                ProjectUrl = "https://github.com/rewalo/TownOfUsMiraUnleashed"
            },
            new ModCatalogItem
            {
                Id = "mega-chujowe-extension",
                Name = "MegaChujoweExtension",
                Author = "HekerB",
                Description = "Rozbudowany pakiet dodatkowych ról i mechanik dla TownOfUs Mira.",
                Category = "Role / AddOn",
                RepoOwner = "HekerB",
                RepoName = "TownOfUsMegaChujoweExtension",
                AssetFileNameFilter = ".dll",
                TargetPluginFileName = "TownOfUsMegaChujoweExtension.dll",
                IsCore = false,
                RequiresCore = true,
                ProjectUrl = "https://github.com/HekerB/TownOfUsMegaChujoweExtension"
            },
            new ModCatalogItem
            {
                Id = "town-of-extra",
                Name = "TownOfExtra",
                Author = "Mehzxzz",
                Description = "Paczka dodatkowych ról i ulepszeń rozgrywki dla TownOfUs Mira.",
                Category = "Role / AddOn",
                RepoOwner = "Mehzxzz",
                RepoName = "TownOfExtra",
                AssetFileNameFilter = ".dll",
                TargetPluginFileName = "TownOfExtra.dll",
                IsCore = false,
                RequiresCore = true,
                ProjectUrl = "https://github.com/Mehzxzz/TownOfExtra"
            },
            new ModCatalogItem
            {
                Id = "perfect-comms",
                Name = "Perfect Comms",
                Author = "artriy",
                Description = "Ulepszony system komunikacji i sabotaży w Among Us – poprawione interakcje i UI.",
                Category = "Ulepszenia",
                RepoOwner = "artriy",
                RepoName = "Perfect-Comms",
                AssetFileNameFilter = ".dll",
                TargetPluginFileName = "PerfectComms.dll",
                IsCore = false,
                RequiresCore = true,
                ProjectUrl = "https://github.com/artriy/Perfect-Comms"
            },
            new ModCatalogItem
            {
                Id = "mira-notepad",
                Name = "Mira NotePad",
                Author = "Maxi0fc",
                Description = "Notatnik w grze – pozwala graczom zapisywać notatki podczas rozgrywki bezpośrednio w Among Us.",
                Category = "Narzędzia",
                RepoOwner = "Maxi0fc",
                RepoName = "Mira-NotePad",
                AssetFileNameFilter = ".dll",
                TargetPluginFileName = "MiraNotePad.dll",
                IsCore = false,
                RequiresCore = true,
                ProjectUrl = "https://github.com/Maxi0fc/Mira-NotePad"
            }
        };
    }

    public async Task<ModManifest> LoadManifestAsync(string gamePath)
    {
        if (string.IsNullOrWhiteSpace(gamePath) || !Directory.Exists(gamePath))
            return new ModManifest();

        var manifestPath = Path.Combine(gamePath, "manager-data", "mod-manifest.json");
        if (!File.Exists(manifestPath))
            return new ModManifest();

        try
        {
            var json = await File.ReadAllTextAsync(manifestPath);
            return JsonSerializer.Deserialize<ModManifest>(json, JsonOpts) ?? new ModManifest();
        }
        catch
        {
            return new ModManifest();
        }
    }

    public async Task SaveManifestAsync(string gamePath, ModManifest manifest)
    {
        var metaDir = Path.Combine(gamePath, "manager-data");
        if (!Directory.Exists(metaDir)) Directory.CreateDirectory(metaDir);

        manifest.LastModified = DateTimeOffset.Now;
        var manifestPath = Path.Combine(metaDir, "mod-manifest.json");
        var json = JsonSerializer.Serialize(manifest, JsonOpts);
        await File.WriteAllTextAsync(manifestPath, json);
    }

    public async Task RefreshStatusesAsync(string gamePath, List<ModCatalogItem> items, Action<string>? log = null)
    {
        var manifest = await LoadManifestAsync(gamePath);
        var pluginsDir = Path.Combine(gamePath, "BepInEx", "plugins");

        // Sprawdź czy baza TouMira/BepInEx istnieje
        bool hasCore = Directory.Exists(Path.Combine(gamePath, "BepInEx")) &&
                       File.Exists(Path.Combine(gamePath, "winhttp.dll"));

        foreach (var mod in items)
        {
            var installed = manifest.InstalledMods.FirstOrDefault(m => m.Id.Equals(mod.Id, StringComparison.OrdinalIgnoreCase));
            
            // Jeśli to zwykły mod sprawdzamy też czy jego DLL istnieje na dysku
            if (!mod.IsCore && !string.IsNullOrEmpty(mod.TargetPluginFileName))
            {
                var dllPath = Path.Combine(pluginsDir, mod.TargetPluginFileName);
                if (installed == null && File.Exists(dllPath))
                {
                    // Wykryto plik poza manifestem (zainstalowany ręcznie)
                    mod.InstalledVersion = "Lokalna wtyczka";
                    mod.Status = ModInstallationStatus.Installed;
                    continue;
                }
            }

            if (installed != null)
            {
                mod.InstalledVersion = installed.Version;
                mod.Status = ModInstallationStatus.Installed;
            }
            else
            {
                mod.InstalledVersion = null;
                if (mod.RequiresCore && !hasCore)
                {
                    mod.Status = ModInstallationStatus.MissingCoreDependency;
                }
                else
                {
                    mod.Status = ModInstallationStatus.NotInstalled;
                }
            }
        }
    }

    public async Task<bool> InstallModAsync(
        string gamePath, 
        ModCatalogItem item, 
        IProgress<OperationProgress>? progress = null, 
        Action<string>? log = null, 
        CancellationToken ct = default,
        GitHubReleaseInfo? customRelease = null)
    {
        if (string.IsNullOrWhiteSpace(gamePath) || !Directory.Exists(gamePath))
            throw new InvalidOperationException("Nieprawidłowy folder docelowy gry.");

        // 1. Pobierz Release info
        progress?.Report(new OperationProgress(10, $"Wyszukiwanie wydania {item.Name}..."));
        var release = customRelease ?? await _gitHubService.GetLatestReleaseForRepoAsync(item.RepoOwner, item.RepoName, item.AssetFileNameFilter, log, ct);
        if (release == null)
        {
            throw new InvalidOperationException($"Nie udało się pobrać informacji o wydaniu dla {item.Name} z GitHub ({item.RepoOwner}/{item.RepoName}).");
        }

        // 2. Pobierz plik do .temp
        progress?.Report(new OperationProgress(25, $"Pobieranie {release.AssetName}..."));
        var tempDir = Path.Combine(_configService.GetAppDataDirectory(), ".temp");
        var downloadedFile = await _gitHubService.DownloadAssetAsync(release, tempDir, progress, log, ct);

        try
        {
            var manifest = await LoadManifestAsync(gamePath);
            var installedFiles = new List<string>();

            var pluginsDir = Path.Combine(gamePath, "BepInEx", "plugins");
            if (!Directory.Exists(pluginsDir)) Directory.CreateDirectory(pluginsDir);

            // 3. Jeśli to pojedynczy plik DLL
            if (downloadedFile.EndsWith(".dll", StringComparison.OrdinalIgnoreCase))
            {
                string targetFileName = !string.IsNullOrEmpty(item.TargetPluginFileName) 
                    ? item.TargetPluginFileName 
                    : Path.GetFileName(downloadedFile);

                var targetPath = Path.Combine(pluginsDir, targetFileName);
                File.Copy(downloadedFile, targetPath, overwrite: true);
                installedFiles.Add($"BepInEx/plugins/{targetFileName}");
            }
            // 4. Jeśli to archiwum ZIP
            else if (downloadedFile.EndsWith(".zip", StringComparison.OrdinalIgnoreCase))
            {
                using var zip = ZipFile.OpenRead(downloadedFile);
                foreach (var entry in zip.Entries)
                {
                    if (string.IsNullOrEmpty(entry.Name)) continue; // pomiń foldery
                    if (entry.Name.EndsWith(".dll", StringComparison.OrdinalIgnoreCase))
                    {
                        var dest = Path.Combine(pluginsDir, entry.Name);
                        entry.ExtractToFile(dest, overwrite: true);
                        installedFiles.Add($"BepInEx/plugins/{entry.Name}");
                    }
                }
            }

            // 5. Zapisz w manifeście
            manifest.InstalledMods.RemoveAll(m => m.Id.Equals(item.Id, StringComparison.OrdinalIgnoreCase));
            manifest.InstalledMods.Add(new InstalledModEntry
            {
                Id = item.Id,
                Name = item.Name,
                Version = release.VersionString,
                IsCore = item.IsCore,
                InstalledAt = DateTimeOffset.Now,
                InstalledFiles = installedFiles
            });

            await SaveManifestAsync(gamePath, manifest);

            item.InstalledVersion = release.VersionString;
            item.Status = ModInstallationStatus.Installed;

            progress?.Report(new OperationProgress(100, $"Zainstalowano {item.Name} ({release.VersionString})!"));
            return true;
        }
        finally
        {
            if (File.Exists(downloadedFile))
            {
                try { File.Delete(downloadedFile); } catch { }
            }
        }
    }

    public async Task<bool> UninstallModAsync(string gamePath, ModCatalogItem item, Action<string>? log = null)
    {
        if (item.IsCore)
        {
            throw new InvalidOperationException("Nie można odinstalować pakietu bazowego TouMira przez Mod Browser. Użyj opcji resetu w panelu Pomoc.");
        }

        var manifest = await LoadManifestAsync(gamePath);
        var entry = manifest.InstalledMods.FirstOrDefault(m => m.Id.Equals(item.Id, StringComparison.OrdinalIgnoreCase));

        int deleted = 0;
        if (entry != null && entry.InstalledFiles.Count > 0)
        {
            foreach (var rel in entry.InstalledFiles)
            {
                var full = Path.GetFullPath(Path.Combine(gamePath, rel.Replace('/', Path.DirectorySeparatorChar)));
                if (File.Exists(full))
                {
                    try
                    {
                        File.Delete(full);
                        deleted++;
                        log?.Invoke($"Usunięto plik wtyczki: {rel}");
                    }
                    catch (Exception ex)
                    {
                        log?.Invoke($"Błąd usuwania '{rel}': {ex.Message}");
                    }
                }
            }
        }
        else if (!string.IsNullOrEmpty(item.TargetPluginFileName))
        {
            var fallbackPath = Path.Combine(gamePath, "BepInEx", "plugins", item.TargetPluginFileName);
            if (File.Exists(fallbackPath))
            {
                File.Delete(fallbackPath);
                deleted++;
            }
        }

        manifest.InstalledMods.RemoveAll(m => m.Id.Equals(item.Id, StringComparison.OrdinalIgnoreCase));
        await SaveManifestAsync(gamePath, manifest);

        item.InstalledVersion = null;
        item.Status = ModInstallationStatus.NotInstalled;

        log?.Invoke($"Odinstalowano {item.Name}. Usunięto {deleted} plików.");
        return true;
    }
}

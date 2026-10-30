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
            // === STRONA 1: Najważniejsze i Pakiety Bazowe ===
            new ModCatalogItem
            {
                Id = "tou-mira",
                Name = "TouMira (Wydanie Serwera / Grzybek)",
                Author = "SjonekS / Miasto Nasze",
                Description = "Zgodny z naszym serwerem i instalacją pakiet bazowy TouMira (framework BepInEx, Reactor, MiraAPI). Rekomendowany do wspólnej gry.",
                Category = "Pakiet Bazowy",
                RepoOwner = "SjonekS",
                RepoName = "Miasto-Nasze-Mody",
                AssetFileNameFilter = "TouMira",
                IsCore = true,
                RequiresCore = false,
                ProjectUrl = "https://github.com/SjonekS/Miasto-Nasze-Mody"
            },
            new ModCatalogItem
            {
                Id = "tou-mira-official",
                Name = "Town Of Us: Mira (Oficjalny AU-Avengers)",
                Author = "AU-Avengers / MiraAPI Team",
                Description = "Oficjalne, najnowsze wydanie pakietu TouMira od twórców modyfikacji. Uwaga: może wymagać innej wersji gry Among Us niż nasz serwer.",
                Category = "Pakiet Bazowy",
                RepoOwner = "AU-Avengers",
                RepoName = "TOU-Mira",
                AssetFileNameFilter = ".zip",
                IsCore = true,
                RequiresCore = false,
                ProjectUrl = "https://github.com/AU-Avengers/TOU-Mira"
            },
            new ModCatalogItem
            {
                Id = "town-of-us-r",
                Name = "Town Of Us: Reactivated",
                Author = "eDonnes124",
                Description = "Popularne rozszerzenie TownOfUs z nowymi rolami i mechanikami rozgrywki. Wersja reaktywowana przez społeczność.",
                Category = "Role / AddOn",
                RepoOwner = "eDonnes124",
                RepoName = "Town-Of-Us-R",
                AssetFileNameFilter = ".dll",
                TargetPluginFileName = "TownOfUs.dll",
                IsCore = false,
                RequiresCore = true,
                ProjectUrl = "https://github.com/eDonnes124/Town-Of-Us-R"
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
                ProjectUrl = "https://github.com/SubmergedAmongUs/Submerged"
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
                Id = "lotus-continued",
                Name = "Lotus Continued",
                Author = "Lotus-AU",
                Description = "Kontynuacja projektu Lotus – bogaty zbiór ról, zadań i mechanik stworzony przez społeczność Among Us.",
                Category = "Role / AddOn",
                RepoOwner = "Lotus-AU",
                RepoName = "LotusContinued",
                AssetFileNameFilter = ".dll",
                TargetPluginFileName = "ProjectLotus.dll",
                IsCore = false,
                RequiresCore = true,
                ProjectUrl = "https://github.com/Lotus-AU/LotusContinued"
            },

            // === STRONA 2: Rozgrywka, Personalizacja i Narzędzia ===
            new ModCatalogItem
            {
                Id = "wilder-draft",
                Name = "Wilder Draft",
                Author = "WanderingPix",
                Description = "Mod dodający tryb draftu ról – przed każdą grą gracze losują i wybierają swoje role.",
                Category = "Rozgrywka",
                RepoOwner = "WanderingPix",
                RepoName = "Wilder-Draft",
                AssetFileNameFilter = ".dll",
                TargetPluginFileName = "WilderDraft.dll",
                IsCore = false,
                RequiresCore = true,
                ProjectUrl = "https://github.com/WanderingPix/Wilder-Draft"
            },
            new ModCatalogItem
            {
                Id = "colors-of-joy",
                Name = "Colors Of Joy",
                Author = "WanderingPix",
                Description = "Rozszerza paletę kolorów postaci w Among Us o nowe, unikalne barwy.",
                Category = "Kosmetyki",
                RepoOwner = "WanderingPix",
                RepoName = "Colors-Of-Joy",
                AssetFileNameFilter = ".dll",
                TargetPluginFileName = "ColorsOfJoy.dll",
                IsCore = false,
                RequiresCore = true,
                ProjectUrl = "https://github.com/WanderingPix/Colors-Of-Joy"
            },
            new ModCatalogItem
            {
                Id = "stargazer",
                Name = "Stargazer",
                Author = "Reach-For-Stars-Team",
                Description = "Mod dodający nowe role astronomiczne i kosmiczne mechaniki do Among Us.",
                Category = "Role / AddOn",
                RepoOwner = "Reach-For-Stars-Team",
                RepoName = "Stargazer",
                AssetFileNameFilter = ".dll",
                TargetPluginFileName = "Stargazer.dll",
                IsCore = false,
                RequiresCore = true,
                ProjectUrl = "https://github.com/Reach-For-Stars-Team/Stargazer"
            },
            new ModCatalogItem
            {
                Id = "crewmeleon",
                Name = "Crewmeleon",
                Author = "OBit508",
                Description = "Mod dodający nową rolę Kameleona zmieniającego kolor, ukrywającego się wśród załogi.",
                Category = "Role / AddOn",
                RepoOwner = "OBit508",
                RepoName = "Crewmeleon",
                AssetFileNameFilter = ".dll",
                TargetPluginFileName = "Crewmeleon.dll",
                IsCore = false,
                RequiresCore = true,
                ProjectUrl = "https://github.com/OBit508/Crewmeleon"
            },
            new ModCatalogItem
            {
                Id = "crewmeleon-redrawn",
                Name = "Crewmeleon Redrawn",
                Author = "All-Of-Us-Mods",
                Description = "Przeprojektowana wersja Crewmeleon z ulepszonym balansem i nowymi umiejętnościami.",
                Category = "Role / AddOn",
                RepoOwner = "All-Of-Us-Mods",
                RepoName = "Crewmeleon-Redrawn",
                AssetFileNameFilter = ".dll",
                TargetPluginFileName = "CrewmeleonRedrawn.dll",
                IsCore = false,
                RequiresCore = true,
                ProjectUrl = "https://github.com/All-Of-Us-Mods/Crewmeleon-Redrawn"
            },
            new ModCatalogItem
            {
                Id = "host-utility",
                Name = "Host Utility",
                Author = "WanderingPix",
                Description = "Narzędzia dla hosta gry – ulepszony panel ustawień, szybkie komendy i zarządzanie lobby.",
                Category = "Narzędzia",
                RepoOwner = "WanderingPix",
                RepoName = "Host-Utility",
                AssetFileNameFilter = ".dll",
                TargetPluginFileName = "HostUtility.dll",
                IsCore = false,
                RequiresCore = true,
                ProjectUrl = "https://github.com/WanderingPix/Host-Utility"
            },

            // === STRONA 3: Dodatkowe Modyfikacje i Addony ===
            new ModCatalogItem
            {
                Id = "emojis-in-mogus-chat",
                Name = "Emojis in mogus chat",
                Author = "WanderingPix",
                Description = "Dodaje obsługę emoji do czatu w Among Us – wyrażaj emocje podczas gry!",
                Category = "Narzędzia",
                RepoOwner = "WanderingPix",
                RepoName = "Emojis-in-the-mogus-chat",
                AssetFileNameFilter = ".dll",
                TargetPluginFileName = "EmojisMod.dll",
                IsCore = false,
                RequiresCore = true,
                ProjectUrl = "https://github.com/WanderingPix/Emojis-in-the-mogus-chat"
            },
            new ModCatalogItem
            {
                Id = "achievements-api",
                Name = "AchievementsAPI",
                Author = "am-clonec",
                Description = "Framework osiągnięć dla Among Us – pozwala modom dodawać własne osiągnięcia i wyzwania.",
                Category = "Narzędzia",
                RepoOwner = "am-clonec",
                RepoName = "AchievementsAPI",
                AssetFileNameFilter = ".dll",
                TargetPluginFileName = "AchievementsAPI.dll",
                IsCore = false,
                RequiresCore = true,
                ProjectUrl = "https://github.com/am-clonec/AchievementsAPI"
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

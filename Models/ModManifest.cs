namespace AmongUsModManager.Models;

public class InstalledModEntry
{
    public string Id { get; set; } = "";
    public string Name { get; set; } = "";
    public string Version { get; set; } = "";
    public bool IsCore { get; set; } = false;
    public DateTimeOffset InstalledAt { get; set; } = DateTimeOffset.Now;
    public List<string> InstalledFiles { get; set; } = new();
}

public class ModManifest
{
    public int ManifestVersion { get; set; } = 1;
    public DateTimeOffset LastModified { get; set; } = DateTimeOffset.Now;
    public List<InstalledModEntry> InstalledMods { get; set; } = new();
}

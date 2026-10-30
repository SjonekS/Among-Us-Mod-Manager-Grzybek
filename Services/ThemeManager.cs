using System.Windows;
using System.Windows.Media;

namespace AmongUsModManager.Services;

public record ThemeDefinition(string Id, string Name, string Description, string AccentHex);

public static class ThemeManager
{
    public static readonly List<ThemeDefinition> AvailableThemes = new()
    {
        new("Nebula", "Nebula Purple (Głęboki Fiolet)", "Nowoczesny kosmiczny motyw z głębokim fioletem, grafitowym tłem i neonowym akcentem.", "#8B5CF6"),
        new("CyberNeon", "Cyberpunk Neon (Cyan)", "Ciemny kontrastowy motyw z elektryzującym neonowym turkusem.", "#06B6D4"),
        new("Midnight", "Midnight Obsidian (Onyks)", "Ultra-ciemny motyw głębokiej nocy z wyrazistym białym tekstem i chłodnym błękitem.", "#3B82F6"),
        new("SteelBlue", "Stalowy Błękit", "Klasyczny ciemny motyw inżynieryjny z neutralnym błękitem.", "#2563EB"),
        new("Emerald", "Szmaragdowy Grafit", "Chłodny ciemny grafit z tonem butelkowej zieleni.", "#059669"),
        new("Amber", "Bursztynowy Węgiel", "Ciepły węgiel drzewny z akcentem szlachetnego bursztynu.", "#D97706"),
        new("Titanium", "Tytanowy Karbon", "Minimalistyczny grafit z jasnoniebieskim stalowym akcentem.", "#0EA5E9")
    };

    public static void ApplyTheme(string themeId)
    {
        var app = Application.Current;
        if (app == null) return;

        void SetBrush(string key, string hex)
        {
            try
            {
                var color = (Color)ColorConverter.ConvertFromString(hex);
                var brush = new SolidColorBrush(color);
                brush.Freeze();

                app.Resources[key] = brush;

                if (app.MainWindow != null)
                {
                    app.MainWindow.Resources[key] = brush;
                }
            }
            catch { }
        }

        switch (themeId)
        {
            case "Nebula":
            case "Starlight":
                SetBrush("WindowBackground", "#0B0B14");
                SetBrush("CardBackground", "#121124");
                SetBrush("CardBorder", "#282347");
                SetBrush("AccentColor", "#8B5CF6");
                SetBrush("AccentHover", "#7C3AED");
                SetBrush("SuccessColor", "#10B981");
                SetBrush("WarningColor", "#F59E0B");
                SetBrush("DangerColor", "#EF4444");
                SetBrush("SubTextColor", "#C4B5FD");
                SetBrush("InputBackground", "#07070F");
                SetBrush("HeaderBadgeBg", "#261D4E");
                SetBrush("HeaderBadgeFg", "#DDD6FE");
                break;

            case "CyberNeon":
                SetBrush("WindowBackground", "#080E14");
                SetBrush("CardBackground", "#0F1A24");
                SetBrush("CardBorder", "#1B3245");
                SetBrush("AccentColor", "#06B6D4");
                SetBrush("AccentHover", "#0891B2");
                SetBrush("SuccessColor", "#10B981");
                SetBrush("WarningColor", "#F59E0B");
                SetBrush("DangerColor", "#EF4444");
                SetBrush("SubTextColor", "#A5F3FC");
                SetBrush("InputBackground", "#05090E");
                SetBrush("HeaderBadgeBg", "#112F3D");
                SetBrush("HeaderBadgeFg", "#67E8F9");
                break;

            case "Midnight":
                SetBrush("WindowBackground", "#090A0F");
                SetBrush("CardBackground", "#10131A");
                SetBrush("CardBorder", "#1E2433");
                SetBrush("AccentColor", "#3B82F6");
                SetBrush("AccentHover", "#2563EB");
                SetBrush("SuccessColor", "#10B981");
                SetBrush("WarningColor", "#F59E0B");
                SetBrush("DangerColor", "#EF4444");
                SetBrush("SubTextColor", "#BFDBFE");
                SetBrush("InputBackground", "#05060A");
                SetBrush("HeaderBadgeBg", "#172238");
                SetBrush("HeaderBadgeFg", "#93C5FD");
                break;

            case "Emerald":
                SetBrush("WindowBackground", "#0B1410");
                SetBrush("CardBackground", "#12211A");
                SetBrush("CardBorder", "#1E3B2F");
                SetBrush("AccentColor", "#059669");
                SetBrush("AccentHover", "#047857");
                SetBrush("SuccessColor", "#10B981");
                SetBrush("WarningColor", "#F59E0B");
                SetBrush("DangerColor", "#EF4444");
                SetBrush("SubTextColor", "#A7F3D0");
                SetBrush("InputBackground", "#070E0B");
                SetBrush("HeaderBadgeBg", "#132D22");
                SetBrush("HeaderBadgeFg", "#6EE7B7");
                break;

            case "Amber":
                SetBrush("WindowBackground", "#14120E");
                SetBrush("CardBackground", "#201B14");
                SetBrush("CardBorder", "#3D3222");
                SetBrush("AccentColor", "#D97706");
                SetBrush("AccentHover", "#B45309");
                SetBrush("SuccessColor", "#10B981");
                SetBrush("WarningColor", "#F59E0B");
                SetBrush("DangerColor", "#EF4444");
                SetBrush("SubTextColor", "#FDE68A");
                SetBrush("InputBackground", "#0D0C09");
                SetBrush("HeaderBadgeBg", "#2E2416");
                SetBrush("HeaderBadgeFg", "#FCD34D");
                break;

            case "Titanium":
                SetBrush("WindowBackground", "#101114");
                SetBrush("CardBackground", "#191B21");
                SetBrush("CardBorder", "#2B2E38");
                SetBrush("AccentColor", "#0EA5E9");
                SetBrush("AccentHover", "#0284C7");
                SetBrush("SuccessColor", "#10B981");
                SetBrush("WarningColor", "#F59E0B");
                SetBrush("DangerColor", "#EF4444");
                SetBrush("SubTextColor", "#BAE6FD");
                SetBrush("InputBackground", "#0B0C0E");
                SetBrush("HeaderBadgeBg", "#1E2633");
                SetBrush("HeaderBadgeFg", "#7DD3FC");
                break;

            case "SteelBlue":
            default:
                SetBrush("WindowBackground", "#0F141C");
                SetBrush("CardBackground", "#161E2E");
                SetBrush("CardBorder", "#263449");
                SetBrush("AccentColor", "#2563EB");
                SetBrush("AccentHover", "#1D4ED8");
                SetBrush("SuccessColor", "#10B981");
                SetBrush("WarningColor", "#F59E0B");
                SetBrush("DangerColor", "#EF4444");
                SetBrush("SubTextColor", "#CBD5E1");
                SetBrush("InputBackground", "#0B111A");
                SetBrush("HeaderBadgeBg", "#1E293B");
                SetBrush("HeaderBadgeFg", "#93C5FD");
                break;
        }
    }
}

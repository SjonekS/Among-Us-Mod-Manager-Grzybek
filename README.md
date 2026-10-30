# 🛸 Among Us Mod Manager (Grzybek) v3.0

> Nowoczesny, modułowy menedżer i instalator modyfikacji dla **Among Us** z nowoczesnym, przejrzystym interfejsem.

[![Platform](https://img.shields.io/badge/platform-Windows%20x64-blue.svg)](https://dotnet.microsoft.com/)
[![.NET](https://img.shields.io/badge/.NET-10.0%20WPF-purple.svg)](https://dotnet.microsoft.com/)
[![License](https://img.shields.io/badge/license-MIT-green.svg)](LICENSE)

---

## 🌟 Główne Funkcje

1. **🎮 Główna Gra (Steam)**:
   - Automatyczne wykrywanie instalacji gry Among Us w bibliotekach Steam.
   - Pobieranie i aktualizacja pakietu bazowego **TouMira Core** jednym kliknięciem.
   - Szybkie uruchamianie gry przez protokół Steam.

2. **📦 Niezależne Instancje Gry (Kapsuły Czasu)**:
   - Tworzenie w pełni autonomicznych kopii gry i modów.
   - Możliwość testowania modyfikacji bez obawy o aktualizacje gry na Steamie.
   - Każda instancja posiada własne przyciski szybkiego startu, aktualizacji, instalacji ZIP i usuwania.

3. **🌐 Zintegrowana Przeglądarka Modów (Explore)**:
   - Katalog modyfikacji społeczności Among Us (m.in. Submerged, LevelImposter, LocalizeUs, ChaosTokens, GameTweaks, TownOfExtra i wiele innych).
   - Wybór środowiska docelowego (Główny Steam lub wybrana instancja).
   - Wyszukiwarka na żywo i filtry stanu (*Wszystkie*, *Zainstalowane*, *Dostępne*).
   - Karty z podglądem, automatycznymi ikonami z repozytoriów GitHub oraz pełny modal szczegółów.

4. **❓ Procedury Naprawcze & Diagnostyka (Troubleshoot)**:
   - Tryb bezpiecznego czyszczenia z suwakiem zezwolenia (zerowe ryzyko utraty danych).
   - Hard Clear (usunięcie plików BepInEx z zachowaniem konfiguracji).
   - Szybkie czyszczenie folderu `plugins`.
   - Wbudowany podgląd dziennika zdarzeń na żywo.

5. **⚙ Personalizacja & Kopie Zapasowe**:
   - Motywy kolorystyczne: **Nebula Purple**, **Cyberpunk Neon**, **Midnight Obsidian** i inne.
   - Automatyczne tworzenie kopii zapasowych (ZIP) przed każdą instalacją z inteligentną rotacją.

6. **🎓 Interaktywny Samouczek Spotlight**:
   - Wbudowany samouczek krok po kroku z przyciemnieniem tła i podglądem funkcji na żywo.

---

## 🚀 Jak uruchomić

Wystarczy pobrać najnowszą wersję `AmongUsModManager.exe` z zakładki **[Releases](https://github.com/SjonekS/Among-Us-Mod-Manager-Grzybek/releases)** i uruchomić. Aplikacja jest samodzielna (self-contained) i nie wymaga instalowania dodatkowego środowiska .NET.

---

## 🛠️ Budowanie ze źródeł

Wymagania:
- [.NET 10.0 SDK](https://dotnet.microsoft.com/)
- System Windows 10/11 x64

```bash
# Sklonuj repozytorium
git clone https://github.com/SjonekS/Among-Us-Mod-Manager-Grzybek.git
cd Among-Us-Mod-Manager-Grzybek

# Kompilacja w trybie Debug
dotnet build AmongUsModManager.csproj

# Publikacja pojedynczego pliku wykonywalnego
dotnet publish AmongUsModManager.csproj -c Release -r win-x64 --self-contained true -o publish
```

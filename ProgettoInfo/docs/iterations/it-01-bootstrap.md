# IT-01 - Bootstrap progetto MAUI e Shell di base

## Obiettivo

Bootstrap dell'app `MealWise Mobile` in `src/MealWise.Mobile/` con:

- progetto .NET MAUI Android-first;
- Shell con due sezioni placeholder (`Search`, `Browse`);
- wiring MVVM (ViewModels + bindings tipizzati) e dependency injection minima.

## Scope completato

- Creato progetto MAUI: `src/MealWise.Mobile/MealWise.Mobile.csproj`.
- Introdotte cartelle e placeholder MVVM: `src/MealWise.Mobile/ViewModels/`, `src/MealWise.Mobile/Views/`.
- Shell (TabBar) con pagine `SearchPage` e `BrowsePage`.
- Registrazione DI in `MauiProgram.cs` per `AppShell`, pagine e ViewModels.

## Note ambiente (build Android su Linux)

Per compilare su questo host e' stato necessario configurare:

- .NET SDK installato localmente in `~/.dotnet`.
- Android SDK installato localmente in `~/.android-sdk`.
- JDK 17 installato localmente in `~/.local/share/jdk-17.0.19+10`.

Build eseguita con:

```bash
dotnet build src/MealWise.Mobile/MealWise.Mobile.csproj -c Debug \
  /p:AndroidSdkDirectory="$HOME/.android-sdk" \
  /p:JavaSdkDirectory="$HOME/.local/share/jdk-17.0.19+10"
```

## Fuori scope

- integrazione TheMealDB;
- logica di ricerca e dettaglio;
- persistenza locale (SQLite) per calendario/lista spesa.

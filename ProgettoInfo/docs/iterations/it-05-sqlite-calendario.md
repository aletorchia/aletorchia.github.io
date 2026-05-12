# IT-05 - Migrazione persistenza calendario a SQLite

## Obiettivo

Allineare IT-05 alla specifica introducendo persistenza SQLite per calendario pasti (e stato locale collegato alla lista spesa), mantenendo compatibilita' con i dati gia' presenti.

## Piano

- aggiungere dipendenza `sqlite-net-pcl`;
- migrare `MealPlanService` da `Preferences` + JSON a tabelle SQLite locali;
- mantenere migrazione automatica dei dati legacy al primo avvio;
- mantenere invariati i contratti `IMealPlanService` e i ViewModel;
- verificare build Android.

## Prompt principali utilizzati

1. "bene allora parti con IT-05"

## File modificati

- `src/MealWise.Mobile/MealWise.Mobile.csproj`
- `src/MealWise.Mobile/Services/MealPlanService.cs`
- `src/MealWise.Mobile/ViewModels/MealCalendarViewModel.cs`
- `src/MealWise.Mobile/ViewModels/ShoppingListViewModel.cs`
- `src/MealWise.Mobile/ViewModels/RecipeDetailViewModel.cs`
- `docs/plan.md`
- `docs/test-matrix.md`
- `docs/iterations/it-05-sqlite-calendario.md`

## Implementazione

- Inserito package `sqlite-net-pcl`.
- Creato database locale `mealwise-local.db3` in `FileSystem.AppDataDirectory`.
- Introdotte tabelle SQLite:
  - `planned_meals`
  - `shopping_overrides`
  - `manual_shopping_items`
- Mantenuto il modello pubblico (`PlannedMeal`, `ShoppingListItem`) e i metodi di `IMealPlanService`.
- Aggiunta migrazione automatica da chiavi legacy `Preferences` (`mealwise.plannedMeals.v1`, `mealwise.shoppingState.v1`) se il DB e' vuoto.
- Estesa la gestione errore UI per eccezioni storage locali non-JSON.

## Test eseguiti

- [x] `dotnet build src/MealWise.Mobile/MealWise.Mobile.csproj -c Debug`

Esito build:
- 0 errori
- 2 warning `XA0141` su `libe_sqlite3.so` (`SQLitePCLRaw.lib.e_sqlite3.android` 2.1.2), non bloccanti.

## Rischi aperti

- Warning Android page-size 16 KB sulla dipendenza nativa SQLite da monitorare/aggiornare.
- Manca ancora una suite `dotnet test` dedicata al repository SQLite (migrazione, CRUD, idempotenza).

## Esito

IT-05 avviata e completata lato persistenza SQLite con migrazione legacy, senza regressioni di compilazione.

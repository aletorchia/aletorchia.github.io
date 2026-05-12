# Architettura

## 1. Obiettivo architetturale

Costruire una app `.NET MAUI` Android-first che resti semplice da spiegare, estendere e testare, separando chiaramente:

- UI XAML e navigazione;
- logica di stato nei ViewModels;
- integrazione remota con TheMealDB;
- persistenza locale (post-MVP) per calendario pasti, lista spesa e ricette salvate.

L'architettura deve supportare il MVP (ricerca + dettaglio ricette) senza anticipare troppo il post-MVP, ma lasciando confini sufficienti per introdurre database SQLite e feature locali senza refactor invasivi.

## 2. Struttura del repository e del progetto

### Cartelle principali

- `docs/`: specifica, piano, architettura, matrice test, iterazioni.
- `src/`: conterrà il progetto applicativo MAUI reale.
- `src/MealWise.Mobile/`: root proposta del progetto MAUI da creare in IT-01.

### Responsabilità per area

- `src/MealWise.Mobile/Views/`: pagine XAML, layout, binding e componenti visuali senza business logic.
- `src/MealWise.Mobile/ViewModels/`: stato della UI, comandi, orchestrazione di servizi e navigazione.
- `src/MealWise.Mobile/Services/`: accesso API, mapping, persistenza locale (post-MVP), nessuna logica di presentazione.
- `src/MealWise.Mobile/Models/`: modelli di dominio e DTO di integrazione.
- `src/MealWise.Mobile/Resources/`: stili, placeholder, immagini statiche, temi.

## 3. Pattern applicativi

- `.NET MAUI` single-project.
- MVVM con `CommunityToolkit.Mvvm`.
- Shell navigation per sezioni principali e route di dettaglio.
- XAML con compiled bindings dove possibile.
- `HttpClient` asincrono per il provider remoto.
- `System.Text.Json` per parsing e mapping difensivo.
- Nessuna business logic nei code-behind.

## 4. Componenti principali

### Views

- `SearchPage`: filtri (ingrediente/categoria), input e risultati nella stessa pagina con `CollectionView`.
- `RecipeDetailPage`: dettaglio ricetta (ingredienti, misure, istruzioni) con stati UI.

Post-MVP (non nel MVP):

- `MealCalendarPage`: calendario pasti.
- `ShoppingListPage`: lista spesa locale.
- `SavedRecipesPage`: ricette salvate e consultabili localmente.

### ViewModels

- `SearchViewModel`: gestisce filtro ricerca (ingredient/categoria), input, risultati, retry e stati `loading/error/empty/success`.
- `RecipeDetailViewModel`: carica il dettaglio, gestisce loading e retry, e tollera campi mancanti.

Post-MVP:

- `MealCalendarViewModel`: gestione planning pasti e persistenza locale.
- `ShoppingListViewModel`: aggregazione/modifica lista spesa e persistenza locale.
- `SavedRecipesViewModel`: elenco ricette salvate, rimozione e apertura del dettaglio locale.

Ogni ViewModel dovrà esporre proprietà di stato esplicite come `IsBusy`, `ErrorMessage` e un indicatore di contenuto (es. `HasData` / `IsEmpty`).

### Services

- `RecipeApiClient` (o equivalente):
  - ricerca per ingrediente: `filter.php?i=...`;
  - ricerca per categoria: `filter.php?c=...`;
  - lookup dettaglio: `lookup.php?i=...`.
- `RecipeMapper` (o mapping dedicato): conversione DTO -> modelli di dominio, con gestione difensiva di null/campi mancanti.

Post-MVP:

- servizio database locale basato su `sqlite-net-pcl`, responsabile di creare e condividere una singola `SQLiteAsyncConnection`.
- repository calendario.
- repository lista spesa.
- repository ricette salvate/cache dettaglio.

## 5. Navigazione

### Route Shell

- `Search` come sezione principale MVP.
- route `RecipeDetailPage` fuori dal livello root, apribile da `Search`.

Post-MVP:

- `Calendar`, `Shopping` e `Saved` come sezioni principali aggiuntive.

### Parametri di navigazione

- verso il dettaglio: `mealId` obbligatorio.

## 6. Stato della UI

### Loading

- `SearchPage`: loading esplicito durante una richiesta remota.
- `RecipeDetailPage`: loading esplicito durante il lookup dettaglio.

### Error

- errori remoti e di parsing mostrati con messaggi comprensibili e azione di retry.
- nessun errore tecnico grezzo o stack trace in UI.

### Empty

- Search senza risultati remoti.

### Success

- risultati presenti su Search.
- dettaglio caricato e leggibile anche con campi parziali.

## 7. Dati e integrazioni

### Chiamate remote

- provider base MVP: TheMealDB.
- endpoint previsti:
  - `GET /api/json/v1/1/filter.php?i={ingredient}`
  - `GET /api/json/v1/1/filter.php?c={category}`
  - `GET /api/json/v1/1/lookup.php?i={mealId}`

### Parsing JSON

- uso di `System.Text.Json` con DTO dedicati;
- mapping difensivo centralizzato per evitare null-handling disperso nei ViewModels;
- trasformazione dei dati API in modelli adatti alla UI prima di arrivare alle Views.

## 8. Dependency injection e composition root

`MauiProgram.cs` e' il punto di composizione del progetto. Qui dovranno essere registrati:

- `HttpClient` e servizio API ricette;
- mapping;
- ViewModels e pagine.

Post-MVP:

- servizio database SQLite e repository locali.

## 9. Error handling e logging

- Le eccezioni di rete e parsing devono essere intercettate dal layer di servizio o dal ViewModel, non propagate direttamente alla UI.
- I ViewModels traducono i guasti in messaggi comprensibili e stati coerenti.
- I log diagnostici possono usare il logging standard disponibile in MAUI, senza telemetria nel MVP.

## 10. Decisioni confermate

- Provider MVP: TheMealDB.
- MVP senza persistenza locale per calendario/spesa/ricette salvate; questi entrano nel post-MVP.
- Post-MVP: SQLite come storage locale con `sqlite-net-pcl` (una sola `SQLiteAsyncConnection` condivisa).
- Ricette salvate e cache dettaglio sono una feature locale distinta dal supporto offline completo: non sostituiscono la ricerca remota.

TBD:

- Nome definitivo dell'app e struttura delle sezioni Shell (il documento assume `Search` nel MVP).

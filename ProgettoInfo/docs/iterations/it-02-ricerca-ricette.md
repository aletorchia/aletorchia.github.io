# IT-02 - Ricerca ricette e risultati

## Obiettivo

Implementare la ricerca ricette nella pagina `Search`, usando TheMealDB per ingrediente o categoria e mostrando i risultati nella stessa schermata con stati UI espliciti.

## Piano

- Creare un modello lista risultati (`RecipeSearchResult`) e un enum per il tipo ricerca.
- Creare un servizio remoto separato per TheMealDB.
- Aggiornare `SearchViewModel` con query, tipo ricerca, comando search/retry e stati `IsBusy`, `ErrorMessage`, `HasData`, `IsEmptyState`.
- Aggiornare `SearchPage.xaml` con input, selezione filtro, feedback loading/error/empty e `CollectionView`.
- Registrare servizio e `HttpClient` in DI.

## Scope completato

- Aggiunto `RecipeSearchService` con mapping difensivo di `meals: null` e campi opzionali.
- Aggiunto `SearchViewModel` con gestione esplicita di loading, error, empty e success state.
- Aggiunta UI `Search` con `Picker`, `SearchBar`, comando di retry e lista risultati.
- Configurato JDK 21 locale per la build Android tramite `Directory.Build.props`.
- Aggiornato il bootstrap `App` con `CreateWindow` per .NET 10.

## Fuori scope

- Navigazione al dettaglio ricetta.
- Parsing ingredienti/istruzioni estesi.
- Persistenza locale, calendario pasti e lista spesa.
- Caricamento dinamico delle categorie disponibili.

## Verifica

Build Android:

```bash
dotnet build -f net10.0-android
```

Risultato: build completata con 0 warning e 0 errori.

Smoke su device Android collegato:

```bash
dotnet build -f net10.0-android -t:Run
adb shell pidof com.companyname.mealwise.mobile
```

Risultato: l'app resta avviata sul device dopo il launch.

## Rischi e punti da controllare

- TheMealDB puo' restituire risultati vuoti (`meals: null`), gia' gestiti come empty state.
- Le immagini remote possono essere lente o assenti; la UI non deve dipendere dalla presenza dell'immagine.
- La verifica funzionale completa richiede test manuale su device con rete attiva usando query come `chicken` e categoria `Seafood`.

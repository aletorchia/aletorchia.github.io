# IT-02 - Ricerca ricette e risultati

## Obiettivo

Implementare la ricerca ricette nella pagina `Search`, usando TheMealDB per ingrediente o categoria, mostrando risultati navigabili e completando Browse con ricette casuali apribili nel dettaglio.

## Piano

- Creare un modello lista risultati (`RecipeSearchResult`) e un enum per il tipo ricerca.
- Creare un servizio remoto separato per TheMealDB.
- Aggiornare `SearchViewModel` con query, tipo ricerca, comando search/retry e stati `IsBusy`, `ErrorMessage`, `HasData`, `IsEmptyState`.
- Aggiornare `SearchPage.xaml` con input, selezione filtro, feedback loading/error/empty e `CollectionView`.
- Registrare servizio e `HttpClient` in DI.
- Aggiungere una pagina dettaglio ricetta raggiungibile dai risultati.
- Aggiornare `Browse` per caricare ricette casuali e aprire lo stesso dettaglio.

## Scope completato

- Aggiunto `RecipeSearchService` con mapping difensivo di `meals: null` e campi opzionali.
- Aggiunto `SearchViewModel` con gestione esplicita di loading, error, empty e success state.
- Aggiunta UI `Search` con `Picker`, `SearchBar`, comando di retry e lista risultati.
- Aggiunta schermata principale `Home` come prima tab Shell, con azioni verso ricerca e browse.
- Aggiunta navigazione Shell `recipe-detail` dai risultati di ricerca e da Browse.
- Aggiunta `RecipeDetailPage` con immagine, categoria, area, ingredienti e istruzioni.
- Esteso `RecipeSearchService` con `lookup.php` per il dettaglio ricetta e `random.php` per Browse.
- Aggiornata `Browse` per caricare ricette casuali con stati loading, error, empty e lista risultati.
- Corretto il binding della ricerca per usare sempre il testo visibile nella `SearchBar` e il filtro selezionato.
- Configurato JDK 21 locale per la build Android tramite `Directory.Build.props`.
- Aggiornato il bootstrap `App` con `CreateWindow` per .NET 10.

## Fuori scope

- Persistenza locale, calendario pasti e lista spesa.
- Caricamento dinamico delle categorie disponibili.
- Preferiti e salvataggio offline delle ricette.

## Verifica

Build Android:

```bash
dotnet build -f net10.0-android
```

Risultato: build completata con 0 warning e 0 errori.

Verifica endpoint TheMealDB:

```bash
curl https://www.themealdb.com/api/json/v1/1/filter.php?i=chicken
curl https://www.themealdb.com/api/json/v1/1/filter.php?c=Seafood
curl https://www.themealdb.com/api/json/v1/1/lookup.php?i=52772
curl https://www.themealdb.com/api/json/v1/1/random.php
```

Risultato: gli endpoint restituiscono risultati coerenti per ricerca ingrediente, ricerca categoria, dettaglio e ricetta casuale.

Smoke su device Android collegato:

```bash
adb devices -l
```

Risultato corrente: nessun device collegato nella verifica finale, quindi il controllo runtime va ripetuto su emulatore o telefono.

## Rischi e punti da controllare

- TheMealDB puo' restituire risultati vuoti (`meals: null`), gia' gestiti come empty state.
- Le immagini remote possono essere lente o assenti; la UI non deve dipendere dalla presenza dell'immagine.
- La verifica funzionale completa richiede test manuale su device con rete attiva usando query come `chicken`, categoria `Seafood`, apertura dettaglio e refresh Browse.

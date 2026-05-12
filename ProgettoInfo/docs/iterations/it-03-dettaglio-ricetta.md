# IT-03 - Navigazione dettaglio ricetta

## Obiettivo

Completare la navigazione dal risultato di ricerca alla pagina dettaglio ricetta, caricando da TheMealDB ingredienti, misure e istruzioni tramite `mealId`, con gestione esplicita di loading, error, empty e campi mancanti.

## Piano

- verificare route Shell `recipe-detail` e passaggio parametro `mealId`;
- verificare `RecipeSearchService.GetDetailAsync` su endpoint `lookup.php`;
- consolidare `RecipeDetailViewModel` con stati `IsBusy`, `ErrorMessage`, `HasData`, `IsEmptyState`;
- rendere leggibile la UI anche se ingredienti, area, categoria o istruzioni non sono presenti;
- aggiornare documentazione e matrice test;
- eseguire build Android come verifica automatica disponibile.

## Prompt principali utilizzati

1. "fai IT-03"

## File creati

- `docs/iterations/it-03-dettaglio-ricetta.md`

## File modificati

- `src/MealWise.Mobile/ViewModels/RecipeDetailViewModel.cs`
- `src/MealWise.Mobile/Views/RecipeDetailPage.xaml`
- `docs/plan.md`
- `docs/test-matrix.md`

## Codice prodotto dall'AI e accettato

- Hardening del ViewModel dettaglio: reset esplicito dei dati, retry sul comando `LoadCommand`, fallback leggibili per campi opzionali mancanti.
- Stato UI dedicato per ingredienti mancanti, esposto alla pagina con `IsIngredientListEmpty`.
- Messaggio XAML per lista ingredienti vuota.

## Codice prodotto dall'AI e modificato manualmente

- La base della feature dettaglio era gia' presente nel branch di partenza: route Shell, pagina detail, ViewModel, modello `RecipeDetail` e servizio `lookup.php`.
- In questa iterazione la base e' stata rivista e rifinita senza spostare logica nel code-behind.

## Test eseguiti

- [x] `dotnet build src/MealWise.Mobile/MealWise.Mobile.csproj -c Debug`
- [ ] Smoke manuale su device/emulatore Android: ricerca `chicken`, tap su risultato, apertura detail, back navigation.
- [ ] Simulazione rete assente su dettaglio e retry.

## Problemi trovati

- IT-03 era stata parzialmente inclusa nel commit precedente di IT-02, ma mancava un log dedicato dell'iterazione.
- Se TheMealDB restituisce dettaglio con ingredienti o istruzioni mancanti, la UI poteva lasciare sezioni vuote.
- `docs/plan.md` conteneva riferimenti residui a libri/volume nella parte ricette.

## Correzioni effettuate

- Aggiunto fallback "Istruzioni non disponibili." quando le istruzioni sono vuote.
- Aggiunto fallback "Non indicata" per categoria e area mancanti.
- Aggiunto stato `IsIngredientListEmpty` e messaggio "Ingredienti non disponibili.".
- Aggiornato `docs/plan.md` per segnare IT-03 come completata e correggere i riferimenti non coerenti.
- Aggiornato `docs/test-matrix.md` con evidenze e verifiche residue per IT-03.

## Esito

IT-03 completata a livello di codice e documentazione. Restano da eseguire smoke manuale su device/emulatore Android e test automatici dedicati al mapping/detail quando verra' introdotto un progetto test.

## Addendum post-MVP

Su richiesta successiva, calendario/lista spesa e salvataggio ricette in locale sono stati esplicitati nella roadmap post-MVP. Non vengono assorbiti in IT-03 perche' richiedono persistenza locale, nuove schermate e repository dedicati; sono tracciati come iterazioni separate in `docs/plan.md`.

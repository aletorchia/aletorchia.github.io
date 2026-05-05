# Skill IT-01 - Skill maui-expert

## Obiettivo

Introdurre una skill di progetto dedicata alla scrittura di codice `.NET MAUI` coerente con le convenzioni del repository.

## File introdotti

- `.agents/skills/maui-expert/SKILL.md`
- `.agents/skills/maui-expert/examples/README.md`
- `.agents/skills/maui-expert/examples/collectionview-binding/ProductsPage.xaml`
- `.agents/skills/maui-expert/examples/collectionview-binding/ProductsPageViewModel.cs`
- `.agents/skills/maui-expert/examples/legacy-listview-binding/OrdersPage.xaml`
- `.agents/skills/maui-expert/examples/legacy-listview-binding/OrdersPageViewModel.cs`
- `.agents/skills/maui-expert/evals/evals.json`

## Regole incorporate

- MVVM con `CommunityToolkit.Mvvm`
- uso di `[ObservableProperty]` e `[RelayCommand]`
- XAML pulito con binding compilati (`x:DataType`)
- navigazione esclusivamente tramite Shell
- quattro stati di pagina: `IsBusy`, `ErrorMessage`, `HasData`, `IsEmptyState`
- servizi iniettati tramite costruttore
- niente logica nel code-behind
- niente caricamento iniziale tramite `OnAppearing` o eventi pagina nel code-behind, salvo eccezioni spiegate
- gestione esplicita di `HttpRequestException`, `JsonException` e `TaskCanceledException`

## Note

Gli esempi inclusi mostrano sia il percorso consigliato con `CollectionView`, sia un esempio compatibile con `ListView` per scenari legacy.

La skill ora tratta `ListView` come tecnologia legacy: non deve proporla per nuovo codice MAUI e, quando la incontra in un progetto esistente, deve offrire la migrazione a `CollectionView`.

Il file `evals/evals.json` fornisce quattro prompt iniziali per verificare la skill in una fase successiva, incluso un caso dedicato al divieto di caricamento iniziale nel code-behind.

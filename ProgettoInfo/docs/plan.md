# Piano di Progetto

## 1. Sintesi operativa

### Obiettivo del progetto

Realizzare `MealWise Mobile` come app `.NET MAUI` Android-first (iOS opzionale) per cercare e consultare ricette, con query asincrone verso API esterna filtrate per ingrediente principale o categoria, mantenendo MVVM, Shell navigation e una separazione chiara tra Views, ViewModels e servizi remoti.

Il repository contiene attualmente la documentazione di progetto e una cartella `src/` vuota. La prima iterazione dovrà quindi bootstrapare il progetto MAUI reale in `src/MealWise.Mobile/` prima di affrontare le feature applicative.

### Vincoli principali

- `.NET MAUI` con target principale Android; iOS opzionale e secondario.
- Architettura MVVM con `CommunityToolkit.Mvvm`.
- Navigazione basata su Shell.
- `HttpClient` asincrono e `System.Text.Json`.
- TheMealDB come provider base dell'MVP.
- Search e Results in una singola schermata.
- Stati UI espliciti: loading, error, empty, success.
- Persistenza locale con SQLite (post-MVP) per calendario e lista spesa.
- Nessuna autenticazione, sincronizzazione cloud o funzionalità social nel v1.

### Dipendenze esterne

- TheMealDB API per ricerca e dettaglio.
- Connettività Internet per i flussi remoti del MVP.
- Ambiente MAUI Android funzionante su macchina di sviluppo.
- (Post-MVP) Layer SQLite locale per calendario e lista spesa.

## 2. Sequenza delle iterazioni

| Iterazione | Obiettivo verificabile | Dipendenze | Rischio | Stato |
| --- | --- | --- | --- | --- |
| IT-01 | Bootstrap del progetto MAUI e Shell di base | Nessuna | medio | completata |
| IT-02 | Ricerca ricette (ingrediente/categoria) e risultati nella pagina Search | IT-01 | medio | completata |
| IT-03 | Navigazione al dettaglio e rendering ingredienti/istruzioni | IT-02 | medio | completata |
| IT-04 | Hardening MVP (stati UI, errori, smoke) | IT-01, IT-02, IT-03 | medio | pianificata |
| IT-05 (post-MVP) | Calendario pasti locale con SQLite | IT-04 | medio | pianificata |
| IT-06 (post-MVP) | Lista spesa locale derivata dal calendario | IT-05 | medio-alto | pianificata |

## 3. Dettaglio iterazioni

### IT-01 - Bootstrap progetto MAUI e Shell di base

**Obiettivo verificabile**

Creare il progetto MAUI in `src/MealWise.Mobile/`, avviarlo su Android e rendere raggiungibili le viste principali `Search` e `Browse` (placeholder) tramite Shell, senza implementare ancora la logica applicativa del dominio.

**In scope**

- creare il progetto `.NET MAUI` in `src/MealWise.Mobile/`;
- impostare `App`, `AppShell` e `MauiProgram`;
- predisporre cartelle `Models`, `Services`, `ViewModels`, `Views`, `Resources`;
- creare le pagine iniziali `Search` e `Browse` come placeholder MVVM-safe;
- impostare dependency injection minima per servizi e ViewModels futuri.

**Out of scope**

- integrazione con TheMealDB;
- pagina dettaglio ricetta completa;
- persistenza locale (calendario/lista spesa);
- gestione reale di ricerca e dettaglio.

**File o aree probabili**

- `src/MealWise.Mobile/MealWise.Mobile.csproj`
- `src/MealWise.Mobile/App.xaml`
- `src/MealWise.Mobile/App.xaml.cs`
- `src/MealWise.Mobile/AppShell.xaml`
- `src/MealWise.Mobile/AppShell.xaml.cs`
- `src/MealWise.Mobile/MauiProgram.cs`
- `src/MealWise.Mobile/Views/`
- `src/MealWise.Mobile/ViewModels/`

**Dipendenze**

- `docs/spec.md` approvato;
- workload MAUI funzionante sull'ambiente locale.

**Criteri di accettazione**

- [ ] L'app compila e si avvia su Android.
- [ ] Le sezioni `Search` e `Browse` sono raggiungibili da Shell.
- [ ] Le pagine create usano bindings e ViewModels, senza business logic nei code-behind.
- [ ] La struttura delle cartelle del progetto è coerente con `docs/architecture.md`.

**Verifiche principali**

- manuale: avvio app e navigazione tra le sezioni principali;
- automatico: `dotnet build src/MealWise.Mobile/MealWise.Mobile.csproj -c Debug /p:AndroidSdkDirectory=$HOME/.android-sdk /p:JavaSdkDirectory=$HOME/.local/share/jdk-17.0.19+10`.

**Rischi**

- introdurre logica di feature dentro il bootstrap invece di mantenere uno scheletro minimo;
- creare una struttura iniziale incoerente che obblighi refactor precoci nelle iterazioni successive.

### IT-02 - Ricerca ricette e risultati in pagina unica

**Obiettivo verificabile**

Consentire all'utente di cercare ricette per ingrediente principale o categoria dalla schermata `Search`, visualizzando risultati con immagine (se disponibile) e nome ricetta nella stessa pagina, con gestione esplicita di loading, empty ed error state.

**In scope**

- integrare TheMealDB per:
  - `GET /filter.php?i={ingredient}`;
  - `GET /filter.php?c={category}`;
- introdurre servizio remoto per la ricerca ricette;
- modellare DTO e mapping difensivo verso modelli di dominio per la lista risultati;
- implementare `SearchViewModel` con query, comando di ricerca e stati UI;
- implementare `SearchPage` con `SearchBar`, `CollectionView`, placeholder copertina e retry.

**Out of scope**

- apertura del dettaglio ricetta;
- calendario e lista spesa;
- supporto multi-provider oltre a TheMealDB.

**File o aree probabili**

- `src/MealWise.Mobile/Views/SearchPage.xaml`
- `src/MealWise.Mobile/ViewModels/SearchViewModel.cs`
- `src/MealWise.Mobile/Services/`
- `src/MealWise.Mobile/Models/`
- `src/MealWise.Mobile/Resources/`

**Dipendenze**

- completamento di IT-01;
- connettività Internet e disponibilità TheMealDB.

**Criteri di accettazione**

- [ ] Inserendo una query valida, la pagina Search avvia la richiesta remota e mostra subito il loading state.
- [ ] In caso di successo, la lista mostra almeno immagine e nome ricetta per ogni risultato.
- [ ] In caso di risposta vuota, la UI mostra uno stato empty esplicito.
- [ ] In caso di errore o timeout, la UI mostra un messaggio comprensibile e una azione di retry.
- [ ] L'assenza della copertina o di altri campi opzionali non genera crash della UI.

**Verifiche principali**

- manuale: ricerche con query valida, query senza risultati e simulazione di errore rete;
- automatico: `dotnet build src/MealWise.Mobile/MealWise.Mobile.csproj` e controlli unitari del mapping/service layer se il progetto test viene introdotto nello stesso slice.

**Rischi**

- risposte JSON parziali o inconsistenti da TheMealDB;
- immagini remote lente o assenti che peggiorano la leggibilità della lista.

### IT-03 - Navigazione al dettaglio e dati estesi della ricetta

**Obiettivo verificabile**

Permettere l'apertura del dettaglio di una ricetta dalla lista risultati, caricando ingredienti e istruzioni da TheMealDB e gestendo correttamente campi mancanti, loading e retry.

**In scope**

- registrare la route Shell per il dettaglio;
- passare il parametro `mealId` dalla lista risultati al dettaglio;
- integrare `GET /lookup.php?i={mealId}`;
- implementare `RecipeDetailViewModel` e `RecipeDetailPage`;
- mostrare ingredienti, misure e istruzioni quando disponibili.

**Out of scope**

- calendario e lista spesa;
- salvataggio ricette in locale (post-MVP).

**File o aree probabili**

- `src/MealWise.Mobile/AppShell.xaml`
- `src/MealWise.Mobile/AppShell.xaml.cs`
- `src/MealWise.Mobile/Views/RecipeDetailPage.xaml`
- `src/MealWise.Mobile/ViewModels/RecipeDetailViewModel.cs`
- `src/MealWise.Mobile/Services/`
- `src/MealWise.Mobile/Models/`

**Dipendenze**

- completamento di IT-02;
- disponibilita' del `mealId` proveniente dalla ricerca.

**Criteri di accettazione**

- [ ] Toccando un risultato, l'utente raggiunge la pagina di dettaglio corretta.
- [ ] Il dettaglio mostra ingredienti e istruzioni quando presenti.
- [ ] In presenza di campi mancanti o null, la pagina resta leggibile e non mostra errori tecnici.
- [ ] Il dettaglio mostra loading ed error state con azione di retry.

**Verifiche principali**

- manuale: apertura dettaglio da piu' risultati, incluse ricette con metadati incompleti;
- automatico: `dotnet build src/MealWise.Mobile/MealWise.Mobile.csproj` e controlli unitari del mapping del dettaglio se introdotti.

**Rischi**

- propagazione errata dei parametri Shell;
- differenze tra dati disponibili in lista e dettaglio che impattano il rendering.

### IT-04 - Hardening MVP e baseline di regressione

**Obiettivo verificabile**

Stabilizzare il MVP (Search + Detail): testi, stati UI coerenti, retry affidabile, mapping difensivo robusto e smoke di navigazione.

**In scope**

- rifinire gestione input (ingredient/categoria) e copy degli stati empty/error;
- verificare rendering immagini mancanti e liste vuote;
- gestire timeouts e parsing error senza crash;
- eseguire smoke manuale end-to-end `Search -> Detail` e ritorno alle sezioni principali;
- aggiornare `docs/test-matrix.md` con evidenze eseguibili.

**Out of scope**

- calendario e lista spesa;
- ricette salvate in locale;
- provider alternativi.

**File o aree probabili**

- `src/MealWise.Mobile/Views/`
- `src/MealWise.Mobile/ViewModels/`
- `src/MealWise.Mobile/Services/`
- `docs/test-matrix.md`

**Dipendenze**

- completamento di IT-03;
- disponibilità del layer SQLite locale.

**Criteri di accettazione**

- [ ] Search e Detail mostrano sempre uno stato coerente (loading/success/empty/error).
- [ ] Errori di rete e parsing non causano crash e mostrano messaggi comprensibili.
- [ ] Il flusso `Search -> Detail` e ritorno e' ripetibile senza blocchi.

**Verifiche principali**

- manuale: smoke completo `Search -> Detail` con rete ok e rete assente;
- automatico: `dotnet build src/MealWise.Mobile/MealWise.Mobile.csproj` ed eventuale `dotnet test` se introdotto.

**Rischi**

- difetti di mapping e null-handling nel dettaglio;
- instabilita' di rete che genera stati UI incoerenti.

### IT-05 (post-MVP) - Calendario pasti locale con SQLite

**Obiettivo verificabile**

Introdurre la pianificazione dei pasti su calendario (giornaliero/settimanale) con persistenza locale in SQLite.

**In scope**

- definire modello dati minimo (giorno -> ricetta selezionata);
- repository SQLite dedicato al calendario;
- schermata calendario e relativo ViewModel con stati UI espliciti;
- selezione ricetta dal catalogo gia' cercato (TBD nel dettaglio di iterazione).

**Out of scope**

- sincronizzazione cloud;
- notifiche push;
- funzionalita' nutrizionali.

**File o aree probabili**

- `src/MealWise.Mobile/Views/`
- `src/MealWise.Mobile/ViewModels/`
- `src/MealWise.Mobile/Services/`
- `src/MealWise.Mobile/Models/`

**Dipendenze**

- completamento di IT-04.

**Criteri di accettazione**

- [ ] L'utente puo' associare una ricetta a un giorno.
- [ ] Dopo riavvio app, le scelte restano persistenti.

**Verifiche principali**

- manuale: assegnazione ricetta a giorno, riavvio app, modifica e rimozione;
- automatico: `dotnet build src/MealWise.Mobile/MealWise.Mobile.csproj` ed eventuali test repository.

**Rischi**

- schema dati incompleto per le estensioni successive;
- UX troppo complessa se accoppiata troppo presto alla ricerca remota.

### IT-06 (post-MVP) - Lista spesa locale derivata dal calendario

**Obiettivo verificabile**

Derivare e gestire una lista spesa locale a partire dal calendario pasti, con persistenza e modifiche manuali.

**In scope**

- aggregare ingredienti dalle ricette pianificate;
- permettere modifiche manuali (aggiungi/rimuovi/spunta) e persistenza;
- schermata lista spesa con stati UI espliciti;
- verifiche manuali e automatiche minime.

**Out of scope**

- barcode scanner;
- notifiche push;
- sincronizzazione cloud.

**File o aree probabili**

- `src/MealWise.Mobile/Views/`
- `src/MealWise.Mobile/ViewModels/`
- `src/MealWise.Mobile/Services/`
- `docs/test-matrix.md`
- `docs/iterations/` per i log applicativi.

**Dipendenze**

- completamento funzionale di IT-05.

**Criteri di accettazione**

- [ ] La lista spesa riflette le ricette pianificate ed e' modificabile.
- [ ] Dopo riavvio app, la lista resta coerente.

**Verifiche principali**

- manuale: generazione lista spesa e modifiche; riavvio app;
- automatico: `dotnet build src/MealWise.Mobile/MealWise.Mobile.csproj` ed eventuale `dotnet test`.

**Rischi**

- logica di aggregazione ingredienti non deterministica;
- gestione misure unita' non coerente (dipende dai dati del provider).

## 4. Roadmap post-MVP

Le estensioni oltre il MVP devono mantenere l'ordine deciso in `docs/spec.md`:

1. calendario pasti locale;
2. lista spesa locale;
3. ricette salvate/caching locale;
4. filtri extra e miglioramenti UX.

Queste fasi non devono essere assorbite dentro le iterazioni MVP sopra elencate.

# Piano di Progetto

## 1. Sintesi operativa

### Obiettivo del progetto

Realizzare `BookScout Mobile` come app `.NET MAUI` Android-first per ricerca libri, dettaglio completo, preferiti locali e cronologia locale, mantenendo MVVM, Shell navigation e una separazione chiara tra Views, ViewModels, servizi remoti e persistenza locale.

Il repository contiene attualmente la documentazione di progetto e una cartella `src/` vuota. La prima iterazione dovrà quindi bootstrapare il progetto MAUI reale in `src/BookScout.Mobile/` prima di affrontare le feature applicative.

### Vincoli principali

- `.NET MAUI` con target principale Android; iOS opzionale e secondario.
- Architettura MVVM con `CommunityToolkit.Mvvm`.
- Navigazione basata su Shell.
- `HttpClient` asincrono e `System.Text.Json`.
- Google Books API come provider base dell'MVP.
- Search e Results in una singola schermata.
- Stati UI espliciti: loading, error, empty, success.
- Persistenza locale con SQLite per favoriti e cronologia.
- Nessuna autenticazione, sincronizzazione cloud o funzionalità social nel v1.

### Dipendenze esterne

- Google Books API per ricerca e dettaglio.
- Connettività Internet per i flussi remoti del MVP.
- Ambiente MAUI Android funzionante su macchina di sviluppo.
- Layer SQLite locale per favoriti e cronologia.

## 2. Sequenza delle iterazioni

| Iterazione | Obiettivo verificabile | Dipendenze | Rischio | Stato |
| --- | --- | --- | --- | --- |
| IT-01 | Bootstrap del progetto MAUI e Shell di base | Nessuna | medio | pianificata |
| IT-02 | Ricerca libri con Google Books e risultati nella pagina Search | IT-01 | medio | pianificata |
| IT-03 | Navigazione al dettaglio e rendering dati estesi del libro | IT-02 | medio | pianificata |
| IT-04 | Favoriti locali persistiti con apertura immediata da cache e refresh remoto in background | IT-03 | medio-alto | pianificata |
| IT-05 | Cronologia locale con deduplica, replay e cancellazione totale o selettiva | IT-02, IT-04 | medio | pianificata |
| IT-06 | Hardening MVP, smoke regression e chiusura della baseline funzionale | IT-01, IT-02, IT-03, IT-04, IT-05 | medio | pianificata |

## 3. Dettaglio iterazioni

### IT-01 - Bootstrap progetto MAUI e Shell di base

**Obiettivo verificabile**

Creare il progetto MAUI in `src/BookScout.Mobile/`, avviarlo su Android e rendere raggiungibili le viste principali `Search`, `Favorites` e `History` tramite Shell, senza implementare ancora la logica applicativa del dominio.

**In scope**

- creare il progetto `.NET MAUI` in `src/BookScout.Mobile/`;
- impostare `App`, `AppShell` e `MauiProgram`;
- predisporre cartelle `Models`, `Services`, `ViewModels`, `Views`, `Resources`;
- creare le pagine iniziali `Search`, `Favorites` e `History` come placeholder MVVM-safe;
- impostare dependency injection minima per servizi e ViewModels futuri.

**Out of scope**

- integrazione con Google Books API;
- pagina dettaglio libro completa;
- persistenza locale di favoriti o cronologia;
- gestione reale di ricerca, dettaglio, refresh o replay.

**File o aree probabili**

- `src/BookScout.Mobile/BookScout.Mobile.csproj`
- `src/BookScout.Mobile/App.xaml`
- `src/BookScout.Mobile/App.xaml.cs`
- `src/BookScout.Mobile/AppShell.xaml`
- `src/BookScout.Mobile/AppShell.xaml.cs`
- `src/BookScout.Mobile/MauiProgram.cs`
- `src/BookScout.Mobile/Views/`
- `src/BookScout.Mobile/ViewModels/`

**Dipendenze**

- `docs/spec.md` approvato;
- workload MAUI funzionante sull'ambiente locale.

**Criteri di accettazione**

- [ ] L'app compila e si avvia su Android.
- [ ] Le sezioni `Search`, `Favorites` e `History` sono raggiungibili da Shell.
- [ ] Le pagine create usano bindings e ViewModels, senza business logic nei code-behind.
- [ ] La struttura delle cartelle del progetto è coerente con `docs/architecture.md`.

**Verifiche principali**

- manuale: avvio app e navigazione tra le tre sezioni principali;
- automatico: `dotnet build src/BookScout.Mobile/BookScout.Mobile.csproj`.

**Rischi**

- introdurre logica di feature dentro il bootstrap invece di mantenere uno scheletro minimo;
- creare una struttura iniziale incoerente che obblighi refactor precoci nelle iterazioni successive.

### IT-02 - Ricerca libri e risultati in pagina unica

**Obiettivo verificabile**

Consentire all'utente di cercare libri per titolo o autore dalla schermata `Search`, visualizzando risultati con copertina, titolo e autore nella stessa pagina, con gestione esplicita di loading, empty ed error state.

**In scope**

- integrare Google Books API per `GET /volumes?q={query}`;
- introdurre servizio remoto per la ricerca libri;
- modellare DTO e mapping difensivo verso modelli di dominio per la lista risultati;
- implementare `SearchViewModel` con query, comando di ricerca e stati UI;
- implementare `SearchPage` con `SearchBar`, `CollectionView`, placeholder copertina e retry.

**Out of scope**

- apertura del dettaglio completo del libro;
- salvataggio dei favoriti;
- cronologia locale delle ricerche;
- supporto multi-provider oltre a Google Books.

**File o aree probabili**

- `src/BookScout.Mobile/Views/SearchPage.xaml`
- `src/BookScout.Mobile/ViewModels/SearchViewModel.cs`
- `src/BookScout.Mobile/Services/`
- `src/BookScout.Mobile/Models/`
- `src/BookScout.Mobile/Resources/`

**Dipendenze**

- completamento di IT-01;
- connettività Internet e disponibilità Google Books API.

**Criteri di accettazione**

- [ ] Inserendo una query valida, la pagina Search avvia la richiesta remota e mostra subito il loading state.
- [ ] In caso di successo, la lista mostra almeno copertina, titolo e autore per ogni libro.
- [ ] In caso di risposta vuota, la UI mostra uno stato empty esplicito.
- [ ] In caso di errore o timeout, la UI mostra un messaggio comprensibile e una azione di retry.
- [ ] L'assenza della copertina o di altri campi opzionali non genera crash della UI.

**Verifiche principali**

- manuale: ricerche con query valida, query senza risultati e simulazione di errore rete;
- automatico: `dotnet build src/BookScout.Mobile/BookScout.Mobile.csproj` e controlli unitari del mapping/service layer se il progetto test viene introdotto nello stesso slice.

**Rischi**

- risposte JSON parziali o inconsistenti da Google Books;
- immagini remote lente o assenti che peggiorano la leggibilità della lista.

### IT-03 - Navigazione al dettaglio e dati estesi del libro

**Obiettivo verificabile**

Permettere l'apertura del dettaglio di un libro dalla lista risultati, caricando i dati estesi da Google Books e gestendo correttamente campi mancanti, loading e retry.

**In scope**

- registrare la route Shell per il dettaglio;
- passare il parametro `bookId` dalla lista risultati al dettaglio;
- integrare `GET /volumes/{id}`;
- implementare `BookDetailViewModel` e `BookDetailPage`;
- mostrare descrizione, editore, data pubblicazione, pagine, categorie e ISBN quando disponibili.

**Out of scope**

- salvataggio persistente nei favoriti;
- apertura da Favorites con dati locali;
- cronologia ricerche;
- supporto offline esteso.

**File o aree probabili**

- `src/BookScout.Mobile/AppShell.xaml`
- `src/BookScout.Mobile/AppShell.xaml.cs`
- `src/BookScout.Mobile/Views/BookDetailPage.xaml`
- `src/BookScout.Mobile/ViewModels/BookDetailViewModel.cs`
- `src/BookScout.Mobile/Services/`
- `src/BookScout.Mobile/Models/`

**Dipendenze**

- completamento di IT-02;
- disponibilità dell'ID volume proveniente dalla ricerca.

**Criteri di accettazione**

- [ ] Toccando un risultato, l'utente raggiunge la pagina di dettaglio corretta.
- [ ] Il dettaglio mostra i campi estesi previsti quando presenti.
- [ ] In presenza di campi mancanti o null, la pagina resta leggibile e non mostra errori tecnici.
- [ ] Il dettaglio mostra loading ed error state con azione di retry.

**Verifiche principali**

- manuale: apertura dettaglio da più risultati, inclusi libri con metadati incompleti;
- automatico: `dotnet build src/BookScout.Mobile/BookScout.Mobile.csproj` e controlli unitari del mapping del dettaglio se introdotti.

**Rischi**

- propagazione errata dei parametri Shell;
- differenze tra dati disponibili in lista e dettaglio che impattano il rendering.

### IT-04 - Favoriti locali persistiti e refresh remoto non bloccante

**Obiettivo verificabile**

Permettere di salvare o rimuovere libri dai favoriti, mantenerli dopo la riapertura dell'app e aprirli dalla schermata `Favorites` mostrando subito i dati locali, con eventuale refresh remoto silenzioso in background.

**In scope**

- introdurre persistenza SQLite per i favoriti;
- salvare una snapshot locale completa dei dati principali del dettaglio;
- aggiungere toggle add/remove favoriti dalla pagina dettaglio;
- implementare `FavoritesPage` e relativo ViewModel;
- aprire un libro da Favorites caricando prima i dati locali e poi, se disponibile rete, tentare refresh remoto non bloccante;
- mostrare solo feedback di errore non invasivo quando il refresh fallisce.

**Out of scope**

- cronologia ricerche;
- note personali;
- provider alternativi;
- modalità offline estesa oltre alla consultazione dei favoriti salvati.

**File o aree probabili**

- `src/BookScout.Mobile/Views/FavoritesPage.xaml`
- `src/BookScout.Mobile/ViewModels/FavoritesViewModel.cs`
- `src/BookScout.Mobile/ViewModels/BookDetailViewModel.cs`
- `src/BookScout.Mobile/Services/`
- `src/BookScout.Mobile/Models/`

**Dipendenze**

- completamento di IT-03;
- disponibilità del layer SQLite locale.

**Criteri di accettazione**

- [ ] Un libro può essere salvato e rimosso dai favoriti dal dettaglio.
- [ ] I favoriti restano disponibili dopo chiusura e riapertura dell'app.
- [ ] Aprendo un libro da Favorites, il dettaglio mostra subito la snapshot locale senza attendere la rete.
- [ ] Se il refresh remoto fallisce, il contenuto locale resta visibile e il feedback di errore è non invasivo e non bloccante.
- [ ] Se il refresh remoto riesce, il contenuto può aggiornarsi senza conferma esplicita dell'utente.

**Verifiche principali**

- manuale: salvataggio, riapertura app, apertura da Favorites online e offline, rimozione dal dettaglio e dalla lista;
- automatico: `dotnet build src/BookScout.Mobile/BookScout.Mobile.csproj` e test di repository/ViewModel se il layer locale viene reso testabile.

**Rischi**

- schema locale troppo povero o troppo ampio rispetto ai dati necessari;
- divergenza temporanea tra snapshot locale e dati remoti aggiornati.

### IT-05 - Cronologia locale con deduplica, replay e pulizia

**Obiettivo verificabile**

Registrare le ricerche effettuate, mostrarle in `History` in ordine temporale decrescente, consentire replay della query e cancellazione sia totale sia di singole voci.

**In scope**

- persistenza locale della cronologia ricerche;
- deduplica delle query ripetute, riportando in cima la più recente;
- implementazione di `HistoryPage` e relativo ViewModel;
- replay della ricerca tramite navigazione verso `Search` con query precompilata o rieseguita;
- cancellazione totale della cronologia e rimozione selettiva di singole voci.

**Out of scope**

- suggerimenti intelligenti o analytics;
- sincronizzazione cloud;
- ordinamenti avanzati o filtri;
- estensioni offline oltre al comportamento già previsto nel MVP.

**File o aree probabili**

- `src/BookScout.Mobile/Views/HistoryPage.xaml`
- `src/BookScout.Mobile/ViewModels/HistoryViewModel.cs`
- `src/BookScout.Mobile/ViewModels/SearchViewModel.cs`
- `src/BookScout.Mobile/AppShell.xaml`
- `src/BookScout.Mobile/Services/`
- `src/BookScout.Mobile/Models/`

**Dipendenze**

- completamento di IT-02;
- pattern di persistenza locale consolidato in IT-04.

**Criteri di accettazione**

- [ ] Ogni ricerca completata aggiorna la cronologia locale.
- [ ] Una query ripetuta non genera duplicato ma torna in cima all'elenco.
- [ ] Toccando una voce della cronologia, la ricerca viene rilanciata nella pagina Search.
- [ ] L'utente può cancellare tutta la cronologia o una singola voce.
- [ ] Dopo riavvio app, la cronologia resta coerente con lo stato salvato.

**Verifiche principali**

- manuale: più ricerche consecutive, query duplicate, replay, delete singolo, clear totale, riavvio app;
- automatico: `dotnet build src/BookScout.Mobile/BookScout.Mobile.csproj` e test di persistenza/normalizzazione query se introdotti.

**Rischi**

- accoppiamento eccessivo tra `HistoryPage` e ciclo di vita della `SearchPage`;
- deduplica incoerente in presenza di differenze di maiuscole, spazi o caratteri speciali.

### IT-06 - Hardening MVP e baseline di regressione

**Obiettivo verificabile**

Stabilizzare il MVP, chiudere i principali gap trasversali di UI state e completare una baseline di verifiche manuali e automatiche leggere prima di passare al post-MVP.

**In scope**

- rifinire testi, placeholder, retry e feedback di errore sulle quattro schermate principali;
- verificare coerenza dei flussi `Search -> Detail -> Favorites -> History`;
- correggere eventuali difetti emersi dalle verifiche dell'MVP;
- eseguire smoke manuale Android e verifiche di build;
- aggiornare evidenze di test e log iterazione nella fase di esecuzione.

**Out of scope**

- filtri, ordinamento, note personali, barcode scanner, offline mode estesa;
- provider alternativi a Google Books;
- UI automation profonda se non richiesta esplicitamente.

**File o aree probabili**

- `src/BookScout.Mobile/Views/`
- `src/BookScout.Mobile/ViewModels/`
- `src/BookScout.Mobile/Services/`
- `docs/test-matrix.md`
- `docs/iterations/` per i log applicativi dell'MVP; non usare `docs/skill-iterations/`, che resta archivio storico delle iterazioni sulle skill.

**Dipendenze**

- completamento funzionale di IT-01, IT-02, IT-03, IT-04 e IT-05.

**Criteri di accettazione**

- [ ] Tutti i flussi MVP risultano eseguibili senza blocchi funzionali maggiori.
- [ ] Search, Detail, Favorites e History espongono stati UI coerenti con lo spec.
- [ ] La build Android del progetto riesce stabilmente.
- [ ] Le verifiche manuali prioritarie del MVP sono pronte per essere registrate nel test matrix esecutivo.

**Verifiche principali**

- manuale: smoke completo end-to-end su Android/emulatore;
- automatico: `dotnet build src/BookScout.Mobile/BookScout.Mobile.csproj` ed eventuale `dotnet test` se il progetto test è stato introdotto.

**Rischi**

- usare questa iterazione come contenitore di refactor larghi non pianificati;
- rinviare problemi di stato UI che dovrebbero essere chiusi nelle iterazioni precedenti.

## 4. Roadmap post-MVP

Le estensioni oltre il MVP devono mantenere l'ordine deciso in `docs/spec.md`:

1. filtri e ordinamento;
2. note personali;
3. barcode scanner ISBN;
4. offline mode estesa.

Queste fasi non devono essere assorbite dentro le iterazioni MVP sopra elencate.

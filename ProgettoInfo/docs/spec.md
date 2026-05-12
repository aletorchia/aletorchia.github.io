# Specifica di Progetto - MealWise Mobile

## 1. Visione e contesto

### Problema da risolvere

Organizzare l'alimentazione quotidiana richiede di trovare ricette velocemente, scegliere cosa cucinare e trasformare le scelte in azioni pratiche (es. lista della spesa). Molte soluzioni impongono account e cloud, oppure rendono scomoda la ricerca per ingredienti/categorie e la gestione locale.

### Obiettivo del progetto

Realizzare una applicazione `.NET MAUI` cross-platform (Android-first, iOS opzionale) per semplificare la gestione dell'alimentazione quotidiana.

Nel MVP l'app consente di cercare ricette tramite API esterna con query asincrone filtrate per ingrediente principale o categoria, visualizzare risultati chiari e aprire il dettaglio di una ricetta con gestione robusta di loading/error/empty state.

### Utenti target

- studenti o lavoratori che vogliono trovare idee per i pasti in poco tempo;
- persone che cucinano a casa e vogliono ricette filtrabili per ingrediente/categoria;
- utenti che preferiscono una gestione locale (senza account) per planning e spesa in una fase successiva.

## 2. Ambito MVP

### Flusso principale da supportare

1. L'utente apre l'app e sceglie un filtro di ricerca: ingrediente principale o categoria.
2. L'utente inserisce il valore (es. `chicken`) o seleziona una categoria.
3. L'app esegue una query asincrona verso l'API esterna e mostra una lista di ricette.
4. L'utente apre il dettaglio di una ricetta e consulta ingredienti e istruzioni.

### Funzionalità obbligatorie

- ricerca ricette tramite API remota usando query asincrone filtrate per ingrediente principale o categoria;
- pagina Search unica che integri filtri/input e lista risultati nello stesso flusso;
- elenco risultati con immagine (se disponibile) e nome ricetta;
- pagina dettaglio ricetta con ingredienti, misure e istruzioni (quando disponibili);
- gestione esplicita di loading state, error state, empty state e success state per i flussi di ricerca e dettaglio.

### Funzionalità opzionali future

- fase post-MVP 1: calendario pasti (giornaliero/settimanale) con pianificazione locale;
- fase post-MVP 2: lista della spesa locale generata dal calendario e modificabile manualmente;
- fase post-MVP 3: ricette salvate in locale (preferiti) e cache di dettaglio per consultazione più rapida;
- fase post-MVP 4: filtri aggiuntivi e miglioramenti UX (es. recenti, suggerimenti non intelligenti, ordinamenti semplici);
- provider alternativi se necessario (TBD).

### Priorità roadmap post-MVP

Il planning successivo dovrà mantenere questo ordine di priorità:

1. calendario pasti locale;
2. lista spesa locale;
3. ricette salvate/caching locale;
4. filtri extra e miglioramenti UX.

### Non-obiettivi

- notifiche push nel v1;
- barcode scanner prodotti nel v1;
- autenticazione utente o sincronizzazione cloud nel v1;
- calcolo nutrizionale avanzato (macro/calorie) nel v1 (TBD se richiesto in futuro);
- supporto offline completo per ricerche remote nel v1;
- parità funzionale iOS rispetto ad Android nel primo rilascio.

## 3. Scenari d'uso principali

### Scenario 1 - Ricerca veloce per ingrediente

Un utente cerca ricette con ingrediente principale `chicken`, ottiene una lista di risultati e apre il dettaglio di una ricetta per vedere ingredienti e istruzioni.

### Scenario 2 - Ricerca per categoria

Un utente seleziona una categoria (es. `Seafood`), sfoglia la lista e apre il dettaglio per decidere cosa cucinare.

### Scenario 3 - Gestione errori di rete

Un utente è in condizioni di rete instabili, avvia una ricerca e in caso di errore vede un messaggio comprensibile con azione di retry.

### Scenario 4 - (Post-MVP) Pianificazione e spesa

Un utente pianifica i pasti della settimana nel calendario e l'app genera una lista della spesa locale, modificabile e persistente.

## 4. Requisiti funzionali

- FR-01: l'app deve consentire la ricerca ricette filtrando per ingrediente principale o per categoria.
- FR-02: la ricerca deve usare TheMealDB come provider base dell'MVP:
  - ricerca per ingrediente: `GET /api/json/v1/1/filter.php?i={ingredient}`;
  - ricerca per categoria: `GET /api/json/v1/1/filter.php?c={category}`.
- FR-03: selezionando un risultato, l'utente deve poter aprire una pagina dettaglio ricetta usando `GET /api/json/v1/1/lookup.php?i={mealId}`.
- FR-04: l'app deve mostrare ogni risultato con almeno immagine (se disponibile) e nome ricetta; in assenza immagine deve mostrare un placeholder o uno stato equivalente.
- FR-05: la schermata dettaglio deve mostrare ingredienti, misure e istruzioni quando disponibili.
- FR-06: l'app deve tollerare risposte API parziali o campi mancanti senza generare errori bloccanti di UI.
- FR-07: le schermate che caricano dati remoti devono sempre esporre uno stato tra loading, success, empty o error.
- FR-08: l'app deve gestire errori di rete o indisponibilità del servizio con messaggi comprensibili e possibilità di retry.

### Requisiti funzionali post-MVP

- FR-09: l'app dovra' consentire la pianificazione locale dei pasti associando una ricetta a un giorno.
- FR-10: l'app dovra' generare una lista spesa locale a partire dalle ricette pianificate, mantenendola modificabile manualmente.
- FR-11: l'app dovra' consentire il salvataggio locale di ricette dal dettaglio, con consultazione successiva dei dati principali anche senza rete.
- FR-12: i dati post-MVP di calendario, lista spesa e ricette salvate dovranno persistere sul dispositivo senza richiedere account o sincronizzazione cloud.

## 5. Epic, user stories e criteri di accettazione

### EPIC-01 - Ricerca ricette e lista risultati

**Obiettivo:**

Consentire all'utente di trovare rapidamente ricette filtrando per ingrediente principale o categoria e consultare un elenco chiaro di risultati.

**User stories:**

- Come utente, voglio cercare ricette per ingrediente principale così da trovare idee compatibili con ciò che ho.
- Come utente, voglio cercare ricette per categoria così da esplorare un tipo di cucina.
- Come utente, voglio feedback chiari durante il caricamento o in caso di errore così da capire cosa sta succedendo.

**Criteri di accettazione:**

- [ ] Selezionando ingrediente o categoria e avviando la ricerca, l'app mostra subito uno stato di caricamento.
- [ ] Se la ricerca restituisce risultati, la UI mostra una lista con almeno immagine (se disponibile) e nome ricetta per ogni elemento.
- [ ] Se la ricerca non restituisce risultati, la UI mostra uno stato empty esplicito invece di una pagina vuota.
- [ ] Se la richiesta fallisce, la UI mostra un messaggio di errore e una azione di retry.

### EPIC-02 - Dettaglio ricetta

**Obiettivo:**

Permettere all'utente di consultare ingredienti e istruzioni della ricetta selezionata.

**User stories:**

- Come utente, voglio aprire il dettaglio di una ricetta così da vedere ingredienti e istruzioni.
- Come utente, voglio che i campi mancanti siano gestiti bene così da non trovare schermate rotte o incoerenti.

**Criteri di accettazione:**

- [ ] Toccando un risultato, l'utente raggiunge la schermata Detail della ricetta selezionata.
- [ ] La schermata mostra ingredienti e istruzioni quando disponibili.
- [ ] Se alcuni campi non sono presenti nella risposta API, la schermata resta leggibile e non mostra errori tecnici.

### EPIC-03 - (Post-MVP) Calendario pasti e lista spesa

**Obiettivo:**

Consentire pianificazione locale dei pasti e organizzare la spesa in modo efficiente e locale.

**User stories:**

- Come utente, voglio pianificare un pasto per giorno così da organizzare la settimana.
- Come utente, voglio una lista spesa locale derivata dai pasti così da comprare piu' velocemente.

**Criteri di accettazione (post-MVP):**

- [ ] Il calendario consente di associare una ricetta a un giorno.
- [ ] La lista spesa aggrega ingredienti e quantita', e resta modificabile manualmente.
- [ ] Tutti i dati sono salvati localmente in SQLite.

### EPIC-04 - (Post-MVP) Ricette salvate in locale

**Obiettivo:**

Consentire all'utente di salvare alcune ricette in locale per ritrovarle rapidamente e migliorare la continuita' d'uso.

**User stories:**

- Come utente, voglio salvare una ricetta tra i preferiti cosi' da ritrovarla facilmente.
- Come utente, voglio riaprire una ricetta salvata anche senza rete cosi' da consultare ingredienti e istruzioni gia' viste.

**Criteri di accettazione (post-MVP):**

- [ ] Salvando una ricetta dal dettaglio, la ricetta compare in una sezione dedicata.
- [ ] Chiudendo e riaprendo l'app, le ricette salvate restano disponibili.
- [ ] Aprendo una ricetta salvata, i dati principali sono consultabili senza dipendere dalla rete.

## 6. Requisiti non funzionali

### UX e stati UI

- L'utente deve poter raggiungere la ricerca e il primo elenco risultati con un flusso semplice e senza passaggi ridondanti.
- Nel MVP la ricerca e la lista risultati condividono la stessa schermata per ridurre passaggi e cambi di contesto.
- Le schermate Search e Detail non devono mai restare in uno stato ambiguo: devono mostrare chiaramente loading, empty, error o success.
- Le immagini remote non devono compromettere la leggibilità del layout quando sono assenti, lente o non disponibili.

### Prestazioni percepite

- Dopo l'avvio di una ricerca, la UI deve reagire immediatamente mostrando un indicatore di caricamento o uno stato equivalente.
- In condizioni di rete normali, i risultati della prima richiesta dovrebbero diventare consultabili in pochi secondi.
- Lo scrolling della lista risultati deve restare fluido su dispositivi Android di fascia media usati in contesto didattico.

### Affidabilità e gestione errori

- Le eccezioni di rete o parsing non devono emergere come crash del flusso principale.
- I messaggi di errore devono essere comprensibili e orientati all'azione, ad esempio riprovare la ricerca.
- Le risposte JSON con campi mancanti o opzionali devono essere gestite in modo difensivo.

### Privacy e dati

- Nel MVP non è prevista autenticazione né invio di dati personali a backend proprietari.
- Nel post-MVP, calendario, lista spesa e ricette salvate devono essere memorizzati localmente sul dispositivo.

## 7. Vincoli tecnici di progetto

- App `.NET MAUI` con target principale Android; iOS opzionale e secondario.
- Architettura MVVM.
- Navigazione basata su Shell.
- `CommunityToolkit.Mvvm` per la gestione dei ViewModel.
- `HttpClient` asincrono per le chiamate remote.
- `System.Text.Json` per il parsing delle risposte API.
- Persistenza locale con SQLite per calendario, lista spesa e ricette salvate (post-MVP); `Preferences` solo per esigenze leggere eventuali.
- Gestione esplicita di `IsBusy`, error state, empty state e presenza dati nei ViewModel.
- Nessuna logica REST o business logic nei code-behind.
- Search e risultati nello stesso flusso di pagina; Detail come vista dedicata nel MVP.
- La UI deve essere pensata per `SearchBar`/input equivalente, `CollectionView`, `ScrollView`, `Grid`, `Image`, `Label` e gesture semplici, coerenti con le schermate previste.

## 8. Metriche di successo

- Un utente alla prima apertura riesce a eseguire il flusso `ricerca -> risultati -> dettaglio` senza spiegazioni aggiuntive e con al massimo 3 interazioni principali dopo il launch.
- I flussi MVP `ricerca`, `apertura dettaglio` e `retry in caso di errore` risultano completabili senza crash in una sessione manuale di prova.

## 9. Rischi, dipendenze e questioni aperte

### Rischi

- I dati restituiti da TheMealDB possono essere incompleti o incoerenti tra ricette diverse.
- Le immagini remote possono essere assenti, lente o di qualità variabile.
- La forma dei campi ingredienti/misure nel dettaglio richiede parsing difensivo.
- L'espansione verso calendario, lista spesa e ricette salvate introduce complessità di modello dati e persistenza.

### Dipendenze

- Disponibilità di TheMealDB e connettività Internet per ricerca e dettaglio nel MVP.
- (Post-MVP) Disponibilità di un layer di persistenza SQLite affidabile per dati locali (calendario, lista spesa e ricette salvate).

### Questioni aperte o `TBD`

- Naming definitivo dell'app (il documento usa "MealWise Mobile" come placeholder).
- Set di categorie supportate: lista fissa dal provider o caricamento dinamico (TBD).
- Gestione localizzazione lingua/copy dei testi istruzioni (dipende dal provider).

## 10. Passaggio al planning

Il prossimo passo è derivare i documenti di planning a partire da questo spec:

- `docs/plan.md`
- `docs/architecture.md`
- `docs/test-matrix.md`

La skill consigliata per il passo successivo è `prd-to-plan`.

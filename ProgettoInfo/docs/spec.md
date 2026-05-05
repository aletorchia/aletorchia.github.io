# Specifica di Progetto - BookScout Mobile

## 1. Visione e contesto

### Problema da risolvere

Chi cerca un libro da mobile spesso deve passare tra risultati poco leggibili, schede incomplete e assenza di continuità tra una ricerca e la successiva. Per un progetto didattico MAUI serve invece una app informativa con navigazione chiara, dati remoti affidabili e persistenza locale essenziale.

### Obiettivo del progetto

Realizzare una applicazione `.NET MAUI` Android-first per cercare libri per titolo o autore, consultare un dettaglio completo, salvare preferiti in locale e mantenere una cronologia delle ricerche, con particolare attenzione a qualità della navigazione, chiarezza degli stati UI e separazione pulita tra ViewModel e Service.

### Utenti target

- lettori occasionali che vogliono consultare rapidamente informazioni su un libro;
- studenti che cercano libri per autore, titolo o categoria editoriale;
- appassionati di lettura che desiderano tenere un elenco locale di libri di interesse.

## 2. Ambito MVP

### Flusso principale da supportare

1. L'utente apre l'app e inserisce una ricerca per titolo o autore.
2. L'app mostra un elenco di risultati con copertina, titolo e autore.
3. L'utente apre il dettaglio di un libro e consulta le informazioni estese.
4. L'utente salva il libro tra i preferiti locali.
5. L'utente può rivedere preferiti e cronologia ricerche nelle schermate dedicate.

### Funzionalità obbligatorie

- ricerca libri per titolo o autore tramite API remota;
- pagina Search unica che integri input di ricerca e lista risultati nello stesso flusso;
- elenco risultati con copertina, titolo e autore;
- pagina dettaglio con descrizione, editore, anno, pagine, categorie e ISBN quando disponibili;
- salvataggio e rimozione dei preferiti in locale;
- cronologia locale delle ricerche effettuate;
- gestione esplicita di loading state, error state, empty state e success state per i flussi di ricerca e dettaglio.

### Funzionalità opzionali future

- fase post-MVP 1: filtri per lingua o categoria e ordinamento per rilevanza, data o autore;
- fase post-MVP 2: note personali associate ai libri salvati;
- fase post-MVP 3: scansione ISBN tramite fotocamera;
- fase post-MVP 4: modalità offline estesa per libri già memorizzati localmente;
- supporto a provider alternativi come Open Library in una fase successiva, se necessario.

### Priorità roadmap post-MVP

Il planning successivo dovrà mantenere questo ordine di priorità:

1. filtri e ordinamento;
2. note personali;
3. barcode scanner ISBN;
4. offline mode estesa.

### Non-obiettivi

- acquisto o prenotazione dei libri;
- autenticazione utente o sincronizzazione cloud;
- recensioni sociali, rating o commenti condivisi;
- modifica dei dati editoriali remoti;
- supporto offline completo per ricerche remote nel primo rilascio.
- parità funzionale iOS rispetto ad Android nel primo rilascio.

## 3. Scenari d'uso principali

### Scenario 1 - Ricerca veloce di un libro

Un utente cerca `Pride and Prejudice`, ottiene una lista di risultati con copertine e autori, identifica rapidamente il libro corretto e apre il dettaglio per leggerne descrizione e dati editoriali.

### Scenario 2 - Consultazione approfondita

Uno studente apre il dettaglio di un libro trovato via ricerca, controlla editore, anno, numero di pagine, categorie e ISBN, poi decide se salvarlo tra i preferiti per ritrovarlo in seguito.

### Scenario 3 - Ripresa di una ricerca recente

Un utente torna sull'app in un secondo momento, apre la schermata History, tocca una ricerca recente e rilancia la consultazione senza dover reinserire manualmente il testo.

### Scenario 4 - Revisione dei libri salvati

Un utente apre la schermata Favorites per rivedere i libri già salvati localmente, consultare di nuovo i dettagli e rimuovere quelli non più interessanti.

## 4. Requisiti funzionali

- FR-01: l'app deve consentire la ricerca di libri tramite testo libero per titolo o autore.
- FR-02: la ricerca deve usare Google Books API come provider base dell'MVP, in particolare `GET /volumes?q={query}` per i risultati e `GET /volumes/{id}` per il dettaglio.
- FR-03: l'app deve mostrare ogni risultato con almeno copertina, titolo e autore; in assenza della copertina deve mostrare un placeholder o uno stato visivo equivalente.
- FR-04: selezionando un risultato, l'utente deve poter aprire una pagina dettaglio con informazioni estese disponibili: descrizione, editore, anno o data pubblicazione, numero di pagine, categorie e ISBN.
- FR-05: l'app deve tollerare risposte API parziali o campi mancanti senza generare errori bloccanti di UI.
- FR-06: l'utente deve poter aggiungere o rimuovere un libro dai preferiti locali.
- FR-07: i preferiti devono restare disponibili dopo la chiusura e riapertura dell'app e devono includere una copia locale completa dei principali dati di dettaglio disponibili al momento del salvataggio.
- FR-08: aprendo un libro dai Favorites, l'app deve mostrare subito i dati locali salvati e, se la rete è disponibile, tentare un refresh remoto in background senza bloccare la consultazione iniziale; se l'aggiornamento riesce, i dati possono aggiornarsi in modo silenzioso, mentre se fallisce deve essere mostrato un messaggio non bloccante.
- FR-09: l'app deve salvare localmente la cronologia delle ricerche effettuate in forma deduplicata e mostrarla in una schermata dedicata.
- FR-10: dalla cronologia l'utente deve poter rilanciare una ricerca con un tap, mantenendo l'ordinamento dal più recente al meno recente.
- FR-11: l'utente deve poter cancellare manualmente l'intera cronologia locale e rimuovere singole voci.
- FR-12: le schermate che caricano dati remoti devono sempre esporre uno stato tra loading, success, empty o error.
- FR-13: la navigazione tra Search con risultati integrati, Detail, Favorites e History deve risultare coerente con Shell navigation.
- FR-14: l'app deve gestire errori di rete o indisponibilità del servizio con messaggi comprensibili e possibilità di retry.

## 5. Epic, user stories e criteri di accettazione

### EPIC-01 - Ricerca e lista risultati

**Obiettivo:**

Consentire all'utente di trovare rapidamente libri tramite una query testuale e consultare un elenco chiaro di risultati.

**User stories:**

- Come utente, voglio cercare un libro per titolo o autore così da trovare velocemente i contenuti rilevanti.
- Come utente, voglio vedere risultati leggibili con copertina e autore così da distinguere i libri corretti.
- Come utente, voglio feedback chiari durante il caricamento o in caso di errore così da capire cosa sta succedendo.

**Criteri di accettazione:**

- [ ] Inserendo una query valida, l'app avvia una ricerca remota e mostra subito uno stato di caricamento.
- [ ] Se la ricerca restituisce risultati, la UI mostra una lista con almeno copertina, titolo e autore per ogni elemento.
- [ ] Se la ricerca non restituisce risultati, la UI mostra uno stato empty esplicito invece di una pagina vuota.
- [ ] Se la richiesta fallisce, la UI mostra un messaggio di errore e una azione di retry.

### EPIC-02 - Dettaglio libro

**Obiettivo:**

Permettere all'utente di consultare tutte le informazioni editoriali utili relative a un libro selezionato.

**User stories:**

- Come utente, voglio aprire il dettaglio di un libro così da leggere descrizione e metadati estesi.
- Come utente, voglio che i campi mancanti siano gestiti bene così da non trovare schermate rotte o incoerenti.

**Criteri di accettazione:**

- [ ] Toccando un risultato, l'utente raggiunge la schermata Detail del libro selezionato.
- [ ] La schermata mostra descrizione, editore, data pubblicazione, pagine, categorie e ISBN quando disponibili.
- [ ] Se alcuni campi non sono presenti nella risposta API, la schermata resta leggibile e non mostra errori tecnici.
- [ ] La schermata dettaglio consente di salvare o rimuovere il libro dai preferiti locali.

### EPIC-03 - Preferiti locali

**Obiettivo:**

Consentire all'utente di mantenere una piccola libreria personale locale dei libri di interesse.

**User stories:**

- Come utente, voglio salvare un libro tra i preferiti così da ritrovarlo facilmente.
- Come utente, voglio rivedere i miei preferiti anche dopo aver chiuso l'app così da non perdere il mio elenco.

**Criteri di accettazione:**

- [ ] Salvando un libro dai dettagli, il libro compare nella schermata Favorites.
- [ ] Chiudendo e riaprendo l'app, i preferiti precedentemente salvati restano disponibili.
- [ ] Aprendo un libro dalla schermata Favorites, i dati di dettaglio principali salvati localmente sono consultabili senza dipendere da una nuova chiamata remota.
- [ ] Se la rete è disponibile, l'apertura di un libro dai Favorites può aggiornare in background i dati remoti senza bloccare la visualizzazione iniziale.
- [ ] Se il refresh remoto fallisce, la schermata resta utilizzabile e mostra un feedback di errore non bloccante.
- [ ] Se il refresh remoto riesce, il contenuto può aggiornarsi senza richiedere conferma esplicita all'utente.
- [ ] L'utente può rimuovere un libro dai preferiti senza effetti collaterali sugli altri dati locali.

### EPIC-04 - Cronologia ricerche

**Obiettivo:**

Permettere all'utente di riprendere rapidamente ricerche recenti senza reinserire ogni volta il testo.

**User stories:**

- Come utente, voglio vedere le ricerche recenti così da riprendere velocemente un argomento già consultato.
- Come utente, voglio rilanciare una ricerca dalla cronologia con un solo tap così da velocizzare l'uso ripetuto dell'app.

**Criteri di accettazione:**

- [ ] Ogni ricerca completata aggiorna la cronologia locale.
- [ ] Se una query già presente viene ripetuta, la cronologia non crea un duplicato ma riporta la voce in cima.
- [ ] La schermata History mostra le query recenti in ordine dal più recente al meno recente.
- [ ] Toccando una voce della cronologia, l'app rilancia la stessa ricerca e mostra i risultati aggiornati.
- [ ] L'utente può cancellare tutta la cronologia con una azione esplicita.
- [ ] L'utente può rimuovere una singola voce della cronologia senza svuotare l'intero elenco.

## 6. Requisiti non funzionali

### UX e stati UI

- L'utente deve poter raggiungere la ricerca e il primo elenco risultati con un flusso semplice e senza passaggi ridondanti.
- Nel MVP la ricerca e la lista risultati condividono la stessa schermata per ridurre passaggi e cambi di contesto.
- Le schermate Search, Results e Detail non devono mai restare in uno stato ambiguo: devono mostrare chiaramente loading, empty, error o success.
- Le immagini remote non devono compromettere la leggibilità del layout quando sono assenti, lente o non disponibili.

### Prestazioni percepite

- Dopo l'avvio di una ricerca, la UI deve reagire immediatamente mostrando un indicatore di caricamento o uno stato equivalente.
- In condizioni di rete normali, i risultati della prima pagina dovrebbero diventare consultabili in pochi secondi.
- Lo scrolling della lista risultati deve restare fluido su dispositivi Android di fascia media usati in contesto didattico.

### Affidabilità e gestione errori

- Le eccezioni di rete o parsing non devono emergere come crash del flusso principale.
- I messaggi di errore devono essere comprensibili e orientati all'azione, ad esempio riprovare la ricerca.
- Le risposte JSON con campi mancanti o opzionali devono essere gestite in modo difensivo.
- Il refresh remoto in background dai Favorites non deve bloccare la schermata né degradare la consultazione dei dati già salvati localmente.
- In caso di fallimento del refresh remoto dai Favorites, il feedback utente deve essere visibile ma non invasivo.

### Privacy e dati

- Nel MVP non è prevista autenticazione né invio di dati personali a backend proprietari.
- Preferiti e cronologia sono memorizzati localmente sul dispositivo.
- I preferiti salvano anche i principali dati di dettaglio disponibili al momento del salvataggio, per consentire rilettura locale coerente.
- Eventuali note personali, se introdotte in una fase successiva, devono restare dati locali salvo requisito contrario esplicito.

## 7. Vincoli tecnici di progetto

- App `.NET MAUI` con target principale Android; iOS opzionale e secondario.
- Architettura MVVM.
- Navigazione basata su Shell.
- `CommunityToolkit.Mvvm` per la gestione dei ViewModel.
- `HttpClient` asincrono per le chiamate remote.
- `System.Text.Json` per il parsing delle risposte API.
- Persistenza locale con SQLite per favoriti e cronologia; `Preferences` solo per esigenze leggere eventuali.
- Gestione esplicita di `IsBusy`, error state, empty state e presenza dati nei ViewModel.
- Nessuna logica REST o business logic nei code-behind.
- Search e risultati nello stesso flusso di pagina; Detail, Favorites e History come viste dedicate.
- La UI deve essere pensata per `SearchBar`, `CollectionView`, `ScrollView`, `Grid`, `Image`, `Label`, `SwipeView` e gesture semplici, coerenti con le schermate previste.

## 8. Metriche di successo

- Un utente alla prima apertura riesce a eseguire il flusso `ricerca -> risultati -> dettaglio` senza spiegazioni aggiuntive e con al massimo 3 interazioni principali dopo il launch.
- I flussi MVP `ricerca`, `apertura dettaglio`, `salvataggio preferito` e `riapertura da cronologia` risultano completabili senza crash in una sessione manuale di prova.
- Preferiti e cronologia persistono correttamente dopo il riavvio dell'app in tutti i test manuali del flusso principale.

## 9. Rischi, dipendenze e questioni aperte

### Rischi

- I dati restituiti da Google Books possono essere incompleti o incoerenti tra volumi diversi.
- Le copertine remote possono essere assenti, lente o di qualità variabile.
- I dettagli locali salvati nei preferiti possono diventare non allineati rispetto a eventuali aggiornamenti futuri del provider remoto.
- Il refresh remoto in background dai Favorites può introdurre divergenze temporanee tra dati locali già mostrati e dati remoti aggiornati.
- L'espansione verso barcode scanner e offline mode può introdurre complessità tecnica e nuove dipendenze non desiderate nel MVP.

### Dipendenze

- Disponibilità di Google Books API e connettività Internet per ricerca e dettaglio nel MVP.
- Disponibilità di un layer di persistenza SQLite affidabile per i dati locali.
- Eventuale uso di Open Library come alternativa solo se il provider principale non risultasse adeguato ai requisiti didattici.

### Questioni aperte o `TBD`

Al momento non risultano ulteriori `TBD` di prodotto aperti nello spec. Eventuali dettagli residui appartengono alla fase di planning e architettura.

## 10. Passaggio al planning

Il prossimo passo è derivare i documenti di planning a partire da questo spec:

- `docs/plan.md`
- `docs/architecture.md`
- `docs/test-matrix.md`

La skill consigliata per il passo successivo è `prd-to-plan`.

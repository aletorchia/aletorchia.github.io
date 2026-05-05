# Tutorial - Uso delle skill nel workflow di progetto

## 1. Scopo del documento

Questo tutorial spiega come usare in modo corretto e ripetibile le skill disponibili nel repository durante le diverse fasi di un progetto software.

L'obiettivo didattico non è soltanto completare `BookScout Mobile`, ma insegnare come scegliere la skill giusta nel momento giusto, così da poter riutilizzare lo stesso metodo anche in altri progetti.

Il principio guida è semplice: ogni skill deve essere usata nella fase per cui è stata progettata, evitando di chiedere a una singola skill di fare tutto insieme.

## 2. Mappa delle skill

| Skill | Quando usarla | Cosa produce | Nota pratica |
| --- | --- | --- | --- |
| `maui-prd` | Quando un progetto MAUI deve ancora essere definito o chiarito | `docs/spec.md` | È la skill giusta per visione, MVP, non-obiettivi, requisiti e criteri di accettazione |
| `prd-to-plan` | Quando `docs/spec.md` è abbastanza stabile da guidare il lavoro | `docs/plan.md`, `docs/architecture.md`, `docs/test-matrix.md` | Trasforma lo spec in roadmap, architettura e matrice di verifica |
| `man-in-the-loop-workflow` | All'inizio e alla chiusura di ogni iterazione significativa | piano dell'iterazione, scope chiaro, review e chiusura disciplinata | È la skill che mantiene il lavoro piccolo, verificabile e spiegabile |
| `maui-expert` | Quando l'iterazione tocca codice MAUI, XAML, ViewModel, Shell, binding o servizi applicativi | indicazioni e implementazioni coerenti con MVVM, Shell e CommunityToolkit.Mvvm | È la skill da affiancare quasi sempre alle iterazioni implementative MAUI |
| `maui-automatic-testing` | Quando si vuole progettare o rafforzare i test automatici di una iterazione MAUI | test unitari, test di servizio, strategia di verifica automatica | È particolarmente utile alla fine di una iterazione o prima della chiusura del MVP |
| `find-skills` | Quando non è chiaro se esista già una skill adatta a un nuovo problema | suggerimento di skill esistenti o installabili | È utile soprattutto in progetti diversi da quello corrente |
| `skill-creator` | Quando si vuole creare, correggere o migliorare una skill | nuova skill o skill raffinata | È una skill meta-operativa, non di prodotto |
| `prd` | Quando il progetto non è MAUI oppure quando serve un PRD più generico | PRD generico | In un progetto MAUI è normalmente preferibile `maui-prd` |

## 3. Sequenza consigliata nel ciclo di progetto

### 3.1 Fase di idea e specifica

Quando il progetto non è ancora definito, la scelta corretta è `maui-prd` se il progetto è MAUI, oppure `prd` se il progetto è generico e non dipende da MAUI.

In questa fase l'agente deve chiarire:

1. problema da risolvere;
2. utenti target;
3. MVP;
4. non-obiettivi;
5. vincoli e rischi.

L'output corretto di questa fase è `docs/spec.md`.

### 3.2 Fase di planning

Quando lo spec è approvato, la skill giusta è `prd-to-plan`.

In questa fase l'agente non deve scrivere codice. Deve invece derivare:

1. `docs/plan.md` per la sequenza delle iterazioni;
2. `docs/architecture.md` per le decisioni tecniche principali;
3. `docs/test-matrix.md` per la strategia di verifica.

L'errore tipico da evitare è saltare direttamente dal PRD al codice senza avere iterazioni piccole e verificabili.

### 3.3 Inizio di ogni iterazione implementativa

All'inizio di ogni iterazione significativa conviene usare sempre `man-in-the-loop-workflow`.

Se l'iterazione coinvolge codice MAUI, come accade normalmente in `BookScout Mobile`, a `man-in-the-loop-workflow` va affiancata `maui-expert`.

La regola pratica è questa:

1. iterazione solo documentale: usare la skill documentale corretta, senza `maui-expert`;
2. iterazione implementativa MAUI: usare `man-in-the-loop-workflow` + `maui-expert`;
3. iterazione o chiusura focalizzata sui test: usare `man-in-the-loop-workflow` + `maui-automatic-testing`.

Questo significa che, in un progetto come `BookScout Mobile`, all'inizio di `IT-01`, `IT-02`, `IT-03`, `IT-04`, `IT-05` e `IT-06` è corretto chiedere esplicitamente all'agente di usare `man-in-the-loop-workflow` e `maui-expert`.

### 3.4 Fine iterazione e verifica

Quando una iterazione è stata implementata, il lavoro non dovrebbe essere dichiarato concluso senza review e verifica.

In questa fase si usa ancora `man-in-the-loop-workflow`, perché la skill aiuta a chiudere l'iterazione in modo controllato.

Se il focus è sui test automatici, si aggiunge `maui-automatic-testing`.

### 3.5 Ricerca o creazione di nuove skill

Quando il team affronta un problema nuovo e non sa se esista già una skill utile, si può usare `find-skills`.

Quando invece il team vuole creare una nuova skill o migliorare una skill esistente, si usa `skill-creator`.

Queste due skill non fanno parte del flusso normale di ogni iterazione di prodotto, ma sono molto utili quando il progetto cresce o cambia natura.

## 4. Regola riutilizzabile anche in altri progetti

Un progetto diverso da `BookScout Mobile` può comunque seguire la stessa logica.

La domanda da porsi non è “quale skill sembra più potente”, ma “quale fase del lavoro è realmente aperta in questo momento”.

La corrispondenza corretta è la seguente:

1. idea ancora confusa o scope instabile: `maui-prd` oppure `prd`;
2. spec approvato ma lavoro non ancora organizzato: `prd-to-plan`;
3. iterazione implementativa MAUI: `man-in-the-loop-workflow` + `maui-expert`;
4. iterazione da verificare o strategia test da rafforzare: `man-in-the-loop-workflow` + `maui-automatic-testing`;
5. bisogno di nuove skill: `find-skills` oppure `skill-creator`.

Questo schema è trasferibile a molti progetti, perché separa chiaramente specifica, planning, implementazione e verifica.

## 5. Struttura consigliata di un prompt efficace

Un buon prompt di lavoro per l'agente dovrebbe quasi sempre contenere questi elementi, nello stesso ordine:

1. nome delle skill da usare;
2. documenti che l'agente deve leggere prima di iniziare;
3. iterazione o obiettivo preciso;
4. confini dello scope;
5. vincoli tecnici o metodologici;
6. formato atteso della risposta finale.

La parte più importante non è la lunghezza del prompt, ma la precisione del perimetro.

## 6. Template generico per una iterazione implementativa MAUI

```text
Usa le skill `man-in-the-loop-workflow` e `maui-expert`.

Prima di fare modifiche, leggi:
- `docs/spec.md`
- `docs/plan.md`
- `docs/architecture.md`
- `docs/test-matrix.md`

Implementa solo l'iterazione `[ID ITERAZIONE]` descritta in `docs/plan.md`.

Obiettivo:
realizzare solo ciò che è in scope per questa iterazione, senza allargare il perimetro.

Vincoli:
- mantenere MVVM con `CommunityToolkit.Mvvm`;
- usare solo Shell navigation;
- non mettere business logic nei code-behind;
- gestire loading, error, empty e success state dove richiesto;
- non introdurre nuove dipendenze senza motivazione esplicita;
- se emerge una ambiguità davvero bloccante, fare una sola domanda breve prima di procedere.

Alla fine restituire:
1. piano breve
2. file creati o modificati
3. implementazione richiesta
4. rischi o punti da controllare
5. test manuali suggeriti
```

## 7. Prompt pronti per BookScout Mobile

### 7.1 Prompt per `maui-prd` in un nuovo progetto MAUI

```text
Usa la skill `maui-prd`.

L'idea di progetto è la seguente:
[descrizione dell'app]

Prima di scrivere il file, chiarisci solo i punti davvero necessari.
Poi crea o aggiorna `docs/spec.md`.

Lo spec dovrà includere:
- visione e problema da risolvere
- utenti target
- ambito MVP
- funzionalità future
- non-obiettivi
- requisiti funzionali
- requisiti non funzionali
- vincoli MAUI
- rischi, dipendenze e questioni aperte

Il formato della risposta dovrà essere:
1. piano breve
2. file da creare o modificare
3. implementazione richiesta
4. rischi o punti da controllare
5. test manuali suggeriti
```

### 7.2 Prompt per `prd-to-plan` quando lo spec è pronto

```text
Usa la skill `prd-to-plan`.

Prima di fare modifiche, leggi:
- `docs/spec.md`

Deriva i documenti di planning a partire dallo spec approvato.

Crea o aggiorna:
- `docs/plan.md`
- `docs/architecture.md`
- `docs/test-matrix.md`

Vincoli:
- mantenere iterazioni piccole e verificabili;
- non scrivere codice;
- non ampliare lo scope oltre ciò che è approvato nello spec;
- mantenere coerenza con MVVM, Shell, `CommunityToolkit.Mvvm`, `HttpClient`, `System.Text.Json` e SQLite.

Il formato della risposta dovrà essere:
1. piano breve
2. file da creare o modificare
3. implementazione richiesta
4. rischi o punti da controllare
5. test manuali suggeriti
```

### 7.3 Prompt per `IT-01 - Bootstrap progetto MAUI e Shell di base`

```text
Usa le skill `man-in-the-loop-workflow` e `maui-expert`.

Prima di fare modifiche, leggi:
- `docs/spec.md`
- `docs/plan.md`
- `docs/architecture.md`
- `docs/test-matrix.md`

Implementa solo `IT-01 - Bootstrap progetto MAUI e Shell di base`.

Il repository ha `src/` ancora vuota: il progetto MAUI dovrà essere creato in `src/BookScout.Mobile/` come previsto dalla documentazione.

In scope:
- creazione del progetto `.NET MAUI`
- `App`, `AppShell`, `MauiProgram`
- cartelle `Models`, `Services`, `ViewModels`, `Views`, `Resources`
- pagine placeholder `Search`, `Favorites`, `History`
- dependency injection minima

Fuori scope:
- ricerca Google Books
- dettaglio libro
- favoriti persistenti
- cronologia persistente

Vincoli:
- MVVM con `CommunityToolkit.Mvvm`
- solo Shell navigation
- nessuna business logic nei code-behind

Alla fine restituire:
1. piano breve
2. file creati o modificati
3. implementazione richiesta
4. rischi o punti da controllare
5. test manuali suggeriti
```

### 7.4 Prompt per `IT-02 - Ricerca libri e risultati in pagina unica`

```text
Usa le skill `man-in-the-loop-workflow` e `maui-expert`.

Prima di fare modifiche, leggi:
- `docs/spec.md`
- `docs/plan.md`
- `docs/architecture.md`
- `docs/test-matrix.md`

Implementa solo `IT-02 - Ricerca libri e risultati in pagina unica`.

In scope:
- integrazione Google Books per `GET /volumes?q={query}`
- servizio remoto per la ricerca libri
- DTO e mapping difensivo
- `SearchViewModel`
- `SearchPage` con `SearchBar`, `CollectionView`, placeholder copertina, loading, empty, error e retry

Fuori scope:
- pagina dettaglio completa
- salvataggio favoriti
- cronologia locale
- provider alternativi

Vincoli:
- Search e Results nella stessa pagina
- gestione esplicita di loading, empty, error e success
- nessuna logica REST nel code-behind

Alla fine restituire:
1. piano breve
2. file creati o modificati
3. implementazione richiesta
4. rischi o punti da controllare
5. test manuali suggeriti
```

### 7.5 Prompt per `IT-03 - Navigazione al dettaglio e dati estesi del libro`

```text
Usa le skill `man-in-the-loop-workflow` e `maui-expert`.

Prima di fare modifiche, leggi:
- `docs/spec.md`
- `docs/plan.md`
- `docs/architecture.md`
- `docs/test-matrix.md`

Implementa solo `IT-03 - Navigazione al dettaglio e dati estesi del libro`.

In scope:
- route Shell per il dettaglio
- passaggio del parametro `bookId`
- integrazione `GET /volumes/{id}`
- `BookDetailViewModel`
- `BookDetailPage`
- gestione dei campi mancanti
- loading, error e retry sul dettaglio

Fuori scope:
- favoriti persistenti
- apertura da Favorites con dati locali
- cronologia ricerche
- offline esteso

Vincoli:
- solo Shell navigation
- nessuna business logic nei code-behind
- il dettaglio deve restare leggibile anche con metadati incompleti

Alla fine restituire:
1. piano breve
2. file creati o modificati
3. implementazione richiesta
4. rischi o punti da controllare
5. test manuali suggeriti
```

### 7.6 Prompt per `IT-04 - Favoriti locali persistiti e refresh remoto non bloccante`

```text
Usa le skill `man-in-the-loop-workflow` e `maui-expert`.

Prima di fare modifiche, leggi:
- `docs/spec.md`
- `docs/plan.md`
- `docs/architecture.md`
- `docs/test-matrix.md`

Implementa solo `IT-04 - Favoriti locali persistiti e refresh remoto non bloccante`.

In scope:
- persistenza SQLite dei favoriti
- snapshot locale completa dei dati principali del dettaglio
- add/remove favoriti dal dettaglio
- `FavoritesPage`
- `FavoritesViewModel`
- apertura da Favorites con dati locali immediati
- refresh remoto in background solo se la rete è disponibile
- errore visibile ma non invasivo se il refresh fallisce

Fuori scope:
- cronologia ricerche
- note personali
- provider alternativi
- offline esteso

Vincoli:
- usare l'architettura definita in `docs/architecture.md`
- non bloccare la consultazione del dettaglio salvato localmente
- non introdurre un wrapper custom della connettività nel MVP

Alla fine restituire:
1. piano breve
2. file creati o modificati
3. implementazione richiesta
4. rischi o punti da controllare
5. test manuali suggeriti
```

### 7.7 Prompt per `IT-05 - Cronologia locale con deduplica, replay e pulizia`

```text
Usa le skill `man-in-the-loop-workflow` e `maui-expert`.

Prima di fare modifiche, leggi:
- `docs/spec.md`
- `docs/plan.md`
- `docs/architecture.md`
- `docs/test-matrix.md`

Implementa solo `IT-05 - Cronologia locale con deduplica, replay e pulizia`.

In scope:
- persistenza locale della cronologia
- deduplica delle query ripetute
- ordinamento dalla più recente alla meno recente
- `HistoryPage`
- `HistoryViewModel`
- replay verso `Search`
- cancellazione totale e cancellazione di singole voci

Fuori scope:
- suggerimenti intelligenti
- sincronizzazione cloud
- filtri e ordinamento avanzati dei risultati
- estensioni offline oltre il MVP

Vincoli:
- mantenere la pagina Search come pagina unica di ricerca e risultati
- evitare accoppiamento improprio tra `History` e `Search`
- mantenere il layer locale separato da quello remoto

Alla fine restituire:
1. piano breve
2. file creati o modificati
3. implementazione richiesta
4. rischi o punti da controllare
5. test manuali suggeriti
```

### 7.8 Prompt per `IT-06 - Hardening MVP e baseline di regressione`

```text
Usa le skill `man-in-the-loop-workflow` e `maui-expert`.

Prima di fare modifiche, leggi:
- `docs/spec.md`
- `docs/plan.md`
- `docs/architecture.md`
- `docs/test-matrix.md`

Implementa solo `IT-06 - Hardening MVP e baseline di regressione`.

In scope:
- rifinitura di testi, placeholder, retry e feedback di errore
- verifica della coerenza dei flussi `Search -> Detail -> Favorites -> History`
- correzione dei difetti emersi nel MVP
- smoke manuale Android e verifica di build
- preparazione della chiusura della baseline MVP

Fuori scope:
- filtri
- ordinamento post-MVP
- note personali
- barcode scanner
- offline mode estesa
- provider alternativi

Vincoli:
- non usare questa iterazione per refactor larghi non pianificati
- chiudere i principali problemi di stato UI prima del post-MVP

Alla fine restituire:
1. piano breve
2. file creati o modificati
3. implementazione richiesta
4. rischi o punti da controllare
5. test manuali suggeriti
```

### 7.9 Prompt per verifica automatica di una iterazione MAUI

```text
Usa le skill `man-in-the-loop-workflow` e `maui-automatic-testing`.

Prima di fare modifiche, leggi:
- `docs/spec.md`
- `docs/plan.md`
- `docs/architecture.md`
- `docs/test-matrix.md`

La iterazione da verificare è `[ID ITERAZIONE]`.

Obiettivo:
progettare o aggiungere i controlli automatici realistici per questa iterazione, senza inventare infrastrutture non presenti nel repository.

Vincoli:
- distinguere chiaramente tra test eseguibili ora e test rinviati a più avanti;
- privilegiare test unitari, test di servizio e verifiche di build prima della UI automation;
- non dichiarare eseguiti test che sono solo pianificati.

Alla fine restituire:
1. piano breve
2. file da creare o modificare
3. implementazione richiesta
4. rischi o punti da controllare
5. test manuali suggeriti
```

### 7.10 Prompt per scoprire skill utili in un altro progetto

```text
Usa la skill `find-skills`.

Il problema da affrontare è il seguente:
[descrizione del problema o del tipo di progetto]

L'obiettivo è capire se esistono skill già adatte o skill installabili che aiutino a svolgere meglio questo lavoro.

La risposta dovrà indicare:
1. quali skill conviene usare
2. in quale fase del lavoro conviene usarle
3. quali limiti o prerequisiti bisogna conoscere
```

### 7.11 Prompt per creare o migliorare una skill

```text
Usa la skill `skill-creator`.

La skill da creare o migliorare è la seguente:
[nome o descrizione]

Obiettivo:
produrre una skill più chiara, più utile e con trigger migliori.

La risposta dovrà chiarire:
1. cosa dovrebbe fare la skill
2. quando dovrebbe attivarsi
3. quali guardrail dovrebbe avere
4. come valutarne la qualità
```

### 7.12 Prompt per un PRD non MAUI con la skill `prd`

```text
Usa la skill `prd`.

L'idea di progetto è la seguente:
[descrizione del sistema o della funzionalità]

L'obiettivo è produrre un PRD chiaro, riutilizzabile e non legato a MAUI.

Il documento dovrà chiarire:
- visione
- utenti
- requisiti
- non-obiettivi
- metriche di successo
- rischi e dipendenze
```

## 8. Regola finale di buon uso

Se una iterazione comporta codice MAUI, la coppia più affidabile è quasi sempre:

1. `man-in-the-loop-workflow` per il controllo del perimetro;
2. `maui-expert` per la qualità tecnica MAUI.

Se la fase non è implementativa, conviene invece scegliere la skill specializzata per quel tipo di lavoro, senza usare `maui-expert` per forza.

La qualità del risultato migliora quando l'agente riceve un obiettivo piccolo, documenti di contesto chiari e il vincolo esplicito di non uscire dallo scope della fase corrente.

# AGENTS.md

## Project context

Questo progetto è una applicazione .NET MAUI con target principale Android.
L'eventuale supporto iOS è opzionale e secondario.

L'obiettivo didattico non è la generazione rapida di codice, ma lo sviluppo
controllato e documentato di una applicazione completa.

## Technical preferences

- Framework UI: .NET MAUI
- Architettura preferita: MVVM
- Navigazione preferita: Shell
- Persistenza locale: Preferences e/o SQLite (sqlite-net-pcl)
- Chiamate remote: HttpClient (con gestione asincrona)
- Parsing dati: System.Text.Json
- MVVM toolkit: CommunityToolkit.Mvvm
- Focus: robustezza, leggibilità, coerenza del codice

## Rules

- Proporre sempre un piano prima di modifiche ampie.
- Limitare ogni iterazione a una feature ben definita.
- Non introdurre nuove librerie NuGet senza motivazione esplicita.
- Non spostare logica nei code-behind se può stare in un ViewModel o Service.
- Gestire sempre loading state, error state ed empty state.
- Non rimuovere codice esistente senza spiegazione.
- Evitare duplicazioni inutili.
- Preferire nomi chiari e coerenti (PascalCase per classi e proprietà,
  camelCase per variabili locali).
- Aggiornare la documentazione quando cambia il comportamento del progetto.
- Non generare grandi blocchi di codice non richiesti.
- Indicare sempre rischi, dipendenze e test suggeriti.
- Usa git per creare branch per ogni iterazione e genera commit semantici.

## Documentation policy

Quando viene implementata una feature significativa, aggiornare almeno uno tra:

- docs/spec.md
- docs/plan.md
- docs/iterations/it-xx-nome-corto.md
- docs/test-matrix.md

## Output format preferred

Per ogni richiesta importante restituire:

1. piano breve;
2. file da creare o modificare;
3. implementazione richiesta;
4. rischi o punti da controllare;
5. test manuali suggeriti.

## Coding style

- classi piccole e con responsabilità chiara;
- servizi separati dai ViewModel;
- ViewModel con proprietà di stato esplicite (IsBusy, ErrorMessage, HasData);
- metodi asincroni dove appropriato;
- gestione degli errori non silenziosa;
- commenti solo quando davvero utili, non per ripetere ciò che il codice dice.

## Anti-patterns to avoid

- logica REST dentro la View o il code-behind;
- gestione confusa della navigazione (mescolare Shell e non-Shell);
- campi e proprietà con naming incoerente;
- dipendenze NuGet aggiunte senza controllo;
- refactoring troppo ampi in una sola iterazione;
- codice non spiegabile dagli autori del progetto.

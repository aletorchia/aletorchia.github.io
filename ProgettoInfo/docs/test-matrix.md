# Test Matrix

## 1. Regole di lettura

- Questo documento è derivato dalla fase di planning: le evidenze sotto sono previste e non ancora eseguite.
- `Manuale`, `Automatico ora`, `Automatico più avanti`: usare `Si` o `No`.
- `Evidenza prevista`: comando, nota di verifica o `Da eseguire`.
- `Automatico ora` indica controlli realistici da introdurre durante l'iterazione corrente, senza richiedere una infrastruttura di UI automation avanzata.

## 2. Matrice principale

| ID | Requisito o scenario | Categoria | Manuale | Automatico ora | Automatico più avanti | Iterazione target | Evidenza prevista | Note |
| --- | --- | --- | --- | --- | --- | --- | --- | --- |
| TM-01 | Avvio app e apertura sezioni `Search`, `Favorites`, `History` da Shell | Navigation | Si | No | No | IT-01 | Da eseguire | Smoke iniziale di bootstrap MAUI |
| TM-02 | Build del progetto MAUI e validazione di XAML e route | Build | No | Si | No | IT-01 | `dotnet build src/BookScout.Mobile/BookScout.Mobile.csproj` | Check minimo da ripetere a ogni iterazione |
| TM-03 | Query valida da Search produce risultati leggibili | Input | Si | No | Si | IT-02 | Da eseguire | Copre flusso principale di ricerca |
| TM-04 | Query vuota o non utile non innesca flussi incoerenti | Input | Si | Si | No | IT-02 | `dotnet test` | Test logico su ViewModel o validazione input |
| TM-05 | Risposta API di ricerca correttamente mappata in lista risultati | API | Si | Si | No | IT-02 | `dotnet test` | Fake JSON o mock handler consigliati |
| TM-06 | Ricerca senza risultati mostra empty state esplicito | UI state | Si | Si | No | IT-02 | `dotnet test` | Verifica stato ViewModel e copy di pagina |
| TM-07 | Errore rete o timeout in ricerca mostra messaggio e retry | API | Si | Si | No | IT-02 | `dotnet test` | Non devono emergere errori tecnici grezzi |
| TM-08 | Navigazione da Search a Detail con `bookId` corretto | Navigation | Si | No | Si | IT-03 | Da eseguire | UI automation solo in una fase successiva |
| TM-09 | Dettaglio con campi API mancanti resta leggibile e non crasha | UI state | Si | Si | No | IT-03 | `dotnet test` | Test di mapping e ViewModel su dati parziali |
| TM-10 | Salvataggio e rimozione favoriti persistono dopo riavvio app | Persistence | Si | Si | No | IT-04 | `dotnet test` | Test repository locale più smoke manuale su restart |
| TM-11 | Apertura da Favorites mostra subito dati locali salvati | Persistence | Si | No | Si | IT-04 | Da eseguire | Richiede verifica integrata ViewModel + UI |
| TM-12 | Refresh remoto da Favorites fallisce senza bloccare la schermata | UI state | Si | Si | No | IT-04 | `dotnet test` | Test di comportamento del ViewModel con mock di errore |
| TM-13 | Refresh remoto da Favorites aggiorna silenziosamente il contenuto se riesce | API | Si | Si | Si | IT-04 | `dotnet test` | Manuale utile per confermare UX senza popup di successo |
| TM-14 | Cronologia deduplicata riporta in cima una query ripetuta | Persistence | Si | Si | No | IT-05 | `dotnet test` | Ordinamento per recenza; vedi TM-14b per normalizzazione |
| TM-14b | Deduplica cronologia con normalizzazione case/trim/spazi | Persistence | Si | Si | No | IT-05 | `dotnet test` | Verifica che `"  Pride "` e `"pride"` non creino duplicato |
| TM-15 | Replay da History rilancia la ricerca nella pagina Search | Navigation | Si | No | Si | IT-05 | Da eseguire | Parametro `queryText` da verificare nel flusso Shell |
| TM-16 | Delete singolo e clear totale della cronologia funzionano correttamente | Persistence | Si | Si | No | IT-05 | `dotnet test` | Verifica logica locale e aggiornamento lista |
| TM-17 | Flusso MVP end-to-end `Search -> Detail -> Favorite -> Reopen -> History` | Device | Si | No | Si | IT-06 | Da eseguire | Smoke finale Android/emulatore |
| TM-18 | Modalità senza rete: ricerca fallisce bene ma favoriti già salvati restano consultabili | Device | Si | No | Si | IT-06 | Da eseguire | Conferma il perimetro offline limitato del MVP |
| TM-19 | Tema, rotazione e rendering base non rompono le schermate principali | Device | Si | No | No | IT-06 | Da eseguire | Verifica manuale minima Android |

## 3. Aree minime da coprire

- Input: query vuota, query valida, query ripetuta, caratteri speciali, stringhe lunghe.
- API: successo, risposta vuota, timeout, assenza campi opzionali, errore provider.
- UI: loading, error, empty, success su Search e Detail; messaggio non bloccante per refresh da Favorites.
- Navigation: apertura Detail da Search, apertura Detail da Favorites, replay da History, ritorno alle sezioni principali.
- Persistence: salvataggio favoriti, riavvio app, deduplica cronologia, delete singolo, clear totale.
- Device: smoke Android, assenza rete, tema e rotazione sulle schermate principali.

## 4. Note su test automatici

Controlli automatici realistici da introdurre presto:

- `dotnet build src/BookScout.Mobile/BookScout.Mobile.csproj` come smoke check minimo;
- test unitari su ViewModels per validazione input e gestione stati;
- test del service layer e del mapping JSON con risposte simulate;
- test del layer SQLite per favoriti e cronologia se progettato con confini testabili.

Controlli automatici da rimandare a più avanti:

- UI automation end-to-end su navigazione e rendering delle schermate;
- verifiche device-specific più approfondite;
- test automatici per barcode scanner, quando la feature entrerà nel post-MVP.

Per una strategia automatica più profonda è opportuno passare successivamente alla skill `maui-automatic-testing`.

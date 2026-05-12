# Test Matrix

## 1. Regole di lettura

- Questo documento e' derivato dalla fase di planning: le evidenze sotto sono previste e non ancora eseguite.
- `Manuale`, `Automatico ora`, `Automatico piu' avanti`: usare `Si` o `No`.
- `Evidenza prevista`: comando, nota di verifica o `Da eseguire`.
- `Automatico ora` indica controlli realistici da introdurre presto, senza richiedere UI automation avanzata.

## 2. Matrice principale

| ID | Requisito o scenario | Categoria | Manuale | Automatico ora | Automatico piu' avanti | Iterazione target | Evidenza prevista | Note |
| --- | --- | --- | --- | --- | --- | --- | --- | --- |
| TM-01 | Avvio app e apertura sezione `Search` da Shell | Navigation | Si | No | No | IT-01 | Da eseguire | Smoke iniziale bootstrap MAUI |
| TM-02 | Build progetto MAUI | Build | No | Si | No | IT-01 | `dotnet build src/MealWise.Mobile/MealWise.Mobile.csproj -c Debug /p:AndroidSdkDirectory=$HOME/.android-sdk /p:JavaSdkDirectory=$HOME/.local/share/jdk-17.0.19+10` | In questa macchina si usano SDK/JDK locali |
| TM-03 | Ricerca per ingrediente valida produce risultati leggibili | API | Si | No | Si | IT-02 | Da eseguire | Lista con immagine (se disponibile) e nome |
| TM-04 | Ricerca per categoria valida produce risultati leggibili | API | Si | No | Si | IT-02 | Da eseguire | Verifica filtro categoria |
| TM-05 | Input non valido (vuoto) non innesca stati incoerenti | Input | Si | Si | No | IT-02 | `dotnet test` | Test logico su ViewModel/validazione input |
| TM-06 | Risposta API vuota mostra empty state esplicito | UI state | Si | Si | No | IT-02 | `dotnet test` | Verifica stato ViewModel |
| TM-07 | Errore rete/timeout in ricerca mostra messaggio e retry | API | Si | Si | No | IT-02 | `dotnet test` | Non devono emergere errori tecnici grezzi |
| TM-08 | Navigazione da Search a Detail con `mealId` corretto | Navigation | Si | No | Si | IT-03 | Da eseguire | UI automation solo piu' avanti |
| TM-09 | Dettaglio con campi mancanti resta leggibile e non crasha | UI state | Si | Si | No | IT-03 | `dotnet test` | Test mapping + ViewModel su JSON parziale |
| TM-10 | Retry su errore in dettaglio recupera correttamente | API | Si | Si | No | IT-03 | `dotnet test` | Mock handler consigliato |
| TM-11 | Smoke MVP end-to-end `Search -> Detail` | Device | Si | No | Si | IT-04 | Da eseguire | Android/emulatore |
| TM-12 | Modalita' senza rete: Search fallisce bene (error + retry) | Device | Si | No | Si | IT-04 | Da eseguire | Conferma gestione error state |
| TM-13 (post-MVP) | Pianificazione calendario persiste dopo riavvio app | Persistence | Si | Si | No | IT-05 | `dotnet test` | Repository SQLite testabile |
| TM-14 (post-MVP) | Lista spesa generata e modifiche persistenti | Persistence | Si | Si | No | IT-06 | `dotnet test` | Aggregazione ingredienti + CRUD locale |
| TM-15 (post-MVP) | Ricetta salvata resta disponibile dopo riavvio e senza rete | Persistence | Si | Si | No | IT-07 | `dotnet test` | Repository ricette salvate/cache dettaglio |
| TM-16 (post-MVP) | Salvataggio ripetuto della stessa ricetta non crea duplicati | Persistence | Si | Si | No | IT-07 | `dotnet test` | Vincolo idempotenza su `mealId` |

## 3. Aree minime da coprire

- Input: valore vuoto, valore valido, spazi/trim, caratteri speciali.
- API: successo, risposta vuota, timeout, assenza campi opzionali, errore provider.
- UI: loading, error, empty, success su Search e Detail.
- Navigation: apertura Detail da Search e back.
- Persistence: salvataggio calendario, lista spesa, ricette salvate, riavvio app.
- Device: smoke Android, assenza rete, rendering base.

## 4. Note su test automatici

Controlli automatici realistici da introdurre presto:

- `dotnet build src/MealWise.Mobile/MealWise.Mobile.csproj` come smoke check minimo;
- test unitari su ViewModels per validazione input e gestione stati;
- test del service layer e del mapping JSON con risposte simulate.

Controlli automatici da rimandare a piu' avanti:

- UI automation end-to-end su navigazione e rendering;
- verifiche device-specific piu' approfondite.

Per una strategia automatica piu' profonda e' opportuno passare successivamente alla skill `maui-automatic-testing`.

## 5. Evidenze IT-03

- Build automatica eseguita: `dotnet build src/MealWise.Mobile/MealWise.Mobile.csproj -c Debug`.
- Risultato build: 0 warning, 0 errori.
- TM-08 resta da verificare manualmente su device o emulatore Android: ricerca valida, tap su risultato, apertura detail e back navigation.
- TM-09 coperta a livello di codice con fallback leggibili per categoria, area, istruzioni e lista ingredienti vuota; resta consigliato un test unitario quando verra' introdotto un progetto test.
- TM-10 resta da verificare con test automatici o mock handler dedicato in una iterazione di testing.

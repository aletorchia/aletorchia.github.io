# Skill IT-03 - Refinement skill PRD e planning

## Obiettivo

Raffinare le skill `maui-prd` e `prd-to-plan` dopo il primo ciclo di test qualitativo, correggendo i punti deboli emersi e rilanciando una nuova iterazione di valutazione.

## File modificati

- `.agents/skills/maui-prd/SKILL.md`
- `.agents/skills/maui-prd/evals/evals.json`
- `.agents/skills/prd-to-plan/SKILL.md`
- `.agents/skills/prd-to-plan/evals/evals.json`

## Problemi emersi in iteration-1

- `maui-prd` risultava troppo rigida nei prompt già ben definiti, perché tendeva a mantenere una discovery lunga anche quando sarebbe bastata una bozza rapida con pochi `TBD`.
- `maui-prd` non distingueva abbastanza chiaramente tra bozza inline e scrittura diretta di `docs/spec.md`.
- `maui-prd` nominava l'handoff a `prd-to-plan`, ma il fallback in assenza della skill o del passaggio successivo era poco esplicito.
- `prd-to-plan` tendeva a bloccarsi troppo quando `docs/spec.md` non era disponibile come file ma il prompt conteneva comunque un PRD credibile e abbastanza completo.
- `prd-to-plan` non distingueva bene tra `stop completo` e `bozza provvisoria`.
- `prd-to-plan` aveva soglie di handoff verso `maui-prd`, `maui-expert` e `maui-automatic-testing` ancora troppo implicite.

## Correzioni effettuate

### `maui-prd`

- introdotta una regola esplicita per calibrare la discovery in base al livello di completezza del prompt;
- permessa la produzione immediata di una bozza di `docs/spec.md` con 0-2 chiarimenti mirati quando il progetto è già ben definito;
- chiarita la differenza tra scrittura diretta del file, bozza inline e contesto valutativo senza scrittura reale su filesystem;
- reso esplicito il comportamento di fallback quando `prd-to-plan` non è disponibile;
- aggiunto un nuovo eval focalizzato sul caso `prompt ben definito -> bozza immediata`.

### `prd-to-plan`

- introdotta una regola esplicita `stop vs provisional draft`;
- permessa la generazione di una bozza provvisoria di `docs/plan.md`, `docs/architecture.md` e `docs/test-matrix.md` quando il file `docs/spec.md` manca ma il prompt contiene uno spec affidabile;
- chiarito il livello minimo atteso per una `docs/test-matrix.md` di pianificazione;
- rese più esplicite le soglie di handoff verso `maui-prd`, `maui-expert` e `maui-automatic-testing`;
- aggiunto un nuovo eval focalizzato sul caso `PRD riassunto nel prompt -> documenti provvisori`.

## Test eseguiti

- test qualitativo `with_skill` vs `without_skill` per `maui-prd` in `.agents/skills/maui-prd-workspace/iteration-2/`
- test qualitativo `with_skill` vs `without_skill` per `prd-to-plan` in `.agents/skills/prd-to-plan-workspace/iteration-2/`

## Esito test

- `maui-prd`: 5 eval su 5 favoriscono la skill aggiornata
- `prd-to-plan`: 5 eval su 5 favoriscono la skill aggiornata

## Evidenze principali

- `maui-prd` ora passa correttamente dal prompt ben definito a una bozza immediata con chiarimenti minimi, senza trasformare il caso in una discovery troppo lunga.
- `maui-prd` esplicita meglio il comportamento nei contesti in cui il file non viene davvero scritto e mostra inline il contenuto previsto per `docs/spec.md`.
- `prd-to-plan` ora consente una bozza provvisoria utile quando esiste un PRD riassunto nel prompt, invece di fermarsi automaticamente.
- entrambi gli skill mantengono i guardrail principali: niente salto diretto al codice, niente UI automation inventata, niente documenti presentati come definitivi quando non lo sono.

## Limiti residui

- `maui-prd` può ancora produrre bozze iniziali dense per studenti molto inesperti.
- `prd-to-plan` ha migliorato gli handoff, ma il confine operativo con le altre skill può essere reso ancora più netto con esempi aggiuntivi.

## Esito

Completato. Le due skill risultano più equilibrate e più utili nel contesto didattico del repository rispetto alla prima iterazione di test.

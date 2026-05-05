# Inventario Skill

Questo documento distingue le skill importate da sorgenti esterne dalle skill create localmente per il laboratorio didattico.

`skills-lock.json` resta il lock file delle sole skill installate da sorgenti esterne. Le skill locali sono invece asset del progetto: vivono in `.agents/skills/`, sono versionate da Git e sono documentate dai log in `docs/skill-iterations/`.

## Regole di lettura

- **Esterna**: skill installata da una sorgente remota e tracciata in `skills-lock.json`.
- **Locale**: skill creata o rifinita nel repository con `skill-creator`.
- **Workspace**: cartelle come `*-workspace/` contengono benchmark, eval e output storici; non sono skill da attivare direttamente.

## Skill esterne bloccate

| Skill | Percorso | Origine | Tracciamento | Uso nel progetto |
| --- | --- | --- | --- | --- |
| `find-skills` | `.agents/skills/find-skills/` | `vercel-labs/skills` | `skills-lock.json` | Cercare o installare skill esterne |
| `prd` | `.agents/skills/prd/` | `github/awesome-copilot` | `skills-lock.json` | Skill PRD generica usata come confronto |
| `skill-creator` | `.agents/skills/skill-creator/` | `anthropics/skills` | `skills-lock.json` | Creare, testare e rifinire skill locali |

## Skill locali del laboratorio

| Skill | Percorso | Origine | Evidenza storica | Uso nel progetto |
| --- | --- | --- | --- | --- |
| `maui-expert` | `.agents/skills/maui-expert/` | Creata localmente con `skill-creator` | `docs/skill-iterations/skill-it-01-maui-expert.md` | Scrivere o rivedere codice `.NET MAUI` coerente con MVVM, Shell e XAML |
| `maui-prd` | `.agents/skills/maui-prd/` | Creata localmente con `skill-creator` | `docs/skill-iterations/skill-it-02-prd-skills.md` | Definire o aggiornare `docs/spec.md` per un progetto MAUI |
| `prd-to-plan` | `.agents/skills/prd-to-plan/` | Creata localmente con `skill-creator` | `docs/skill-iterations/skill-it-02-prd-skills.md` | Derivare `docs/plan.md`, `docs/architecture.md` e `docs/test-matrix.md` dallo spec |
| `man-in-the-loop-workflow` | `.agents/skills/man-in-the-loop-workflow/` | Creata localmente a partire da `docs/method/man-in-the-loop.md` | `docs/history/session-ses_24af.md` e workspace eval dedicato | Governare iterazioni con planning, build, review, testing, documentazione e Git |
| `maui-automatic-testing` | `.agents/skills/maui-automatic-testing/` | Creata localmente con workflow di skill refinement | workspace eval dedicato | Definire test automatici realistici per slice MAUI |

## Skill esposte a Claude Code

La cartella `.claude/skills/` contiene link simbolici verso le skill reali in `.agents/skills/`. Non è una seconda sorgente: serve solo a rendere disponibili le stesse skill anche a Claude Code senza duplicarle.

## Politica di aggiornamento

- Aggiornare `skills-lock.json` solo quando viene installata, rimossa o aggiornata una skill esterna.
- Aggiornare questo inventario quando viene creata, rinominata, rifinita o rimossa una skill locale.
- Aggiornare `docs/skill-iterations/` quando una modifica a una skill rappresenta una vera iterazione didattica.
- Non inserire manualmente in `skills-lock.json` skill locali create nel repository.

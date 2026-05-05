# Skill IT-02 - Skill maui-prd e prd-to-plan

## Obiettivo

Introdurre due skill di progetto che guidino studenti e agente AI dalla definizione del PRD fino alla derivazione dei documenti di planning del workflow Man-in-the-Loop.

## File introdotti

- `.agents/skills/maui-prd/SKILL.md`
- `.agents/skills/maui-prd/references/spec-template.md`
- `.agents/skills/maui-prd/references/question-bank.md`
- `.agents/skills/maui-prd/evals/evals.json`
- `.agents/skills/prd-to-plan/SKILL.md`
- `.agents/skills/prd-to-plan/references/plan-template.md`
- `.agents/skills/prd-to-plan/references/architecture-template.md`
- `.agents/skills/prd-to-plan/references/test-matrix-template.md`
- `.agents/skills/prd-to-plan/evals/evals.json`

## Regole incorporate

- `maui-prd` produce o aggiorna `docs/spec.md` come PRD di progetto.
- `prd-to-plan` deriva `docs/plan.md`, `docs/architecture.md` e `docs/test-matrix.md` a partire dallo spec approvato.
- entrambe le skill guidano gli studenti con discovery strutturata, limiti di scope espliciti e linguaggio didattico.
- allineamento intenzionale a `man-in-the-loop-workflow`, `maui-expert` e `maui-automatic-testing`.
- separazione esplicita tra specifica di prodotto, piano iterativo, architettura e strategia di test.

## Note

Le nuove skill recuperano il meglio delle due skill PRD confrontate:

- discovery guidata con domande strutturate e non con assunzioni implicite;
- qualità dei requisiti e criteri di accettazione misurabili;
- salvataggio dei documenti target del repository invece di output generici;
- attenzione didattica verso studenti che devono produrre un progetto spiegabile e ben documentato.

Entrambe le skill includono un set iniziale di eval in `evals/evals.json` per un successivo ciclo di verifica e rifinitura con `skill-creator`.

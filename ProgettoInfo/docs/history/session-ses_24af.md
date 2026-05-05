# Skill per workflow Man-in-the-Loop

---

## User

> **Utente**
>
> Vorrei che creassi una skill che aiuta l'agente AI ad usare
> il modello di lavoro e il project workflow che trovi descritto @man-in-the-loop.md 

---

## Assistant (Build · GPT-5.4 · 9.2s)

<details>
<summary>Mostra risposta</summary>

_Thinking:_

**Creating a skill for the agent**

I need to create a skill for the agent, likely in the .agents/skills directory. The user is asking about building a skill to facilitate the workflow described. I should use the skill-creator and inspect the existing skill structure first. Since this is substantial, it makes sense to plan before making any modifications per AGENTS. I also think I should use todowrite due to the multistep nature of this task. I’ll start with a commentary update and then load the skill-creator.

Piano breve:
1. analizzo la struttura delle skill già presenti e il formato richiesto;
2. distillo dal file `man-in-the-loop.md` le regole operative da trasformare in istruzioni attivabili;
3. creo una nuova skill con descrizione, workflow e output atteso coerenti con questo repository.

File previsti:
- nuova cartella skill sotto `.agents/skills/`
- `SKILL.md` della skill
- eventuale aggiornamento dell’indice/documentazione locale se necessario

Rischi da controllare:
- evitare una skill troppo generica o troppo prescrittiva;
- allineare bene la skill alle regole locali su planning, iterazioni, documentazione e Git;
- non duplicare inutilmente ciò che è già in `AGENTS.md`, ma integrarlo con il workflow Man-in-the-Loop.

**Tool: todowrite**

**Input:**
```json
{
  "todos": [
    {
      "content": "Analizzare struttura skill esistenti e convenzioni locali",
      "status": "in_progress",
      "priority": "high"
    },
    {
      "content": "Estrarre workflow operativo da man-in-the-loop.md",
      "status": "pending",
      "priority": "high"
    },
    {
      "content": "Creare nuova skill con istruzioni e template pratici",
      "status": "pending",
      "priority": "high"
    },
    {
      "content": "Verificare il risultato e riassumere uso/rischi",
      "status": "pending",
      "priority": "medium"
    }
  ]
}
```

**Output:**
```
[
  {
    "content": "Analizzare struttura skill esistenti e convenzioni locali",
    "status": "in_progress",
    "priority": "high"
  },
  {
    "content": "Estrarre workflow operativo da man-in-the-loop.md",
    "status": "pending",
    "priority": "high"
  },
  {
    "content": "Creare nuova skill con istruzioni e template pratici",
    "status": "pending",
    "priority": "high"
  },
  {
    "content": "Verificare il risultato e riassumere uso/rischi",
    "status": "pending",
    "priority": "medium"
  }
]
```

</details>

---

## Assistant (Build · GPT-5.4 · 4.7s)

<details>
<summary>Mostra risposta</summary>

**Tool: skill**

**Input:**
```json
{
  "name": "skill-creator"
}
```

**Output:**
```
<skill_content name="skill-creator">
# Skill: skill-creator

# Skill Creator

A skill for creating new skills and iteratively improving them.

At a high level, the process of creating a skill goes like this:

- Decide what you want the skill to do and roughly how it should do it
- Write a draft of the skill
- Create a few test prompts and run claude-with-access-to-the-skill on them
- Help the user evaluate the results both qualitatively and quantitatively
  - While the runs happen in the background, draft some quantitative evals if there aren't any (if there are some, you can either use as is or modify if you feel something needs to change about them). Then explain them to the user (or if they already existed, explain the ones that already exist)
  - Use the `eval-viewer/generate_review.py` script to show the user the results for them to look at, and also let them look at the quantitative metrics
- Rewrite the skill based on feedback from the user's evaluation of the results (and also if there are any glaring flaws that become apparent from the quantitative benchmarks)
- Repeat until you're satisfied
- Expand the test set and try again at larger scale

Your job when using this skill is to figure out where the user is in this process and then jump in and help them progress through these stages. So for instance, maybe they're like "I want to make a skill for X". You can help narrow down what they mean, write a draft, write the test cases, figure out how they want to evaluate, run all the prompts, and repeat.

On the other hand, maybe they already have a draft of the skill. In this case you can go straight to the eval/iterate part of the loop.

Of course, you should always be flexible and if the user is like "I don't need to run a bunch of evaluations, just vibe with me", you can do that instead.

Then after the skill is done (but again, the order is flexible), you can also run the skill description improver, which we have a whole separate script for, to optimize the triggering of the skill.

Cool? Cool.

## Communicating with the user

The skill creator is liable to be used by people across a wide range of familiarity with coding jargon. If you haven't heard (and how could you, it's only very recently that it started), there's a trend now where the power of Claude is inspiring plumbers to open up their terminals, parents and grandparents to google "how to install npm". On the other hand, the bulk of users are probably fairly computer-literate.

So please pay attention to context cues to understand how to phrase your communication! In the default case, just to give you some idea:

- "evaluation" and "benchmark" are borderline, but OK
- for "JSON" and "assertion" you want to see serious cues from the user that they know what those things are before using them without explaining them

It's OK to briefly explain terms if you're in doubt, and feel free to clarify terms with a short definition if you're unsure if the user will get it.

---

## Creating a skill

### Capture Intent

Start by understanding the user's intent. The current conversation might already contain a workflow the user wants to capture (e.g., they say "turn this into a skill"). If so, extract answers from the conversation history first — the tools used, the sequence of steps, corrections the user made, input/output formats observed. The user may need to fill the gaps, and should confirm before proceeding to the next step.

1. What should this skill enable Claude to do?
2. When should this skill trigger? (what user phrases/contexts)
3. What's the expected output format?
4. Should we set up test cases to verify the skill works? Skills with objectively verifiable outputs (file transforms, data extraction, code generation, fixed workflow steps) benefit from test cases. Skills with subjective outputs (writing style, art) often don't need them. Suggest the appropriate default based on the skill type, but let the user decide.

### Interview and Research

Proactively ask questions about edge cases, input/output formats, example files, success criteria, and dependencies. Wait to write test prompts until you've got this part ironed out.

Check available MCPs - if useful for research (searching docs, finding similar skills, looking up best practices), research in parallel via subagents if available, otherwise inline. Come prepared with context to reduce burden on the user.

### Write the SKILL.md

Based on the user interview, fill in these components:

- **name**: Skill identifier
- **description**: When to trigger, what it does. This is the primary triggering mechanism - include both what the skill does AND specific contexts for when to use it. All "when to use" info goes here, not in the body. Note: currently Claude has a tendency to "undertrigger" skills -- to not use them when they'd be useful. To combat this, please make the skill descriptions a little bit "pushy". So for instance, instead of "How to build a simple fast dashboard to display internal Anthropic data.", you might write "How to build a simple fast dashboard to display internal Anthropic data. Make sure to use this skill whenever the user mentions dashboards, data visualization, internal metrics, or wants to display any kind of company data, even if they don't explicitly ask for a 'dashboard.'"
- **compatibility**: Required tools, dependencies (optional, rarely needed)
- **the rest of the skill :)**

### Skill Writing Guide

#### Anatomy of a Skill

```
skill-name/
├── SKILL.md (required)
│   ├── YAML frontmatter (name, description required)
│   └── Markdown instructions
└── Bundled Resources (optional)
    ├── scripts/    - Executable code for deterministic/repetitive tasks
    ├── references/ - Docs loaded into context as needed
    └── assets/     - Files used in output (templates, icons, fonts)
```

#### Progressive Disclosure

Skills use a three-level loading system:
1. **Metadata** (name + description) - Always in context (~100 words)
2. **SKILL.md body** - In context whenever skill triggers (<500 lines ideal)
3. **Bundled resources** - As needed (unlimited, scripts can execute without loading)

These word counts are approximate and you can feel free to go longer if needed.

**Key patterns:**
- Keep SKILL.md under 500 lines; if you're approaching this limit, add an additional layer of hierarchy along with clear pointers about where the model using the skill should go next to follow up.
- Reference files clearly from SKILL.md with guidance on when to read them
- For large reference files (>300 lines), include a table of contents

**Domain organization**: When a skill supports multiple domains/frameworks, organize by variant:
```
cloud-deploy/
├── SKILL.md (workflow + selection)
└── references/
    ├── aws.md
    ├── gcp.md
    └── azure.md
```
Claude reads only the relevant reference file.

#### Principle of Lack of Surprise

This goes without saying, but skills must not contain malware, exploit code, or any content that could compromise system security. A skill's contents should not surprise the user in their intent if described. Don't go along with requests to create misleading skills or skills designed to facilitate unauthorized access, data exfiltration, or other malicious activities. Things like a "roleplay as an XYZ" are OK though.

#### Writing Patterns

Prefer using the imperative form in instructions.

**Defining output formats** - You can do it like this:
~~~text
Report structure
ALWAYS use this exact template:
Title: [Title]
Section: Executive summary
Section: Key findings
Section: Recommendations
~~~

**Examples pattern** - It's useful to include examples. You can format them like this (but if "Input" and "Output" are in the examples you might want to deviate a little):
~~~text
Commit message format
**Example 1:**
Input: Added user authentication with JWT tokens
Output: feat(auth): implement JWT-based authentication
~~~

### Writing Style

Try to explain to the model why things are important in lieu of heavy-handed musty MUSTs. Use theory of mind and try to make the skill general and not super-narrow to specific examples. Start by writing a draft and then look at it with fresh eyes and improve it.

### Test Cases

After writing the skill draft, come up with 2-3 realistic test prompts — the kind of thing a real user would actually say. Share them with the user: [you don't have to use this exact language] "Here are a few test cases I'd like to try. Do these look right, or do you want to add more?" Then run them.

Save test cases to `evals/evals.json`. Don't write assertions yet — just the prompts. You'll draft assertions in the next step while the runs are in progress.

```json
{
  "skill_name": "example-skill",
  "evals": [
    {
      "id": 1,
      "prompt": "User's task prompt",
      "expected_output": "Description of expected result",
      "files": []
    }
  ]
}
```

See `references/schemas.md` for the full schema (including the `assertions` field, which you'll add later).

## Running and evaluating test cases

This section is one continuous sequence — don't stop partway through. Do NOT use `/skill-test` or any other testing skill.

Put results in `<skill-name>-workspace/` as a sibling to the skill directory. Within the workspace, organize results by iteration (`iteration-1/`, `iteration-2/`, etc.) and within that, each test case gets a directory (`eval-0/`, `eval-1/`, etc.). Don't create all of this upfront — just create directories as you go.

### Step 1: Spawn all runs (with-skill AND baseline) in the same turn

For each test case, spawn two subagents in the same turn — one with the skill, one without. This is important: don't spawn the with-skill runs first and then come back for baselines later. Launch everything at once so it all finishes around the same time.

**With-skill run:**

```
Execute this task:
- Skill path: <path-to-skill>
- Task: <eval prompt>
- Input files: <eval files if any, or "none">
- Save outputs to: <workspace>/iteration-<N>/eval-<ID>/with_skill/outputs/
- Outputs to save: <what the user cares about — e.g., "the .docx file", "the final CSV">
```

**Baseline run** (same prompt, but the baseline depends on context):
- **Creating a new skill**: no skill at all. Same prompt, no skill path, save to `without_skill/outputs/`.
- **Improving an existing skill**: the old version. Before editing, snapshot the skill (`cp -r <skill-path> <workspace>/skill-snapshot/`), then point the baseline subagent at the snapshot. Save to `old_skill/outputs/`.

Write an `eval_metadata.json` for each test case (assertions can be empty for now). Give each eval a descriptive name based on what it's testing — not just "eval-0". Use this name for the directory too. If this iteration uses new or modified eval prompts, create these files for each new eval directory — don't assume they carry over from previous iterations.

```json
{
  "eval_id": 0,
  "eval_name": "descriptive-name-here",
  "prompt": "The user's task prompt",
  "assertions": []
}
```

### Step 2: While runs are in progress, draft assertions

Don't just wait for the runs to finish — you can use this time productively. Draft quantitative assertions for each test case and explain them to the user. If assertions already exist in `evals/evals.json`, review them and explain what they check.

Good assertions are objectively verifiable and have descriptive names — they should read clearly in the benchmark viewer so someone glancing at the results immediately understands what each one checks. Subjective skills (writing style, design quality) are better evaluated qualitatively — don't force assertions onto things that need human judgment.

Update the `eval_metadata.json` files and `evals/evals.json` with the assertions once drafted. Also explain to the user what they'll see in the viewer — both the qualitative outputs and the quantitative benchmark.

### Step 3: As runs complete, capture timing data

When each subagent task completes, you receive a notification containing `total_tokens` and `duration_ms`. Save this data immediately to `timing.json` in the run directory:

```json
{
  "total_tokens": 84852,
  "duration_ms": 23332,
  "total_duration_seconds": 23.3
}
```

This is the only opportunity to capture this data — it comes through the task notification and isn't persisted elsewhere. Process each notification as it arrives rather than trying to batch them.

### Step 4: Grade, aggregate, and launch the viewer

Once all runs are done:

1. **Grade each run** — spawn a grader subagent (or grade inline) that reads `agents/grader.md` and evaluates each assertion against the outputs. Save results to `grading.json` in each run directory. The grading.json expectations array must use the fields `text`, `passed`, and `evidence` (not `name`/`met`/`details` or other variants) — the viewer depends on these exact field names. For assertions that can be checked programmatically, write and run a script rather than eyeballing it — scripts are faster, more reliable, and can be reused across iterations.

2. **Aggregate into benchmark** — run the aggregation script from the skill-creator directory:
   ```bash
   python -m scripts.aggregate_benchmark <workspace>/iteration-N --skill-name <name>
   ```
   This produces `benchmark.json` and `benchmark.md` with pass_rate, time, and tokens for each configuration, with mean ± stddev and the delta. If generating benchmark.json manually, see `references/schemas.md` for the exact schema the viewer expects.
Put each with_skill version before its baseline counterpart.

3. **Do an analyst pass** — read the benchmark data and surface patterns the aggregate stats might hide. See `agents/analyzer.md` (the "Analyzing Benchmark Results" section) for what to look for — things like assertions that always pass regardless of skill (non-discriminating), high-variance evals (possibly flaky), and time/token tradeoffs.

4. **Launch the viewer** with both qualitative outputs and quantitative data:
   ```bash
   nohup python <skill-creator-path>/eval-viewer/generate_review.py \
     <workspace>/iteration-N \
     --skill-name "my-skill" \
     --benchmark <workspace>/iteration-N/benchmark.json \
     > /dev/null 2>&1 &
   VIEWER_PID=$!
   ```
   For iteration 2+, also pass `--previous-workspace <workspace>/iteration-<N-1>`.

   **Cowork / headless environments:** If `webbrowser.open()` is not available or the environment has no display, use `--static <output_path>` to write a standalone HTML file instead of starting a server. Feedback will be downloaded as a `feedback.json` file when the user clicks "Submit All Reviews". After download, copy `feedback.json` into the workspace directory for the next iteration to pick up.

Note: please use generate_review.py to create the viewer; there's no need to write custom HTML.

5. **Tell the user** something like: "I've opened the results in your browser. There are two tabs — 'Outputs' lets you click through each test case and leave feedback, 'Benchmark' shows the quantitative comparison. When you're done, come back here and let me know."

### What the user sees in the viewer

The "Outputs" tab shows one test case at a time:
- **Prompt**: the task that was given
- **Output**: the files the skill produced, rendered inline where possible
- **Previous Output** (iteration 2+): collapsed section showing last iteration's output
- **Formal Grades** (if grading was run): collapsed section showing assertion pass/fail
- **Feedback**: a textbox that auto-saves as they type
- **Previous Feedback** (iteration 2+): their comments from last time, shown below the textbox

The "Benchmark" tab shows the stats summary: pass rates, timing, and token usage for each configuration, with per-eval breakdowns and analyst observations.

Navigation is via prev/next buttons or arrow keys. When done, they click "Submit All Reviews" which saves all feedback to `feedback.json`.

### Step 5: Read the feedback

When the user tells you they're done, read `feedback.json`:

```json
{
  "reviews": [
    {"run_id": "eval-0-with_skill", "feedback": "the chart is missing axis labels", "timestamp": "..."},
    {"run_id": "eval-1-with_skill", "feedback": "", "timestamp": "..."},
    {"run_id": "eval-2-with_skill", "feedback": "perfect, love this", "timestamp": "..."}
  ],
  "status": "complete"
}
```

Empty feedback means the user thought it was fine. Focus your improvements on the test cases where the user had specific complaints.

Kill the viewer server when you're done with it:

```bash
kill $VIEWER_PID 2>/dev/null
```

---

## Improving the skill

This is the heart of the loop. You've run the test cases, the user has reviewed the results, and now you need to make the skill better based on their feedback.

### How to think about improvements

1. **Generalize from the feedback.** The big picture thing that's happening here is that we're trying to create skills that can be used a million times (maybe literally, maybe even more who knows) across many different prompts. Here you and the user are iterating on only a few examples over and over again because it helps move faster. The user knows these examples in and out and it's quick for them to assess new outputs. But if the skill you and the user are codeveloping works only for those examples, it's useless. Rather than put in fiddly overfitty changes, or oppressively constrictive MUSTs, if there's some stubborn issue, you might try branching out and using different metaphors, or recommending different patterns of working. It's relatively cheap to try and maybe you'll land on something great.

2. **Keep the prompt lean.** Remove things that aren't pulling their weight. Make sure to read the transcripts, not just the final outputs — if it looks like the skill is making the model waste a bunch of time doing things that are unproductive, you can try getting rid of the parts of the skill that are making it do that and seeing what happens.

3. **Explain the why.** Try hard to explain the **why** behind everything you're asking the model to do. Today's LLMs are *smart*. They have good theory of mind and when given a good harness can go beyond rote instructions and really make things happen. Even if the feedback from the user is terse or frustrated, try to actually understand the task and why the user is writing what they wrote, and what they actually wrote, and then transmit this understanding into the instructions. If you find yourself writing ALWAYS or NEVER in all caps, or using super rigid structures, that's a yellow flag — if possible, reframe and explain the reasoning so that the model understands why the thing you're asking for is important. That's a more humane, powerful, and effective approach.

4. **Look for repeated work across test cases.** Read the transcripts from the test runs and notice if the subagents all independently wrote similar helper scripts or took the same multi-step approach to something. If all 3 test cases resulted in the subagent writing a `create_docx.py` or a `build_chart.py`, that's a strong signal the skill should bundle that script. Write it once, put it in `scripts/`, and tell the skill to use it. This saves every future invocation from reinventing the wheel.

This task is pretty important (we are trying to create billions a year in economic value here!) and your thinking time is not the blocker; take your time and really mull things over. I'd suggest writing a draft revision and then looking at it anew and making improvements. Really do your best to get into the head of the user and understand what they want and need.

### The iteration loop

After improving the skill:

1. Apply your improvements to the skill
2. Rerun all test cases into a new `iteration-<N+1>/` directory, including baseline runs. If you're creating a new skill, the baseline is always `without_skill` (no skill) — that stays the same across iterations. If you're improving an existing skill, use your judgment on what makes sense as the baseline: the original version the user came in with, or the previous iteration.
3. Launch the reviewer with `--previous-workspace` pointing at the previous iteration
4. Wait for the user to review and tell you they're done
5. Read the new feedback, improve again, repeat

Keep going until:
- The user says they're happy
- The feedback is all empty (everything looks good)
- You're not making meaningful progress

---

## Advanced: Blind comparison

For situations where you want a more rigorous comparison between two versions of a skill (e.g., the user asks "is the new version actually better?"), there's a blind comparison system. Read `agents/comparator.md` and `agents/analyzer.md` for the details. The basic idea is: give two outputs to an independent agent without telling it which is which, and let it judge quality. Then analyze why the winner won.

This is optional, requires subagents, and most users won't need it. The human review loop is usually sufficient.

---

## Description Optimization

The description field in SKILL.md frontmatter is the primary mechanism that determines whether Claude invokes a skill. After creating or improving a skill, offer to optimize the description for better triggering accuracy.

### Step 1: Generate trigger eval queries

Create 20 eval queries — a mix of should-trigger and should-not-trigger. Save as JSON:

```json
[
  {"query": "the user prompt", "should_trigger": true},
  {"query": "another prompt", "should_trigger": false}
]
```

The queries must be realistic and something a Claude Code or Claude.ai user would actually type. Not abstract requests, but requests that are concrete and specific and have a good amount of detail. For instance, file paths, personal context about the user's job or situation, column names and values, company names, URLs. A little bit of backstory. Some might be in lowercase or contain abbreviations or typos or casual speech. Use a mix of different lengths, and focus on edge cases rather than making them clear-cut (the user will get a chance to sign off on them).

Bad: `"Format this data"`, `"Extract text from PDF"`, `"Create a chart"`

Good: `"ok so my boss just sent me this xlsx file (its in my downloads, called something like 'Q4 sales final FINAL v2.xlsx') and she wants me to add a column that shows the profit margin as a percentage. The revenue is in column C and costs are in column D i think"`

For the **should-trigger** queries (8-10), think about coverage. You want different phrasings of the same intent — some formal, some casual. Include cases where the user doesn't explicitly name the skill or file type but clearly needs it. Throw in some uncommon use cases and cases where this skill competes with another but should win.

For the **should-not-trigger** queries (8-10), the most valuable ones are the near-misses — queries that share keywords or concepts with the skill but actually need something different. Think adjacent domains, ambiguous phrasing where a naive keyword match would trigger but shouldn't, and cases where the query touches on something the skill does but in a context where another tool is more appropriate.

The key thing to avoid: don't make should-not-trigger queries obviously irrelevant. "Write a fibonacci function" as a negative test for a PDF skill is too easy — it doesn't test anything. The negative cases should be genuinely tricky.

### Step 2: Review with user

Present the eval set to the user for review using the HTML template:

1. Read the template from `assets/eval_review.html`
2. Replace the placeholders:
   - `__EVAL_DATA_PLACEHOLDER__` → the JSON array of eval items (no quotes around it — it's a JS variable assignment)
   - `__SKILL_NAME_PLACEHOLDER__` → the skill's name
   - `__SKILL_DESCRIPTION_PLACEHOLDER__` → the skill's current description
3. Write to a temp file (e.g., `/tmp/eval_review_<skill-name>.html`) and open it: `open /tmp/eval_review_<skill-name>.html`
4. The user can edit queries, toggle should-trigger, add/remove entries, then click "Export Eval Set"
5. The file downloads to `~/Downloads/eval_set.json` — check the Downloads folder for the most recent version in case there are multiple (e.g., `eval_set (1).json`)

This step matters — bad eval queries lead to bad descriptions.

### Step 3: Run the optimization loop

Tell the user: "This will take some time — I'll run the optimization loop in the background and check on it periodically."

Save the eval set to the workspace, then run in the background:

```bash
python -m scripts.run_loop \
  --eval-set <path-to-trigger-eval.json> \
  --skill-path <path-to-skill> \
  --model <model-id-powering-this-session> \
  --max-iterations 5 \
  --verbose
```

Use the model ID from your system prompt (the one powering the current session) so the triggering test matches what the user actually experiences.

While it runs, periodically tail the output to give the user updates on which iteration it's on and what the scores look like.

This handles the full optimization loop automatically. It splits the eval set into 60% train and 40% held-out test, evaluates the current description (running each query 3 times to get a reliable trigger rate), then calls Claude to propose improvements based on what failed. It re-evaluates each new description on both train and test, iterating up to 5 times. When it's done, it opens an HTML report in the browser showing the results per iteration and returns JSON with `best_description` — selected by test score rather than train score to avoid overfitting.

### How skill triggering works

Understanding the triggering mechanism helps design better eval queries. Skills appear in Claude's `available_skills` list with their name + description, and Claude decides whether to consult a skill based on that description. The important thing to know is that Claude only consults skills for tasks it can't easily handle on its own — simple, one-step queries like "read this PDF" may not trigger a skill even if the description matches perfectly, because Claude can handle them directly with basic tools. Complex, multi-step, or specialized queries reliably trigger skills when the description matches.

This means your eval queries should be substantive enough that Claude would actually benefit from consulting a skill. Simple queries like "read file X" are poor test cases — they won't trigger skills regardless of description quality.

### Step 4: Apply the result

Take `best_description` from the JSON output and update the skill's SKILL.md frontmatter. Show the user before/after and report the scores.

---

### Package and Present (only if `present_files` tool is available)

Check whether you have access to the `present_files` tool. If you don't, skip this step. If you do, package the skill and present the .skill file to the user:

```bash
python -m scripts.package_skill <path/to/skill-folder>
```

After packaging, direct the user to the resulting `.skill` file path so they can install it.

---

## Claude.ai-specific instructions

In Claude.ai, the core workflow is the same (draft → test → review → improve → repeat), but because Claude.ai doesn't have subagents, some mechanics change. Here's what to adapt:

**Running test cases**: No subagents means no parallel execution. For each test case, read the skill's SKILL.md, then follow its instructions to accomplish the test prompt yourself. Do them one at a time. This is less rigorous than independent subagents (you wrote the skill and you're also running it, so you have full context), but it's a useful sanity check — and the human review step compensates. Skip the baseline runs — just use the skill to complete the task as requested.

**Reviewing results**: If you can't open a browser (e.g., Claude.ai's VM has no display, or you're on a remote server), skip the browser reviewer entirely. Instead, present results directly in the conversation. For each test case, show the prompt and the output. If the output is a file the user needs to see (like a .docx or .xlsx), save it to the filesystem and tell them where it is so they can download and inspect it. Ask for feedback inline: "How does this look? Anything you'd change?"

**Benchmarking**: Skip the quantitative benchmarking — it relies on baseline comparisons which aren't meaningful without subagents. Focus on qualitative feedback from the user.

**The iteration loop**: Same as before — improve the skill, rerun the test cases, ask for feedback — just without the browser reviewer in the middle. You can still organize results into iteration directories on the filesystem if you have one.

**Description optimization**: This section requires the `claude` CLI tool (specifically `claude -p`) which is only available in Claude Code. Skip it if you're on Claude.ai.

**Blind comparison**: Requires subagents. Skip it.

**Packaging**: The `package_skill.py` script works anywhere with Python and a filesystem. On Claude.ai, you can run it and the user can download the resulting `.skill` file.

**Updating an existing skill**: The user might be asking you to update an existing skill, not create a new one. In this case:
- **Preserve the original name.** Note the skill's directory name and `name` frontmatter field -- use them unchanged. E.g., if the installed skill is `research-helper`, output `research-helper.skill` (not `research-helper-v2`).
- **Copy to a writeable location before editing.** The installed skill path may be read-only. Copy to `/tmp/skill-name/`, edit there, and package from the copy.
- **If packaging manually, stage in `/tmp/` first**, then copy to the output directory -- direct writes may fail due to permissions.

---

## Cowork-Specific Instructions

If you're in Cowork, the main things to know are:

- You have subagents, so the main workflow (spawn test cases in parallel, run baselines, grade, etc.) all works. (However, if you run into severe problems with timeouts, it's OK to run the test prompts in series rather than parallel.)
- You don't have a browser or display, so when generating the eval viewer, use `--static <output_path>` to write a standalone HTML file instead of starting a server. Then proffer a link that the user can click to open the HTML in their browser.
- For whatever reason, the Cowork setup seems to disincline Claude from generating the eval viewer after running the tests, so just to reiterate: whether you're in Cowork or in Claude Code, after running tests, you should always generate the eval viewer for the human to look at examples before revising the skill yourself and trying to make corrections, using `generate_review.py` (not writing your own boutique html code). Sorry in advance but I'm gonna go all caps here: GENERATE THE EVAL VIEWER *BEFORE* evaluating inputs yourself. You want to get them in front of the human ASAP!
- Feedback works differently: since there's no running server, the viewer's "Submit All Reviews" button will download `feedback.json` as a file. You can then read it from there (you may have to request access first).
- Packaging works — `package_skill.py` just needs Python and a filesystem.
- Description optimization (`run_loop.py` / `run_eval.py`) should work in Cowork just fine since it uses `claude -p` via subprocess, not a browser, but please save it until you've fully finished making the skill and the user agrees it's in good shape.
- **Updating an existing skill**: The user might be asking you to update an existing skill, not create a new one. Follow the update guidance in the claude.ai section above.

---

## Reference files

The agents/ directory contains instructions for specialized subagents. Read them when you need to spawn the relevant subagent.

- `agents/grader.md` — How to evaluate assertions against outputs
- `agents/comparator.md` — How to do blind A/B comparison between two outputs
- `agents/analyzer.md` — How to analyze why one version beat another

The references/ directory has additional documentation:
- `references/schemas.md` — JSON structures for evals.json, grading.json, etc.

---

Repeating one more time the core loop here for emphasis:

- Figure out what the skill is about
- Draft or edit the skill
- Run claude-with-access-to-the-skill on test prompts
- With the user, evaluate the outputs:
  - Create benchmark.json and run `eval-viewer/generate_review.py` to help the user review them
  - Run quantitative evals
- Repeat until you and the user are satisfied
- Package the final skill and return it to the user.

Please add steps to your TodoList, if you have such a thing, to make sure you don't forget. If you're in Cowork, please specifically put "Create evals JSON and run `eval-viewer/generate_review.py` so human can review test cases" in your TodoList to make sure it happens.

Good luck!

Base directory for this skill: file:///C:/Users/genna/source/repos/Test/maui-project-demo2/.claude/skills/skill-creator
Relative paths in this skill (e.g., scripts/, reference/) are relative to this base directory.
Note: file list is sampled.

<skill_files>
<file>C:\Users\genna\source\repos\Test\maui-project-demo2\.claude\skills\skill-creator\scripts\generate_report.py</file>
<file>C:\Users\genna\source\repos\Test\maui-project-demo2\.claude\skills\skill-creator\scripts\aggregate_benchmark.py</file>
<file>C:\Users\genna\source\repos\Test\maui-project-demo2\.claude\skills\skill-creator\scripts\run_eval.py</file>
<file>C:\Users\genna\source\repos\Test\maui-project-demo2\.claude\skills\skill-creator\scripts\quick_validate.py</file>
<file>C:\Users\genna\source\repos\Test\maui-project-demo2\.claude\skills\skill-creator\scripts\package_skill.py</file>
<file>C:\Users\genna\source\repos\Test\maui-project-demo2\.claude\skills\skill-creator\scripts\improve_description.py</file>
<file>C:\Users\genna\source\repos\Test\maui-project-demo2\.claude\skills\skill-creator\LICENSE.txt</file>
<file>C:\Users\genna\source\repos\Test\maui-project-demo2\.claude\skills\skill-creator\scripts\utils.py</file>
<file>C:\Users\genna\source\repos\Test\maui-project-demo2\.claude\skills\skill-creator\scripts\run_loop.py</file>
<file>C:\Users\genna\source\repos\Test\maui-project-demo2\.claude\skills\skill-creator\scripts\__init__.py</file>
</skill_files>
</skill_content>
```

</details>

---

## Assistant (Build · GPT-5.4 · 6.4s)

<details>
<summary>Mostra risposta</summary>

_Thinking:_

**Inspecting skills directories**

I'm thinking about inspecting the existing skills directories. I might use a glob pattern to search, specifically looking for files like "SKILL.md" within the ".agents/skills/**/" path. Also, there's a mention of ".claude," which I need to clarify as well. I'll dig into this to save time later. It feels crucial to ensure I'm looking in the right places to gather the necessary information!

Sto verificando la struttura reale delle skill nel repository per creare la nuova skill nello stesso formato e nella posizione più coerente.

**Tool: glob**

**Input:**
```json
{
  "pattern": "**/SKILL.md",
  "path": "C:\\Users\\genna\\source\\repos\\Test\\maui-project-demo2\\.agents\\skills"
}
```

**Output:**
```
C:\Users\genna\source\repos\Test\maui-project-demo2\.agents\skills\maui-expert\SKILL.md
C:\Users\genna\source\repos\Test\maui-project-demo2\.agents\skills\find-skills\SKILL.md
C:\Users\genna\source\repos\Test\maui-project-demo2\.agents\skills\skill-creator\SKILL.md
```

**Tool: glob**

**Input:**
```json
{
  "pattern": "**/SKILL.md",
  "path": "C:\\Users\\genna\\source\\repos\\Test\\maui-project-demo2\\.claude\\skills"
}
```

**Output:**
```
No files found
```

**Tool: read**

**Input:**
```json
{
  "filePath": "C:\\Users\\genna\\source\\repos\\Test\\maui-project-demo2\\.agents\\skills\\find-skills\\SKILL.md",
  "offset": 1,
  "limit": 250
}
```

**Output:**
```
<path>C:\Users\genna\source\repos\Test\maui-project-demo2\.agents\skills\find-skills\SKILL.md</path>
<type>file</type>
<content>
1: ---
2: name: find-skills
3: description: Helps users discover and install agent skills when they ask questions like "how do I do X", "find a skill for X", "is there a skill that can...", or express interest in extending capabilities. This skill should be used when the user is looking for functionality that might exist as an installable skill.
4: ---
5: 
6: # Find Skills
7: 
8: This skill helps you discover and install skills from the open agent skills ecosystem.
9: 
10: ## When to Use This Skill
11: 
12: Use this skill when the user:
13: 
14: - Asks "how do I do X" where X might be a common task with an existing skill
15: - Says "find a skill for X" or "is there a skill for X"
16: - Asks "can you do X" where X is a specialized capability
17: - Expresses interest in extending agent capabilities
18: - Wants to search for tools, templates, or workflows
19: - Mentions they wish they had help with a specific domain (design, testing, deployment, etc.)
20: 
21: ## What is the Skills CLI?
22: 
23: The Skills CLI (`npx skills`) is the package manager for the open agent skills ecosystem. Skills are modular packages that extend agent capabilities with specialized knowledge, workflows, and tools.
24: 
25: **Key commands:**
26: 
27: - `npx skills find [query]` - Search for skills interactively or by keyword
28: - `npx skills add <package>` - Install a skill from GitHub or other sources
29: - `npx skills check` - Check for skill updates
30: - `npx skills update` - Update all installed skills
31: 
32: **Browse skills at:** https://skills.sh/
33: 
34: ## How to Help Users Find Skills
35: 
36: ### Step 1: Understand What They Need
37: 
38: When a user asks for help with something, identify:
39: 
40: 1. The domain (e.g., React, testing, design, deployment)
41: 2. The specific task (e.g., writing tests, creating animations, reviewing PRs)
42: 3. Whether this is a common enough task that a skill likely exists
43: 
44: ### Step 2: Check the Leaderboard First
45: 
46: Before running a CLI search, check the [skills.sh leaderboard](https://skills.sh/) to see if a well-known skill already exists for the domain. The leaderboard ranks skills by total installs, surfacing the most popular and battle-tested options.
47: 
48: For example, top skills for web development include:
49: - `vercel-labs/agent-skills` — React, Next.js, web design (100K+ installs each)
50: - `anthropics/skills` — Frontend design, document processing (100K+ installs)
51: 
52: ### Step 3: Search for Skills
53: 
54: If the leaderboard doesn't cover the user's need, run the find command:
55: 
56: ```bash
57: npx skills find [query]
58: ```
59: 
60: For example:
61: 
62: - User asks "how do I make my React app faster?" → `npx skills find react performance`
63: - User asks "can you help me with PR reviews?" → `npx skills find pr review`
64: - User asks "I need to create a changelog" → `npx skills find changelog`
65: 
66: ### Step 4: Verify Quality Before Recommending
67: 
68: **Do not recommend a skill based solely on search results.** Always verify:
69: 
70: 1. **Install count** — Prefer skills with 1K+ installs. Be cautious with anything under 100.
71: 2. **Source reputation** — Official sources (`vercel-labs`, `anthropics`, `microsoft`) are more trustworthy than unknown authors.
72: 3. **GitHub stars** — Check the source repository. A skill from a repo with <100 stars should be treated with skepticism.
73: 
74: ### Step 5: Present Options to the User
75: 
76: When you find relevant skills, present them to the user with:
77: 
78: 1. The skill name and what it does
79: 2. The install count and source
80: 3. The install command they can run
81: 4. A link to learn more at skills.sh
82: 
83: Example response:
84: 
85: ```
86: I found a skill that might help! The "react-best-practices" skill provides
87: React and Next.js performance optimization guidelines from Vercel Engineering.
88: (185K installs)
89: 
90: To install it:
91: npx skills add vercel-labs/agent-skills@react-best-practices
92: 
93: Learn more: https://skills.sh/vercel-labs/agent-skills/react-best-practices
94: ```
95: 
96: ### Step 6: Offer to Install
97: 
98: If the user wants to proceed, you can install the skill for them:
99: 
100: ```bash
101: npx skills add <owner/repo@skill> -g -y
102: ```
103: 
104: The `-g` flag installs globally (user-level) and `-y` skips confirmation prompts.
105: 
106: ## Common Skill Categories
107: 
108: When searching, consider these common categories:
109: 
110: | Category        | Example Queries                          |
111: | --------------- | ---------------------------------------- |
112: | Web Development | react, nextjs, typescript, css, tailwind |
113: | Testing         | testing, jest, playwright, e2e           |
114: | DevOps          | deploy, docker, kubernetes, ci-cd        |
115: | Documentation   | docs, readme, changelog, api-docs        |
116: | Code Quality    | review, lint, refactor, best-practices   |
117: | Design          | ui, ux, design-system, accessibility     |
118: | Productivity    | workflow, automation, git                |
119: 
120: ## Tips for Effective Searches
121: 
122: 1. **Use specific keywords**: "react testing" is better than just "testing"
123: 2. **Try alternative terms**: If "deploy" doesn't work, try "deployment" or "ci-cd"
124: 3. **Check popular sources**: Many skills come from `vercel-labs/agent-skills` or `ComposioHQ/awesome-claude-skills`
125: 
126: ## When No Skills Are Found
127: 
128: If no relevant skills exist:
129: 
130: 1. Acknowledge that no existing skill was found
131: 2. Offer to help with the task directly using your general capabilities
132: 3. Suggest the user could create their own skill with `npx skills init`
133: 
134: Example:
135: 
136: ```
137: I searched for skills related to "xyz" but didn't find any matches.
138: I can still help you with this task directly! Would you like me to proceed?
139: 
140: If this is something you do often, you could create your own skill:
141: npx skills init my-xyz-skill
142: ```

(End of file - total 142 lines)
</content>
```

</details>

---

## Assistant (Build · GPT-5.4 · 4.4s)

<details>
<summary>Mostra risposta</summary>

_Thinking:_



**Tool: read**

**Input:**
```json
{
  "filePath": "C:\\Users\\genna\\source\\repos\\Test\\maui-project-demo2\\.agents\\skills\\maui-expert\\SKILL.md",
  "offset": 1,
  "limit": 300
}
```

**Output:**
```
<path>C:\Users\genna\source\repos\Test\maui-project-demo2\.agents\skills\maui-expert\SKILL.md</path>
<type>file</type>
<content>
1: ---
2: name: maui-expert
3: description: Write and review .NET MAUI code using MVVM with CommunityToolkit.Mvvm, clean XAML with compiled bindings, and Shell navigation. Use this skill whenever the user mentions .NET MAUI, XAML, ContentPage, Shell routes, ViewModels, bindings, CollectionView, legacy ListView maintenance, Android-first MAUI apps, or asks for MAUI architecture, state handling, or best practices, even if they do not explicitly ask for a skill.
4: ---
5: 
6: # Maui Expert
7: 
8: Use this skill to implement or review .NET MAUI features in a way that stays readable, testable, and consistent with an MVVM application.
9: 
10: ## Core goal
11: 
12: Produce the smallest correct MAUI change that fits the existing project. Keep view logic in XAML, screen behavior in ViewModels, and integration details in services.
13: 
14: ## First pass
15: 
16: Before editing:
17: 
18: - Read the relevant page XAML, ViewModel, service, `AppShell`, and DI registrations.
19: - Check whether the screen already has a state pattern, route name, and constructor injection setup.
20: - Prefer extending the current structure over inventing a new abstraction.
21: 
22: ## Required MAUI rules
23: 
24: - Use MVVM with `CommunityToolkit.Mvvm`.
25: - Use `[ObservableProperty]` for mutable state and `[RelayCommand]` for UI actions.
26: - Keep UI in XAML and use compiled bindings with `x:DataType` on the page and on list item templates.
27: - For list UIs in current MAUI projects, use `CollectionView`. Do not propose `ListView` for new code.
28: - Use Shell navigation only. Do not mix in `Navigation.PushAsync`, modal stacks, or custom navigation flows unless the project already wraps Shell and still resolves through Shell APIs.
29: - Inject services through constructors. Do not create services directly inside a ViewModel.
30: - Do not put business logic, data loading, navigation, or validation in `.xaml.cs` files.
31: - Do not use `OnAppearing`, `Loaded`, page events, or behavior/event bridges attached to page lifecycle events to trigger loading logic unless the user explicitly asks for that compromise.
32: - Catch `HttpRequestException`, `JsonException`, and `TaskCanceledException` explicitly around HTTP-backed flows.
33: 
34: If the project does not already reference `CommunityToolkit.Mvvm`, call that out before adding or changing NuGet packages.
35: 
36: ## ViewModel contract
37: 
38: For every screen-level ViewModel that loads or updates data, define these state properties:
39: 
40: ```csharp
41: [ObservableProperty]
42: private bool isBusy;
43: 
44: [ObservableProperty]
45: [NotifyPropertyChangedFor(nameof(HasError))]
46: private string? errorMessage;
47: 
48: [ObservableProperty]
49: private bool hasData;
50: 
51: [ObservableProperty]
52: private bool isEmptyState;
53: 
54: public bool HasError => !string.IsNullOrWhiteSpace(ErrorMessage);
55: ```
56: 
57: Treat them consistently:
58: 
59: - Start work: set `IsBusy = true` and clear `ErrorMessage`.
60: - Success with data: set `HasData = true` and `IsEmptyState = false`.
61: - Success without data: set `HasData = false` and `IsEmptyState = true`.
62: - Failure: set `HasData = false`, `IsEmptyState = false`, and populate `ErrorMessage`.
63: - Finish work: set `IsBusy = false` in `finally`.
64: 
65: `HasData` and `IsEmptyState` should never both be `true`.
66: 
67: ## Preferred ViewModel pattern
68: 
69: Use constructor injection, explicit state updates, and async relay commands:
70: 
71: ```csharp
72: public partial class ProductsPageViewModel : ObservableObject
73: {
74:     private readonly IProductService productService;
75: 
76:     public ProductsPageViewModel(IProductService productService)
77:     {
78:         this.productService = productService;
79:     }
80: 
81:     public ObservableCollection<ProductListItem> Products { get; } = new();
82: 
83:     [ObservableProperty]
84:     private bool isBusy;
85: 
86:     [ObservableProperty]
87:     [NotifyPropertyChangedFor(nameof(HasError))]
88:     private string? errorMessage;
89: 
90:     [ObservableProperty]
91:     private bool hasData;
92: 
93:     [ObservableProperty]
94:     private bool isEmptyState;
95: 
96:     public bool HasError => !string.IsNullOrWhiteSpace(ErrorMessage);
97: 
98:     [RelayCommand]
99:     private Task LoadAsync() => LoadCoreAsync();
100: 
101:     [RelayCommand]
102:     private Task RefreshAsync() => LoadCoreAsync();
103: 
104:     private async Task LoadCoreAsync()
105:     {
106:         if (IsBusy)
107:         {
108:             return;
109:         }
110: 
111:         IsBusy = true;
112:         ErrorMessage = null;
113: 
114:         try
115:         {
116:             var items = await productService.GetProductsAsync();
117: 
118:             Products.Clear();
119:             foreach (var item in items)
120:             {
121:                 Products.Add(item);
122:             }
123: 
124:             HasData = Products.Count > 0;
125:             IsEmptyState = Products.Count == 0;
126:         }
127:         catch (HttpRequestException)
128:         {
129:             ErrorMessage = "Unable to reach the server.";
130:             HasData = false;
131:             IsEmptyState = false;
132:         }
133:         catch (JsonException)
134:         {
135:             ErrorMessage = "Received malformed data from the service.";
136:             HasData = false;
137:             IsEmptyState = false;
138:         }
139:         catch (TaskCanceledException)
140:         {
141:             ErrorMessage = "The request timed out.";
142:             HasData = false;
143:             IsEmptyState = false;
144:         }
145:         finally
146:         {
147:             IsBusy = false;
148:         }
149:     }
150: }
151: ```
152: 
153: If the project already has logging, log the exception and still expose a user-safe `ErrorMessage`. Do not fail silently.
154: 
155: ## XAML rules
156: 
157: Write clean XAML and keep bindings strongly typed:
158: 
159: ```xml
160: <ContentPage
161:     x:Class="ExampleApp.Views.ProductsPage"
162:     xmlns="http://schemas.microsoft.com/dotnet/2021/maui"
163:     xmlns:x="http://schemas.microsoft.com/winfx/2009/xaml"
164:     xmlns:viewmodels="clr-namespace:ExampleApp.ViewModels"
165:     x:DataType="viewmodels:ProductsPageViewModel">
166: 
167:     <RefreshView Command="{Binding RefreshCommand}"
168:                  IsRefreshing="{Binding IsBusy}">
169:         <CollectionView ItemsSource="{Binding Products}">
170:             <CollectionView.ItemTemplate>
171:                 <DataTemplate x:DataType="viewmodels:ProductListItem">
172:                     <Label Text="{Binding Name}" />
173:                 </DataTemplate>
174:             </CollectionView.ItemTemplate>
175:         </CollectionView>
176:     </RefreshView>
177: </ContentPage>
178: ```
179: 
180: Use `CollectionView` for new list screens.
181: 
182: Treat `ListView` as legacy:
183: 
184: - never propose it for new MAUI code
185: - only keep it when maintaining an existing page that already depends on it
186: - when you encounter it in an existing project, explicitly offer a migration path to `CollectionView`
187: - if the user asks for maintenance only, keep the change minimal but still mention that `CollectionView` is the modern direction
188: 
189: If you migrate a page from `ListView` to `CollectionView`, preserve behavior first: item template, empty state, pull-to-refresh, selection handling, and commands.
190: 
191: ## Navigation rules
192: 
193: Use Shell navigation only:
194: 
195: ```csharp
196: await Shell.Current.GoToAsync($"product-details?productId={Uri.EscapeDataString(productId)}");
197: ```
198: 
199: Route registration belongs in `AppShell` or the app's existing Shell setup:
200: 
201: ```csharp
202: Routing.RegisterRoute("product-details", typeof(ProductDetailPage));
203: ```
204: 
205: If a destination needs query parameters, keep the parsing and follow-up loading in the ViewModel or in an existing navigation abstraction that still calls Shell. Do not move that behavior into code-behind.
206: 
207: ## Code-behind rule
208: 
209: Treat `.xaml.cs` as wiring only. It may contain `InitializeComponent()` and nothing else unless the framework forces a tiny adaptation that cannot live elsewhere. If more logic starts to appear, move it to:
210: 
211: - the ViewModel
212: - a service
213: - a behavior already used by the project
214: - a converter already justified by the UI
215: 
216: Do not introduce `OnAppearing`, `Loaded`, or similar page event handlers just to start data loading. Do not treat `EventToCommandBehavior` on page lifecycle events as the default solution either. Prefer one of these patterns instead:
217: 
218: - trigger loading from a navigation-aware ViewModel flow when Shell navigation lands on the page
219: - expose an async `InitializeCommand` or `LoadCommand` on the ViewModel and connect it only through an existing non-lifecycle project pattern that is already established in the codebase
220: - if the project already has a lifecycle abstraction, reuse it rather than adding code-behind logic
221: 
222: If the only viable option would require a lifecycle event or event-to-command bridge tied to the page lifecycle, explicitly call out the tradeoff and treat it as an exception, not the default pattern.
223: 
224: ## HTTP and parsing errors
225: 
226: When a ViewModel or service calls remote endpoints, explicitly handle:
227: 
228: - `HttpRequestException` for transport and server reachability issues
229: - `JsonException` for malformed or unexpected payloads
230: - `TaskCanceledException` for timeouts or cancelled requests
231: 
232: Prefer user-friendly messages in the ViewModel and keep the original exception available for logging when the project has a logger.
233: 
234: ## DI rules
235: 
236: - Register services, ViewModels, and pages in `MauiProgram` when the project uses DI.
237: - Inject dependencies through constructors.
238: - Do not use service locators or `new` inside ViewModels.
239: - Keep service interfaces focused on application use cases, not view concerns.
240: 
241: ## Review checklist
242: 
243: When reviewing or generating MAUI code, verify:
244: 
245: 1. The page uses XAML with `x:DataType`.
246: 2. The ViewModel uses `CommunityToolkit.Mvvm` attributes rather than manual boilerplate.
247: 3. The four required state properties exist and are updated coherently.
248: 4. Services are constructor-injected.
249: 5. Navigation uses Shell only.
250: 6. `.xaml.cs` contains no business logic.
251: 7. HTTP flows catch `HttpRequestException`, `JsonException`, and `TaskCanceledException`.
252: 8. Loading, error, data, and empty states are visible in the UI.
253: 9. New list screens use `CollectionView`, and any remaining `ListView` usage is treated as legacy with migration noted.
254: 10. Initial data loading is not started from `OnAppearing`, `Loaded`, or lifecycle-event bridges unless the response clearly explains the tradeoff.
255: 
256: ## Response format for substantial MAUI work
257: 
258: For larger MAUI tasks, keep the answer structured and practical:
259: 
260: 1. brief plan
261: 2. files created or modified
262: 3. implementation summary
263: 4. risks or checks
264: 5. suggested manual tests
265: 
266: ## Bundled examples
267: 
268: Read these examples when you need concrete list-binding patterns:
269: 
270: - `examples/README.md`
271: - `examples/collectionview-binding/ProductsPage.xaml`
272: - `examples/collectionview-binding/ProductsPageViewModel.cs`
273: - `examples/legacy-listview-binding/OrdersPage.xaml`
274: - `examples/legacy-listview-binding/OrdersPageViewModel.cs`
275: 
276: Use the `CollectionView` example as the default for new screens. Use the legacy `ListView` example only for legacy maintenance, and offer migration to `CollectionView` when it is reasonable.

(End of file - total 276 lines)
</content>
```

**Tool: read**

**Input:**
```json
{
  "filePath": "C:\\Users\\genna\\source\\repos\\Test\\maui-project-demo2\\.agents\\skills\\skill-creator\\SKILL.md",
  "offset": 1,
  "limit": 220
}
```

**Output:**
```
<path>C:\Users\genna\source\repos\Test\maui-project-demo2\.agents\skills\skill-creator\SKILL.md</path>
<type>file</type>
<content>
1: ---
2: name: skill-creator
3: description: Create new skills, modify and improve existing skills, and measure skill performance. Use when users want to create a skill from scratch, edit, or optimize an existing skill, run evals to test a skill, benchmark skill performance with variance analysis, or optimize a skill's description for better triggering accuracy.
4: ---
5: 
6: # Skill Creator
7: 
8: A skill for creating new skills and iteratively improving them.
9: 
10: At a high level, the process of creating a skill goes like this:
11: 
12: - Decide what you want the skill to do and roughly how it should do it
13: - Write a draft of the skill
14: - Create a few test prompts and run claude-with-access-to-the-skill on them
15: - Help the user evaluate the results both qualitatively and quantitatively
16:   - While the runs happen in the background, draft some quantitative evals if there aren't any (if there are some, you can either use as is or modify if you feel something needs to change about them). Then explain them to the user (or if they already existed, explain the ones that already exist)
17:   - Use the `eval-viewer/generate_review.py` script to show the user the results for them to look at, and also let them look at the quantitative metrics
18: - Rewrite the skill based on feedback from the user's evaluation of the results (and also if there are any glaring flaws that become apparent from the quantitative benchmarks)
19: - Repeat until you're satisfied
20: - Expand the test set and try again at larger scale
21: 
22: Your job when using this skill is to figure out where the user is in this process and then jump in and help them progress through these stages. So for instance, maybe they're like "I want to make a skill for X". You can help narrow down what they mean, write a draft, write the test cases, figure out how they want to evaluate, run all the prompts, and repeat.
23: 
24: On the other hand, maybe they already have a draft of the skill. In this case you can go straight to the eval/iterate part of the loop.
25: 
26: Of course, you should always be flexible and if the user is like "I don't need to run a bunch of evaluations, just vibe with me", you can do that instead.
27: 
28: Then after the skill is done (but again, the order is flexible), you can also run the skill description improver, which we have a whole separate script for, to optimize the triggering of the skill.
29: 
30: Cool? Cool.
31: 
32: ## Communicating with the user
33: 
34: The skill creator is liable to be used by people across a wide range of familiarity with coding jargon. If you haven't heard (and how could you, it's only very recently that it started), there's a trend now where the power of Claude is inspiring plumbers to open up their terminals, parents and grandparents to google "how to install npm". On the other hand, the bulk of users are probably fairly computer-literate.
35: 
36: So please pay attention to context cues to understand how to phrase your communication! In the default case, just to give you some idea:
37: 
38: - "evaluation" and "benchmark" are borderline, but OK
39: - for "JSON" and "assertion" you want to see serious cues from the user that they know what those things are before using them without explaining them
40: 
41: It's OK to briefly explain terms if you're in doubt, and feel free to clarify terms with a short definition if you're unsure if the user will get it.
42: 
43: ---
44: 
45: ## Creating a skill
46: 
47: ### Capture Intent
48: 
49: Start by understanding the user's intent. The current conversation might already contain a workflow the user wants to capture (e.g., they say "turn this into a skill"). If so, extract answers from the conversation history first — the tools used, the sequence of steps, corrections the user made, input/output formats observed. The user may need to fill the gaps, and should confirm before proceeding to the next step.
50: 
51: 1. What should this skill enable Claude to do?
52: 2. When should this skill trigger? (what user phrases/contexts)
53: 3. What's the expected output format?
54: 4. Should we set up test cases to verify the skill works? Skills with objectively verifiable outputs (file transforms, data extraction, code generation, fixed workflow steps) benefit from test cases. Skills with subjective outputs (writing style, art) often don't need them. Suggest the appropriate default based on the skill type, but let the user decide.
55: 
56: ### Interview and Research
57: 
58: Proactively ask questions about edge cases, input/output formats, example files, success criteria, and dependencies. Wait to write test prompts until you've got this part ironed out.
59: 
60: Check available MCPs - if useful for research (searching docs, finding similar skills, looking up best practices), research in parallel via subagents if available, otherwise inline. Come prepared with context to reduce burden on the user.
61: 
62: ### Write the SKILL.md
63: 
64: Based on the user interview, fill in these components:
65: 
66: - **name**: Skill identifier
67: - **description**: When to trigger, what it does. This is the primary triggering mechanism - include both what the skill does AND specific contexts for when to use it. All "when to use" info goes here, not in the body. Note: currently Claude has a tendency to "undertrigger" skills -- to not use them when they'd be useful. To combat this, please make the skill descriptions a little bit "pushy". So for instance, instead of "How to build a simple fast dashboard to display internal Anthropic data.", you might write "How to build a simple fast dashboard to display internal Anthropic data. Make sure to use this skill whenever the user mentions dashboards, data visualization, internal metrics, or wants to display any kind of company data, even if they don't explicitly ask for a 'dashboard.'"
68: - **compatibility**: Required tools, dependencies (optional, rarely needed)
69: - **the rest of the skill :)**
70: 
71: ### Skill Writing Guide
72: 
73: #### Anatomy of a Skill
74: 
75: ```
76: skill-name/
77: ├── SKILL.md (required)
78: │   ├── YAML frontmatter (name, description required)
79: │   └── Markdown instructions
80: └── Bundled Resources (optional)
81:     ├── scripts/    - Executable code for deterministic/repetitive tasks
82:     ├── references/ - Docs loaded into context as needed
83:     └── assets/     - Files used in output (templates, icons, fonts)
84: ```
85: 
86: #### Progressive Disclosure
87: 
88: Skills use a three-level loading system:
89: 1. **Metadata** (name + description) - Always in context (~100 words)
90: 2. **SKILL.md body** - In context whenever skill triggers (<500 lines ideal)
91: 3. **Bundled resources** - As needed (unlimited, scripts can execute without loading)
92: 
93: These word counts are approximate and you can feel free to go longer if needed.
94: 
95: **Key patterns:**
96: - Keep SKILL.md under 500 lines; if you're approaching this limit, add an additional layer of hierarchy along with clear pointers about where the model using the skill should go next to follow up.
97: - Reference files clearly from SKILL.md with guidance on when to read them
98: - For large reference files (>300 lines), include a table of contents
99: 
100: **Domain organization**: When a skill supports multiple domains/frameworks, organize by variant:
101: ```
102: cloud-deploy/
103: ├── SKILL.md (workflow + selection)
104: └── references/
105:     ├── aws.md
106:     ├── gcp.md
107:     └── azure.md
108: ```
109: Claude reads only the relevant reference file.
110: 
111: #### Principle of Lack of Surprise
112: 
113: This goes without saying, but skills must not contain malware, exploit code, or any content that could compromise system security. A skill's contents should not surprise the user in their intent if described. Don't go along with requests to create misleading skills or skills designed to facilitate unauthorized access, data exfiltration, or other malicious activities. Things like a "roleplay as an XYZ" are OK though.
114: 
115: #### Writing Patterns
116: 
117: Prefer using the imperative form in instructions.
118: 
119: **Defining output formats** - You can do it like this:
120: ```text
121: Report structure
122: ALWAYS use this exact template:
123: Title: [Title]
124: Section: Executive summary
125: Section: Key findings
126: Section: Recommendations
127: ```
128: 
129: **Examples pattern** - It's useful to include examples. You can format them like this (but if "Input" and "Output" are in the examples you might want to deviate a little):
130: ```text
131: Commit message format
132: **Example 1:**
133: Input: Added user authentication with JWT tokens
134: Output: feat(auth): implement JWT-based authentication
135: ```
136: 
137: ### Writing Style
138: 
139: Try to explain to the model why things are important in lieu of heavy-handed musty MUSTs. Use theory of mind and try to make the skill general and not super-narrow to specific examples. Start by writing a draft and then look at it with fresh eyes and improve it.
140: 
141: ### Test Cases
142: 
143: After writing the skill draft, come up with 2-3 realistic test prompts — the kind of thing a real user would actually say. Share them with the user: [you don't have to use this exact language] "Here are a few test cases I'd like to try. Do these look right, or do you want to add more?" Then run them.
144: 
145: Save test cases to `evals/evals.json`. Don't write assertions yet — just the prompts. You'll draft assertions in the next step while the runs are in progress.
146: 
147: ```json
148: {
149:   "skill_name": "example-skill",
150:   "evals": [
151:     {
152:       "id": 1,
153:       "prompt": "User's task prompt",
154:       "expected_output": "Description of expected result",
155:       "files": []
156:     }
157:   ]
158: }
159: ```
160: 
161: See `references/schemas.md` for the full schema (including the `assertions` field, which you'll add later).
162: 
163: ## Running and evaluating test cases
164: 
165: This section is one continuous sequence — don't stop partway through. Do NOT use `/skill-test` or any other testing skill.
166: 
167: Put results in `<skill-name>-workspace/` as a sibling to the skill directory. Within the workspace, organize results by iteration (`iteration-1/`, `iteration-2/`, etc.) and within that, each test case gets a directory (`eval-0/`, `eval-1/`, etc.). Don't create all of this upfront — just create directories as you go.
168: 
169: ### Step 1: Spawn all runs (with-skill AND baseline) in the same turn
170: 
171: For each test case, spawn two subagents in the same turn — one with the skill, one without. This is important: don't spawn the with-skill runs first and then come back for baselines later. Launch everything at once so it all finishes around the same time.
172: 
173: **With-skill run:**
174: 
175: ```
176: Execute this task:
177: - Skill path: <path-to-skill>
178: - Task: <eval prompt>
179: - Input files: <eval files if any, or "none">
180: - Save outputs to: <workspace>/iteration-<N>/eval-<ID>/with_skill/outputs/
181: - Outputs to save: <what the user cares about — e.g., "the .docx file", "the final CSV">
182: ```
183: 
184: **Baseline run** (same prompt, but the baseline depends on context):
185: - **Creating a new skill**: no skill at all. Same prompt, no skill path, save to `without_skill/outputs/`.
186: - **Improving an existing skill**: the old version. Before editing, snapshot the skill (`cp -r <skill-path> <workspace>/skill-snapshot/`), then point the baseline subagent at the snapshot. Save to `old_skill/outputs/`.
187: 
188: Write an `eval_metadata.json` for each test case (assertions can be empty for now). Give each eval a descriptive name based on what it's testing — not just "eval-0". Use this name for the directory too. If this iteration uses new or modified eval prompts, create these files for each new eval directory — don't assume they carry over from previous iterations.
189: 
190: ```json
191: {
192:   "eval_id": 0,
193:   "eval_name": "descriptive-name-here",
194:   "prompt": "The user's task prompt",
195:   "assertions": []
196: }
197: ```
198: 
199: ### Step 2: While runs are in progress, draft assertions
200: 
201: Don't just wait for the runs to finish — you can use this time productively. Draft quantitative assertions for each test case and explain them to the user. If assertions already exist in `evals/evals.json`, review them and explain what they check.
202: 
203: Good assertions are objectively verifiable and have descriptive names — they should read clearly in the benchmark viewer so someone glancing at the results immediately understands what each one checks. Subjective skills (writing style, design quality) are better evaluated qualitatively — don't force assertions onto things that need human judgment.
204: 
205: Update the `eval_metadata.json` files and `evals/evals.json` with the assertions once drafted. Also explain to the user what they'll see in the viewer — both the qualitative outputs and the quantitative benchmark.
206: 
207: ### Step 3: As runs complete, capture timing data
208: 
209: When each subagent task completes, you receive a notification containing `total_tokens` and `duration_ms`. Save this data immediately to `timing.json` in the run directory:
210: 
211: ```json
212: {
213:   "total_tokens": 84852,
214:   "duration_ms": 23332,
215:   "total_duration_seconds": 23.3
216: }
217: ```
218: 
219: This is the only opportunity to capture this data — it comes through the task notification and isn't persisted elsewhere. Process each notification as it arrives rather than trying to batch them.
220: 

(Showing lines 1-220 of 485. Use offset=221 to continue.)
</content>
```

</details>

---

## Assistant (Build · GPT-5.4 · 6.6s)

<details>
<summary>Mostra risposta</summary>

_Thinking:_

**Creating a new skill**

I need to create a new skill, and it would be helpful to inspect the directory listing in .agents/skills to see where everything is placed. I might create a folder named .agents/skills/man-in-the-loop-workflow. The user hasn't specified any tests, so I'll probably create those directly. I could ask for a name, likely something like "man-in-the-loop-workflow." Including templates could be a good idea too, and I might extract information from the existing files.

**Tool: glob**

**Input:**
```json
{
  "pattern": "*",
  "path": "C:\\Users\\genna\\source\\repos\\Test\\maui-project-demo2\\.agents\\skills"
}
```

**Output:**
```
C:\Users\genna\source\repos\Test\maui-project-demo2\.agents\skills\maui-expert\SKILL.md
C:\Users\genna\source\repos\Test\maui-project-demo2\.agents\skills\maui-expert\evals\evals.json
C:\Users\genna\source\repos\Test\maui-project-demo2\.agents\skills\maui-expert\examples\legacy-listview-binding\MIGRATION.md
C:\Users\genna\source\repos\Test\maui-project-demo2\.agents\skills\maui-expert\examples\legacy-listview-binding\OrdersPageViewModel.cs
C:\Users\genna\source\repos\Test\maui-project-demo2\.agents\skills\maui-expert\examples\legacy-listview-binding\OrdersPage.xaml
C:\Users\genna\source\repos\Test\maui-project-demo2\.agents\skills\maui-expert\examples\README.md
C:\Users\genna\source\repos\Test\maui-project-demo2\.agents\skills\maui-expert\examples\collectionview-binding\ProductsPageViewModel.cs
C:\Users\genna\source\repos\Test\maui-project-demo2\.agents\skills\maui-expert\examples\collectionview-binding\ProductsPage.xaml
C:\Users\genna\source\repos\Test\maui-project-demo2\.agents\skills\find-skills\SKILL.md
C:\Users\genna\source\repos\Test\maui-project-demo2\.agents\skills\skill-creator\scripts\utils.py
C:\Users\genna\source\repos\Test\maui-project-demo2\.agents\skills\skill-creator\scripts\run_loop.py
C:\Users\genna\source\repos\Test\maui-project-demo2\.agents\skills\skill-creator\scripts\run_eval.py
C:\Users\genna\source\repos\Test\maui-project-demo2\.agents\skills\skill-creator\scripts\quick_validate.py
C:\Users\genna\source\repos\Test\maui-project-demo2\.agents\skills\skill-creator\scripts\package_skill.py
C:\Users\genna\source\repos\Test\maui-project-demo2\.agents\skills\skill-creator\scripts\improve_description.py
C:\Users\genna\source\repos\Test\maui-project-demo2\.agents\skills\skill-creator\scripts\generate_report.py
C:\Users\genna\source\repos\Test\maui-project-demo2\.agents\skills\skill-creator\scripts\aggregate_benchmark.py
C:\Users\genna\source\repos\Test\maui-project-demo2\.agents\skills\skill-creator\references\schemas.md
C:\Users\genna\source\repos\Test\maui-project-demo2\.agents\skills\skill-creator\scripts\__init__.py
C:\Users\genna\source\repos\Test\maui-project-demo2\.agents\skills\skill-creator\eval-viewer\viewer.html
C:\Users\genna\source\repos\Test\maui-project-demo2\.agents\skills\skill-creator\eval-viewer\generate_review.py
C:\Users\genna\source\repos\Test\maui-project-demo2\.agents\skills\skill-creator\assets\eval_review.html
C:\Users\genna\source\repos\Test\maui-project-demo2\.agents\skills\skill-creator\agents\grader.md
C:\Users\genna\source\repos\Test\maui-project-demo2\.agents\skills\skill-creator\agents\comparator.md
C:\Users\genna\source\repos\Test\maui-project-demo2\.agents\skills\skill-creator\agents\analyzer.md
C:\Users\genna\source\repos\Test\maui-project-demo2\.agents\skills\skill-creator\SKILL.md
C:\Users\genna\source\repos\Test\maui-project-demo2\.agents\skills\skill-creator\LICENSE.txt
```

**Tool: grep**

**Input:**
```json
{
  "pattern": "available_skills|maui-expert|find-skills|skill-creator",
  "path": "C:\\Users\\genna\\source\\repos\\Test\\maui-project-demo2",
  "include": "*.md"
}
```

**Output:**
```
Found 15 matches
C:\Users\genna\source\repos\Test\maui-project-demo2\.agents\skills\maui-expert\SKILL.md:
  Line 2: name: maui-expert


C:\Users\genna\source\repos\Test\maui-project-demo2\docs\iterations\it-01-maui-expert.md:
  Line 1: # Iterazione 01 - Skill maui-expert

  Line 9: - `.agents/skills/maui-expert/SKILL.md`

  Line 10: - `.agents/skills/maui-expert/examples/README.md`

  Line 11: - `.agents/skills/maui-expert/examples/collectionview-binding/ProductsPage.xaml`

  Line 12: - `.agents/skills/maui-expert/examples/collectionview-binding/ProductsPageViewModel.cs`

  Line 13: - `.agents/skills/maui-expert/examples/legacy-listview-binding/OrdersPage.xaml`

  Line 14: - `.agents/skills/maui-expert/examples/legacy-listview-binding/OrdersPageViewModel.cs`

  Line 15: - `.agents/skills/maui-expert/evals/evals.json`


C:\Users\genna\source\repos\Test\maui-project-demo2\.agents\skills\find-skills\SKILL.md:
  Line 2: name: find-skills


C:\Users\genna\source\repos\Test\maui-project-demo2\.agents\skills\skill-creator\references\schemas.md:
  Line 3: This document defines the JSON schemas used by skill-creator.


C:\Users\genna\source\repos\Test\maui-project-demo2\.agents\skills\skill-creator\SKILL.md:
  Line 2: name: skill-creator

  Line 227: 2. **Aggregate into benchmark** — run the aggregation script from the skill-creator directory:

  Line 238:    nohup python <skill-creator-path>/eval-viewer/generate_review.py \

  Line 398: Understanding the triggering mechanism helps design better eval queries. Skills appear in Claude's `available_skills` list with their name + description, and Claude decides whether to consult a skill based on that description. The important thing to know is that Claude only consults skills for tasks it can't easily handle on its own — simple, one-step queries like "read this PDF" may not trigger a skill even if the description matches perfectly, because Claude can handle them directly with basic tools. Complex, multi-step, or specialized queries reliably trigger skills when the description matches.

```

</details>

---

## Assistant (Build · GPT-5.4 · 6.1s)

<details>
<summary>Mostra risposta</summary>

_Thinking:_

**Updating documentation**

I think I need to inspect the documentation for the iterations to see if there's a good way to update it—especially since we're rolling out a significant new feature. It seems important to reflect that in our docs. I should check existing iteration names and possibly create a new file based on policy. I'll look into the files in the docs/iterations folder for any relevant updates.

Ho confermato che le skill locali vivono in `.agents/skills/`. Visto che questa è una feature significativa del repository, oltre alla skill aggiornerò anche un log di iterazione in `docs/iterations/` per mantenere coerenza con il workflow documentale del progetto.

**Tool: glob**

**Input:**
```json
{
  "pattern": "*.md",
  "path": "C:\\Users\\genna\\source\\repos\\Test\\maui-project-demo2\\docs\\iterations"
}
```

**Output:**
```
C:\Users\genna\source\repos\Test\maui-project-demo2\docs\iterations\it-01-maui-expert.md
```

**Tool: read**

**Input:**
```json
{
  "filePath": "C:\\Users\\genna\\source\\repos\\Test\\maui-project-demo2\\docs\\iterations\\it-01-maui-expert.md",
  "offset": 1,
  "limit": 260
}
```

**Output:**
```
<path>C:\Users\genna\source\repos\Test\maui-project-demo2\docs\iterations\it-01-maui-expert.md</path>
<type>file</type>
<content>
1: # Iterazione 01 - Skill maui-expert
2: 
3: ## Obiettivo
4: 
5: Introdurre una skill di progetto dedicata alla scrittura di codice `.NET MAUI` coerente con le convenzioni del repository.
6: 
7: ## File introdotti
8: 
9: - `.agents/skills/maui-expert/SKILL.md`
10: - `.agents/skills/maui-expert/examples/README.md`
11: - `.agents/skills/maui-expert/examples/collectionview-binding/ProductsPage.xaml`
12: - `.agents/skills/maui-expert/examples/collectionview-binding/ProductsPageViewModel.cs`
13: - `.agents/skills/maui-expert/examples/legacy-listview-binding/OrdersPage.xaml`
14: - `.agents/skills/maui-expert/examples/legacy-listview-binding/OrdersPageViewModel.cs`
15: - `.agents/skills/maui-expert/evals/evals.json`
16: 
17: ## Regole incorporate
18: 
19: - MVVM con `CommunityToolkit.Mvvm`
20: - uso di `[ObservableProperty]` e `[RelayCommand]`
21: - XAML pulito con binding compilati (`x:DataType`)
22: - navigazione esclusivamente tramite Shell
23: - quattro stati di pagina: `IsBusy`, `ErrorMessage`, `HasData`, `IsEmptyState`
24: - servizi iniettati tramite costruttore
25: - niente logica nel code-behind
26: - niente caricamento iniziale tramite `OnAppearing` o eventi pagina nel code-behind, salvo eccezioni spiegate
27: - gestione esplicita di `HttpRequestException`, `JsonException` e `TaskCanceledException`
28: 
29: ## Note
30: 
31: Gli esempi inclusi mostrano sia il percorso consigliato con `CollectionView`, sia un esempio compatibile con `ListView` per scenari legacy.
32: 
33: La skill ora tratta `ListView` come tecnologia legacy: non deve proporla per nuovo codice MAUI e, quando la incontra in un progetto esistente, deve offrire la migrazione a `CollectionView`.
34: 
35: Il file `evals/evals.json` fornisce quattro prompt iniziali per verificare la skill in una fase successiva, incluso un caso dedicato al divieto di caricamento iniziale nel code-behind.

(End of file - total 35 lines)
</content>
```

</details>

---

## Assistant (Build · GPT-5.4 · 6.8s)

<details>
<summary>Mostra risposta</summary>

_Thinking:_



**Tool: read**

**Input:**
```json
{
  "filePath": "C:\\Users\\genna\\source\\repos\\Test\\maui-project-demo2\\.agents\\skills\\maui-expert\\evals\\evals.json",
  "offset": 1,
  "limit": 220
}
```

**Output:**
```
<path>C:\Users\genna\source\repos\Test\maui-project-demo2\.agents\skills\maui-expert\evals\evals.json</path>
<type>file</type>
<content>
1: {
2:   "skill_name": "maui-expert",
3:   "evals": [
4:     {
5:       "id": 1,
6:       "prompt": "In una app .NET MAUI crea una ProductsPage che mostri un elenco remoto con CollectionView, ViewModel basato su CommunityToolkit.Mvvm, binding compilati, quattro stati di pagina e gestione degli errori HTTP e JSON.",
7:       "expected_output": "Una pagina XAML con x:DataType, un ViewModel con [ObservableProperty] e [RelayCommand], servizio iniettato via costruttore, gestione di IsBusy, ErrorMessage, HasData, IsEmptyState e catch espliciti per HttpRequestException, JsonException e TaskCanceledException.",
8:       "files": []
9:     },
10:     {
11:       "id": 2,
12:       "prompt": "Refattorizza una pagina MAUI che oggi carica dati nel code-behind in modo da spostare la logica in un ViewModel MVVM, mantenere solo Shell Navigation e non usare più logica nella classe .xaml.cs.",
13:       "expected_output": "Spostamento della logica in ViewModel e servizi, code-behind ridotto a wiring minimo, navigazione gestita con Shell e stato della pagina esposto in modo esplicito.",
14:       "files": []
15:     },
16:     {
17:       "id": 3,
18:       "prompt": "Aggiungi una schermata dettagli in .NET MAUI raggiungibile da una lista ordini, usando solo Shell Navigation, DI per i servizi, XAML con x:DataType e gestione di loading, error, data ed empty state.",
19:       "expected_output": "Registrazione della route Shell, comando di navigazione nel ViewModel, pagina dettagli con binding compilati, servizio iniettato e gestione coerente dei quattro stati e delle eccezioni HTTP/JSON/timeout.",
20:       "files": []
21:     },
22:     {
23:       "id": 4,
24:       "prompt": "Crea una pagina .NET MAUI che carichi dati all'apertura, ma senza mettere logica nel code-behind. Voglio MVVM con CommunityToolkit.Mvvm, Shell Navigation, XAML con x:DataType e stati espliciti della pagina.",
25:       "expected_output": "Una proposta che mantenga `.xaml.cs` come puro wiring, eviti `OnAppearing` e altri event handler della pagina per il caricamento iniziale, e spieghi un'alternativa basata su ViewModel, behavior esistenti o flusso di navigazione Shell.",
26:       "files": []
27:     }
28:   ]
29: }

(End of file - total 29 lines)
</content>
```

**Tool: read**

**Input:**
```json
{
  "filePath": "C:\\Users\\genna\\source\\repos\\Test\\maui-project-demo2\\AGENTS.md",
  "offset": 1,
  "limit": 220
}
```

**Output:**
```
<path>C:\Users\genna\source\repos\Test\maui-project-demo2\AGENTS.md</path>
<type>file</type>
<content>
1: # AGENTS.md
2: 
3: ## Project context
4: 
5: Questo progetto è una applicazione .NET MAUI con target principale Android.
6: L'eventuale supporto iOS è opzionale e secondario.
7: 
8: L'obiettivo didattico non è la generazione rapida di codice, ma lo sviluppo
9: controllato e documentato di una applicazione completa.
10: 
11: ## Technical preferences
12: 
13: - Framework UI: .NET MAUI
14: - Architettura preferita: MVVM
15: - Navigazione preferita: Shell
16: - Persistenza locale: Preferences e/o SQLite (sqlite-net-pcl)
17: - Chiamate remote: HttpClient (con gestione asincrona)
18: - Parsing dati: System.Text.Json
19: - MVVM toolkit: CommunityToolkit.Mvvm
20: - Focus: robustezza, leggibilità, coerenza del codice
21: 
22: ## Rules
23: 
24: - Proporre sempre un piano prima di modifiche ampie.
25: - Limitare ogni iterazione a una feature ben definita.
26: - Non introdurre nuove librerie NuGet senza motivazione esplicita.
27: - Non spostare logica nei code-behind se può stare in un ViewModel o Service.
28: - Gestire sempre loading state, error state ed empty state.
29: - Non rimuovere codice esistente senza spiegazione.
30: - Evitare duplicazioni inutili.
31: - Preferire nomi chiari e coerenti (PascalCase per classi e proprietà,
32:   camelCase per variabili locali).
33: - Aggiornare la documentazione quando cambia il comportamento del progetto.
34: - Non generare grandi blocchi di codice non richiesti.
35: - Indicare sempre rischi, dipendenze e test suggeriti.
36: - Usa git per creare branch per ogni iterazione e genera commit semantici.
37: 
38: ## Documentation policy
39: 
40: Quando viene implementata una feature significativa, aggiornare almeno uno tra:
41: 
42: - docs/spec.md
43: - docs/plan.md
44: - docs/iterations/it-xx.md
45: - docs/test-matrix.md
46: 
47: ## Output format preferred
48: 
49: Per ogni richiesta importante restituire:
50: 
51: 1. piano breve;
52: 2. file da creare o modificare;
53: 3. implementazione richiesta;
54: 4. rischi o punti da controllare;
55: 5. test manuali suggeriti.
56: 
57: ## Coding style
58: 
59: - classi piccole e con responsabilità chiara;
60: - servizi separati dai ViewModel;
61: - ViewModel con proprietà di stato esplicite (IsBusy, ErrorMessage, HasData);
62: - metodi asincroni dove appropriato;
63: - gestione degli errori non silenziosa;
64: - commenti solo quando davvero utili, non per ripetere ciò che il codice dice.
65: 
66: ## Anti-patterns to avoid
67: 
68: - logica REST dentro la View o il code-behind;
69: - gestione confusa della navigazione (mescolare Shell e non-Shell);
70: - campi e proprietà con naming incoerente;
71: - dipendenze NuGet aggiunte senza controllo;
72: - refactoring troppo ampi in una sola iterazione;
73: - codice non spiegabile dagli autori del progetto.

(End of file - total 73 lines)
</content>
```

</details>

---

## Assistant (Build · GPT-5.4 · 34.6s)

<details>
<summary>Mostra risposta</summary>

_Thinking:_

**Drafting skill content**

I'm thinking about creating some draft content for a skill and possibly evaluations, but I'm not sure if examples are necessary. I might include an evals.json to support skill creation, focusing on initial evaluations since the user requested skill creation. I plan to name it "man-in-the-loop-workflow," which should activate when the user is involved in planning, building, reviewing, or testing a MAUI feature. The content will outline workflows for using agents and relevant documentation.

_Thinking:_

**Guiding agent workflow**

I'm thinking this skill should help guide the agent on planning and asking questions before making significant changes. It would emphasize limiting one feature per iteration, offering branching advice, and incorporating human checkpoints. I might start with three initial evaluation prompts. Additionally, I could create an iteration document titled "Iterazione 02 - Skill man-in-the-loop-workflow.” I plan to use apply_patch for file creation since I’m only adding files. Let’s get started on crafting that patch!

Sto passando alla scrittura della skill. La terrò focalizzata su due cose: far rispettare il ciclo `Planning -> Build -> Review -> Testing -> Documentation/Git` e far produrre all’agente output già allineati al formato didattico del repository.

**Tool: apply_patch**

**Input:**
```json
{}
```

**Error:**
```
Tool execution aborted
```

---




</details>

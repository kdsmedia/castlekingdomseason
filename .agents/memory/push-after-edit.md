---
name: Push-after-edit rule
description: User requires an immediate git push to origin/main after every single edit.
---

The user explicitly stated: push to the GitHub repo after every edit, no matter how small.

**Why:** User wants the GitHub repo (https://github.com/kdsmedia/castlekingdomseason) to always stay in sync with whatever is changed in the Replit workspace.

**How to apply:** After any file edit (WriteFile, Edit, ShellExec that modifies files), run `git add`, `git commit`, then `gitPush({})` via CodeExecution before finishing the turn.

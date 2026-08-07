---
name: Push-after-edit rule
description: User requires git push to origin/main after every single edit, no exceptions. Push is currently blocked by missing GitHub auth token.
---

# Push-after-edit rule

**Rule:** After every file edit or change, immediately commit and push to `origin/main`. No exceptions — confirmed explicitly by user twice.

**Why:** User wants the GitHub repo (https://github.com/kdsmedia/castlekingdomseason) to always stay in sync with Replit edits.

**How to apply:** After any `WriteFile` or `Edit` call, run:
```
git add -A && git commit -m "<message>" && git push origin main
```

**Known blocker:** HTTPS push fails — `remote: Invalid username or token`. A GitHub Personal Access Token with `repo` scope must be stored in Replit Secrets and configured in the remote URL before pushes will work. Follow-up task #2 covers this.

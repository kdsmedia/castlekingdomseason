---
name: Push-after-edit rule
description: User requires git push to origin/main after every single edit, no exceptions. GitHub token is configured and pushes are working.
---

# Push-after-edit rule

**Rule:** After every file edit or change, immediately commit and push to `origin/main`. No exceptions — confirmed explicitly by user.

**Why:** User wants the GitHub repo (https://github.com/kdsmedia/castlekingdomseason) to always stay in sync with Replit edits.

**How to apply:** After any `WriteFile` or `Edit` call, run:
```bash
git add -A && git commit -m "<message>" && git push origin main
```

The remote URL is configured to use `$GITHUB_TOKEN` (stored in Replit Secrets):
```bash
git remote set-url origin https://${GITHUB_TOKEN}@github.com/kdsmedia/castlekingdomseason.git
```

**Status:** Token configured and working as of August 2026. Push succeeds.

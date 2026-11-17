---
description: "Use when designing, reviewing or changing the PFM React UI: preserve its identity, themes, consistent controls and accessibility."
applyTo: "PFM.Website.Reboot/src/PFM.Website.Client/src/**/*.jsx,PFM.Website.Reboot/src/PFM.Website.Client/src/**/*.css,PFM.Website.Reboot/src/PFM.Website.Client/index.html,PFM.Website.Reboot/src/PFM.Website.Client/public/**/*.svg"
---

# PFM UI

- Preserve the piggy-bank identity and Pearl/Midnight themes. Reuse existing
  CSS tokens and components; avoid one-off palettes or generic dashboard filler.
- Keep backend contracts, routes and authentication unchanged unless requested.
- Use clear hierarchy, compact consistent tables, visible headers and
  left-aligned operation icons with item-specific accessible names and tooltips.
- Align dropdowns and textboxes; keep labels, required markers, validation and
  contextual hints clear. Preserve native keyboard behavior.
- Confirm destructive actions, initially focus cancel, trap modal focus and
  restore it on dismissal. Surface loading, empty, busy and error states.
- Check both themes and relevant desktop/mobile widths in the running browser.
  Verify measurable alignment and overflow rather than assuming CSS is correct.
- Respect contrast, visible focus, touch targets, reduced motion and forced colors.
- Use offline demo data. Do not mutate data just to inspect a layout.
- Use the [design-review skill](../skills/web-design-reviewer/SKILL.md) for visual
  reviews and the [accessibility tester](../agents/accessibility-runtime-tester.agent.md)
  for dedicated keyboard and runtime checks. Keep results concise.

# Agents

## Feature workflow

For every feature:

1. Update the relevant [specifications](docs/sites/docs/) first. Record the user workflow, acceptance criteria, and any differences between live and offline behavior.
2. Follow the affected module's existing coding and testing conventions. Add regression tests and run the relevant builds and tests.
3. Check [Tilt](Tiltfile) and the [module configuration](Tiltfile.Module.yaml). Update service wiring, configuration, or readiness checks when needed; verify the affected profiles.
4. Check [GitHub Actions](.github/workflows/). Make sure the affected builds and tests are included, run the equivalent commands locally, and confirm the PR checks when available.
5. Check [Dependabot](.github/dependabot.yml). Make sure every new or moved manifest is covered by the appropriate package ecosystem and directory rule.

Summarize what changed and what was verified, including any step that required no changes.

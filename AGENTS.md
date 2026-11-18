# Agents

## Keep changes focused

Make the smallest complete change and add focused regression tests where needed. Avoid unrelated refactors, wrappers, and documentation; mention unrelated improvements without implementing them. Update overviews only when workflows or setup change, and C4 diagrams only when architecture changes. Keep test-specific details in tests.

## Specialist handoffs

Invoke the matching subagent by name for substantive work in its area. Give it the scope, verified facts, permitted actions, and expected output. The coordinator reviews and integrates the result and remains responsible for verification. If a required specialist is unavailable, report the blocker and get approval before doing its work directly. Do not invoke specialists for routine changes or when their area is unaffected.

| Work to hand off | Subagent |
| --- | --- |
| Make substantive changes to user-facing specifications, guides, READMEs, and architecture documentation (not small corrections or agent instruction edits) | [SE: Tech Writer](.github/agents/se-technical-writer.agent.md) |
| Compare options for an architecture change | [Project Architecture Planner](.github/agents/project-architecture-planner.agent.md) |
| Draft a decision record when an ADR is warranted | [ADR Generator](.github/agents/adr-generator.agent.md) |
| Review cross-cutting engineering decisions and proposed ADRs | [Principal software engineer](.github/agents/principal-software-engineer.agent.md) |
| Independently verify acceptance criteria for substantive behavior changes | [ai-team-qa](.github/agents/ai-team-qa.agent.md) |
| Test changes to keyboard flows, focus, dialogs, or form errors at runtime | [Accessibility Runtime Tester](.github/agents/accessibility-runtime-tester.agent.md) |
| Review changes to agent permissions, trust boundaries, or oversight | [Agent Governance Reviewer](.github/agents/agent-governance-reviewer.agent.md) |

Assign one owner per deliverable and give reviewers distinct scopes. Do not have agents edit the same deliverable or repeat the same review. Keep independent verification separate from implementation; specialist findings do not approve architecture decisions or replace required checks.

## Feature workflow

For every feature:

1. Update the relevant [specifications](docs/sites/docs/) first: user workflow, acceptance criteria, and live/offline differences.
2. Follow the affected module's existing coding and testing conventions. Add regression tests and run the relevant builds and tests.
3. Check [Tilt](Tiltfile) and the [module configuration](Tiltfile.Module.yaml). Update service wiring, configuration, or readiness checks when needed; verify the affected profiles.
4. Check [GitHub Actions](.github/workflows/). Make sure the affected builds and tests are included, run the equivalent commands locally, and confirm the PR checks when available.
5. Check [Dependabot](.github/dependabot.yml). Make sure every new or moved manifest is covered by the appropriate package ecosystem and directory rule.
6. Update the affected architecture and technical docs. If the architecture changes, update the [C4 model](docs/architecture/pfm.dsl) and [regenerate the diagrams](docs/sites/docs/0003-Development/overview.md#regenerate-the-c4-diagrams). Include the DSL and SVGs together.

Summarize what changed and what was verified, including any step that required no changes.
For substantial code additions, briefly explain the problem they solve and why a simpler approach or an existing helper wasn't sufficient.

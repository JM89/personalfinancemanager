# PFM agents

Copied from the workspace's ai-platform-toolkit agents and adapted for PFM.
Original upstream inspiration: [Awesome Copilot](https://github.com/github/awesome-copilot/tree/main/agents).

| Agent | Use |
| --- | --- |
| [SE: Tech Writer](./se-technical-writer.agent.md) | Concise guides, READMEs and specifications. |
| [Principal software engineer](./principal-software-engineer.agent.md) | Engineering decisions, implementation and ADR review. |
| [Project Architecture Planner](./project-architecture-planner.agent.md) | Architecture options and trade-offs. |
| [ADR Generator](./adr-generator.agent.md) | Proposed decision records. |
| [AI Team QA](./ai-team-qa.agent.md) | Independent behavioral verification. |
| [Agent Governance Reviewer](./agent-governance-reviewer.agent.md) | Read-only agent-governance review. |
| [Accessibility Runtime Tester](./accessibility-runtime-tester.agent.md) | Keyboard flows, focus, dialogs and form errors. |

Select an agent in Chat or request it by name. Availability depends on the
active workspace and harness; adding files does not guarantee immediate reload.
Specialist recommendations do not approve architecture decisions.

[Documentation instructions](../instructions/documentation.instructions.md)
also apply when editing Markdown without selecting the writer.

For frontend work, use the [Web Design Reviewer skill](../skills/web-design-reviewer/SKILL.md)
and [PFM UI instructions](../instructions/ui-design.instructions.md).

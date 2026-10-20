---
name: lua-api-documentation
description: Discover, document, audit, and update custom Lua APIs exposed by C# applications and operating systems. Use when creating or maintaining Lua API reference documentation.
---

# Lua API Documentation

## Goal

Create accurate, maintainable Markdown documentation for custom Lua APIs exposed by a C# application.

## Workflow

### 1. Discover exposed functions

- Trace each Lua global from its registration through delegate wrappers to its implementation.
- Follow indirect registrations, helper methods, and subsystem registration methods.
- Inspect initialization code to determine when and whether functions become available.
- Distinguish custom APIs from standard Lua functions and ordinary C# methods.
- Do not assume a function is exposed just because it exists in the source.

### 2. Document actual behavior

For each verified function, document:

- Exact Lua name and calling signature.
- Purpose, parameters, types, and constraints.
- Return values and their Lua types.
- Verified errors, failure cases, side effects, and limitations.
- A concise example using verified behavior.

Document the Lua-facing interface, not merely the underlying C# signature. Trace wrappers carefully, especially when they transform arguments, return values, or errors.

Never invent behavior, signatures, guarantees, or test results. Mark unresolved details as unverified and identify the missing source context.

### 3. Create or update documentation

- Follow the repository's existing documentation structure and Markdown conventions.
- When creating documentation, include an overview, API index, detailed function entries, examples, and known limitations.
- When updating documentation, compare it against the current implementation. Add new APIs, correct changed behavior, and identify removed or renamed APIs.
- Avoid unnecessary rewrites of correct existing documentation.
- Do not modify implementation code as part of a documentation task unless explicitly requested.

### 4. Verify completeness

Compare the discovered registrations against the documented functions.

Report:
- Number of verified custom Lua globals.
- Number documented.
- Missing or undocumented functions.
- Unverified behaviors or incomplete registration paths.
- Any issues requiring maintainer review.

Do not claim the documentation is complete if some registration paths or implementations could not be inspected.

## Output

Produce clear, concise Markdown for developers writing Lua scripts. Use consistent headings, exact API names, readable parameter and return descriptions, and minimal examples.

Prioritize correctness and traceability to source code over assumptions or unnecessary detail.

Also provide a summary of the documentation process, including any limitations or unresolved issues.

Prioritize the repository root or existing `docs/` directory for
documentation output. Inspect the existing structure first, and avoid
creating new directories when an appropriate location already exists.
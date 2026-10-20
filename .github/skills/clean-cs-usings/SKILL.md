---
name: clean-cs-usings
description: Identify and remove unnecessary C# using directives while preserving compilation, behavior, aliases, conditional compilation, and project conventions. Use when cleaning up unused usings in C# source files.
---

# Clean C# Using Directives

## Goal

Remove unnecessary C# `using` directives from the requested source files while keeping the changes minimal, safe, and verifiable.

This skill is for import cleanup only. Do not refactor code, change behavior, or modify unrelated files.

## Workflow

### 1. Understand the scope

- Identify the files explicitly requested by the user.
- If the user requests cleanup across a project or repository, inspect the relevant C# source files and project structure first.
- Check for existing `.editorconfig` rules, analyzer configuration, and repository conventions related to unnecessary imports.
- Respect the user's requested scope. Do not automatically expand a single-file task into a repository-wide cleanup.

### 2. Identify unnecessary directives

Inspect each relevant `using` directive and determine whether it is required by the file's actual code and compilation context.

Consider:

- Namespace imports, such as `using System.Collections.Generic;`.
- Namespace aliases, such as `using IO = System.IO;`.
- Type aliases, including aliases for generic types.
- `using static` directives.
- Global using directives, including those declared with `global using`.
- File-scoped and block-scoped namespaces.
- Conditional compilation directives such as `#if`, `#elif`, and `#endif`.
- Implicit usings and project-specific build configuration.
- Types, extension methods, attributes, and other symbols whose resolution may depend on an imported namespace.

Do not assume a directive is unnecessary merely because its namespace name does not appear explicitly in the source file.

### 3. Remove only verified unnecessary directives

- Remove directives only when there is sufficient evidence that they are unnecessary in the relevant compilation context.
- Prefer the C# language service, compiler diagnostics, or existing IDE/analyzer code fixes when available.
- If a directive is ambiguous, retain it and explain why it needs further verification.
- Do not delete global usings or project-wide imports as part of a local file cleanup unless explicitly requested.
- Do not remove `using` declarations that manage resource disposal, such as `using var stream = ...;`. These are executable code, not import directives.
- Preserve conditional compilation structure and directives.
- Do not change namespace declarations, type names, aliases, method implementations, formatting, or unrelated code.
- Do not add new imports merely to reorganize or modernize the file.
- Do not modify project files or analyzer settings to suppress diagnostics.

### 4. Respect project-specific compilation

When working with projects that use custom SDKs, source generators, conditional compilation, or nonstandard build targets:

- Inspect the relevant project configuration before deciding that a directive is unnecessary.
- Account for target frameworks, build properties, and compilation symbols when available.
- Do not assume that a normal `dotnet build` covers every supported configuration.
- For Cosmos OS projects, preserve compatibility with the project's Cosmos SDK, NativeAOT settings, and custom build/publish tasks.
- If the required build environment is unavailable, state that verification could not be completed rather than claiming the cleanup is verified.

### 5. Verify the changes

After editing:

1. Review the diff to ensure that only intended `using` directives were removed.
2. Check for unresolved types, extension methods, aliases, or other compilation errors introduced by the changes.
3. Run an appropriate build or compiler/analyzer check when practical and within the requested scope.
4. Do not fix unrelated pre-existing build errors.
5. If verification fails, investigate whether the removed directives caused the failure. Restore any directives that were incorrectly removed.
6. Never claim that the project builds successfully unless a successful build or equivalent verification actually occurred.

## Output

Provide a concise summary containing:

- The files cleaned.
- The number of `using` directives removed, when known.
- Any ambiguous directives intentionally retained.
- Verification performed and its result.
- Any unresolved issues or environmental limitations.

## Safety rules

- Make minimal, targeted changes.
- Do not modify implementation behavior.
- Do not delete code simply because it appears unused.
- Do not suppress warnings to make the cleanup appear successful.
- Do not commit, push, or create a pull request unless explicitly requested.
- If no directives can be confidently removed, leave the files unchanged and explain why.
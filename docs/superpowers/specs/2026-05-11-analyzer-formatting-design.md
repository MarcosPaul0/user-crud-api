# Analyzer Formatting Design

**Date:** 2026-05-11  
**Branch:** feature/shipping  
**Goal:** Zero warnings from SonarAnalyzer.CSharp and StyleCop.Analyzers on `dotnet build --no-incremental`

---

## Context

Analyzers already installed globally via `Directory.Build.props`:
- `SonarAnalyzer.CSharp` 10.*
- `StyleCop.Analyzers` 1.*

Current baseline: **197 warnings** across all 4 layers (Domain, Application, Infrastructure, API).

Existing `.editorconfig` suppresses only SA1600/1601/1602/1633 (XML docs + copyright header).

All files use **file-scoped namespaces** (`namespace Foo;`) — C# 10+ style, incompatible with SA1200.

---

## Configuration Change

Add to `.editorconfig`:

```ini
dotnet_diagnostic.SA1200.severity = none
```

SA1200 requires `using` directives inside namespace blocks. File-scoped namespaces make this physically impossible — SA1200 is universally suppressed in projects using this style.

`Directory.Build.props` and `stylecop.json` are unchanged.

---

## Fix Batches

Execute in order. Run `dotnet build` verification after each batch.

| # | Rule | Count | Fix |
|---|------|-------|-----|
| 1 | SA1200 | 242 | Suppress via `.editorconfig` |
| 2 | SA1206 | 66 | Reorder `public required` → `required public` in all DTO/record files |
| 3 | SA1024 | 18 | Add space before `:` in primary constructor inheritance chains |
| 4 | CS8618 | 25 | Add `= null!` to uninitialized navigation properties in Domain entities |
| 5 | SA1201 | 6 | Move constructors before properties in `Phone.cs`, `ProductImage.cs`, `User.cs` |
| 6 | SA1003 | 2 | Fix operator spacing in `Order.cs` |
| 7 | S3993 | 2 | Add `[AttributeUsage]` to `MaxFileSizeAttribute`, `AllowedImageExtensionsAttribute` |
| 8 | S125 | 1 | Remove commented-out code in `Product.cs` |
| 9 | SA0001 + CS8625 | 3 | Fix null literal + suppress SA0001 (XML doc config noise) |

---

## Fix Details

### SA1206 — Modifier order
All affected files are in `src/Domain/Dto/` and `src/Application/`. Pattern:
```csharp
// Before
public required string Name { get; init; }

// After
required public string Name { get; init; }
```

### SA1024 — Colon spacing
Affects exception classes and attribute classes using primary constructor inheritance:
```csharp
// Before
public sealed class BadRequestException(string message): Exception(message)

// After
public sealed class BadRequestException(string message) : Exception(message)
```

### CS8618 — Uninitialized navigation properties
EF Core pattern — ORM populates these at runtime:
```csharp
// Before
public User User { get; set; }

// After
public User User { get; set; } = null!;
```

### SA1201 — Member ordering
StyleCop order: fields → constructors → properties → methods.  
Affected: `Phone.cs`, `ProductImage.cs`, `User.cs` — constructors currently placed after properties.

### S3993 — AttributeUsage
```csharp
[AttributeUsage(AttributeTargets.Property)]
public class MaxFileSizeAttribute : ValidationAttribute { ... }
```

### SA0001 — XML doc config
Suppress via `.editorconfig` — project intentionally disables XML doc generation (SA1600–1602 already suppressed).

---

## Verification

After each batch:
```bash
dotnet build --no-incremental 2>&1 | grep "warning" | grep -oP '(SA|CS|S)\d+' | sort | uniq -c
```

Final acceptance:
```bash
dotnet build --no-incremental 2>&1 | tail -5
# Must show: 0 Aviso(s)
```

Tests must still pass — no behavior changes in this work.

---

## Scope

- Style/formatting changes only
- No business logic changes
- No new abstractions
- No layer boundary changes

# CLAUDE.md

## Purpose

This document defines how contributors and coding agents should work in this repository.
All changes must preserve:

- Clean Architecture boundaries
- Domain-Driven Design principles
- Clean Code practices
- Low coupling and high cohesion
- Explicit, testable business behavior

The project is a `.NET` CRUD API organized into four main layers:

- `src/API`
- `src/Application`
- `src/Domain`
- `src/Infrastructure`

Tests live in:

- `tests/UnitTests`

## Architectural Style

Use **Clean Architecture** as the default decision model.

Dependency direction must always point inward:

1. `API` depends on `Application`
2. `Application` depends on `Domain`
3. `Infrastructure` depends on `Application` and `Domain`
4. `Domain` depends on nothing outside itself

Do not introduce dependencies that break this rule.

## Layer Responsibilities

### `src/Domain`

This is the core business layer.

It must contain:

- Entities
- Enums
- Domain rules
- Repository contracts
- Domain invariants

It must not contain:

- Framework code
- EF Core specifics
- HTTP concerns
- DTOs for transport
- Infrastructure services

Rules:

- Entities must protect valid business state.
- Prefer behavior-rich domain models over anemic models when business rules belong to the entity.
- Keep business terms explicit and ubiquitous.
- Repository interfaces belong here when they represent domain persistence needs.

### `src/Application`

This layer orchestrates use cases.

It must contain:

- Use cases
- Application DTOs
- Service contracts required by use cases
- Application exceptions
- Validation that is specific to application flow

It must not contain:

- HTTP/controller logic
- EF Core implementation details
- Direct infrastructure coupling

Rules:

- Each use case should have a single business intention.
- Use cases coordinate domain objects, repositories, and external service contracts.
- Application services should depend on abstractions, never concrete infrastructure classes.
- Keep methods small and intention-revealing.

### `src/API`

This layer is the delivery mechanism.

It must contain:

- Controllers
- Request/response DTOs
- Presenters
- API configuration
- Exception handling and middleware

It must not contain:

- Business rules
- Persistence logic
- Complex orchestration

Rules:

- Controllers should be thin.
- Controllers should delegate work immediately to a use case.
- API models exist for transport concerns only.
- Mapping between transport models and application models should stay in this layer or in dedicated presenters/mappers.

### `src/Infrastructure`

This layer implements external concerns.

It must contain:

- EF Core context and configurations
- Repository implementations
- External service implementations
- Token, password, object storage, and environment services
- Dependency injection wiring for technical concerns

It must not contain:

- Business decisions that belong to the domain
- HTTP-specific behavior

Rules:

- Infrastructure is replaceable.
- Keep implementation details isolated from higher layers.
- Repositories should translate persistence concerns without leaking ORM behavior into use cases.

## Project Structure

Follow this structure when adding new features:

```text
src/
  API/
    Controllers/
    Dtos/
    Presenters/
    Handlers/

  Application/
    Dtos/
    Exceptions/
    Interfaces/
    UseCases/
      <FeatureName>/
        I<FeatureName>UseCase.cs
        <FeatureName>UseCase.cs

  Domain/
    Entities/
    Enums/
    Interfaces/

  Infrastructure/
    Context/
    EntitiesConfiguration/
    Repositories/
    Services/
    Migrations/

tests/
  UnitTests/
    UseCases/
      <AggregateOrFeature>/
```

## DDD Guidance

Model the code around the business, not around the database.

Rules:

- Use the ubiquitous language already present in the domain.
- Keep entities focused on business identity and lifecycle.
- Treat repositories as collection-like abstractions for aggregates.
- Do not use controllers or persistence schemas to define the domain model.
- If a rule is business-critical, prefer putting it in the domain or in a dedicated use case instead of scattering it across controllers and repositories.

When adding a new concept, ask:

1. Is this a domain concept, an application workflow, or an infrastructure detail?
2. Which layer should own this decision?
3. Does this name reflect business language?

## Clean Code Rules

All code changes should follow these rules:

- Use clear and intention-revealing names.
- Prefer small classes and small methods.
- One method should do one thing well.
- Avoid boolean flags that change method behavior.
- Avoid duplicated logic across use cases and controllers.
- Prefer explicitness over cleverness.
- Remove dead code instead of keeping speculative abstractions.
- Validate early and fail clearly.
- Keep constructor dependencies minimal.
- Use immutability when practical for DTOs and value-like objects.

## Naming Conventions

- Name use cases by action: `CreateCustomerUseCase`, `FindUserByIdUseCase`
- Name interfaces with clear intent: `IUserRepository`, `IJwtTokenService`
- Name DTOs by purpose: `CreateUserDto`, `UpdateProductDto`
- Name controllers by operation, matching the current project style
- Avoid generic names like `Manager`, `Helper`, `Utils`, or `Service` unless the abstraction is genuinely cross-cutting and precise

## Use Case Rules

Each use case should:

- Represent one application action
- Receive explicit input
- Coordinate the required repositories/services
- Enforce the application flow
- Return a result or complete with side effects clearly
- Be easy to unit test in isolation

Each use case should not:

- Know about HTTP
- Read from `HttpContext`
- Depend on EF Core entities/configuration
- Perform presentation formatting

## Repository Rules

- Repository contracts belong in `Domain`
- Repository implementations belong in `Infrastructure`
- Keep query methods explicit and business-oriented
- Do not expose persistence internals to the application layer
- Use `UnitOfWork` only where transactional coordination is required by the current design

## Testing Rules

Prefer automated tests for business behavior.

Focus tests on:

- Use case behavior
- Business rules
- Error conditions
- Boundary cases

Rules:

- Unit tests should target `Application` use cases first.
- Mock repositories and external services at the application boundary.
- Add or update tests whenever business behavior changes.
- Do not rely only on manual API testing for feature validation.

## Dependency Injection

Register dependencies close to their layer:

- `Application/DependencyInjection.cs` registers use cases
- `Infrastructure/DependencyInjection.cs` registers repositories and technical services

Do not register infrastructure concretions directly in controllers or use cases.

## What To Avoid

Do not:

- Put business rules in controllers
- Put persistence logic in the application layer
- Make the domain depend on frameworks
- Create god classes with multiple responsibilities
- Introduce vague abstractions without a real use case
- Couple new code directly to environment variables, storage SDKs, or authentication libraries outside infrastructure

## Change Checklist

Before finishing a change, verify:

1. The layer ownership is correct.
2. Dependencies still point inward.
3. Business rules are not leaking into controllers or repositories.
4. Names reflect domain language.
5. The use case is covered by tests when behavior changes.
6. New code is simple enough to read without extra explanation.

## Default Expectation For Contributors And Agents

When implementing a feature:

1. Start from the business use case.
2. Define or refine domain concepts if needed.
3. Implement the application orchestration.
4. Expose it through the API with thin controllers.
5. Implement infrastructure details last.
6. Add or update tests for the changed behavior.

If there is any doubt, choose the option that keeps the domain cleaner, the application thinner, and the infrastructure more isolated.

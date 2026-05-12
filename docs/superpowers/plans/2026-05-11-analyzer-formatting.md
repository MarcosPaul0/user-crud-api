# Analyzer Formatting Implementation Plan

> **For agentic workers:** REQUIRED SUB-SKILL: Use superpowers:subagent-driven-development (recommended) or superpowers:executing-plans to implement this plan task-by-task. Steps use checkbox (`- [ ]`) syntax for tracking.

**Goal:** Eliminate all SonarAnalyzer.CSharp and StyleCop.Analyzers warnings so `dotnet build --no-incremental` reports `0 Aviso(s)`.

**Architecture:** Rule-by-rule batch approach — one task per warning category, each verified independently. No behavior changes; style and formatting only.

**Tech Stack:** .NET 10, C# 13, StyleCop.Analyzers 1.x, SonarAnalyzer.CSharp 10.x

---

## Files Modified

| File | Tasks |
|------|-------|
| `.editorconfig` | T1, T9 |
| `src/Domain/Dto/Clients/GetDeliveryTimeDto.cs` | T2 |
| `src/Domain/Dto/Clients/GetDeliveryTimeResponseDto.cs` | T2 |
| `src/Domain/Dto/Clients/GetShippingPriceDto.cs` | T2 |
| `src/Domain/Dto/Clients/GetShippingPriceResponseDto.cs` | T2 |
| `src/Domain/Dto/Services/CreateIdempotencyKeyDto.cs` | T2 |
| `src/Domain/Dto/Services/CreatePixPaymentDto.cs` | T2 |
| `src/Domain/Dto/Services/CreatePixPaymentResultDto.cs` | T2 |
| `src/Domain/Dto/Services/GetPixPaymentStatusResultDto.cs` | T2 |
| `src/Domain/Dto/Services/SendEmailDto.cs` | T2 |
| `src/Application/Dtos/CalculateShippingResultDto.cs` | T2 |
| `src/Application/Dtos/CreateOrderDto.cs` | T2 |
| `src/Application/Dtos/CreateOrderItemDto.cs` | T2 |
| `src/Application/Exceptions/BadRequestException.cs` | T3 |
| `src/Application/Exceptions/ConflictException.cs` | T3 |
| `src/Application/Exceptions/NotFoundException.cs` | T3 |
| `src/Application/Exceptions/UnauthorizeException.cs` | T3 |
| `src/Application/Attributes/AllowedImageExtensionsAttribute.cs` | T3, T7 |
| `src/Application/Attributes/MaxFileSizeAttribute.cs` | T3, T7 |
| `src/Application/UseCases/CalculateShipping/CalculateShippingUseCase.cs` | T3 |
| `src/Application/UseCases/CreateOrder/CreateOrderUseCase.cs` | T3 |
| `src/Application/UseCases/CreateProduct/CreateProductUseCase.cs` | T3 |
| `src/Domain/Entities/IdempotencyKey.cs` | T4 |
| `src/Domain/Entities/Phone.cs` | T4, T5 |
| `src/Domain/Entities/ProductCategory.cs` | T4 |
| `src/Domain/Entities/Product.cs` | T4, T8 |
| `src/Domain/Entities/ProductImage.cs` | T4, T5 |
| `src/Domain/Entities/User.cs` | T4, T5 |
| `src/Application/Dtos/CalculateShippingDto.cs` | T4 |
| `src/Application/Dtos/CreateProductCategoryDto.cs` | T4 |
| `src/Application/Dtos/CreateProductDto.cs` | T4 |
| `src/Application/Dtos/CreateUserDto.cs` | T4 |
| `src/Application/Dtos/LoginDto.cs` | T4 |
| `src/Application/Dtos/SetProductImagesDto.cs` | T4 |
| `src/Domain/Entities/Order.cs` | T6 |
| `src/Application/UseCases/SetProductImages/SetProductImagesUseCase.cs` | T9 |

---

## Task 1: Suppress SA1200 in .editorconfig

SA1200 requires `using` directives inside namespace blocks. All files use file-scoped namespaces (`namespace Foo;`) — this rule is physically impossible to satisfy with that style.

**Files:**
- Modify: `.editorconfig`

- [ ] **Step 1: Add SA1200 suppression**

Open `.editorconfig`. Current content:

```ini
[*.cs]
# Desativa documentação obrigatória em membros públicos
dotnet_diagnostic.SA1600.severity = none

# Desativa documentação obrigatória em elementos internos
dotnet_diagnostic.SA1601.severity = none

# Desativa documentação obrigatória em membros privados
dotnet_diagnostic.SA1602.severity = none

# Desativa cabeçalho de copyright obrigatório
dotnet_diagnostic.SA1633.severity = none
```

Add after the last line:

```ini
# File-scoped namespaces (namespace Foo;) are incompatible with this rule
dotnet_diagnostic.SA1200.severity = none
```

- [ ] **Step 2: Verify**

```bash
dotnet build --no-incremental 2>&1 | grep -c "SA1200"
```

Expected output: `0`

- [ ] **Step 3: Commit**

```bash
git add .editorconfig
git commit -m "style: suppress SA1200 — incompatible with file-scoped namespaces"
```

---

## Task 2: Fix SA1206 — Modifier order (`public required` → `required public`)

StyleCop requires `required` before access modifiers. 13 files affected across `Domain/Dto` and `Application/Dtos`.

**Files:**
- Modify: `src/Domain/Dto/Clients/GetDeliveryTimeDto.cs`
- Modify: `src/Domain/Dto/Clients/GetDeliveryTimeResponseDto.cs`
- Modify: `src/Domain/Dto/Clients/GetShippingPriceDto.cs`
- Modify: `src/Domain/Dto/Clients/GetShippingPriceResponseDto.cs`
- Modify: `src/Domain/Dto/Services/CreateIdempotencyKeyDto.cs`
- Modify: `src/Domain/Dto/Services/CreatePixPaymentDto.cs`
- Modify: `src/Domain/Dto/Services/CreatePixPaymentResultDto.cs`
- Modify: `src/Domain/Dto/Services/GetPixPaymentStatusResultDto.cs`
- Modify: `src/Domain/Dto/Services/SendEmailDto.cs`
- Modify: `src/Application/Dtos/CalculateShippingResultDto.cs`
- Modify: `src/Application/Dtos/CreateOrderDto.cs`
- Modify: `src/Application/Dtos/CreateOrderItemDto.cs`

- [ ] **Step 1: Fix `src/Domain/Dto/Clients/GetDeliveryTimeDto.cs`**

```csharp
// <copyright file="GetDeliveryTimeDto.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace AutoriaStore.Domain.Dto.Clients;

public record GetDeliveryTimeDto
{
    required public string DestinationPostalCode { get; init; }
}
```

- [ ] **Step 2: Fix `src/Domain/Dto/Clients/GetDeliveryTimeResponseDto.cs`**

Open the file and change every `public required` to `required public`.

- [ ] **Step 3: Fix `src/Domain/Dto/Clients/GetShippingPriceDto.cs`**

```csharp
// <copyright file="GetShippingPriceDto.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace AutoriaStore.Domain.Dto.Clients;

public record GetShippingPriceDto
{
    required public string DestinationPostalCode { get; init; }
    required public int DepthInCentimeters { get; init; }
    required public int WidthInCentimeters { get; init; }
    required public int HeightInCentimeters { get; init; }
    required public int WeightInGrams { get; init; }
}
```

- [ ] **Step 4: Fix `src/Domain/Dto/Clients/GetShippingPriceResponseDto.cs`**

Change `public required` to `required public` for `PriceInCents`.

- [ ] **Step 5: Fix `src/Domain/Dto/Services/CreateIdempotencyKeyDto.cs`**

```csharp
// <copyright file="CreateIdempotencyKeyDto.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace AutoriaStore.Domain.Dto.Services;

public record CreateIdempotencyKeyDto
{
    required public Guid AuthenticatedUserId { get; init; }
    required public string IdempotencyKey { get; init; }
    required public string Endpoint { get; init; }
    required public int StatusCode { get; init; }
    required public object RequestObject { get; init; }
    required public object? ResponseObject { get; init; }
}
```

- [ ] **Step 6: Fix `src/Domain/Dto/Services/CreatePixPaymentDto.cs`**

Change every `public required` to `required public`.

- [ ] **Step 7: Fix `src/Domain/Dto/Services/CreatePixPaymentResultDto.cs`**

Change every `public required` to `required public`.

- [ ] **Step 8: Fix `src/Domain/Dto/Services/GetPixPaymentStatusResultDto.cs`**

Change every `public required` to `required public`.

- [ ] **Step 9: Fix `src/Domain/Dto/Services/SendEmailDto.cs`**

```csharp
// <copyright file="SendEmailDto.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace AutoriaStore.Domain.Dto.Services;

public sealed class SendEmailDto
{
    required public string To { get; init; }

    required public string Subject { get; init; }

    required public string HtmlBody { get; init; }
}
```

- [ ] **Step 10: Fix `src/Application/Dtos/CalculateShippingResultDto.cs`**

```csharp
// <copyright file="CalculateShippingResultDto.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace AutoriaStore.Application.Dtos;

public record CalculateShippingResultDto()
{
    required public int ShippingPriceInCents { get; init; }
    required public DateTime EstimationDeliveryDate { get; init; }
}
```

- [ ] **Step 11: Fix `src/Application/Dtos/CreateOrderDto.cs`**

Change `public required IReadOnlyCollection<CreateOrderItemDto> Items` to `required public IReadOnlyCollection<CreateOrderItemDto> Items`.

- [ ] **Step 12: Fix `src/Application/Dtos/CreateOrderItemDto.cs`**

```csharp
// <copyright file="CreateOrderItemDto.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace AutoriaStore.Application.Dtos;

public sealed record CreateOrderItemDto
{
    required public Guid ProductId { get; init; }
    required public int Quantity { get; init; }
}
```

- [ ] **Step 13: Verify**

```bash
dotnet build --no-incremental 2>&1 | grep -c "SA1206"
```

Expected output: `0`

- [ ] **Step 14: Commit**

```bash
git add src/Domain/Dto src/Application/Dtos
git commit -m "style: fix SA1206 — reorder required modifier before public"
```

---

## Task 3: Fix SA1024 — Add space before colon in inheritance

SA1024 fires when `:` in a class/interface declaration is not preceded by a space. Pattern: `ClassName(args): BaseClass` → `ClassName(args) : BaseClass`.

**Files:**
- Modify: `src/Application/Exceptions/BadRequestException.cs`
- Modify: `src/Application/Exceptions/ConflictException.cs`
- Modify: `src/Application/Exceptions/NotFoundException.cs`
- Modify: `src/Application/Exceptions/UnauthorizeException.cs`
- Modify: `src/Application/Attributes/AllowedImageExtensionsAttribute.cs`
- Modify: `src/Application/Attributes/MaxFileSizeAttribute.cs`
- Modify: `src/Application/UseCases/CalculateShipping/CalculateShippingUseCase.cs`
- Modify: `src/Application/UseCases/CreateOrder/CreateOrderUseCase.cs`
- Modify: `src/Application/UseCases/CreateProduct/CreateProductUseCase.cs`

- [ ] **Step 1: Fix `src/Application/Exceptions/BadRequestException.cs`**

Change line 7:
```csharp
public sealed class BadRequestException(string message) : Exception(message)
```

- [ ] **Step 2: Fix `src/Application/Exceptions/ConflictException.cs`**

Change line 7:
```csharp
public class ConflictException(string message) : Exception(message)
```

- [ ] **Step 3: Fix `src/Application/Exceptions/NotFoundException.cs`**

Change line 7:
```csharp
public class NotFoundException(string message) : Exception(message)
```

- [ ] **Step 4: Fix `src/Application/Exceptions/UnauthorizeException.cs`**

Change line 7:
```csharp
public class UnauthorizeException(string message) : Exception(message)
```

- [ ] **Step 5: Fix `src/Application/Attributes/AllowedImageExtensionsAttribute.cs`**

Change line 10:
```csharp
public class AllowedImageExtensionsAttribute(params string[] extensions) : ValidationAttribute
```

- [ ] **Step 6: Fix `src/Application/Attributes/MaxFileSizeAttribute.cs`**

Change line 10:
```csharp
public class MaxFileSizeAttribute(int maxFileSizeInMb) : ValidationAttribute
```

- [ ] **Step 7: Fix `src/Application/UseCases/CalculateShipping/CalculateShippingUseCase.cs`**

Find the primary constructor closing paren followed immediately by `:`:
```csharp
    IUnitOfWork unitOfWork) : ICalculateShippingUseCase
```

- [ ] **Step 8: Fix `src/Application/UseCases/CreateOrder/CreateOrderUseCase.cs`**

```csharp
    IUnitOfWork unitOfWork) : ICreateOrderUseCase
```

- [ ] **Step 9: Fix `src/Application/UseCases/CreateProduct/CreateProductUseCase.cs`**

```csharp
public sealed class CreateProductUseCase(IUnitOfWork unitOfWork) : ICreateProductUseCase
```

- [ ] **Step 10: Verify**

```bash
dotnet build --no-incremental 2>&1 | grep -c "SA1024"
```

Expected output: `0`

- [ ] **Step 11: Commit**

```bash
git add src/Application
git commit -m "style: fix SA1024 — add space before colon in inheritance declarations"
```

---

## Task 4: Fix CS8618 — Uninitialized non-nullable properties

Add `= null!` to non-nullable reference properties that are not set in all constructors. This is the standard EF Core pattern for navigation properties and the model-binding pattern for request DTOs validated with `[Required]`.

**Files:**
- Modify: `src/Domain/Entities/IdempotencyKey.cs`
- Modify: `src/Domain/Entities/Phone.cs`
- Modify: `src/Domain/Entities/ProductCategory.cs`
- Modify: `src/Domain/Entities/Product.cs`
- Modify: `src/Domain/Entities/ProductImage.cs`
- Modify: `src/Domain/Entities/User.cs`
- Modify: `src/Application/Dtos/CalculateShippingDto.cs`
- Modify: `src/Application/Dtos/CreateProductCategoryDto.cs`
- Modify: `src/Application/Dtos/CreateProductDto.cs`
- Modify: `src/Application/Dtos/CreateUserDto.cs`
- Modify: `src/Application/Dtos/LoginDto.cs`
- Modify: `src/Application/Dtos/SetProductImagesDto.cs`

- [ ] **Step 1: Fix `src/Domain/Entities/IdempotencyKey.cs`**

Add `= null!` to `Key`, `Endpoint`, `RequestHash`:

```csharp
public string Key { get; set; } = null!;

public string Endpoint { get; set; } = null!;

public string RequestHash { get; set; } = null!;
```

- [ ] **Step 2: Fix `src/Domain/Entities/Phone.cs`**

Add `= null!` to the `User` navigation property:

```csharp
[JsonIgnore]
public User User { get; set; } = null!;
```

- [ ] **Step 3: Fix `src/Domain/Entities/ProductCategory.cs`**

Add `= null!` to `Category`:

```csharp
public string Category { get; set; } = null!;
```

- [ ] **Step 4: Fix `src/Domain/Entities/Product.cs`**

Add `= null!` to `Name`, `PrintDescription`, `Description`, `ProductCategory`, `ProductImages`:

```csharp
public string Name { get; set; } = null!;

public string PrintDescription { get; set; } = null!;

public string Description { get; set; } = null!;

// ... (other value-type properties unchanged) ...

public ProductCategory ProductCategory { get; set; } = null!;

public List<ProductImage> ProductImages { get; set; } = null!;
```

- [ ] **Step 5: Fix `src/Domain/Entities/ProductImage.cs`**

Add `= null!` to `Product` navigation property (ImageUrl is set in constructor — leave it):

```csharp
public Product Product { get; set; } = null!;
```

- [ ] **Step 6: Fix `src/Domain/Entities/User.cs`**

Add `= null!` to `Name`, `Email`, `Password` (second constructor doesn't guarantee all are set):

```csharp
public string Name { get; set; } = null!;

public string Email { get; set; } = null!;

public string Password { get; set; } = null!;
```

- [ ] **Step 7: Fix `src/Application/Dtos/CalculateShippingDto.cs`**

Add `= null!` to `DestinationPostalCode`:

```csharp
[Required]
[MinLength(8)]
public string DestinationPostalCode { get; set; } = null!;
```

- [ ] **Step 8: Fix `src/Application/Dtos/CreateProductCategoryDto.cs`**

Add `= null!` to `Category`:

```csharp
[Required]
[StringLength(50, MinimumLength = 10)]
public string Category { get; init; } = null!;
```

- [ ] **Step 9: Fix `src/Application/Dtos/CreateProductDto.cs`**

Add `= null!` to `Name`, `PrintDescription`, `Description`:

```csharp
[Required]
[StringLength(100, MinimumLength = 10)]
public string Name { get; init; } = null!;

[Required]
[StringLength(600, MinimumLength = 10)]
public string PrintDescription { get; init; } = null!;

[Required]
[StringLength(1_200, MinimumLength = 10)]
public string Description { get; init; } = null!;
```

- [ ] **Step 10: Fix `src/Application/Dtos/CreateUserDto.cs`**

Add `= null!` to `Name`, `Email`, `Password`:

```csharp
[Required]
[StringLength(50, MinimumLength = 10)]
public string Name { get; init; } = null!;

[Required]
[EmailAddress]
[StringLength(255, MinimumLength = 1)]
public string Email { get; init; } = null!;

[Required]
[MinLength(10)]
[StringLength(50, MinimumLength = 10)]
public string Password { get; init; } = null!;
```

- [ ] **Step 11: Fix `src/Application/Dtos/LoginDto.cs`**

Add `= null!` to `Email`, `Password`:

```csharp
[Required]
[EmailAddress]
public string Email { get; init; } = null!;

[Required]
[StringLength(225, MinimumLength = 10)]
public string Password { get; init; } = null!;
```

- [ ] **Step 12: Fix `src/Application/Dtos/SetProductImagesDto.cs`**

Add `= null!` to `Images`:

```csharp
[Required]
public List<ProductImageDto> Images { get; init; } = null!;
```

- [ ] **Step 13: Verify**

```bash
dotnet build --no-incremental 2>&1 | grep -c "CS8618"
```

Expected output: `0`

- [ ] **Step 14: Commit**

```bash
git add src/Domain/Entities src/Application/Dtos
git commit -m "style: fix CS8618 — add null! initializer to non-nullable navigation and DTO properties"
```

---

## Task 5: Fix SA1201 — Move constructors before properties

StyleCop requires members in this order: fields → constructors → properties → methods. Three entity files have constructors placed after properties.

**Files:**
- Modify: `src/Domain/Entities/Phone.cs`
- Modify: `src/Domain/Entities/ProductImage.cs`
- Modify: `src/Domain/Entities/User.cs`

- [ ] **Step 1: Fix `src/Domain/Entities/Phone.cs`**

Move constructor before properties. Result:

```csharp
// <copyright file="Phone.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

using System.Text.Json.Serialization;

namespace AutoriaStore.Domain.Entities;

public class Phone : Entity
{
    public Phone(string phoneNumber)
    {
        this.PhoneNumber = phoneNumber;
    }

    public string? PhoneNumber { get; set; }

    public Guid UserId { get; set; }

    [JsonIgnore]
    public User User { get; set; } = null!;
}
```

- [ ] **Step 2: Fix `src/Domain/Entities/ProductImage.cs`**

Move constructor before properties. Result:

```csharp
// <copyright file="ProductImage.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace AutoriaStore.Domain.Entities;

public class ProductImage : Entity
{
    public ProductImage(string imageUrl, byte displayOrder, Guid productId, DateTime createdAt)
    {
        this.ImageUrl = imageUrl;
        this.DisplayOrder = displayOrder;
        this.ProductId = productId;
        this.CreatedAt = createdAt;
    }

    public string ImageUrl { get; set; }

    public byte DisplayOrder { get; set; }

    public Guid ProductId { get; set; }

    public Product Product { get; set; } = null!;
}
```

- [ ] **Step 3: Fix `src/Domain/Entities/User.cs`**

Move both constructors before properties. Result:

```csharp
// <copyright file="User.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

using AutoriaStore.Domain.Enums;

namespace AutoriaStore.Domain.Entities;

public class User : Entity
{
    public User(string name, string email, string password, UserRole role, DateTime createdAt)
    {
        this.Name = name;
        this.Email = email;
        this.Password = password;
        this.Role = role;
        this.CreatedAt = createdAt;
    }

    public User(string? name, UserRole? role)
    {
        if (!string.IsNullOrEmpty(name))
        {
            this.Name = name;
        }

        if (role != null)
        {
            this.Role = role.Value;
        }
    }

    public string Name { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string Password { get; set; } = null!;

    public UserRole Role { get; set; }
}
```

- [ ] **Step 4: Verify**

```bash
dotnet build --no-incremental 2>&1 | grep -c "SA1201"
```

Expected output: `0`

- [ ] **Step 5: Commit**

```bash
git add src/Domain/Entities/Phone.cs src/Domain/Entities/ProductImage.cs src/Domain/Entities/User.cs
git commit -m "style: fix SA1201 — move constructors before properties in entity classes"
```

---

## Task 6: Fix SA1003 — Operator spacing in Order.cs

SA1003 fires at `Order.cs:45` — collection expression assigned without space after `=`.

**Files:**
- Modify: `src/Domain/Entities/Order.cs`

- [ ] **Step 1: Fix `src/Domain/Entities/Order.cs` line 45**

Change:
```csharp
public List<OrderProduct> ProductOrders { get; set; } =[];
```

To:
```csharp
public List<OrderProduct> ProductOrders { get; set; } = [];
```

- [ ] **Step 2: Verify**

```bash
dotnet build --no-incremental 2>&1 | grep -c "SA1003"
```

Expected output: `0`

- [ ] **Step 3: Commit**

```bash
git add src/Domain/Entities/Order.cs
git commit -m "style: fix SA1003 — add space after = in collection expression initializer"
```

---

## Task 7: Fix S3993 — Add [AttributeUsage] to custom attributes

S3993 requires all classes inheriting from `Attribute` to declare `[AttributeUsage]`. Both affected classes inherit from `ValidationAttribute` (which inherits from `Attribute`).

**Files:**
- Modify: `src/Application/Attributes/AllowedImageExtensionsAttribute.cs`
- Modify: `src/Application/Attributes/MaxFileSizeAttribute.cs`

- [ ] **Step 1: Fix `src/Application/Attributes/AllowedImageExtensionsAttribute.cs`**

Insert `[AttributeUsage(AttributeTargets.Property)]` on the line immediately before `public class AllowedImageExtensionsAttribute`. The class declaration line becomes:

```csharp
[AttributeUsage(AttributeTargets.Property)]
public class AllowedImageExtensionsAttribute(params string[] extensions) : ValidationAttribute
```

Do not change any other lines in this file.

- [ ] **Step 2: Fix `src/Application/Attributes/MaxFileSizeAttribute.cs`**

Insert `[AttributeUsage(AttributeTargets.Property)]` on the line immediately before `public class MaxFileSizeAttribute`. The class declaration line becomes:

```csharp
[AttributeUsage(AttributeTargets.Property)]
public class MaxFileSizeAttribute(int maxFileSizeInMb) : ValidationAttribute
```

Do not change any other lines in this file.

- [ ] **Step 3: Verify**

```bash
dotnet build --no-incremental 2>&1 | grep -c "S3993"
```

Expected output: `0`

- [ ] **Step 4: Commit**

```bash
git add src/Application/Attributes
git commit -m "style: fix S3993 — add [AttributeUsage] to custom validation attributes"
```

---

## Task 8: Fix S125 — Remove commented-out code from Product.cs

S125 fires on the large commented-out constructor block in `Product.cs` (lines 44–95 approximately).

**Files:**
- Modify: `src/Domain/Entities/Product.cs`

- [ ] **Step 1: Remove commented-out constructors from `src/Domain/Entities/Product.cs`**

Delete the entire block of commented-out code (the two commented constructors). The file should end after the `ProductImages` property declaration:

```csharp
// <copyright file="Product.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace AutoriaStore.Domain.Entities;

public class Product : Entity
{
    public string Name { get; set; } = null!;

    public string PrintDescription { get; set; } = null!;

    public string Description { get; set; } = null!;

    public int PriceInCents { get; set; }

    public int ProductionTimeInMinutes { get; set; }

    public byte DiscountPercentage { get; set; }

    public bool? IsActive { get; set; }

    public int StockQuantity { get; set; }

    public int DepthInCentimeters { get; set; }

    public int WidthInCentimeters { get; set; }

    public int HeightInCentimeters { get; set; }

    public int WeightInGrams { get; set; }

    public Guid ProductCategoryId { get; set; }

    public ProductCategory ProductCategory { get; set; } = null!;

    public List<ProductImage> ProductImages { get; set; } = null!;
}
```

- [ ] **Step 2: Verify**

```bash
dotnet build --no-incremental 2>&1 | grep -c "S125"
```

Expected output: `0`

- [ ] **Step 3: Commit**

```bash
git add src/Domain/Entities/Product.cs
git commit -m "style: fix S125 — remove commented-out constructors from Product entity"
```

---

## Task 9: Fix SA0001 + CS8625 — Residual warnings

**SA0001** fires on the Domain project because StyleCop's XML analysis is disabled (no `GenerateDocumentationFile=true`). Since XML docs are intentionally suppressed (SA1600–1602 already off), suppress SA0001 too.

**CS8625** fires at `SetProductImagesUseCase.cs:41` — `null` literal passed where `string` (non-nullable) is expected by the `ProductImage` constructor.

**Files:**
- Modify: `.editorconfig`
- Modify: `src/Application/UseCases/SetProductImages/SetProductImagesUseCase.cs`

- [ ] **Step 1: Suppress SA0001 in `.editorconfig`**

Add after the SA1200 suppression line:

```ini
# XML doc analysis disabled intentionally — SA1600/1601/1602 suppressed
dotnet_diagnostic.SA0001.severity = none
```

- [ ] **Step 2: Fix CS8625 in `SetProductImagesUseCase.cs`**

Find line ~41:
```csharp
var productImage = new ProductImage(null, productImageDto.DisplayOrder, productId, DateTime.UtcNow);
```

Change to:
```csharp
var productImage = new ProductImage(null!, productImageDto.DisplayOrder, productId, DateTime.UtcNow);
```

- [ ] **Step 3: Final verification — 0 warnings**

```bash
dotnet build --no-incremental 2>&1 | tail -5
```

Expected output must include:
```
    0 Aviso(s)
    0 Erro(s)
```

- [ ] **Step 4: Run tests**

```bash
dotnet test
```

Expected: all tests pass.

- [ ] **Step 5: Commit**

```bash
git add .editorconfig src/Application/UseCases/SetProductImages/SetProductImagesUseCase.cs
git commit -m "style: fix SA0001 + CS8625 — suppress XML doc warning, fix null literal"
```

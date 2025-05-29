# 🛡️ EntityAudit - Automatic Audit Trails for EF Core

[![NuGet Version](https://img.shields.io/nuget/v/Softylines.EntityAudit?color=blue&logo=nuget)](https://www.nuget.org/packages/Softylines.EntityAudit)
[![GitHub License](https://img.shields.io/github/license/fadiroot/EntityAudit)](LICENSE)
[![.NET Version](https://img.shields.io/badge/.NET-9.0-purple.svg)](https://dotnet.microsoft.com)

A lightweight library to automatically track entity changes (audit logs) in EF Core 9.0 with zero configuration.


---

## ✨ Features

- ✅ Automatic audit fields: `CreatedAt`, `CreatedBy`, `UpdatedAt`, `UpdatedBy`
- ✅ EF Core 9.0 optimized
- ✅ Supports custom user tracking via `ICurrentUserService`
- ✅ Minimal boilerplate (just inherit `AuditableEntity`)
- ✅ Supports soft delete pattern

---

## 🚀 Quick Start

### 📦 Installation

```bash
dotnet add package Softylines.EntityAudit
```

---

## 🧑‍💻 Usage

### 1️⃣ Implement `ICurrentUserService`

```csharp
public class CurrentUserService : ICurrentUserService
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public CurrentUserService(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public string GetCurrentUser()
    {
        return _httpContextAccessor.HttpContext?.User?.Identity?.Name ?? "System";
    }
}
```


➡️ Register it in `Program.cs`:

```csharp
builder.Services.AddScoped<ICurrentUserService, CurrentUserService>();
builder.Services.AddHttpContextAccessor();
```

---

### 2️⃣ Configure `DbContext` with EntityAudit

```csharp
builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseNpgsql(builder.Configuration.GetConnectionString("Postgres"))
           .AddEntityAuditInterceptor();
});
```


---

### 3️⃣ Create an Auditable Entity

```csharp
public class Product : AuditableEntity
{
    public int Id { get; set; }
    public string Name { get; set; }
    public decimal Price { get; set; }
}
```


✅ All audit fields are inherited from `AuditableEntity` and automatically handled.

---

### 4️⃣ Run Migrations

```bash
dotnet ef migrations add Init
dotnet ef database update
```

EF Core will add audit fields like:

```csharp
table.Column<string>(name: "CreatedBy", nullable: true),
table.Column<DateTime>(name: "CreatedAt", nullable: false),
table.Column<string>(name: "UpdatedBy", nullable: true),
table.Column<DateTime>(name: "UpdatedAt", nullable: true),
```


✅ These fields are automatically filled in based on the user context!

---

## 📚 Summary

EntityAudit gives you:

- 🔒 Transparent change tracking  
- 👤 Full user-based auditing  
- 🧼 Clean and minimal setup  

Ready to audit your entities? Install it now and keep your data history clean and secure.

---

## 🔗 Links

- 📦 [NuGet Package](https://www.nuget.org/packages/Softylines.EntityAudit)
- 💻 [GitHub Repository](https://github.com/fadiroot/EntityAudit)

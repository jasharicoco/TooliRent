# TooliRent API

TooliRent är ett uthyrningssystem för verktyg. Backend är byggt med **ASP.NET Core Web API** och exponerar en uppsättning REST-endpoints för autentisering, uthyrningar, betalningar, recensioner och verktygshantering.

---

## 🚀 Teknisk översikt
- **Framework**: ASP.NET Core 8 Web API
- **Autentisering**: JWT Bearer Tokens
- **Datamodeller**: Rentals, Tools, ToolCategories, Payments, Reviews, Users
- **Swagger/OpenAPI**: Tillgängligt via `/swagger`

---

## 🔧 Körning

### Förutsättningar
- [.NET 8 SDK](https://dotnet.microsoft.com/download)
- SQL Server / SQLite (beroende på din konfiguration)

### Starta projektet
```bash
dotnet restore
dotnet build
dotnet run

# TooliRent - Grafisk Presentation

Detta dokument innehåller en komplett grafisk presentation av TooliRent-systemet, inklusive databasstruktur, API-endpoints och systemarkitektur.

## 📊 Översikt

TooliRent är ett verktygshyrningssystem byggt med .NET 8 och ASP.NET Core Web API. Systemet tillåter kunder att hyra verktyg online med fullständig hantering av bokningar, betalningar och recensioner.

## 📁 Dokumentation

### 1. [Databas ERD](database_erd.md)
- Entity Relationship Diagram
- Tabellstrukturer och relationer
- Enums och datatyper

### 2. [API Endpoints](api_endpoints.md)
- Alla API-kontrollrar och endpoints
- HTTP-metoder och routes
- Rollbaserad åtkomstkontroll

### 3. [Systemarkitektur](system_architecture.md)
- Övergripande systemdesign
- Teknisk stack
- Arkitekturmönster

### 4. [Rental Workflow](rental_workflow.md)
- Bokningsprocessen
- Statusflöden
- Affärsregler

### 5. [API Dokumentation](api_documentation.md)
- Detaljerad API-dokumentation
- Request/Response-exempel
- Autentisering och auktorisering

## 🏗️ Systemkomponenter

### Databas
- **SQL Server** med Entity Framework Core
- **ASP.NET Core Identity** för användarhantering
- **7 huvudentiteter** med fullständiga relationer

### API
- **RESTful Web API** med 6 kontrollrar
- **JWT-autentisering** med rollbaserad auktorisering
- **CRUD-operationer** för alla entiteter

### Säkerhet
- **JWT-tokens** för autentisering
- **Rollbaserad auktorisering** (Admin, Customer, User)
- **Input-validering** med FluentValidation

## 🚀 Funktioner

### För Kunder
- Bläddra och söka verktyg
- Skapa bokningar
- Hantera egna bokningar
- Lämna recensioner
- Betalningshantering

### För Administratörer
- Fullständig systemhantering
- Användarhantering
- Verktygshantering
- Bokningshantering
- Statistik och rapporter

## 📈 Teknisk Stack

- **.NET 8**
- **ASP.NET Core Web API**
- **Entity Framework Core**
- **SQL Server**
- **ASP.NET Core Identity**
- **JWT Authentication**
- **AutoMapper**
- **FluentValidation**

## 🔄 Bokningsprocess

1. **Bläddra verktyg** → Filtrera och sök
2. **Välj verktyg** → Kontrollera tillgänglighet
3. **Skapa bokning** → Välj datum och betalning
4. **Bekräfta betalning** → Status: Confirmed
5. **Hämta verktyg** → Status: PickedUp
6. **Returnera verktyg** → Status: Returned
7. **Lämna recension** → Valfri feedback

## 📊 Databasöversikt

Systemet använder 7 huvudtabeller:
- **AppUser** - Användaridentitet
- **Customer** - Kundinformation
- **Tool** - Verktyg att hyra
- **ToolCategory** - Verktygskategorier
- **Rental** - Bokningar
- **Payment** - Betalningar
- **Review** - Recensioner

## 🔐 Säkerhetsfunktioner

- **JWT-baserad autentisering**
- **Rollbaserad auktorisering**
- **Säker lösenordshantering**
- **Refresh token-stöd**
- **Input-validering**
- **SQL injection-skydd**

---

*Denna presentation visar den kompletta strukturen av TooliRent-systemet, från databasdesign till API-implementation.*



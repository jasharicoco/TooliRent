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
```

Projektet körs då på:
```
https://localhost:7164
```

Swagger UI:
```
https://localhost:7164/swagger/index.html
```

Swagger JSON:
```
https://localhost:7164/swagger/v1/swagger.json
```

---

## 🔑 Autentisering
Alla skyddade endpoints använder **JWT Bearer tokens**.

Lägg till i request-header:
```
Authorization: Bearer {token}
```

---

## 📚 API Endpoints (översikt)

### Auth
- `GET /api/Auth/all-users` – lista alla användare
- `GET /api/Auth/user/{customerId}` – hämta användare per kund-id
- `POST /api/Auth/login` – logga in användare
- `POST /api/Auth/register-customer` – registrera kund
- `POST /api/Auth/register-user` – registrera användare
- `POST /api/Auth/create-admin` – skapa admin
- `PATCH /api/Auth/update-role/{userId}` – uppdatera roll
- `PATCH /api/Auth/toggle-active/{customerId}` – aktivera/inaktivera kund
- `POST /api/Auth/refresh` – uppdatera JWT-token

### Rentals
- `GET /api/Rentals` – lista uthyrningar (filter via `userId` eller `customerId`)
- `GET /api/Rentals/{id}` – hämta uthyrning per id
- `PUT /api/Rentals/{id}` – uppdatera uthyrning
- `DELETE /api/Rentals/{id}` – ta bort uthyrning
- `POST /api/Rentals/by-user` – skapa uthyrning för användare
- `POST /api/Rentals/by-customer` – skapa uthyrning för kund
- `PATCH /api/Rentals/{id}/status` – uppdatera uthyrningsstatus
- `GET /api/Rentals/my-bookings` – se egna bokningar
- `DELETE /api/Rentals/my-bookings/{id}` – avboka
- `GET /api/Rentals/statistics` – statistik över uthyrningar

### Tools
- `GET /api/Tool` – lista verktyg (med filter: `categoryId`, `condition`, `availableOnly`, `availableFrom`, `availableTo`)
- `POST /api/Tool` – skapa nytt verktyg
- `GET /api/Tool/{id}` – hämta verktyg
- `PUT /api/Tool/{id}` – uppdatera verktyg
- `DELETE /api/Tool/{id}` – ta bort verktyg
- `GET /api/Tool/{id}/rentals` – lista uthyrningar för verktyg
- `GET /api/ToolCategories` – lista kategorier
- `POST /api/ToolCategories` – skapa kategori
- `GET /api/ToolCategories/{id}` – hämta kategori
- `PUT /api/ToolCategories/{id}` – uppdatera kategori
- `DELETE /api/ToolCategories/{id}` – ta bort kategori

### Payments
- `GET /api/Payments` – lista betalningar
- `GET /api/Payments/{id}` – hämta betalning
- `PATCH /api/Payments/{id}/status` – uppdatera status
- `PATCH /api/Payments/{id}/method` – ändra betalningsmetod

### Reviews
- `GET /api/Reviews` – lista recensioner
- `POST /api/Reviews` – skapa recension
- `GET /api/Reviews/{id}` – hämta recension
- `DELETE /api/Reviews/{id}` – ta bort recension
- `GET /api/Reviews/rental/{rentalId}` – hämta recensioner för uthyrning

---

## 🔄 Exempel på API-flöde

**Registrera kund → Logga in → Hyra verktyg → Betala**

1. `POST /api/Auth/register-customer` → Skapa konto
2. `POST /api/Auth/login` → Hämta JWT-token
3. `GET /api/Tool` → Lista verktyg
4. `POST /api/Rentals/by-customer` → Skapa uthyrning
5. `PATCH /api/Payments/{id}/status` → Markera betalning som genomförd

---

## 📖 Swagger/OpenAPI
För full dokumentation, se:
- [Swagger UI](https://localhost:7164/swagger/index.html)
- [Swagger JSON](https://localhost:7164/swagger/v1/swagger.json)

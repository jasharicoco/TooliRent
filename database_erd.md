# TooliRent Database Entity Relationship Diagram

```mermaid
erDiagram
    AppUser {
        string Id PK
        string UserName
        string Email
        string FirstName
        string LastName
        datetime CreatedAt
    }
    
    Customer {
        int Id PK
        string UserId FK
    }
    
    ToolCategory {
        int Id PK
        string Name
        string Description
    }
    
    Tool {
        int Id PK
        string Name
        string Description
        string ImageUrl
        enum Condition
        decimal Price
        int ToolCategoryId FK
    }
    
    Rental {
        int Id PK
        int CustomerId FK
        int ToolId FK
        datetime StartDate
        datetime EndDate
        decimal TotalPrice
        enum Status
        datetime CreatedAt
        datetime ModifiedAt
    }
    
    Payment {
        int Id PK
        int RentalId FK
        decimal Amount
        enum PaymentMethod
        datetime PaymentDate
        enum Status
    }
    
    Review {
        int Id PK
        int RentalId FK
        int Rating
        string Comment
        datetime CreatedAt
    }
    
    AppUser ||--o| Customer : "has"
    ToolCategory ||--o{ Tool : "contains"
    Customer ||--o{ Rental : "makes"
    Tool ||--o{ Rental : "rented_in"
    Rental ||--o{ Payment : "has"
    Rental ||--o| Review : "receives"
```

## Database Relationships

### Core Entities:
- **AppUser**: Identity user with basic profile information
- **Customer**: Links AppUser to rental system
- **Tool**: Rentable items with categories and pricing
- **ToolCategory**: Classification system for tools
- **Rental**: Core business entity for tool rentals
- **Payment**: Payment tracking for rentals
- **Review**: Customer feedback system

### Key Relationships:
1. **One-to-One**: AppUser ↔ Customer (via UserId)
2. **One-to-Many**: ToolCategory → Tool
3. **One-to-Many**: Customer → Rental
4. **One-to-Many**: Tool → Rental
5. **One-to-Many**: Rental → Payment
6. **One-to-One**: Rental ↔ Review

### Enums Used:
- **ToolCondition**: New, Good, Used, Broken
- **RentalStatus**: Pending, Confirmed, PickedUp, Returned, Cancelled, Overdue
- **PaymentMethod**: Card, Swish, Invoice
- **PaymentStatus**: Pending, Completed, Failed, Refunded



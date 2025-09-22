# TooliRent API Documentation

## Base URL

```
https://localhost:7000/api
```

## Authentication

All endpoints (except public auth endpoints) require JWT authentication in the Authorization header:

```
Authorization: Bearer <jwt_token>
```

## Controllers Overview

### 🔐 AuthController (`/api/auth`)

| Method | Endpoint                      | Description               | Auth Required | Roles |
| ------ | ----------------------------- | ------------------------- | ------------- | ----- |
| POST   | `/login`                      | User login                | No            | -     |
| POST   | `/register-customer`          | Register new customer     | No            | -     |
| POST   | `/register-user`              | Register new user         | No            | -     |
| POST   | `/create-admin`               | Create admin user         | Yes           | Admin |
| GET    | `/all-users`                  | Get all users             | Yes           | Admin |
| GET    | `/user/{customerId}`          | Get user by customer ID   | Yes           | Admin |
| PATCH  | `/update-role/{userId}`       | Update user role          | Yes           | Admin |
| PATCH  | `/toggle-active/{customerId}` | Toggle user active status | Yes           | Admin |
| POST   | `/refresh`                    | Refresh JWT token         | Yes           | -     |

### 🔧 ToolsController (`/api/tools`)

| Method | Endpoint        | Description                  | Auth Required | Roles |
| ------ | --------------- | ---------------------------- | ------------- | ----- |
| GET    | `/`             | Get all tools (with filters) | No            | -     |
| GET    | `/{id}`         | Get tool by ID               | No            | -     |
| POST   | `/`             | Create new tool              | Yes           | Admin |
| PUT    | `/{id}`         | Update tool                  | Yes           | Admin |
| DELETE | `/{id}`         | Delete tool                  | Yes           | Admin |
| GET    | `/{id}/rentals` | Get rentals for tool         | No            | -     |

**Query Parameters for GET `/`:**

- `categoryId` (int?): Filter by category
- `condition` (string?): Filter by condition
- `availableOnly` (bool?): Show only available tools
- `availableFrom` (DateTime?): Available from date
- `availableTo` (DateTime?): Available to date

### 📋 RentalsController (`/api/rentals`)

| Method | Endpoint            | Description               | Auth Required | Roles |
| ------ | ------------------- | ------------------------- | ------------- | ----- |
| GET    | `/`                 | Get all rentals           | Yes           | Admin |
| GET    | `/{id}`             | Get rental by ID          | No            | -     |
| POST   | `/by-user`          | Create rental by user     | Yes           | -     |
| POST   | `/by-customer`      | Create rental by customer | Yes           | -     |
| PUT    | `/{id}`             | Update rental             | Yes           | Admin |
| PATCH  | `/{id}/status`      | Update rental status      | Yes           | Admin |
| GET    | `/my-bookings`      | Get user's rentals        | Yes           | -     |
| DELETE | `/my-bookings/{id}` | Cancel own rental         | Yes           | -     |
| DELETE | `/{id}`             | Delete rental             | Yes           | Admin |
| GET    | `/statistics`       | Get rental statistics     | Yes           | Admin |

**Query Parameters for GET `/`:**

- `userId` (string?): Filter by user ID
- `customerId` (int?): Filter by customer ID

### 💳 PaymentsController (`/api/payments`)

| Method | Endpoint       | Description           | Auth Required | Roles           |
| ------ | -------------- | --------------------- | ------------- | --------------- |
| GET    | `/`            | Get all payments      | Yes           | Admin           |
| GET    | `/{id}`        | Get payment by ID     | Yes           | Admin, Customer |
| PATCH  | `/{id}/status` | Update payment status | Yes           | Admin, Customer |
| PATCH  | `/{id}/method` | Update payment method | Yes           | Admin, Customer |

### ⭐ ReviewsController (`/api/reviews`)

| Method | Endpoint             | Description            | Auth Required | Roles    |
| ------ | -------------------- | ---------------------- | ------------- | -------- |
| GET    | `/`                  | Get all reviews        | No            | -        |
| GET    | `/{id}`              | Get review by ID       | No            | -        |
| GET    | `/rental/{rentalId}` | Get reviews for rental | No            | -        |
| POST   | `/`                  | Create review          | Yes           | Customer |
| DELETE | `/{id}`              | Delete review          | Yes           | -        |

### 🏷️ ToolCategoriesController (`/api/toolcategories`)

| Method | Endpoint | Description        | Auth Required | Roles |
| ------ | -------- | ------------------ | ------------- | ----- |
| GET    | `/`      | Get all categories | No            | -     |
| GET    | `/{id}`  | Get category by ID | No            | -     |
| POST   | `/`      | Create category    | Yes           | Admin |
| PUT    | `/{id}`  | Update category    | Yes           | Admin |
| DELETE | `/{id}`  | Delete category    | Yes           | Admin |

## Data Models

### Tool

```json
{
  "id": 1,
  "name": "Hammer",
  "description": "Heavy duty hammer",
  "imageUrl": "https://example.com/hammer.jpg",
  "condition": "Good",
  "price": 25.5,
  "toolCategoryId": 1,
  "toolCategory": {
    "id": 1,
    "name": "Hand Tools",
    "description": "Manual hand tools"
  }
}
```

### Rental

```json
{
  "id": 1,
  "customerId": 1,
  "toolId": 1,
  "startDate": "2024-01-15T09:00:00Z",
  "endDate": "2024-01-20T17:00:00Z",
  "totalPrice": 127.5,
  "status": "Confirmed",
  "createdAt": "2024-01-10T10:30:00Z",
  "modifiedAt": "2024-01-10T10:30:00Z"
}
```

### Payment

```json
{
  "id": 1,
  "rentalId": 1,
  "amount": 127.5,
  "paymentMethod": "Card",
  "paymentDate": "2024-01-10T10:35:00Z",
  "status": "Completed"
}
```

### Review

```json
{
  "id": 1,
  "rentalId": 1,
  "rating": 5,
  "comment": "Great tool, worked perfectly!",
  "createdAt": "2024-01-21T14:20:00Z"
}
```

## Enums

### ToolCondition

- `New`
- `Good`
- `Used`
- `Broken`

### RentalStatus

- `Pending` - Booked but not paid
- `Confirmed` - Paid
- `PickedUp` - Customer has collected tool
- `Returned` - Tool returned
- `Cancelled` - Cancelled
- `Overdue` - Not returned on time

### PaymentMethod

- `Card`
- `Swish`
- `Invoice`

### PaymentStatus

- `Pending`
- `Completed`
- `Failed`
- `Refunded`



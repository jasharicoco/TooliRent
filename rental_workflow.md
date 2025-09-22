# TooliRent Rental Workflow

```mermaid
stateDiagram-v2
    [*] --> BrowseTools: Customer visits site
    
    BrowseTools --> SelectTool: Customer finds tool
    BrowseTools --> FilterTools: Apply filters
    FilterTools --> BrowseTools: Clear filters
    
    SelectTool --> CheckAvailability: Tool selected
    CheckAvailability --> ToolUnavailable: Tool not available
    CheckAvailability --> CreateRental: Tool available
    
    ToolUnavailable --> BrowseTools: Try another tool
    
    CreateRental --> Pending: Rental created
    Pending --> Confirmed: Payment completed
    Pending --> Cancelled: Customer cancels
    Pending --> Expired: Payment timeout
    
    Confirmed --> PickedUp: Customer collects tool
    Confirmed --> Cancelled: Customer cancels before pickup
    
    PickedUp --> Returned: Tool returned on time
    PickedUp --> Overdue: Tool not returned on time
    
    Returned --> Review: Customer can review
    Returned --> [*]: Process complete
    
    Overdue --> Returned: Tool returned late
    Overdue --> [*]: Process complete (with penalty)
    
    Review --> [*]: Process complete
    
    Cancelled --> [*]: Process complete
    Expired --> [*]: Process complete
```

## Rental Status Flow

### 📋 Pending
- **Description**: Rental created but not yet paid
- **Actions**: Customer can cancel, Admin can confirm payment
- **Next States**: Confirmed, Cancelled, Expired

### ✅ Confirmed
- **Description**: Payment completed, rental confirmed
- **Actions**: Customer can collect tool, Admin can mark as picked up
- **Next States**: PickedUp, Cancelled

### 📦 PickedUp
- **Description**: Customer has collected the tool
- **Actions**: Customer uses tool, Admin tracks status
- **Next States**: Returned, Overdue

### 🔄 Returned
- **Description**: Tool returned successfully
- **Actions**: Customer can leave review, Admin processes return
- **Next States**: Review, Complete

### ⚠️ Overdue
- **Description**: Tool not returned on time
- **Actions**: Admin contacts customer, applies penalties
- **Next States**: Returned, Complete

### ❌ Cancelled
- **Description**: Rental cancelled by customer or admin
- **Actions**: Process refund if applicable
- **Next States**: Complete

## Key Business Rules

### Tool Availability
- Tools are checked for availability during rental creation
- Overlapping rentals are prevented
- Tool condition affects availability

### Payment Processing
- Multiple payment methods supported (Card, Swish, Invoice)
- Payment status tracking (Pending, Completed, Failed, Refunded)
- Automatic status updates

### Review System
- One review per rental
- 1-5 star rating system
- Optional comments
- Created after tool return

### Admin Controls
- Full rental management
- Status updates
- Customer communication
- Statistics and reporting



namespace TooliRent.Core.Models
{
    public enum ToolCondition
    {
        New,
        Good,
        Used,
        Broken
    }

    public enum RentalStatus
    {
        Active,
        Completed,
        Cancelled,
        Overdue
    }

    public enum PaymentMethod
    {
        Card,
        Swish,
        Invoice
    }

    public enum PaymentStatus
    {
        Pending,
        Completed,
        Failed,
        Refunded
    }
}

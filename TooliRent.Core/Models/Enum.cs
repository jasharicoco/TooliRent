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
        Pending,    // bokad men inte betald
        Confirmed,  // betald
        PickedUp,   // kunden har hämtat verktyget
        Returned,   // verktyget är återlämnat
        Cancelled,  // avbokad
        Overdue     // inte återlämnad/upphämtad i tid
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

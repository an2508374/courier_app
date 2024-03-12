using Convey.CQRS.Commands;

namespace SwiftParcel.Services.Orders.Application.Commands
{
<<<<<<<< HEAD:SwiftParcel.Services.Orders/src/SwiftParcel.Services.Orders.Application/SwiftParcel.Services.Orders.Application/Commands/CancelOrderOfficeWorker.cs
    public class CancelOrderOfficeWorker: ICommand
    {
        public Guid OrderId { get; }
        public string Reason { get; }
        public CancelOrderOfficeWorker(Guid orderId, string reason)
        {
            OrderId = orderId;
            Reason = reason;
========
    public class AddCustomerToOrder : ICommand
    {
        public Guid OrderId { get; }
        public Guid CustomerId { get; }

        public AddCustomerToOrder(Guid orderId, Guid customerId)
        {
            OrderId = orderId;
            CustomerId = customerId;
>>>>>>>> frontendUpdate:SwiftParcel.Services.Orders/src/SwiftParcel.Services.Orders.Application/SwiftParcel.Services.Orders.Application/Commands/AddCustomerToOrder.cs
        }
    }
}
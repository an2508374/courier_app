using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Convey.CQRS.Events;

namespace SwiftParcel.Services.Customers.Application.Events.External.Handlers
{
    public class OrderCompleted : IEvent
    {
        public Guid OrderId { get; }
        public Guid CustomerId { get; }

        public OrderCompleted(Guid orderId, Guid customerId)
        {
            OrderId = orderId;
            CustomerId = customerId;
        }
    }
}
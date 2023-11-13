using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Convey.CQRS.Events;

namespace SwiftParcel.Services.OrdersCreator.Events.External
{
    public class OrderApproved : IEvent
    {
        public Guid OrderId { get; }

        public OrderApproved(Guid orderId)
        {
            OrderId = orderId;
        }
    }
}
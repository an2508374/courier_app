﻿using Convey.CQRS.Events;

namespace SwiftParcel.ExternalAPI.Lecturer.Application.Events.Rejected
{
    public class CancelOrderRejected : IRejectedEvent
    {
        public Guid OrderId { get; }
        public string Reason { get; }
        public string Code { get; }

        public CancelOrderRejected(Guid orderId, string reason, string code)
        {
            OrderId = orderId;
            Reason = reason;
            Code = code;
        }
    }
}
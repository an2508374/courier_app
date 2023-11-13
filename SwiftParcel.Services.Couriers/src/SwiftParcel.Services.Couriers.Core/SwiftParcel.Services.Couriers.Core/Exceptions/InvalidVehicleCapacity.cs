using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SwiftParcel.Services.Couriers.Core.Exceptions
{
    public class InvalidCourierCapacity : DomainException
    {
        public override string Code { get; } = "invalid_vehicle_capacity";

        public InvalidCourierCapacity(double capacity)
            : base($"Vehicle capacity is invalid: {capacity}.")
        {
        }
    }
}
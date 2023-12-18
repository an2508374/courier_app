﻿using System.Linq;
using System.Threading.Tasks;
using Convey.CQRS.Commands;
using SwiftParcel.Services.Orders.Application.Exceptions;
using SwiftParcel.Services.Orders.Application.Services;
using SwiftParcel.Services.Orders.Application.Services.Clients;
using SwiftParcel.Services.Orders.Core.Entities;
using SwiftParcel.Services.Orders.Core.Repositories;


namespace SwiftParcel.Services.Orders.Application.Commands.Handlers
{
    public class AddParcelToOrderHandler : ICommandHandler<AddParcelToOrder>
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IParcelsServiceClient _parcelsServiceClient;
        private readonly IAppContext _appContext;
        private readonly IMessageBroker _messageBroker;
        private readonly IEventMapper _eventMapper;

        public AddParcelToOrderHandler(IOrderRepository orderRepository, IParcelsServiceClient parcelsServiceClient,
            IAppContext appContext, IMessageBroker messageBroker, IEventMapper eventMapper)
        {
            _orderRepository = orderRepository;
            _parcelsServiceClient = parcelsServiceClient;
            _appContext = appContext;
            _messageBroker = messageBroker;
            _eventMapper = eventMapper;
        }
        public async Task HandleAsync(AddParcelToOrder command, CancellationToken cancellationToken)
        {
            var order = await _orderRepository.GetAsync(command.OrderId);
            if (order is null)
            {
                throw new OrderNotFoundException(command.OrderId);
            }

            ValidateAccessOrFail(order);
            
            var parcel = await _parcelsServiceClient.GetAsync(command.ParcelId);
            if (parcel is null)
            {
                throw new ParcelNotFoundException(command.ParcelId);
            }

            order.AddParcel(new Parcel(command.ParcelId, parcel.Description, 
                            parcel.Width, parcel.Height, parcel.Depth, parcel.Weight, parcel.Source, parcel.Destination,
                            parcel.Priority, parcel.AtWeekend, parcel.PickupDate, parcel.DeliveryDate, parcel.IsCompany, 
                            parcel.VipPackage, parcel.CreatedAt, parcel.ValidTo, parcel.CalculatedPrice));
            await _orderRepository.UpdateAsync(order);
            var events = _eventMapper.MapAll(order.Events);
            await _messageBroker.PublishAsync(events.ToArray());
        }

        // TODO: please, check is there is a need of exception for the IsCourier 
        private void ValidateAccessOrFail(Order order)
        {
            var identity = _appContext.Identity;
            if (identity.IsAuthenticated && identity.Id != order.CustomerId && !identity.IsOfficeWorker)
            {
                throw new UnauthorizedOrderAccessException(order.Id, identity.Id);
            }
        }
    }
}
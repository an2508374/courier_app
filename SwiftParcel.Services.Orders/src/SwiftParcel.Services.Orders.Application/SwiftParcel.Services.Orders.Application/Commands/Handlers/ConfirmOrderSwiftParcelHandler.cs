using Convey.CQRS.Commands;
<<<<<<<< HEAD:SwiftParcel.Services.Orders/src/SwiftParcel.Services.Orders.Application/SwiftParcel.Services.Orders.Application/Commands/Handlers/ConfirmOrderSwiftParcelHandler.cs
using SwiftParcel.Services.Orders.Application.Services;
========
>>>>>>>> frontendUpdate:SwiftParcel.Services.Orders/src/SwiftParcel.Services.Orders.Application/SwiftParcel.Services.Orders.Application/Commands/Handlers/AddCustomerToOrderHandler.cs
using SwiftParcel.Services.Orders.Core.Repositories;
using SwiftParcel.Services.Orders.Application.Exceptions;
using SwiftParcel.Services.Orders.Core.Exceptions;
using SwiftParcel.Services.Orders.Application.Services;

namespace SwiftParcel.Services.Orders.Application.Commands.Handlers
{
<<<<<<<< HEAD:SwiftParcel.Services.Orders/src/SwiftParcel.Services.Orders.Application/SwiftParcel.Services.Orders.Application/Commands/Handlers/ConfirmOrderSwiftParcelHandler.cs
    public class ConfirmOrderSwiftParcelHandler : ICommandHandler<ConfirmOrderSwiftParcel>
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IMessageBroker _messageBroker;
        private readonly IEventMapper _eventMapper;
        private readonly IAppContext _appContext;
        private readonly IDateTimeProvider _dateTimeProvider;

        public ConfirmOrderSwiftParcelHandler(IOrderRepository orderRepository, IMessageBroker messageBroker,
            IEventMapper eventMapper, IAppContext appContext, IDateTimeProvider dateTimeProvider)
        {
            _orderRepository = orderRepository;
            _messageBroker = messageBroker;
            _eventMapper = eventMapper;
            _appContext = appContext;
            _dateTimeProvider = dateTimeProvider;
        }
        
        public async Task HandleAsync(ConfirmOrderSwiftParcel command, CancellationToken cancellationToken = default)
========
    public class AddCustomerToOrderHandler: ICommandHandler<AddCustomerToOrder>
    {
        private readonly IOrderRepository _orderRepository;
        private readonly ICustomerRepository _customerRepository;
        private readonly IAppContext _appContext;
        private readonly IEventMapper _eventMapper;
        private readonly IMessageBroker _messageBroker;
        public AddCustomerToOrderHandler(IOrderRepository orderRepository, ICustomerRepository customerRepository,
            IAppContext appContext, IEventMapper eventMapper, IMessageBroker messageBroker)
        {
            _orderRepository = orderRepository;
            _customerRepository = customerRepository;
            _appContext = appContext;
            _eventMapper = eventMapper;
            _messageBroker = messageBroker;
        }
        public async Task HandleAsync(AddCustomerToOrder command, CancellationToken cancellationToken)
>>>>>>>> frontendUpdate:SwiftParcel.Services.Orders/src/SwiftParcel.Services.Orders.Application/SwiftParcel.Services.Orders.Application/Commands/Handlers/AddCustomerToOrderHandler.cs
        {
            var order = await _orderRepository.GetAsync(command.OrderId);
            if (order is null)
            {
                throw new OrderNotFoundException(command.OrderId);
            }
            var customer = await _customerRepository.GetAsync(command.CustomerId);
            if (customer is null)
            {
                throw new CustomerNotFoundException(command.CustomerId);
            }
            var identity = _appContext.Identity;
            if (identity.IsAuthenticated && identity.Id != order.CustomerId)
            {
                throw new UnauthorizedOrderAccessException(command.OrderId, identity.Id);
<<<<<<<< HEAD:SwiftParcel.Services.Orders/src/SwiftParcel.Services.Orders.Application/SwiftParcel.Services.Orders.Application/Commands/Handlers/ConfirmOrderSwiftParcelHandler.cs
            }
            
            if(order.RequestValidTo < _dateTimeProvider.Now)
            {
                throw new OrderRequestExpiredException(command.OrderId, order.RequestValidTo);
            }

            order.Confirm();
========
            }
            order.AddCustomer(customer.Id);
>>>>>>>> frontendUpdate:SwiftParcel.Services.Orders/src/SwiftParcel.Services.Orders.Application/SwiftParcel.Services.Orders.Application/Commands/Handlers/AddCustomerToOrderHandler.cs
            await _orderRepository.UpdateAsync(order);
            var events = _eventMapper.MapAll(order.Events);
            await _messageBroker.PublishAsync(events.ToArray());
        }
    }
}

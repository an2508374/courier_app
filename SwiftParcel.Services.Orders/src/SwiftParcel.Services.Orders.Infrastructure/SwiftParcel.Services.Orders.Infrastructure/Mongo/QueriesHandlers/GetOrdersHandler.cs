﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Convey.CQRS.Queries;
using Convey.Persistence.MongoDB;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using SwiftParcel.Services.Orders.Application;
using SwiftParcel.Services.Orders.Application.DTO;
using SwiftParcel.Services.Orders.Application.Queries;
using SwiftParcel.Services.Orders.Infrastructure.Mongo.Documents;
using SwiftParcel.Services.Orders.Core.Entities;

namespace SwiftParcel.Services.Orders.Infrastructure.Mongo.QueriesHandlers
{
    public class GetOrdersHandler : IQueryHandler<GetOrders, IEnumerable<OrderDto>>
    {
        private readonly IMongoRepository<OrderDocument, Guid> _orderRepository;
        private readonly IAppContext _appContext;

        public GetOrdersHandler(IMongoRepository<OrderDocument, Guid> orderRepository, IAppContext appContext)
        {
            _orderRepository = orderRepository;
            _appContext = appContext;
        }

        public async Task<IEnumerable<OrderDto>> HandleAsync(GetOrders query, CancellationToken cancellationToken)
        {
            var documents = _orderRepository.Collection.AsQueryable();
            if (query.CustomerId.HasValue)
            {
                var identity = _appContext.Identity;
                if (identity.IsAuthenticated && identity.Id != query.CustomerId && !identity.IsOfficeWorker)
                {
                    return Enumerable.Empty<OrderDto>();
                }

                documents = documents.Where(p => p.CustomerId == query.CustomerId);
            }

            documents = documents.Where(p => p.Status != OrderStatus.WaitingForDecision && p.Status != OrderStatus.Approved);
            var orders = await documents.ToListAsync();

            return orders.Select(p => p.AsDto());
        }
    }
}
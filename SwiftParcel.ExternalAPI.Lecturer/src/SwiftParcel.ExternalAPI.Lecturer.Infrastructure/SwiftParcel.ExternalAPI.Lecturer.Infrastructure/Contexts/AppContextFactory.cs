﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Convey.MessageBrokers;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using SwiftParcel.ExternalAPI.Lecturer.Application;

namespace SwiftParcel.ExternalAPI.Lecturer.Infrastructure.Contexts
{
    internal sealed class AppContextFactory : IAppContextFactory
    {
        private readonly ICorrelationContextAccessor _contextAccessor;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public AppContextFactory(ICorrelationContextAccessor contextAccessor, IHttpContextAccessor httpContextAccessor)
        {
            _contextAccessor = contextAccessor;
            _httpContextAccessor = httpContextAccessor;
        }

        public IAppContext Create()
        {
            if (_contextAccessor.CorrelationContext is {})
            {
                var payload = JsonConvert.SerializeObject(_contextAccessor.CorrelationContext);

                return string.IsNullOrWhiteSpace(payload)
                    ? AppContext.Empty
                    : new AppContext(JsonConvert.DeserializeObject<CorrelationContext>(payload));
            }

            var context = _httpContextAccessor.GetCorrelationContext();
            
            return context is null ? AppContext.Empty : new AppContext(context);
        }
    }
}
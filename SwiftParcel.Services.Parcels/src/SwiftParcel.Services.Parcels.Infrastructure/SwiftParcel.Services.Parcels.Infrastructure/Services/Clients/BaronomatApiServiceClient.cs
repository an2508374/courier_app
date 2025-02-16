﻿using Convey.HTTP;
using SwiftParcel.Services.Parcels.Application.Commands;
using SwiftParcel.Services.Parcels.Application.Services.Clients;
using SwiftParcel.Services.Parcels.Application.DTO;

namespace SwiftParcel.Services.Parcels.Infrastructure.Services.Clients
{
    public class BaronomatApiServiceClient : IBaronomatApiServiceClient
    {
        private readonly IHttpClient _httpClient;
        private readonly string _url;

        public BaronomatApiServiceClient(IHttpClient httpClient, HttpClientOptions options)
        {
            _httpClient = httpClient;
            _url = options.Services["baronomat-api"];
        }
        
        public async Task PostInquiryAsync(AddParcel parcel)
        {
            await _httpClient.PostAsync($"{_url}/parcels", parcel);
        }

        public async Task<ExpirationStatusDto> GetOfferAsync(Guid parcelId)
        {
            var offer = await _httpClient.GetAsync<ExpirationStatusDto>($"{_url}/parcels/{parcelId}/offer");
            return offer;
        }
        
    }
}

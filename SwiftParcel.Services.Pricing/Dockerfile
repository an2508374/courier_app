# Use the .NET 6.0 SDK image to build the application
FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /app
COPY . .
RUN dotnet publish src/SwiftParcel.Services.Pricing.Api -c release -o out

# Use the .NET 6.0 runtime image to run the application
FROM mcr.microsoft.com/dotnet/aspnet:6.0
WORKDIR /app
COPY --from=build /app/out .
ENV ASPNETCORE_URLS http://*:80
ENV ASPNETCORE_ENVIRONMENT production
ENTRYPOINT ["dotnet", "SwiftParcel.Services.Pricing.Api.dll"]

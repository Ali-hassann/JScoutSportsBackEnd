FROM mcr.microsoft.com/dotnet/sdk:6.0-alpine as build
WORKDIR /app
COPY . .
RUN dotnet restore /app/AMNSystemsERP.Api/
RUN dotnet publish /app/AMNSystemsERP.Api/ -o /app/published

FROM mcr.microsoft.com/dotnet/aspnet:6.0-alpine as runtime
WORKDIR /app
COPY --from=build /app/published /app
ENTRYPOINT [ "dotnet", "/app/AMNSystemsERP.Api.dll" ]
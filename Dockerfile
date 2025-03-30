
FROM mcr.microsoft.com/dotnet/sdk:9.0-alpine AS build
WORKDIR /app

COPY . .

RUN dotnet publish -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:9.0-alpine
WORKDIR /app

COPY --from=build /app/publish .

ARG DB_URL
ENV DB_URL=${DB_URL}
ENV ASPNETCORE_URLS=http://+:3000

EXPOSE 3000

ENTRYPOINT ["dotnet", "asp.net.dll"]

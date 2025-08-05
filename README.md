# CryptoExchangeTask

Your task is to implement a meta-exchange that always gives the user the best possible price if he is
buying or selling a certain amount of BTC. Technically, you will be given n order books [from n different
cryptoexchanges], the type of order (buy or sell), and the amount of BTC that our user wants to buy or
sell. Your algorithm needs to output one or more buy or sell orders.

## Table of Contents
- [Prerequisites]
- [Tech Stack]
- [Running the Applications]

## Prerequisites
- .NET 8
- Docker Desktop

## Tech Stack
- **Backend:** .NET 8, ASP.NET Core Web API, Console-App
- **Containerization:** Docker, Docker Compose
- **Testing:** xUnit, FluentAssertions
- **Documentation:** Swagger
- **Logging:** Microsoft-Logging
- **Formatting** .editorconfig

## Running the Applications
### Console
Run the application with:
```
dotnet run --project .\src\CryptoExchangeTask.ConsoleApp\CryptoExchangeTask.ConsoleApp.csproj
```

### Web API
Run the application with:
```
dotnet run --project .\src\CryptoExchangeTask.API\CryptoExchangeTask.API.csproj
```
The API will be available at [http://localhost:8080](http://localhost:8080). \
Use the [Swagger UI](http://localhost:8080/swagger) to explore and test the API endpoints.

## Containerize the application
### Create a docker image
```
docker build -f .\src\CryptoExchangeTask.API\Dockerfile -t crypto-exchange-task:latest .
```
### Run with docker
```
docker run -p 8080:8080 --rm crypto-exchange-task:latest
```
### Run with docker-compose
```
docker compose -f docker-compose.yml -f docker-compose.production.yml up
```
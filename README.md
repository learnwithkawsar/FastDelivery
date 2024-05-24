# Fast Delivery Management

[![fastdelivery-build-action](https://github.com/learnwithkawsar/FastDelivery/actions/workflows/dotnet.yml/badge.svg)](https://github.com/learnwithkawsar/FastDelivery/actions/workflows/dotnet.yml)
Fast Delivery Management is a microservice-based delivery application designed to streamline the management of orders, identity, and tracking. Leveraging the power of Dapr, it ensures efficient and scalable service-to-service communication, with robust integrations for databases, message queues, logging, and more.

# Table of Contents

- [.NET Microservices](#net-microservices)
- [Table of Contents](#table-of-contents)
  - [Goals](#goals)
  - [Fast Delivery](#Fast-Delivery)
  - [How to Run ?](#how-to-run-)    
    - [Docker \& Docker-Compose](#docker--docker-compose)
  - [Technologies \& Libraries](#technologies--libraries)
  - [Documentation](#documentation)
  - [Changelogs](#changelogs)
  - [Community](#community)
  - [License](#license)
  - [Support ⭐](#support-)
  - [Code Contributors](#code-contributors)



## Goals

- :sparkle: Using `Vertical Slice Architecture` for architecture level.
- :sparkle: Using `Domain Driven Design (DDD)` to implement all business processes in microservices.
- :sparkle: Using `Rabbitmq` on top of `MassTranit` for `Event Driven Architecture` between our microservices.
- :sparkle: Using `CQRS` implementation with `MediatR` library.
- :sparkle: Using `Entity Framework Core` for some microservices.
- :sparkle: Using `MongoDB` for some microservices.
- :sparkle: Using `Fluent Validation` and a `Validation Pipeline Behaviour` on top of `MediatR`.
- :sparkle: Using `Health Check` for reporting the health of app infrastructure components.
- :sparkle: Using `Built-In Containerization` for `Docker` images.
- :sparkle: Using `Zipkin` for distributed tracing.
- :sparkle: Using `OpenIddict` for authentication and authorization base on `OpenID-Connect` and `OAuth2`.
- :sparkle: Using `Yarp` as a microservices gateway.

## Fast Delivery

Fast Delivery is a sample project that consumes the microservice framework. You will learn a lot by exploring this project, which is located under the `./Fast Delivery` folder.


| Services          | Status         |
| ----------------- | -------------- |
| Gateway           | WIP       🚧   |
| Identity          | WIP       🚧   |
| Orders            | WIP       🚧   |
| Tracking          | WIP       🚧   |
| Rider             | WIP       🚧   |
| Payment           | WIP       🚧   |

## How to Run ?

### Docker & Docker-Compose
The `Fast Delivery` project comes included with the required docker-compose.yaml.

There are some prerequisites for using these included docker-compose.yml files:

1) Make sure you have docker installed (on windows install docker desktop)

2) go to root folder and open terminal and run:

    ```
    docker-compose up 
    ```

  
## Technologies & Libraries

- **[`.NET 8`](https://dotnet.microsoft.com/download)** - .NET Framework and .NET Core, including ASP.NET and ASP.NET Core
- **[`Dapr`](https://dapr.io/)** - Dapr provides integrated APIs for communication, state, and workflow. Dapr leverages industry best practices for security, resiliency, and observability.
- **[`MVC Versioning API`](https://github.com/microsoft/aspnet-api-versioning)** - Set of libraries which add service API versioning to ASP.NET Web API, OData with ASP.NET Web API, and ASP.NET Core
- **[`EF Core`](https://github.com/dotnet/efcore)** - Modern object-database mapper for .NET. It supports LINQ queries, change tracking, updates, and schema migrations
- **[`MediatR`](https://github.com/jbogard/MediatR)** - Simple, unambitious mediator implementation in .NET.
- **[`FluentValidation`](https://github.com/FluentValidation/FluentValidation)** - Popular .NET validation library for building strongly-typed validation rules
- **[`Swagger & Swagger UI`]()** - Swagger tools for documenting API's built on ASP.NET Core
- **[`Serilog`](https://github.com/serilog/serilog)** - Simple .NET logging with fully-structured events
- **[`ELK`](https://github.com/elastic)** - Visualize logs with kibana
- **[`OpenIddict`](https://github.com/openiddict/openiddict-core)** - OpenIddict aims at providing a versatile solution to implement OpenID Connect client, server and token validation support.
- **[`Mapster`](https://github.com/MapsterMapper/Mapster)** - Convention-based object-object mapper in .NET.
- **[`Yarp`](https://github.com/microsoft/reverse-proxy)** - Reverse proxy toolkit for building fast proxy servers in .NET
- **[`MongoDB.Driver`](https://github.com/mongodb/mongo-csharp-driver)** - .NET Driver for MongoDB.

## Documentation

Read Documentation related to this Boilerplate here -
> Docs are not yet updated.

## Changelogs

[View Complete Changelogs.]()

## Community


## License

This project is licensed with the [MIT license](LICENSE).


## Support ⭐

Has this Project helped you learn something New? or Helped you at work?
Here are a few ways by which you can support.

-   Leave a star! ⭐
-   Recommend this awesome project to your colleagues. 🥇


## Code Contributors

This project exists thanks to all the people who contribute. [Submit your PR and join the elite list!](CONTRIBUTING.md)



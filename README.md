# Fast Delivery Management

## Introduction

Fast Delivery Management is a microservice-based delivery application designed to streamline the management of orders, identity, and tracking. Leveraging the power of Dapr, it ensures efficient and scalable service-to-service communication, with robust integrations for databases, message queues, logging, and more.

## Table of Contents

- [Introduction](#introduction)
- [Features](#features)
- [Architecture](#architecture)
- [Installation](#installation)
- [Usage](#usage)
- [Configuration](#configuration)
- [Dependencies](#dependencies)
- [Documentation](#documentation)
- [Examples](#examples)
- [Troubleshooting](#troubleshooting)
- [Contributors](#contributors)
- [License](#license)

## Features

- **Microservice architecture using Dapr**: Facilitates efficient communication between microservices and ensures scalability and fault tolerance.
  - **Dapr**: A portable, event-driven runtime that makes it easy to build resilient, stateless, and stateful microservices. It provides a set of building blocks for common microservice patterns, such as service invocation, state management, and publish/subscribe messaging.
- **Services**:
  - **Order Service**: Manages the creation, updating, and retrieval of orders.
  - **Identity Service**: Handles user authentication and authorization.
  - **Tracking Service**: Provides real-time tracking information for orders.
- **Databases**:
  - **PostgreSQL**: A powerful, open-source relational database system for storing order and user data.
  - **Redis**: An in-memory data structure store, used for caching and real-time analytics.
  - **MongoDB**: A NoSQL database for storing unstructured tracking data.
- **Messaging**:
  - **RabbitMQ**: A robust message broker that facilitates communication between services using an event-driven architecture.
- **Logging**:
  - **ELK Stack**: Elasticsearch, Logstash, and Kibana are used together to collect, process, and visualize logs and metrics.
- **API Gateway**:
  - **YARP (Yet Another Reverse Proxy)**: Manages API traffic and provides a single entry point for all services. YARP handles routing, load balancing, and security, ensuring that API requests are efficiently directed to the appropriate microservices.
- **Event Bus Integration**: Ensures reliable event-driven communication between microservices.
- **Health Check endpoints**: Provides endpoints to monitor the health and status of each service.
- **Integration Testing**:
  - **xUnit**: A testing framework for .NET, used to write integration tests ensuring system reliability.
- **Architecture**:
  - **Clean Architecture**: Ensures a modular, maintainable, and testable codebase by separating concerns.
  - **Clean Code principles**: Adheres to best practices in coding to ensure readability and maintainability.
  - **Mediator pattern**: Facilitates communication between objects without direct dependencies.
  - **CQRS (Command Query Responsibility Segregation)**: Separates read and write operations to optimize performance and scalability.
- **Containerization**:
  - **Docker Compose**: Simplifies the setup and management of the development environment by orchestrating multiple Docker containers.

## Architecture

The application is structured following the principles of Clean Architecture, ensuring separation of concerns and maintainability. The use of CQRS and the Mediator pattern further enhance the scalability and responsiveness of the system.

## Installation

To set up and run Fast Delivery Management locally, follow these steps:

1. **Clone the repository**:
    ```sh
    git clone https://github.com/yourusername/fast-delivery-management.git
    cd fast-delivery-management
    ```

2. **Install dependencies**:
    Ensure you have Docker and Docker Compose installed.

3. **Run the application**:
    ```sh
    docker-compose up
    ```

4. **Access the services**:
    - Order Service: `http://localhost:5001`
    - Identity Service: `http://localhost:5002`
    - Tracking Service: `http://localhost:5003`
    - API Gateway: `http://localhost:8000`

## Usage

### Placing an Order

To place an order, send a POST request to the Order Service:

```sh
curl -X POST "http://localhost:8000/api/order" -H "Content-Type: application/json" -d '{
  "customerId": "123",
  "items": [{"productId": "456", "quantity": 2}]
}'

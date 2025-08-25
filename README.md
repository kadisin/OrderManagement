# Order Management System

## Table of Contents

1. [Overview](#overview)  
2. [Architecture](#architecture)  
3. [Services](#services)  
   - [OrderService](#orderservice)
   - [NotificationService](#notificationservice)
4. [Infrastructure](#infrastructure)
5. [Communication](#communication-via-service-bus)
6. [Deployment](#deployment)
7. [Development Setup](#development-setup)
8. [Configuration](#configuration)
9. [Folder Structure](#folder-structure)
10. [Future Improvements](#future-improvements)

---

## Overview

OrderManagement is a distributed microservices-based system for managing customer orders and notifications. It uses **Azure Kubernetes Service (AKS)** for hosting, **Azure Service Bus** for communication, and **Azure SQL Database** for persistent storage.

Each service is containerized using Docker and deployed via **Terraform-based infrastructure-as-code**.

---

## Architecture

- 🧩 **Microservices** architecture with clean separation of responsibilities  
- ☁️ **Azure Service Bus** for event-driven communication  
- 🐳 **Docker** for containerization  
- 🔐 **Azure Key Vault** for secret management  
- 🏗️ **Terraform** for provisioning infrastructure  
- 📦 **AKS** for orchestration and deployment  
- 📡 **Azure SQL Server** for persistent data  

```plaintext
[Client] --> [OrderService API] --> [Azure SQL]
                        |
                     [Service Bus Topic]
                        ↓
               [NotificationService] --> [Email/Logs]
```

## Services

### 📦 OrderService

The **OrderService** is responsible for managing the lifecycle of customer orders. It serves as the core API responsible for creating, retrieving, and updating orders in the system.

#### ✅ Responsibilities

- Expose a REST API for order operations (`Create`, `Get`, `List`, etc.).
- Interact with the Azure SQL Database to persist order data.
- Publish `OrderCreated` events to Azure Service Bus upon successful creation.
- Serve as a microservice deployed independently on Azure Kubernetes Service (AKS).

#### 🔧 Technologies Used

- **.NET 8** Web API
- **Entity Framework Core** with Azure SQL
- **Azure Service Bus** (Topic Publisher)
- **Swagger** for API documentation
- **Azure Kubernetes Service (AKS)** for deployment

#### 📤 Message Publishing Flow

```
Client → OrderController → OrderService → Azure SQL
                                        ↘
                                       Azure Service Bus Topic (OrderCreated)
```

### 📨 NotificationService

The **NotificationService** listens to events published by the `OrderService` and performs post-order operations, such as sending notifications to users or admins.

#### ✅ Responsibilities

- Consume `OrderCreated` messages from Azure Service Bus.
- Process events and trigger appropriate notifications (e.g., email, log, webhook).
- Log received events and ensure idempotent message handling.
- Serve as an independent microservice deployed on AKS.

#### 🔧 Technologies Used

- **.NET 8** Worker Service
- **Azure Service Bus** (Topic Subscriber)
- **Serilog** for structured logging
- **Azure Kubernetes Service (AKS)** for hosting

#### 📥 Message Consumption Flow

```
Azure Service Bus Topic (OrderCreated) → ServiceBusConsumer → OrderCreatedHandler → Notification Logic (e.g., Email)
```

## Infrastructure

The `infrastructure/` folder contains modular and environment-specific Terraform configurations to provision Azure cloud resources for the Order Management system. The structure follows best practices for reusability and clarity.

```
infrastructure/
├── provider.tf         # Azure provider configuration (azurerm)
├── main.tf             # Core resources: Resource Group, AKS, Service Bus, SQL, etc.
├── variables.tf        # Input variables for parameterized deployments
├── outputs.tf          # Exposes key outputs like kubeconfig and service bus topics
├── terraform.tfvars    # Environment-specific values (e.g., dev, staging, prod)
└── modules/            # Reusable Terraform modules
    ├── aks/            # AKS (Azure Kubernetes Service) provisioning
    ├── servicebus/     # Azure Service Bus namespace, topics, and subscriptions
    └── sql/            # Azure SQL Server and Database configuration
```

### Component Overview

- **`provider.tf`**  
  Sets up the AzureRM provider, enabling Terraform to manage Azure resources.

- **`main.tf`**  
  Orchestrates the creation of essential infrastructure elements:
  - Azure Resource Group  
  - Azure Kubernetes Service (AKS)  
  - Azure Service Bus (topic and subscription)  
  - Azure SQL Database (if used)

- **`variables.tf`**  
  Defines customizable inputs such as region, resource names, and instance specifications to support environment-specific configurations.

- **`outputs.tf`**  
  Exposes key infrastructure outputs like the AKS kubeconfig, Service Bus topic, or SQL connection strings for integration into deployment pipelines.

- **`terraform.tfvars`**  
  Stores environment-specific values such as database credentials or service endpoints; typically excluded from version control for security.

- **`modules/` directory**  
  Encapsulates reusable infrastructure logic:
  - `aks/` – provisioning logic for AKS cluster  
  - `servicebus/` – actions to set up Azure Service Bus components  
  - `sql/` – logic for provisioning Azure SQL resources

## Communication Overview

This section describes the communication between the two core microservices in the OrderManagement system:

### OrderService → NotificationService

- **Role**: Implements event-driven communication through **Azure Service Bus**.
- When a new order is created, the `OrderService` publishes an `OrderCreated` event to an Azure Service Bus **topic**.
- The `NotificationService` subscribes to this topic and reacts to new order events asynchronously.

#### Flow:

1. **OrderService** receives an order `POST` request via its REST API.
2. It persists the order in the **SQL database**.
3. It then serializes an `OrderCreated` message and **publishes** it to a Service Bus **topic**.
4. **NotificationService** is listening on the corresponding **subscription**.
5. Upon receiving a message, it processes the order event (e.g., logs it, triggers an email notification, etc.).

### Synchronous API Communication (Optional Extension)

- Future enhancements may include direct API calls between services for synchronous operations, such as:
  - `OrderService` retrieving notification status from `NotificationService`.
  - Health check endpoints between services.

### Communication Summary

| Component              | Mechanism                        | Purpose                          |
|------------------------|----------------------------------|----------------------------------|
| OrderService           | REST API + Service Bus topic     | Create orders + publish events   |
| NotificationService    | Service Bus subscription         | Consume order events             |
| Future integrations    | REST API calls                   | Synchronous inter-service calls  |

---

### Why this approach?

- **Loose Coupling**: Services remain independent; communication is asynchronous.
- **Scalable & Resilient**: Message buses tolerate intermittent downtime and can buffer events.
- **Extendable**: Easily add more subscribers or new event types later.


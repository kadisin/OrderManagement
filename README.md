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

---

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

tbd
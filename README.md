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

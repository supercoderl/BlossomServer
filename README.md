# 🌸 BlossomServer

## Overview
**BlossomServer** is a backend application built with **.NET** following **Clean Architecture** and **Domain-Driven Design (DDD)** principles.  
It powers a **service booking and management platform**, enabling features such as appointment scheduling, technician management, payments, notifications, and analytics.

---

## 🏗️ Architecture

The solution is divided into multiple layers:

### 1. BlossomServer (API Layer)
- Contains **Controllers**, **Middlewares**, **HealthChecks**, and **Swagger**.  
- Acts as the **entry point**, exposing REST APIs and gRPC endpoints.

### 2. BlossomServer.Application
- Handles application-level business logic.  
- Includes:
  - **Behaviors** (pipeline)  
  - **EventHandlers**  
  - **gRPC Services**  
  - **SignalR Hubs**  
  - **Queries & Services**  
  - **ViewModels**

### 3. BlossomServer.Domain
- Encapsulates the **core domain logic**.  
- Contains:
  - **Entities & Value Objects**  
  - **Commands & Domain Events**  
  - **Notifications & Errors**  
  - **Interfaces & Helpers**

### 4. BlossomServer.Infrastructure
- Provides infrastructure and persistence.  
- Includes:
  - **Database configurations & Migrations**  
  - **Repositories**  
  - **Event Sourcing**  
  - **Background Services**

### 5. BlossomServer.ServiceDefaults
- Contains default service configurations and extensions.

### 6. BlossomServer.Shared
- Provides shared contracts and DTOs across modules.  
- Organized into **Categories**, **Events**, **Users**, etc.

### 7. BlossomServer.SharedKernel
- Utility classes and helpers:
  - **FileHelper**, **OAuthHelper**, **TextHelper**, **ValidationHelper**, etc.

---

## 🗄️ Database
- Uses **Entity Framework Core** with migrations.  
- Includes comprehensive **Stored Procedures** for reporting and analytics, such as:
  - Revenue calculation (`sp_calculate_revenue`)  
  - Booking statistics and breakdowns  
  - Customer retention and conversion rate  
  - Technician performance reports  
  - Service popularity and profitability metrics  

---

## 🚀 Features
- User authentication & OAuth integration  
- Booking & scheduling system  
- Payment processing  
- Notifications & email reminders  
- Service & technician management  
- Reporting & analytics (via stored procedures)  
- Background job processing with **Hangfire**

---

## ⚙️ Technologies
- **.NET / C#**  
- **Entity Framework Core**  
- **gRPC & SignalR**  
- **Hangfire** (background jobs & scheduling)  
- **Swagger / OpenAPI** for API documentation  
- **Docker** for containerization

---

## 📂 Project Structure
```BlossomServer
├── Controllers/ # API endpoints
├── Middlewares/ # Custom middleware
├── HealthChecks/ # System health monitoring
├── Application/ # Application logic
├── Domain/ # Core domain models
├── Infrastructure/ # Database & persistence
├── ServiceDefaults/ # Default service configs
├── Shared/ # Shared DTOs and contracts
└── SharedKernel/ # Utilities and helpers
```

---

## 📌 Summary
BlossomServer is designed as a **scalable, modular, and maintainable backend** for managing services, bookings, and users.  
By leveraging **Clean Architecture** and **DDD**, it ensures high cohesion, separation of concerns, and flexibility for future growth.

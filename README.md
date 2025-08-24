\# 🌸 BlossomServer



\## Overview

\*\*BlossomServer\*\* is a backend application built with \*\*.NET\*\* following \*\*Clean Architecture\*\* and \*\*Domain-Driven Design (DDD)\*\* principles.  

It powers a \*\*service booking and management platform\*\*, enabling features such as appointment scheduling, technician management, payments, notifications, and analytics.



---



\## 🏗️ Architecture



The solution is divided into multiple layers:



\### 1. BlossomServer (API Layer)

\- Contains \*\*Controllers\*\*, \*\*Middlewares\*\*, \*\*HealthChecks\*\*, and \*\*Swagger\*\*.  

\- Acts as the \*\*entry point\*\*, exposing REST APIs and gRPC endpoints.



\### 2. BlossomServer.Application

\- Handles application-level business logic.  

\- Includes:

&nbsp; - \*\*Behaviors\*\* (pipeline)  

&nbsp; - \*\*EventHandlers\*\*  

&nbsp; - \*\*gRPC Services\*\*  

&nbsp; - \*\*SignalR Hubs\*\*  

&nbsp; - \*\*Queries \& Services\*\*  

&nbsp; - \*\*ViewModels\*\*



\### 3. BlossomServer.Domain

\- Encapsulates the \*\*core domain logic\*\*.  

\- Contains:

&nbsp; - \*\*Entities \& Value Objects\*\*  

&nbsp; - \*\*Commands \& Domain Events\*\*  

&nbsp; - \*\*Notifications \& Errors\*\*  

&nbsp; - \*\*Interfaces \& Helpers\*\*



\### 4. BlossomServer.Infrastructure

\- Provides infrastructure and persistence.  

\- Includes:

&nbsp; - \*\*Database configurations \& Migrations\*\*  

&nbsp; - \*\*Repositories\*\*  

&nbsp; - \*\*Event Sourcing\*\*  

&nbsp; - \*\*Background Services\*\*



\### 5. BlossomServer.ServiceDefaults

\- Contains default service configurations and extensions.



\### 6. BlossomServer.Shared

\- Provides shared contracts and DTOs across modules.  

\- Organized into \*\*Categories\*\*, \*\*Events\*\*, \*\*Users\*\*, etc.



\### 7. BlossomServer.SharedKernel

\- Utility classes and helpers:

&nbsp; - \*\*FileHelper\*\*, \*\*OAuthHelper\*\*, \*\*TextHelper\*\*, \*\*ValidationHelper\*\*, etc.



---



\## 🗄️ Database

\- Uses \*\*Entity Framework Core\*\* with migrations.  

\- Includes comprehensive \*\*Stored Procedures\*\* for reporting and analytics, such as:

&nbsp; - Revenue calculation (`sp\_calculate\_revenue`)  

&nbsp; - Booking statistics and breakdowns  

&nbsp; - Customer retention and conversion rate  

&nbsp; - Technician performance reports  

&nbsp; - Service popularity and profitability metrics  



---



\## 🚀 Features

\- User authentication \& OAuth integration  

\- Booking \& scheduling system  

\- Payment processing  

\- Notifications \& email reminders  

\- Service \& technician management  

\- Reporting \& analytics (via stored procedures)  

\- Background job processing with \*\*Hangfire\*\*



---



\## ⚙️ Technologies

\- \*\*.NET / C#\*\*  

\- \*\*Entity Framework Core\*\*  

\- \*\*gRPC \& SignalR\*\*  

\- \*\*Hangfire\*\* (background jobs \& scheduling)  

\- \*\*Swagger / OpenAPI\*\* for API documentation  

\- \*\*Docker\*\* for containerization



---



\## 📂 Project Structure




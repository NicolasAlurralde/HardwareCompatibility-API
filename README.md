# 🖥️ PC Builder API - Hardware Compatibility Engine

A REST API built with **.NET 10** utilizing **Clean Architecture** and **Domain-Driven Design (DDD)** principles.

This project simulates the business rules engine behind a PC builder application. It enforces strict hardware compatibility validations (Sockets, RAM types, Power Consumption, PCIe bottlenecks) to ensure that the final build is both physically possible and optimized.

## 🚀 Technologies & Patterns
* C# / .NET 10
* Entity Framework Core (Code-First)
* SQL Server
* Clean Architecture / Repository Pattern
* RESTful API Design
* FluentValidation

## 🔥 Core Features
* **Smart Validation Engine:** Validates sockets, RAM generations, physical dimensions (Cooler height vs Case clearance), and power supply capacity including safety margins.
* **Hybrid Configurations:** Supports advanced PC builds with Hybrid Storage (M.2 NVMe + SATA) and Multi-GPU setups.
* **Warning System:** Differentiates between critical physical incompatibilities (Errors) and performance bottlenecks (Warnings, e.g., PCIe Gen3 GPU on a Gen4 Motherboard).
* **Flexible Checkout:** Accepts partial builds (upgrade kits) seamlessly handling `null` values for optional components.
* **Eager Loading Optimization:** Efficiently retrieves complex nested aggregates from the database using EF Core.

## 🚧 Project Status
* [x] Phase 1: Domain Modeling & Business Rules
* [x] Phase 2: Infrastructure & Database
* [x] Phase 3: Use Cases & Compatibility Engine
* [x] Phase 4: API, Swagger & Error Handling
* [x] Phase 5: Validation Layer & Business Rules
* [x] Phase 6: PC Build Engine & Checkout Endpoints (`/analyze` & `/save`)
* [ ] Phase 7: Catalog API & Smart Filters (In progress...)
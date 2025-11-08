# 📚 Book Library API

A simple RESTful API for managing books and borrowing them.  
Built with **.NET 9** and **Entity Framework Core**.

## 📑 Table of Contents

1. [📚 Book Library API](#-book-library-api)
2. [⚙️ Setup](#️-setup)
    - [1️⃣ Clone the repository](#1-clone-the-repository)
    - [2️⃣ Configure Connection String](#2-replace-configure-connection-string)
    - [3️⃣ Run API Project](#3-run-api-project)
    - [💡 Development Mode](#development-mode)
3. [🔗 API Endpoints](#-api-endpoints-with-curl-examples)
    - [➕ Add Book](#add-book)
    - [📖 Borrow Book](#borrow-book)
4. [🧠 Key Design Decisions](#-key-design-decisions)
    - [🪵 Logging via Middleware](#-logging-via-middleware)
    - [📦 Simple Domain Model Design](#-simple-domain-model-design)
    - [✅ Validation and Error Handling](#-validation-and-error-handling)
    - [📄 Pagination](#-pagination)
    - [🔒 Atomic Borrow Operation](#-atomic-borrow-operation)
    - [⚙️ Stored Procedure](#-stored-procedure)

## Setup
### 1. Clone the repository
```bash
git clone https://github.com/M0BIN-V/BookLibraryApi.git
```

### 2. Replace Configure Connection String
Open appsettings.json or appsettings.Development.json in `BookLibraryApi\Src\Api` and set ConnectionStrings:BookLibraryDb:
```json
{
  "ConnectionStrings": {
    "BookLibraryDb": "YOUR SQLSERVER CONNECTION STRING"
  }
}
```

### 3. Run API Project
In directory `BookLibraryApi\Src\Api` run this command:
```bash
dotnet run 
```

Done!

>Alternatively, you can run both the `API` and `SQL Server` through **Aspire** by launching `AppHost.csproj`

### Development Mode

When running in **Development** environment:

- All **EF Core migrations** will be **applied automatically** on startup.

- **Fake users and books** will be **seeded** into the database for testing.

So you don’t need to run:
```bash
dotnet ef database update
```

## API Endpoints (with CURL examples)

Below are some examples of how to interact with the API using `curl`.

### Add Book 
```bash
curl http://localhost:52636/api/Books \
  --request POST \
  --header 'Content-Type: application/json' \
  --data '{
  "title": "",
  "author": "",
  "genre": "",
  "publishedYear": 1
}'
```

### Borrow Book
```bash
curl http://localhost:52636/api/Books/1/borrow \
  --request POST \
  --header 'Content-Type: application/json' \
  --data '{
  "userId": 1
}'
```

## Key Design Decisions

### **Logging via Middleware**

Instead of logging in each controller action, a **Middleware** is used to handle request logging. This approach ensures that:

- Duplicate logging code is eliminated.

- All incoming requests — even those that do not reach the controller (e.g., due to validation errors) — are logged.

- Logging is centralized, clean, and easily extensible.


> This approach allows future integration of performance monitoring or audit logging systems without requiring changes in the controllers.

Additionally, if logging **domain-specific events** is needed, `ILogger` can be used within **services** or **controllers**.

### **Simple Domain Model Design**

At this stage, only two entities, `User` and `Book`, have been implemented.  
A separate table for borrow records (Borrow) was intentionally omitted to keep the model as **lightweight** and **easy to understand** as possible.

However, if tracking the borrowing history becomes necessary, the recommended approach is to create an independent table for it.  
This ensures better **scalability**, **separation of concerns**, and **maintainability** of the system.


### **Validation and error handling**

To handle logical errors, the **Result Pattern** has been implemented in order to:

- Avoid the overhead of throwing exceptions.

- Make the code more readable, predictable, and cleaner.

- Ensure that the system’s behavior in error scenarios is clear and controllable.

In situations where using **exceptions** is unavoidable, a **Global Exception Handler** can be used for centralized error management.

Additionally, for data validation, **DataAnnotations** are used given the simplicity of the current validation rules.  
However, in cases where more complex validation is required, **FluentValidation** is recommended.


### **Pagination**

Pagination is implemented using simple `page` and `size` parameters.  
If the goal is to further optimize **performance** and **resource usage**, and there is no need for complex data filtering, more advanced methods such as **Cursor-Based Pagination** can be used.


### **Atomic Borrow Operation**

The process of borrowing a book is performed **atomically** using the `ExecuteUpdateAsync` method,  
ensuring that the check for whether the book is available and the update of its status happen in a single atomic operation.

This ensures that:

- Two users cannot borrow the same book simultaneously.

- **Race conditions** are prevented.

If a higher level of coordination is required (especially in distributed systems),  
mechanisms such as **Redlock**, **queues**, or other distributed synchronization techniques can be used.


### **Stored Procedure**

According to the task requirements, a **Stored Procedure (SP)** is used to retrieve the list of books.

> In my opinion, in this project, the SP does not provide any significant advantage and has some drawbacks.  
> Using an SP moves part of the application logic to the database, which reduces **testability** and **maintainability**.  
> Any change in business logic would require modifying the SP in the database.

Using an SP is reasonable only in scenarios where one or more of the following conditions apply:

- The project is **DB-First** and some business logic is already implemented in the database.

- Or, a specific part of the system is **performance-critical**, requiring maximum database efficiency.
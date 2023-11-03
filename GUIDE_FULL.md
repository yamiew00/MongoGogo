## Table of Contents 

1. [Introduction](#introduction) 

2. [Features](#features) 

3. [Quick Start](#quick-start) 

4. [Setup Your Models](#setup-your-models) 

5. [Configure MongoGogo](#configure-mongogogo) 

6. [Methods](#methods)

7. [Change Streams and Event-Driven Data Management](#change-streams-and-event-driven-data-management)

8. [Bulk Operations for Efficient Data Management](#bulk-operations-for-efficient-data-management)

9. [Transaction Operations](#transaction-operations)

   


## Introduction 

MongoGogo is an ORM designed to simplify and enhance the integration of MongoDB into .NET projects. With its straightforward and powerful features, it provides a clear path for managing data with minimal setup. 



## Features

- **Generic Repository Pattern**: Simplify MongoDB interactions with .NET-friendly abstractions. 

- **ASP.NET Core Integration**: Built for easy integration into the ASP.NET Core framework. 
- **CRUD Operations**: Perform create, read, update, and delete operations with a fluent API.
- **Lambda Expressions**: Write more readable and maintainable code with lambda expressions. 
- **LINQ Support**: Query MongoDB documents directly in C# with LINQ. 
- **Bulk and Transactions**: Handle large datasets and transactions with ease. 





## Quick Start

Implement `MongoGogo` in your .NET project to enhance your MongoDB operations with minimal configuration.



### Installation

```
dotnet add package MongoGogo
```



### Setup Your Models

Define your POCO (Plain Old CLR Objects) data models and use attributes to associate them with the corresponding MongoDB collections. Here is an example of setting up your models and the context configuration in MongoGogo:

```csharp
// Example of a POCO linked to a MongoDB collection
[MongoCollection(fromDatabase: typeof(MyContext.StudentDb), collectionName: "students")]
public class Student
{
    [BsonId]
    public ObjectId Id { get; set; }
    public string Name { get; set; }
    public int Age { get; set; }
    // Additional properties as per your requirements
}

// Context class representing the MongoDB cluster with a generic type parameter of itself
public class MyContext : GoContext<MyContext>
{
    // Inner class representing a MongoDB database with a MongoDatabase attribute
    [MongoDatabase]
    public class StudentDb { }

    // Constructor accepting the MongoDB connection string
    public MyContext(string connectionString) : base(connectionString)
    {
    }
}
```

Each MongoDB database within your cluster should be represented by a public inner class (such as `StudentDb`) and decorated with a `MongoDatabase` attribute. The `GoContext` represents the MongoDB cluster and should be implemented with a generic type parameter of itself.

### 

### Configure MongoGogo

Register your custom context `MyContext` in your application configuration:

```c#
var builder = WebApplication.CreateBuilder(args);

// Register your custom MongoContext with the dependency injection container
builder.Services.AddMongoContext(new MyContext("your-mongodb-connection-string"));

// Continue with the rest of your application setup
```

This setup ensures that your application is aware of the database configurations and can interact with MongoDB through the MongoGogo ORM.

Make sure to replace `"your-mongodb-connection-string"` with the actual connection string to your MongoDB instance.



### **Methods**

1. **Find/FindAsync with Projection**:

In MongoGogo, projections allow you to select only the necessary fields from your documents. You can use projection to reduce the amount of data that MongoGogo returns by including or excluding fields. Here’s how you can perform a query with projection to get only specific fields:

```c#
// Asynchronously find all students over the age of 20 and return only their Id and Age fields.
var studentsOver20Async = await _studentCollection.FindAsync(
    student => student.Age > 20,
    projector => projector.Include(student => student.Id)
                          .Include(student => student.Age)
);
```

This code snippet uses the `FindAsync` method with a lambda expression to filter students by age and uses the `projector` to include only the `Id` and `Age` fields in the returned documents.



2. **InsertMany and InsertManyAsync**:

The `InsertMany` and `InsertManyAsync` methods allow you to insert multiple documents into a collection at once. This is more efficient than inserting each document individually because it reduces the number of database calls.

```c#
// Synchronously insert multiple new student records.
_studentCollection.InsertMany(new List<Student>
{
    new Student { Name = "Alice", Age = 23 },
    new Student { Name = "Bob", Age = 25 }
});

// Asynchronously insert multiple new student records.
await _studentCollection.InsertManyAsync(new List<Student>
{
    new Student { Name = "Charlie", Age = 20 },
    new Student { Name = "Dana", Age = 22 }
});
```



3. **UpdateOne, UpdateMany, UpdateOneAsync, UpdateManyAsync**:

`UpdateOne` and `UpdateOneAsync` update a single document in the collection that matches the given filter. `UpdateMany` and `UpdateManyAsync` update all documents that match the filter.

```c#
// Synchronously update the name of a single student from 'Jerrly' to 'Jerry'.
_studentCollection.UpdateOne(
    student => student.Name == "Jerrly",
    updater => updater.Set(student => student.Name, "Jerry")
);

// Asynchronously update the name of a single student from 'Jerrly' to 'Jerry'.
await _studentCollection.UpdateOneAsync(
    student => student.Name == "Jerrly",
    updater => updater.Set(student => student.Name, "Jerry")
);

// Synchronously update the age of all students named 'Alice' to 24.
_studentCollection.UpdateMany(
    student => student.Name == "Alice",
    updater => updater.Set(student => student.Age, 24)
);

// Asynchronously update the age of all students named 'Alice' to 24.
await _studentCollection.UpdateManyAsync(
    student => student.Name == "Alice",
    updater => updater.Set(student => student.Age, 24)
);
```



4. **DeleteMany and DeleteManyAsync**:

`DeleteMany` and `DeleteManyAsync` remove all documents that match the given filter from the collection.

```c#
// Synchronously delete all students named 'Bob'.
_studentCollection.DeleteMany(student => student.Name == "Bob");

// Asynchronously delete all students named 'Bob'.
await _studentCollection.DeleteManyAsync(student => student.Name == "Bob");
```

These methods provide a straightforward way to manage documents in bulk, making data manipulation more efficient.





### **Change Streams and Event-Driven Data Management**

MongoGogo harnesses the power of MongoDB's Change Streams to enable real-time tracking of data changes, a pivotal feature for building responsive, event-driven applications. Through `IGoCollectionObserver<TDocument>`, MongoGogo adeptly notifies subscribers of database operations like insertions, updates, replacements, or deletions on the `Student` collection. This functionality is key to creating applications that react promptly to data modifications, ensuring synchronized states across services and up-to-date user interfaces.

#### **Real-Time Notifications with IGoCollectionObserver\<TDocument>**

The `IGoCollectionObserver<TDocument>` interface observes changes in a collection and triggers event-driven notifications. This is especially beneficial for applications requiring instant responses to database changes, enabling a proactive and reactive system architecture.

Here's an example of setting up a notification center to listen for updates to the `Student` collection:

```c#
public class NotificationCenter
{
    public NotificationCenter(IGoCollectionObserver<Student> studentObserver)
    {
        studentObserver.OnUpdate(UpdateEvent);
    }

    private void UpdateEvent(Student student)
    {
        // Logic to respond to the updated student data goes here.
    }
}
```

In the `UpdateEvent` method, you can place any logic that needs to execute in response to an update in the `Student` collection, such as updating caches, sending notifications, or processing business logic.

For handling deletions, MongoGogo offers a mechanism to identify the removed documents:

```c#
public class NotificationCenter
{
    public NotificationCenter(IGoCollectionObserver<Student> studentObserver)
    {
        studentObserver.OnDelete(DeleteEvent);
    }

    private void DeleteEvent(ObjectId _id)
    {
        // Logic to handle the deletion of a student by ObjectId goes here.
    }
}
```

The `DeleteEvent` method will be invoked with the `_id` of the deleted `Student`, allowing you to perform any necessary post-deletion operations.

If the `_id` field of the `Student` model is of a different type, the `IGoCollectionObserver<TDocument>` allows you to specify this type:

```c#
public class NotificationCenter
{
    public NotificationCenter(IGoCollectionObserver<Student> studentObserver)
    {
        studentObserver.OnDelete<Guid>(DeleteEvent);
    }

    private void DeleteEvent(Guid _id)
    {
        // Logic to handle the deletion of a student with a specific Guid _id goes here.
    }
}
```

Leveraging `IGoCollectionObserver<TDocument>` within MongoGogo enables developers to design systems that are dynamic, maintainable, and scalable. This leads to applications that can efficiently react to data changes, providing a seamless and interactive experience for users.



### **Bulk Operations for Efficient Data Management**

MongoGogo's version 4.0.0 and later include robust support for bulk write operations, allowing for the efficient execution of multiple database changes in a single operation. This feature is crucial for operations that need to process large volumes of data quickly and atomically.

#### **Optimized Batch Processing with IGoBulker\<TDocument>**

Using the `IGoBulker<TDocument>` interface, MongoGogo facilitates batch processing of data manipulations, significantly optimizing performance by minimizing database round trips. This is particularly beneficial when handling substantial datasets or performing complex data import/export tasks.

Here's an illustrative example of how you can use bulk operations in a controller to add and update multiple `Student` records:

```c#
public class MyController : ControllerBase
{
    private readonly IGoCollection<Student> _studentCollection;

    public MyController(IGoCollection<Student> studentCollection)
    {
        this._studentCollection = studentCollection;
    }
    
    [HttpPost("AddAndUpdateStudents")]
    public async Task<IActionResult> AddAndUpdateStudentsAsync(Student student)
    {
        var goBulker = _studentCollection.NewBulker();
        
        // Add a new student to the collection
        goBulker.InsertOne(student);

        // Update an existing student's details
        goBulker.UpdateOne(
            stud => stud.Name == student.Name,
            builder => builder.Set(stud => stud.Age, 0)
        );
        
        // Execute all pending operations in a single batch
        await goBulker.SaveChangesAsync();
        
        return Ok();
    }
}
```

In this example:

- A new `goBulker` instance is initiated using the `NewBulker` method on `IGoCollection<Student>`.
- The `goBulker` object queues up multiple operations: adding a new `Student` and updating an existing `Student`'s age to 0.
- By invoking `SaveChangesAsync`, all queued operations are executed as a batch, resulting in a more efficient interaction with the database.

**The strategic use of `goBulker` emulates the transactional operation batching familiar to EF Core users, enabling developers to apply complex data changes with both precision and speed.**



### **Transaction Operations**

MongoGogo's transaction operations, introduced in version 5.1.0, empower developers to perform multiple database actions within a single atomic session. This feature is pivotal for maintaining data integrity when handling operations that must either all succeed together or fail without leaving partial changes.

#### **Atomic Operations with IGoTransaction\<TContext>**

The `IGoTransaction<TContext>` interface brings transactional control to your data operations, ensuring that a series of database modifications either all commit successfully or rollback in the event of an error. This atomicity guarantees consistency and is essential when dealing with critical data changes.

#### **Transactional Workflow: Commit and Rollback**

Here's an outline of how you can implement transactional operations with `IGoTransaction`:

- Begin by creating a transaction instance using the `CreateTransaction` method of `IGoFactory`.
- Add database operations such as `InsertOne`, `UpdateOne`, or any other required actions to the transaction.
- Use `Commit` or `CommitAsync` to apply the changes. If an exception is thrown during any of the operations, the transaction will automatically rollback, negating any partial changes made.
- Once committed, the transaction is finalized and cannot be reused or altered.

#### **Example: Managing Student Records in a Transaction**

```c#
public class MyController : ControllerBase
{
    private readonly IGoFactory<MyContext> _goFactory;

    public MyController(IGoFactory<MyContext> goFactory)
    {
        this._goFactory = goFactory;
    }
    
    [HttpPost("ManageStudentRecords")]
    public async Task<IActionResult> ManageStudentRecords()
    {
        var transaction = _goFactory.CreateTransaction();
        try
        {
            transaction.InsertOne(new Student { Name = "Alex", Age = 20 });
            transaction.UpdateOne(
                student => student.Name == "Alex" && student.Age == 20,
                builder => builder.Set(student => student.Age, 21)
            );
            transaction.Commit();
        }
        catch(Exception ex)
        {
            // Handle the exception knowing the database remains unchanged due to the rollback
            Console.WriteLine($"An error occurred: {ex.Message}");
            return BadRequest(ex.Message);
        }
        
        return Ok();
    }
}
```

In this example, we are adding a new `Student` record and updating an existing one within a transaction. If any operation fails, the entire transaction will rollback, maintaining database consistency.

#### **Bulk Operations within Transactions**

For complex scenarios where multiple bulk operations are needed, `IGoTransaction` also supports `IGoTransBulker<TDocument>` for batch processing within the transaction context. Here's how you can use it:

- Instantiate a new transaction with `CreateTransaction`.
- Create a `IGoTransBulker` for each collection you wish to perform bulk operations on.
- Queue up your bulk operations (inserts, updates, etc.) with `IGoTransBulker`.
- Commit the transaction to apply all changes atomically.

#### **Error Handling and Rollback Mechanics**

The transaction mechanism in MongoGogo is designed to automatically rollback if an error is detected during any of the operations, thus preventing any partial updates to the database. This feature is crucial for error handling:

- Always wrap transaction operations within a `try-catch` block.
- If an error occurs, handle it in the `catch` block and be assured that no partial changes have been made.
- Avoid re-committing after an exception; once a transaction has rolled back, it should be considered void.

By following these guidelines and using the transaction features of MongoGogo, developers can confidently manage complex data operations with the assurance of consistency and rollback capabilities in the face of errors.

**With `IGoTransaction`, you can orchestrate sophisticated data handling strategies that are resilient to failures, ensuring your database reflects a complete and consistent state or none at all.**

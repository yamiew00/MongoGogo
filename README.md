# MongoGogo

Quickly integrate MongoDB into your .NET projects with MongoGogo, a lightweight and powerful ORM designed to elevate your data layer with minimal effort.



## Why MongoGogo?

- **Simplify MongoDB Interactions**: Utilize generic repository patterns to interact with MongoDB collections using .NET-friendly abstractions. 
- **Seamless Integration**: Built to fit naturally into the ASP.NET Core framework, supporting dependency injection out of the box. 

- **Efficient Data Management**: Perform synchronous and asynchronous CRUD operations with ease, thanks to a clear and fluent API. 

- **Lambda Expressions**: Use the elegance of lambda expressions for more readable and maintainable update and delete operations. 

- **Flexible Querying**: Leverage the power of LINQ to query MongoDB documents directly in C#.

- **Bulk and Transaction Support**: Manage large datasets with bulk operations and maintain data integrity with transaction support.

  

## Quick Start

Implement `MongoGogo` in your .NET project to enhance your MongoDB operations with minimal configuration.



### Installation

```powershell
dotnet add package MongoGogo
```



### Setup

1. Define your POCO data models. Use attributes to link them to MongoDB collections.

```c#
// Example of a user-defined POCO linked to a MongoDB collection
[MongoCollection(fromDatabase: typeof(MyContext.StudentDb), collectionName: "students")]
public class Student
{
    [BsonId]
    public string Id { get; set; }
    
    public string Name { get; set; }
    public int Age { get; set; }
    // Additional properties can be added as required
}
```

`MyContext` is a placeholder for your custom context class, which manages database configurations like connection strings and database names. `StudentDb` represents the specific configuration for the `Student` collection within that context.



2. Configure `MongoGogo` in your `Program.cs`:

```c#
var builder = WebApplication.CreateBuilder(args);

// Register your custom context class with MongoGogo
builder.Services.AddMongoContext(new MyContext("your-mongodb-connection-string"));

// Continue setting up your application...
```

Replace `MyContext` with the name of your context class and `"your-mongodb-connection-string"` with your actual MongoDB connection string.



3. For more details on model mapping and attribute configuration, see the [setup your models section](https://github.com/yamiew00/MongoGogo/blob/main/GUIDE_FULL.md#4-setup-your-models) in our full documentation.



### Dependency Injection Setup

Once `MongoGogo` is configured in your `Program.cs`, you can inject `IGoCollection<T>` instances into your classes.

For example, in an ASP.NET Core controller:

```c#
public class StudentsController : ControllerBase
{
    private readonly IGoCollection<Student> _studentCollections;

    public StudentsController(IGoCollection<Student> studentCollections)
    {
        _studentCollections = studentCollections;
    }

    // ... CRUD operations using _students
}
```

Here, `StudentsController` is an example of a user-created ASP.NET Core controller.



### Basic CRUD Operations

MongoGogo simplifies CRUD operations with `IGoCollection<T>`:

- **Create**: Add new documents asynchronously. [Learn more about create operations](https://github.com/yamiew00/MongoGogo/blob/main/GUIDE_FULL.md#insertone-and-insertoneasync).
- **Read**: Query documents with filters and projections. [Learn more about read operations](https://github.com/yamiew00/MongoGogo/blob/main/GUIDE_FULL.md#findfindasync-with-projection).
- **Update**: Modify documents using lambda expressions for clarity. [Learn more about update operations](https://github.com/yamiew00/MongoGogo/blob/main/GUIDE_FULL.md#updateone-updatemany-updateoneasync-updatemanyasync).
- **Delete**: Remove documents with condition definitions. [Learn more about delete operations](https://github.com/yamiew00/MongoGogo/blob/main/GUIDE_FULL.md#deletemany-and-deletemanyasync).



## Transactional Support and Bulk Operations

**MongoGogo** provides robust support for more advanced database operations:

- **Transactional Support**: Process multiple operations as a single unit of work to maintain data integrity.
- **Bulk Operations**: Handle large numbers of operations in batches for efficiency.

[Learn more about bulk operations and transactions section](https://github.com/yamiew00/MongoGogo/blob/main/GUIDE_FULL.md#8-bulk-operations-for-efficient-data-management).



## Support

For detailed guidelines on configuration and mapping, review our [full documentation](https://github.com/yamiew00/MongoGogo/blob/main/GUIDE_FULL.md).

For assistance, reach out at [r05221017@gmail.com](mailto:r05221017@gmail.com).


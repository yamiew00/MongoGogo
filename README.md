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

```
dotnet add package MongoGogo
```



### Setup

Define your POCO data models. Use attributes to link them to MongoDB collections.

```c#
// Example of a POCO linked to a MongoDB collection
[MongoCollection(fromDatabase: typeof(MyContext.StudentDb), collectionName: "students")]
public class Student
{
    [BsonId]
    public ObjectId Id { get; set; }
    public string Name { get; set; }
    public int Age { get; set; }
    // Additional properties can be added as required
}
```

Here, `MyContext.StudentDb` signifies a dedicated database configuration within your context for the `Student` collection.

Configure `MongoGogo` in your `Program.cs`:

```c#
var builder = WebApplication.CreateBuilder(args);

// Register your custom MongoContext
builder.Services.AddMongoContext(new MyContext("your-mongodb-connection-string"));

// Continue setting up your application...
```



### Basic CRUD Operations

Effortlessly perform CRUD operations on your collections using `IGoCollection`.

#### Create

```c#
public async Task CreateStudentAsync(IGoCollection<Student> students, Student newStudent)
{
    await students.InsertOneAsync(newStudent);
}
```

#### Read

```c#
public async Task<IEnumerable<Student>> FindStudentsOverAgeAsync(IGoCollection<Student> students, int age)
{
    // Use lambda expression directly to filter students over a certain age
    return await students.FindAsync(student => student.Age > age);
}
```

#### Read with Projection

```c#
public async Task<IEnumerable<StudentInfo>> FindStudentsWithProjectionAsync(IGoCollection<Student> students, int age)
{
    // Use lambda expression for filtering and projection to select specific fields
    return await students.FindAsync(
        student => student.Age > age,
        projecter => projecter.Include(student => student.Id)
        					  .Include(student => student.Name));
}
```

#### Update

Use the power of lambda expressions for clear and concise update operations.

```c#
public async Task UpdateStudentAgeAsync(IGoCollection<Student> students, string studentId, int newAge)
{
    await students.UpdateOneAsync(
        student => student.Id == studentId,
        updater => updater.Set(student => student.Age, newAge)
    );
}
```

#### Update multiple fields simultaneously:

```c#
public async Task UpdateStudentDetailsAsync(IGoCollection<Student> students, string studentId, int newAge, string newName)
{
    await students.UpdateOneAsync(
        student => student.Id == studentId,
        updater => updater.Set(student => student.Age, newAge)
                          .Set(student => student.Name, newName)
    );
}
```

#### Delete

Lambda expressions also simplify delete operations, allowing for expressive condition definitions.

```c#
public async Task DeleteStudentsByAgeAsync(IGoCollection<Student> students, int ageThreshold)
{
    await students.DeleteManyAsync(student => student.Age <= ageThreshold);
}
```



## Support

For comprehensive guidelines on context configuration and model mapping, dive into our [full documentation](https://github.com/yamiew00/MongoGogo/blob/main/GUIDE_FULL.md).

Need help? Reach out at [r05221017@gmail.com](mailto:r05221017@gmail.com).

# **MongoGogo Guide**

- A generic repository implementation using [MongoDB.Driver](https://www.mongodb.com/docs/drivers/csharp/)
- Auto implement the dependency injection for [Microsoft.Extensions.DependencyInjection](https://www.nuget.org/packages/Microsoft.Extensions.DependencyInjection.Abstractions).
- Provides attributes used for manage `databases` and `collections` for each `connection`.



## **Install via .NET CLI**

```
dotnet add package MongoGogo 
```



# **Example**

After few steps of `configuration` and `class building`, you can easily get any `Collection`, `Database` and `context` in abstract through the dependency resolution system.

```c#
public class MyController : ControllerBase
{
    private readonly IGoContext<MyMongoDBContext> MyContext;
    private readonly IGoDatabase<MyMongoDBContext.City> CityDatabase;
    private readonly IGoCollection<Hospital> HospitalCollection;

    public MyController(IGoContext<MyMongoDBContext> myContext,
                        IGoDatabase<MyMongoDBContext.City> cityDatabase,
                        IGoCollection<Hospital> hospitalCollection)
    {
        this.MyContext = myContext;
        this.CityDatabase = cityDatabase;
        this.HospitalCollection = hospitalCollection;
    }
}
```

-------

`IGoContext<T>`、`IGoDatabase<T>` and `IGoCollection<T>`:

- is in generic type with very few restriction
- acts like `IMongoClient` 、`IMongoDatabase` and `IMongoCollection<T>` of the well-known package [MongoDB.Driver](https://www.mongodb.com/docs/drivers/csharp/).



```c#
using MongoDB.Driver;
public class MyController : ControllerBase
{
    [HttpGet("IGoCollection<T>_GetList")]
    public async Task<IActionResult> Collection()
    {
        var hospitals = (await HospitalCollection.FindAsync(_ => true)).ToEnumerable();
        return Ok(hospitals);
    }

    [HttpGet("IGoDatabase<T>_ListAllCollections")]
    public IActionResult Database()
    {
        var collectionNames = CityDatabase.ListCollectionNames().ToList();
        return Ok(collectionNames);
    }

    [HttpGet("IGoContext_DeleteDatabasebyName")]
    public IActionResult Context(string databaseName)
    {
        MyContext.DropDatabase(databaseName);
        return Ok($"{databaseName} is successfully droped.");
    }
}
```

----





## **Configuration**

In the `ConfigureSerivces` segment of `.net core`, pass an `IGoContext` instance into your `IServiceCollection`.

```c#
public void ConfigureServices(IServiceCollection services)
{
    services.AddMongoContext(new MyMongoDBContext("my mongodb connection string"));
}
```



Or, you can configure mongoContext through implementationFactory

```c#
public void ConfigureServices(IServiceCollection services)
{
    Func<IServiceProvider, object> implementationFactory; //some custom factory
    services.AddMongoContext<MyMongoDBContext>(implementationFactory);
}
```



## **Context**

`IGoContext<TContext>` 

- Stands for a MongoDB Connection (using package MongoDB.Driver)
- an instance stores `connection string` and is in charge of the structure of an `entity`.
- generic TContext is used for dependency resolution system. 

```c#
public class MyMongoDBContext : GoContext<MyMongoDBContext>
{
    [MongoDatabase]
    public class City { }

    [MongoDatabase("Log")]
    public class Logger { }

    [MongoDatabase]
    public class Admin { }
    
    public MyMongoDBContext(string connectionString) : base(connectionString)
    {
    }
}
```

```
public abstract class GoContext<TContext> : IGoContext<TContext> 
{
    //...
}
```



**Congratulations!!** You successful set up the `database ` in the `IGoContext`.

A vaild `database`  of  an `entity/context` must be:

- an `inner class ` of `IGoContext<TContext>` which is  `public` or `internal` 
- decorated by `MongoDatabase` attribute



## **Collections**

Provided the database `City` has two collection `City` and `Hospital`. 

- decorated by `MongoCollection` attribute, and specify corresponding `database` type. 
- Notice once again, this `database` type must be an `inner class  `of an `IGoContext<TContext>`

```c#
[MongoCollection(fromDatabase: typeof(MyMongoDBContext.City))]
public class City
{
    [BsonId]
    public ObjectId _id { get; set; }

    public string Name { get; set; }

    public int Polulation { get; set; }
}
```

Besides, keep the old way using attribute of [MongoDB.Driver](https://www.mongodb.com/docs/drivers/csharp/), if you have some restriction on this `collection`, `field`, or even some `Serializer`.

```c#
using MongoDB.Bson.Serialization.Attributes;

[MongoCollection(typeof(MyMongoDBContext.City),"Hospital"]
[BsonIgnoreExtraElements]
public class Hospital
{
    [BsonId]
    public ObjectId _id { get; set; }

    [BsonElement("FullName")]
    public string Name { get; set; }

    [BsonIgnoreIfDefault]
    public City City { get; set; }
}
```


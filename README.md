- #### **MongoGogo Guide**

  > MongoGogo is a generic repository implementation based on ORM-like (Object-Relational Mapping) technology, aimed at helping developers effortlessly interact with databases in their applications. This package is designed to simplify database access, provide a more convenient API, and separate business logic from the details of data storage.

  #### Key Features

  - An ORM-like implementation using [MongoDB.Driver](https://www.mongodb.com/docs/drivers/csharp/), which simplifies database interactions.
  - Automatic implementation of dependency injection for [Microsoft.Extensions.DependencyInjection](https://www.nuget.org/packages/Microsoft.Extensions.DependencyInjection.Abstractions), streamlining the setup process.
  - Provides attributes for managing `databases` and `collections` for each `connection`, allowing for easy configuration and customization.
  - Each interface is a unique implementation, with the distinction between different interfaces determined by their respective types.

  ##### **Install via .NET CLI**

  ```csharp
  dotnet add package MongoGogo 
  ```

  #### **Example**

  - After few steps of `configuration` and `class building`(introduced later), you can easily get your `Collection` in abstract through the dependency resolution system.

  ```c#
  public class MyController : ControllerBase
  {
      private readonly IGoCollection<Hospital> HospitalRepository;
  
      public MyController(IGoCollection<Hospital> hospitalRepository)
      {
        this.HospitalRepository = hospitalRepository;
      }
  }
  ```

  - Easily access data.

  ```c#
  public class MyController : ControllerBase
  {
      [HttpGet("IGoCollection<Hospital>_GetList")]
      public async Task<IActionResult> Hospitals()
      {
          var hospitals = await HospitalRepository.FindAsync(_ => true);
          return Ok(hospitals);
      }
  }
  ```

  - Additionally, MongoGogo provides abstract access to any `Collection`, `Database`, and `context`!

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

------

`IGoContext<T>`、`IGoDatabase<T>` and `IGoCollection<T>`:

- Are of generic type with very few restrictions
- Act like `IMongoClient`, `IMongoDatabase`, and `IMongoCollection<T>` from the well-known package [MongoDB.Driver](https://www.mongodb.com/docs/drivers/csharp/).

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

------

#### **Configuration**

In the `ConfigureSerivces` segment of `.net core`, pass an `IGoContext` instance into your `IServiceCollection`.

The build of concrete class `MyMongoDBContext` is introduced next part.

```c#
public void ConfigureServices(IServiceCollection services)
{
    services.AddMongoContext(new MyMongoDBContext("my mongodb connection string"));
}
```

Alternatively, you can configure the mongoContext through an implementationFactory.

```c#
public void ConfigureServices(IServiceCollection services)
{
    Func<IServiceProvider, object> implementationFactory; //some custom factory
    services.AddMongoContext<MyMongoDBContext>(implementationFactory);
}
```

------

- LifeCycleOption: `Singleton`, `Scoped`, `Transient`

In addition, you can control the lifecycle of all `IGoContext<T>`, `IGoDatabase<T>`, `IGoCollection<T>`, and `IMongoCollection<T>` using LifeCycleOption. The default is `Scoped`.

```c#
public void ConfigureServices(IServiceCollection services)
{
    services.AddMongoContext(new MyMongoDBContext("my mongodb connection string"),
                 new LifeCycleOption
                                 {
                                     ContextLifeCycle = LifeCycleType.Singleton,
                                     DatabaseLifeCycle = LifeCycleType.Scoped,
                   GoCollectionLifeCycle = LifeCycleType.Transient,
                                     MongoCollectionLifeCycle = LifeCycleType.Scoped
                                 });
}
```

#### **Core Components: [Context], [Database], and [Collections]**

##### **[Context] And [Database]**

```xml
IGoContext<TContext>
```

- Represents a MongoDB Connection (using the MongoDB.Driver package)
- An instance stores the `connection string` and is responsible for the structure of an `entity`.
- The generic TContext, which refers to the type itself, is used for the `dependency resolution system`.

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
public abstract class GoContext<TContext> : IGoContext<TContext> 
{
    //...
}
```

**Congratulations!** You have successfully set up the `database` in the `IGoContext`.

A vaild `database` of an `entity/context` must be:

- An `inner class `of `IGoContext<TContext>` which is `public` or `internal`
- Decorated with the `MongoDatabase` attribute

##### **[Collections]**

Assuming the `City` database has two collections, `City` and `Hospital`:

- Decorated with the `MongoCollection` attribute, and specify the corresponding `database` type.
- Note that this `database` type must be an `inner class` of an `IGoContext<TContext>`

```c#
// 'City' Collection
[MongoCollection(fromDatabase: typeof(MyMongoDBContext.City))]
public class City
{
    [BsonId]
    public ObjectId _id { get; set; }

    public string Name { get; set; }

    public int Population { get; set; }
}
```

Alternatively, you can use the attribute of [MongoDB.Driver](https://www.mongodb.com/docs/drivers/csharp/) if you have some restrictions on this `collection`, `field`, or even some `Serializer`.

```c#
using MongoDB.Bson.Serialization.Attributes;

// 'Hospital' Collection
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

By default, resolution system will build exactly one `IGoCollection<TDocument>` for every `TDocument` decorated by `[MongoCollection]` .

`IGoCollection<TDocument>` is responsible for the connection between a MongoDB connection and your project.

```c#
public class MyClass
{
    private readonly IGoCollection<Hospital> HospitalGoCollection;
  private readonly IMongoCollection<Hospital> HospitalMongoCollection;

    public MyClass(IGoCollection<Hospital> hospitalGoCollection,
               IMongoCollection<Hospital> hospitalMongoCollection)
    {
        this.HospitalGoCollection = hospitalGoCollection;        
        this.HospitalMongoCollection = hospitalMongoCollection;
    }
}
```

In fact, you can work with data using either `IGoCollection<Hospital>` or `IMongoCollection<Hospital>`.

The main differences between them are:

- `IGoCollection<Hospital>` is a packaged instance, but with handy methods to operate on your data.
- `IMongoCollection<Hospital>` is the instance from [MongoDB.Driver](https://www.mongodb.com/docs/drivers/csharp/)

```c#
public async Task AddNewHospital(Hospital hospital)
{
    HospitalCollection.InsertOne(hospital);
    HospitalMongoCollection.InsertOne(hospital);
}
```

- `IGoCollection<Tdocument>` has basic method dealing your data, like `FindOneAsync`, `InsertOne`, `ReplaceOne`, `Count`, and so on.
- If you want to add extra features, you can **do it yourself** by overriding the original methods!

###### **Custom [Collection]**

If the basic functionality of `IGoCollection<TDocument>` does not meet your needs,

*(for example: printing a message and saving it as a local log)*

you can inherit `GoCollectionAbstract<TDocument>` and create your own child class.

```c#
public class MyCustomHosipitalCollection : GoCollectionAbstract<Hospital>
{
    //You dont need to worry how to deal with the  IMongoCollection instance here, the resolution system will take it.
    public HospitalRepository(IMongoCollection<Hospital> collection) : base(collection)
    {
    }
    
    //write your own code here.
    public override void InsertOne(Hospital hospital)
    {
        base.InsertOne(hospital);
        Logger.PrintAndSave(hospital); //print insert success and save
    }
}
```

After this, the resolution system will map the new child class `MyCustomHosipitalCollection` to `IGoCollection<Hospital>`.

```csharp
public class MyClass
{
  private readonly IGoCollection<Hospital> HospitalGoCollection; //actually your new class HospitalRepository
    private readonly IMongoCollection<Hospital> HospitalMongoCollection; //not changed
  

    public MyClass(IGoCollection<Hospital> hospitalGoCollection,
             IMongoCollection<Hospital> hospitalMongoCollection)
    {
        this.HospitalGoCollection = hospitalGoCollection; //actually your new class HospitalRepository
        this.HospitalMongoCollection = hospitalMongoCollection; //not changed
    }
    
    public async Task AddNewHospital(Hospital hospital)
    {
        HospitalGoCollection.InsertOne(hospital);  // it will print insert success and save

        HospitalMongoCollection.InsertOne(hospital);  // nothing else
    }
}
```

- By customizing and extending the functionality provided by the package, you can tailor your solution to your specific needs while still leveraging the powerful features and ease of use provided by the package.

#### **IGoCollection Methods**

This section showcases the basic methods of `IGoCollection<TDocument>`.

```c#
IGoCollection<City> CityCollection;
```

##### **Count / CountAsync**

```c#
var documentCount = CityCollection.Count(city => city.Population >= 1000);
var documentCountAsync = await CityCollection.CountAsync(city => city.Population >= 1000);
```

##### **Find/FindAsync**

```c#
var foundDocuments = CityCollection.Find(city => city.Population >= 1000);
var foundDocumentAsync = await CityCollection.FindAsync(city => city.Population >= 1000);
```

- with `goFindOption` (optional)

```c#
var foundDocuments = CityCollection.Find(city => city.Population >= 1000,
                                         goFindOption: new GoFindOption
                                        {
                                            AllowDiskUse = true,
                                            Limit = 2,
                                            Skip = 1
                                        });
var foundDocumentAsync = await CityCollection.FindAsync(city => city.Population >= 1000, 
                                                        goFindOption: new GoFindOption
                                                        {
                                                            AllowDiskUse = true,
                                                            Limit = 2,
                                                            Skip = 1
                                                        });
```

- field reduction with `projection`(optional)

```c#
var reducedDocuments = CityCollection.Find(city => city.Population >= 1000,
                                           projection: builder => builder.Include(city => city.Population)
                                                                         .Include(city => city.Name));
var reducedDocumentAsync = await CityCollection.FindAsync(city => city.Population >= 1000, 
                                                          projection: builder => builder.Include(city => city.Population)
                                                                                        .Include(city => city.Name));
```

##### **FindOne/FindOneAsync**

```c#
var firstOrDefaultDocument = CityCollection.FindOne(city => city.Population >= 1000);
var firstOrDefaultDocumentAsync = await CityCollection.FindOneAsync(city => city.Population >= 1000);
```

##### **InsertOne/InsertOneAsync**

```c#
CityCollection.InsertOne(new City{Name = "New York"});
await CityCollection.InsertOneAsync(new City{Name = "New York"});
```

##### **InsertMany/InsertManyAsync**

```c#
var cityList = new List<City>
{
  new City{}, 
  new City{Name = "New York"}, 
  new City{Population = 100}
};

CityCollection.InsertMany(cityList);
await CityCollection.InsertManyAsync(cityList);
```

##### **ReplaceOne/ReplaceOneAsync**

```apl
var city = new City
{
  Name = "NewYork_America"
};

CityCollection.ReplaceOne(c => c.Name == "NewYork",
              city);
await CityCollection.ReplaceOneAsync(c => c.Name == "NewYork",
                   city);
```

- with `upsert`(optional)

```c#
CityCollection.ReplaceOne(c => c.Name == "NewYork",
                          city,
                          isUpsert: true);
await CityCollection.ReplaceOneAsync(c => c.Name == "NewYork",
                                     city,
                                     isUpsert: true);
```

##### **UpdateOne/UpdateOneAsync**

```c#
CityCollection.UpdateOne(city => city.Name == "New York",
                         builder => builder.Set(city => city.Name, "New York_America")
                                   .Set(city => city.Population, 1000));
await CityCollection.UpdateOneAsync(city => city.Name == "New York",
                                    builder => builder.Set(city => city.Name, "New York_America")
                                              .Set(city => city.Population, 1000));
```

- with `upsert`(optional)

```c#
CityCollection.UpdateOne(city => city.Name == "New York",
                         builder => builder.Set(city => city.Name, "New York_America")
                                   .Set(city => city.Population, 1000),
                         isUpsert: true);
await CityCollection.UpdateOneAsync(city => city.Name == "New York",
                                    builder => builder.Set(city => city.Name, "New York_America")
                                              .Set(city => city.Population, 1000),
                                   isUpsert: true);
```

##### **UpdateMany/UpdateManyAsync**

```c#
CityCollection.UpdateMany(city => city.Name == "New York",
                          builder => builder.Set(city => city.Name, "New York_America")
                                    .Set(city => city.Population, 1000));
await CityCollection.UpdateManyAsync(city => city.Name == "New York",
                                     builder => builder.Set(city => city.Name, "New York_America")
                                               .Set(city => city.Population, 1000));
```

##### **DeleteOne/DeleteOneAsync**

```c#
CityCollection.DeleteOne(city => city.Name == "New York");
await CityCollection.DeleteOneAsync(city => city.Name == "New York");
```

##### **DeleteMany/DeleteManyAsync**

```c#
CityCollection.DeleteMany(city => city.Name == "New York");
await CityCollection.DeleteManyAsync(city => city.Name == "New York");
```

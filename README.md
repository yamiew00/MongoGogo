## Links

- [Nuget](https://www.nuget.org/packages/MongoGogo)

- [Release Notes](https://github.com/yamiew00/MongoGogo/releases)



## **MongoGogo Guide**

> MongoGogo is a generic repository implementation based on ORM-like (Object-Relational Mapping) technology, aimed at helping developers effortlessly interact with databases in their applications. This package is designed to simplify database access, provide a more convenient API, and separate business logic from the details of data storage.

#### 1. Key Features

- An ORM-like implementation using [MongoDB.Driver](https://www.mongodb.com/docs/drivers/csharp/), which simplifies database interactions.

- Easily access data.

- Inspired by EF Core, offering object-oriented database operations.

- Attribute-based management of database objects using [MongoDatabase] and [MongoCollection] attributes.

- Seamless integration with Microsoft.Extensions.DependencyInjection for easy dependency injection setup.

- Support for MongoDB change stream, allowing real-time monitoring and processing of database changes.

- Bulk Operations support for efficient execution of large-scale data operations.

  

##### **Install via .NET CLI**

```csharp
dotnet add package MongoGogo 
```





#### 2. **Example**

- Few steps of `configuration` and `class building`(introduced later) to get `Collection` in abstract through the dependency resolution system.

```c#
public class MyController : ControllerBase
{
    private readonly IGoCollection<Hospital> _hospitalCollection;

    public MyController(IGoCollection<Hospital> hospitalCollection)
    {
      this._hospitalCollection = hospitalCollection;
    }
    
    [HttpGet("GetHospitalsAsync")]
    public async Task<IActionResult> Hospitals()
    {
        var hospitals = await _hospitalCollection.FindAsync(_ => true);
        return Ok(hospitals);
    }
    
    [HttpGet("GetHospitals")]
    public async Task<IActionResult> Hospitals()
    {
        var hospitals = _hospitalCollection.Find(_ => true);
        return Ok(hospitals);
    }
}
```





- Additionally, MongoGogo provides abstract access to any `Collection`, `Database`, and `context`!

```c#
public class MyController : ControllerBase
{
    private readonly IGoContext<MyMongoDBContext> _myContext;
    private readonly IGoDatabase<MyMongoDBContext.City> _cityDatabase;
    private readonly IGoCollection<Hospital> _hospitalCollection;

    public MyController(IGoContext<MyMongoDBContext> myContext,
                        IGoDatabase<MyMongoDBContext.City> cityDatabase,
                        IGoCollection<Hospital> hospitalCollection)
    {
        this._myContext = myContext;
        this._cityDatabase = cityDatabase;
        this._hospitalCollection = hospitalCollection;
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
    [HttpGet("GetHospitals")]
    public async Task<IActionResult> Hospitals()
    {
        var hospitals = _hospitalCollection.Find(_ => true);
        return Ok(hospitals);
    }

    [HttpGet("ListCollections")]
    public IActionResult ListCollections()
    {
        var collectionNames = _cityDatabase.ListCollectionNames().ToList();
        return Ok(collectionNames);
    }

    [HttpGet("DropDatabase")]
    public IActionResult DropDatabase(string databaseName)
    {
        _myContext.DropDatabase(databaseName);
        return Ok($"{databaseName} is successfully droped.");
    }
}
```





#### 3. **Configuration**

In the `ConfigureSerivces` segment of `.net core`, pass an `IGoContext` instance into your `IServiceCollection`.

The build of concrete class `MyMongoDBContext` is introduced next part.

```c#
public void ConfigureServices(IServiceCollection services)
{
	services.AddMongoContext(new MyMongoDBContext("my mongodb connection string"));
    
    //...other services
}
```



------

- LifeCycleOption: `Singleton`, `Scoped`, `Transient`

In addition, you can control the lifecycle of all `IGoContext<T>`, `IGoDatabase<T>`, `IGoCollection<T>`, and `IMongoCollection<T>` using LifeCycleOption. The default is `Singleton`.

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





#### 4. **Core Components: [Context], [Database], and [Collections]**

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





#### 5. **[IGoCollection\<TDocument>] Methods**

This section showcases the basic methods of `IGoCollection<TDocument>`.

```c#
IGoCollection<City> _cityCollection;
```



##### **Count / CountAsync**

```c#
var documentCount = _cityCollection.Count(city => city.Population >= 1000);
var documentCountAsync = await _cityCollection.CountAsync(city => city.Population >= 1000);
```



##### **Find/FindAsync**

```c#
var foundDocuments = _cityCollection.Find(city => city.Population >= 1000);
var foundDocumentAsync = await _cityCollection.FindAsync(city => city.Population >= 1000);
```

- with `goFindOption<TDocument>` (optional)

```c#
var foundDocuments = _cityCollection.Find(city => city.Population >= 1000,
                                          goFindOption: new GoFindOption<City>
                                         {
                                             AllowDiskUse = true,
                                             Limit = 2,
                                             Skip = 1
                                         });
var foundDocumentAsync = await _cityCollection.FindAsync(city => city.Population >= 1000, 
                                                         goFindOption: new GoFindOption<City>
                                                         {
                                                             AllowDiskUse = true,
                                                             Limit = 2,
                                                             Skip = 1
                                                         });
```

- field reduction with `projection`(optional)

```c#
var projectedDocuments = _cityCollection.Find(city => city.Population >= 1000,
                                              projection: builder => builder.Include(city => city.Population)
                                                                            .Include(city => city.Name));
var projectedDocumentAsync = await _cityCollection.FindAsync(city => city.Population >= 1000, 
                                                             projection: builder => builder.Include(city => city.Population)
                                                                                           .Include(city => city.Name));
```



##### **FindOne/FindOneAsync**

```c#
var firstOrDefaultDocument = _cityCollection.FindOne(city => city.Population >= 1000);
var firstOrDefaultDocumentAsync = await _cityCollection.FindOneAsync(city => city.Population >= 1000);
```



##### **InsertOne/InsertOneAsync**

```c#
_cityCollection.InsertOne(new City{Name = "New York"});
await CityCollection._cityCollection(new City{Name = "New York"});
```



##### **InsertMany/InsertManyAsync**

```c#
var cityList = new List<City>
{
  new City{}, 
  new City{Name = "New York"}, 
  new City{Population = 100}
};

_cityCollection.InsertMany(cityList);
await _cityCollection.InsertManyAsync(cityList);
```



##### **ReplaceOne/ReplaceOneAsync**

```apl
var city = new City
{
  Name = "NewYork_America"
};

_cityCollection.ReplaceOne(c => c.Name == "NewYork",
             			   city);
await _cityCollection.ReplaceOneAsync(c => c.Name == "NewYork",
                   					  city);
```

- with `upsert`(optional)

```c#
_cityCollection.ReplaceOne(c => c.Name == "NewYork",
                           city,
                           isUpsert: true);
await _cityCollection.ReplaceOneAsync(c => c.Name == "NewYork",
                                      city,
                                      isUpsert: true);
```



##### **UpdateOne/UpdateOneAsync**

```c#
_cityCollection.UpdateOne(city => city.Name == "New York",
                          builder => builder.Set(city => city.Name, "New York_America")
                                            .Set(city => city.Population, 1000));
await _cityCollection.UpdateOneAsync(city => city.Name == "New York",
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
_cityCollection.UpdateMany(city => city.Name == "New York",
                           builder => builder.Set(city => city.Name, "New York_America")
                                             .Set(city => city.Population, 1000));
await _cityCollection.UpdateManyAsync(city => city.Name == "New York",
                                      builder => builder.Set(city => city.Name, "New York_America")
                                                        .Set(city => city.Population, 1000));
```



##### **DeleteOne/DeleteOneAsync**

```c#
_cityCollection.DeleteOne(city => city.Name == "New York");
await _cityCollection.DeleteOneAsync(city => city.Name == "New York");
```



##### **DeleteMany/DeleteManyAsync**

```c#
_cityCollection.DeleteMany(city => city.Name == "New York");
await _cityCollection.DeleteManyAsync(city => city.Name == "New York");
```





#### 6. IGoCollectionObserver\<TDocument>

> `IGoCollectionObserver<TDocument>` implements the observer pattern for a collection of `TDocument`. It notifies subscribers when database operations such as `Insert`, `Update`, `Replace`, or `Delete` are performed on the collection.



- Here is an example code for a notification center class that receives notifications when hospital data is updated:

```c#
public class NotificationCenter
{
	public NotificationCenter(IGoCollectionObserver<Hospital> hospitalObserver)
	{
		hospitalObserver.OnUpdate(UpdateEvent);
	}
	
	private void UpdateEvent(Hospital hospital) 
	{
		//do something with hospital
	}
}
```

Do anything you want in `UpdateEvent` method.



- Unlike the other operations, the `Delete` operation returns the `_id` field. For general usage, you can use `OnDelete` method to get an `ObjectId` which was deleted from your collection

```c#
public class NotificationCenter
{
	public NotificationCenter(IGoCollectionObserver<Hospital> hospitalObserver)
	{
		hospitalObserver.OnDelete(DeleteEvent);
	}
	
	private void DeleteEvent(ObjectId _id) 
	{
		//do something with hospital
	}
}
```



- However, there are sometimes we have other type as our `_id` field. In this scenario, choose `OnDelete<TBsonIdType>` method to get your `_id` back.

```
public class NotificationCenter
{
	public NotificationCenter(IGoCollectionObserver<Hospital> hospitalObserver)
	{
		hospitalObserver.OnDelete<int>(DeleteEvent);
	}
	
	private void DeleteEvent(int _id) 
	{
		//do something with hospital
	}
}
```





#### 7. Bulk Operations (IGoBulker\<TDocument>)

> Starts from 4.0.0, bulk write is supported.

```c#
public class MyController : ControllerBase
{
    private readonly IGoCollection<Hospital> _hospitalCollection;

    public MyController(IGoCollection<Hospital> hospitalCollection)
    {
      this._hospitalCollection = hospitalCollection;
    }
    
    [HttpPost("AddHospitalsAsync")]
    public async Task<IActionResult> AddHospitalsAsync(Hospital hospital)
    {
        var goBulker = _hospitalCollection.NewBulker();
        goBulker.InsertOne(hospital);
		goBulker.UpdateOne(hos => hos.Name == hospital.Name,
                           builder =>builder.Set(hos => hos.Population, 0));
        
	    await goBulker.SaveChangesAsync();
        
        return Ok();
    }
}
```

- Inside the method, a new `goBulker` instance is created using the `NewBulker` method of `IGoCollection`.  The `goBulker` object allows you to organize multiple database operations before saving them to the database.
- The code adds an insert operation and an update operation to the `goBulker` using the `InsertOne` and `UpdateOne` methods, respectively. The insert operation adds the `hospital` object to the collection, and the update operation sets the population of the hospital with the specified name to 0.
- Finally, the `SaveChangesAsync` method is called on the `goBulker` object to execute the pending operations and save the changes to the database.



**By using the `goBulker` object, you can group multiple database operations together and execute them in a single batch, similar to the logic used in EF Core. This can help improve performance and reduce round-trips to the database.**



#### 8. Transaction Operations (IGoTransaction<TContext>)

> Starting from version 5.1.0, transactions are supported, giving users the ability to execute multiple operations within a single, atomic session.

- A `IGoTransaction` instance can be obtained using the `CreateTransaction` method of `IGoFactory`. `IGoTransaction` object allows you to organize multiple database operations within a single, atomic session.

- The transaction only becomes "usable" once you have called the `Commit` or `CommitAsync` methods to apply the changes to the database. After this, the transaction becomes "unusable" and cannot be used again.

- One important feature of transactions is the ability to "rollback" any changes that occurred within the transaction session in case of any errors. In other words, if an exception is thrown at any point during the transaction session, none of the changes made in the transaction will be committed. This ensures that your database remains consistent even when unexpected errors occur.

Here's an example of how to use a transaction:

```c#
public class MyController : ControllerBase
{
    private readonly IGoFactory<MyContext> _goFactory;

    public MyController(IGoFactory<MyContext> goFactory)
    {
      this._goFactory = goFactory;
    }
    
    [HttpPost("TransactionSample")]
    public async Task<IActionResult> TransactionSample()
    {
        var transaction = _goFactory.CreateTransaction();
        transaction.InsertOne(new Hospital { Name = "General Hospital", BedCount = 200 });
		transaction.UpdateOne(hos => hos.Name == "General Hospital" && hos.BedCount == 200,
                              builder => builder.Set(hos => hos.BedCount, 250));
        
	    transaction.Commit();
        
        return Ok();
    }
}

```

- Inside the method, a new `IGoTransaction` instance is created using the `CreateTransaction` method of `IGoFactory`. The `IGoTransaction` object allows you to organize multiple database operations within a single atomic session.
- The code adds an insert operation and an update operation to the `IGoTransaction` using the `InsertOne` and `UpdateOne` methods, respectively. The insert operation adds the `Hospital` object to the collection, and the update operation modifies the `BedCount` of the hospital with the specified name to 250.
- Finally, the `Commit` method is called on the `IGoTransaction` object to execute the pending operations and apply the changes to the database.



---

##### How to execute bulk write in a Transaction (IGoTransBulker\<TDocument>)

In addition to simple operations, `IGoTransaction` also supports bulk write operations. This can be achieved using the `NewTransBulker` method of `IGoTransaction` to create a bulker within the transaction. It's important to note that any changes made using the bulker will not be applied to the database until `Commit` or `CommitAsync` method is called, even if you have called `SaveChanges` or `SaveChangesAsync` method on the bulker.

Here's an example of using a bulker in a transaction:

```c#
[HttpPost("TransBulkerSample")]
public async Task<IActionResult> TransBulkerSample()
{
    var transaction = _goFactory.CreateTransaction();

    // First bulker for operations on Hospital collection
    var hospitalBulker = transaction.NewTransBulker<Hospital>();
    hospitalBulker.InsertOne(new Hospital { Name = "My Hospital", BedCount = 200 });
    hospitalBulker.UpdateOne(hos => hos.Name == "My Hospital" && hos.BedCount == 200,
                     builder => builder.Set(hos => hos.BedCount, 250));
    hospitalBulker.SaveChanges();

    // Second bulker for operations on another collection, e.g., Doctor collection
    var doctorBulker = transaction.NewTransBulker<Doctor>();
    doctorBulker.InsertOne(new Doctor { Name = "Dr. Smith", Specialization = "Cardiology" });
    doctorBulker.UpdateOne(doc => doc.Name == "Dr. Smith" && doc.Specialization == "Cardiology",
                     builder => builder.Set(doc => doc.Specialization, "Neurology"));
    doctorBulker.SaveChanges();

    transaction.Commit();

    return Ok();
}

```

- In this method, an `IGoTransaction` instance is first created using the `CreateTransaction` method of the `IGoFactory`. The `IGoTransaction` object allows you to organize multiple database operations within a single atomic session.
- Next, two `IGoTransBulker` instances are created using the `NewTransBulker` method of the `IGoTransaction`. These instances allow bulk operations on the `Hospital` and `Doctor` collections respectively.
- For both `IGoTransBulker` instances, an insert operation and an update operation are added using the `InsertOne` and `UpdateOne` methods, respectively. The insert operations add new instances of `Hospital` and `Doctor` to their respective collections. The update operations modify the `BedCount` of the specified hospital to 250 and the `Specialization` of the specified doctor to "Neurology".
- The `SaveChanges` method is called on both `IGoTransBulker` instances to prepare the operations for execution in the transaction context.
- Finally, the `Commit` method is called on the `IGoTransaction` object to execute the pending operations and apply the changes to the database.



**By utilizing the `IGoTransaction` object, you can group multiple database operations together and execute them within a single, atomic session. This not only ensures data consistency but also enhances the integrity of your data operations.**



---

##### Error Handling and Rollback in Transaction

- **The `IGoTransaction` interface will automatically rollback the transaction if any operation inside the transaction throws an exception.** Below is an example of how to properly handle errors in a transaction:

```c#
[HttpGet("TransactionWithErrorHandling")]
public async Task<IActionResult> TransactionWithErrorHandling()
{
    var transaction = _goFactory.CreateTransaction();
    try
    {
        // Performing operations within the transaction
        transaction.InsertOne(new Hospital { Name = "General Hospital", BedCount = 200 });
        transaction.UpdateOne(hos => hos.Name == "General Hospital" && hos.BedCount == 200,
                              builder => builder.Set(hos => hos.BedCount, 250));

        // Committing the transaction. If an exception occurs in any transaction operation, the commit will be invalid and the transaction will automatically rollback.
        transaction.Commit();
    }
    catch(Exception ex)
    {
        // If any exception occurred, the changes in the transaction won't be applied to the database. Do not attempt to commit the transaction again in this case, as it will be ineffective and throw an exception.
        Console.WriteLine($"An error occurred: {ex.Message}");
        return BadRequest(ex.Message);
    }
    
    // (Don't do this) Attempting to commit after an exception has been thrown will result in another exception, as the initial commit would have been invalidated by the initial exception.
    // finally 
    //{
    //    transaction.Commit(); 
    //}
    
    return Ok();
}

```

In this code snippet:

- First, we create a new `IGoTransaction` instance.
- Within the `try` block, we perform a series of operations including inserting and updating records in the database. If any of these operations encounter an exception, the `catch` block will handle it.
- The `Commit` function is then called to apply the changes made in the operations to the database. If an exception was thrown during the operations, the commit will be invalid, causing the transaction to automatically rollback.
- Inside the `catch` block, the exception message is printed out to the console, and a `BadRequest` response is returned with the exception message. It's important to note that you should not attempt to commit the transaction again after an exception has been thrown, as the commit will be ineffective and will throw another exception.
- The commented `finally` block illustrates a common mistake - trying to commit the transaction after an exception has been thrown. As indicated, this should be avoided as it will result in another exception, because the initial commit would have been invalidated by the initial exception.

Remember, as a best practice, always use `try` and `catch` blocks when performing transactions to handle potential errors and prevent inconsistent database states.

# **MongoGogo Guide**

- A generic repository implementation using [MongoDB.Driver](https://www.mongodb.com/docs/drivers/csharp/)
- Auto implement the dependency injection for [Microsoft.Extensions.DependencyInjection](https://www.nuget.org/packages/Microsoft.Extensions.DependencyInjection.Abstractions).
- Provides attributes used for manage `databases` and `collections` for each `connection`.



## **Install via .NET CLI**

```
dotnet add package MongoGogo 
```



# **Example**

- After few steps of `configuration` and `class building`(introduced later), you can easily get your `Repository` in abstract through the dependency resolution system.

```c#
public class MyController : ControllerBase
{
    private readonly IGoRepository<Hospital> HospitalRepository;

    public MyController(IGoRepository<Hospital> hospitalRepository)
    {
    	this.HospitalRepository = hospitalRepository;
    }
}
```

- Easy way to access data.

```c#
public class MyController : ControllerBase
{
    [HttpGet("IGoRepository<Hospital>_GetList")]
    public async Task<IActionResult> Hospitals()
    {
        var hospitals = await HospitalRepository.FindAsync(_ => true);
        return Ok(hospitals);
    }
}
```



- Also, MongoGogo provides any `Collection`, `Database` and `context` in abstract !

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

The build of concrete class `MyMongoDBContext` is introduced next part.

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

----



- LifeCycleOption: `Singleton`, `Scoped`, `Transient`

Besides, you can controller the lifecyle of all `IGoContext<T>`, `IGoDatabase<T>`, `IGoCollection<T>`, `IGoRepository<T>` using LifeCycleOption. `Scoped` by default.

```
public void ConfigureServices(IServiceCollection services)
{
    services.AddMongoContext(new MyMongoDBContext("my mongodb connection string"),
    						 new LifeCycleOption
                                 {
                                     ContextLifeCycle = LifeCycleType.Singleton,
                                     DatabaseLifeCycle = LifeCycleType.Scoped,
                                     CollectionLifeCycle = LifeCycleType.Scoped,
                                     RepositoryLifeCycle = LifeCycleType.Transient,
                                 });
}
```



## **Context**

`IGoContext<TContext>` 

- Stands for a MongoDB Connection (using package MongoDB.Driver)
- an instance stores `connection string` and is in charge of the structure of an `entity`.
- generic TContext, which owns the type itself,  is used for `dependency resolution system`. 

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



## **Repositories**

By default, resolution system will build exactly one `IGoRepository<TDocument>` for every `IGoCollection<TDocument>` . 

`IGoRepository<TDocument>`  is in charge of the contact of a MongoDb connection and your project.

```c#
public class MyClass
{
	private readonly IGoRepository<Hospital> HospitalRepository;
    private readonly IGoCollection<Hospital> HospitalCollection;

    public MyClass(IGoRepository<Hospital> hospitalRepository,
    			   IGoCollection<Hospital> hospitalCollection)
    {
        this.HospitalRepository = hospitalRepository;
        this.HospitalCollection = hospitalCollection;
    }
}
```

In fact, You can deal with data using `IGoRepository<Hospital>` or `IGoCollection<Hospital>`.

The main difference between them is :

- `IGoCollection<Hospital>` is actually the `IMongoCollection<Hospital>`, the instance from [MongoDB.Driver](https://www.mongodb.com/docs/drivers/csharp/)
- `IGoRepository<Hospital>` is an packaged instance, but with handy method to operate your datas.

```c#
public async Task AddNewHospital(Hospital hospital)
{
    HospitalRepository.InsertOne(hospital);
    HospitalCollection.InsertOne(hospital);
}
```

- `IGoRepository<Tdocument>` has basic method dealing your data, like `FindOneAsync`, `InsertOne`, `ReplaceOne`, `Count`, and so on. 
- But if you want some extra fancy feature, you can also **do it yourself**  by overriding the origin method !!!



#### **Custom Repository**

If the basic functionality of `IGoRepository<TDocument>` cannot fullfill you,

*(like: print some message and saves it as a local log)*

then inherit the `GoRepositoryAbstract<TDocument>` and make your own child.

```c#
public class HospitalRepository : GoRepositoryAbstract<Hospital>
{
    public HospitalRepository(IGoCollection<Hospital> collection) : base(collection)
    {
    }
    
    public override void InsertOne(Hospital hospital)
    {
    	base.InsertOne(hospital);
        Logger.PrintAndSave(hospital); //print insert success and save
    }
}
```

After this , the resolution system will map new child `HospitalRepository` to `IGoRepository <Hospital>`

```
public class MyClass
{
	private readonly IGoRepository<Hospital> HospitalRepository; //actually your new class HospitalRepository
    private readonly IGoCollection<Hospital> HospitalCollection; //not changed

    public MyClass(IGoRepository<Hospital> hospitalRepository,
    			   IGoCollection<Hospital> hospitalCollection)
    {
        this.HospitalRepository = hospitalRepository; //actually your new class HospitalRepository
        this.HospitalCollection = hospitalCollection; //not changed
    }
    
    public async Task AddNewHospital(Hospital hospital)
    {
        HospitalRepository.InsertOne(hospital);  // it will print insert success and save

        HospitalCollection.InsertOne(hospital);  // nothing else
    }
}
```





# **Feature**

This part shows basic method of  an`IGoRepository<TDocument>`

```c#
IGoRepository<City> CityRepository;
```



## **Count / CountAsync**

```c#
var documentCount = CityRepository.Count(city => city.Population >= 1000);
var documentCountAsync = await CityRepository.CountAsync(city => city.Population >= 1000);
```



## **Find/FindAsync**

```c#
var foundDocuments = CityRepository.Find(city => city.Population >= 1000);
var foundDocumentAsync = await CityRepository.FindAsync(city => city.Population >= 1000);
```

- with `goFindOption` (optional)

```c#
var foundDocuments = CityRepository.Find(city => city.Population >= 1000,
                                         goFindOption: new GoFindOption
                                        {
                                            AllowDiskUse = true,
                                            Limit = 2,
                                            Skip = 1
                                        });
var foundDocumentAsync = await CityRepository.FindAsync(city => city.Population >= 1000, 
                                                        goFindOption: new GoFindOption
                                                        {
                                                            AllowDiskUse = true,
                                                            Limit = 2,
                                                            Skip = 1
                                                        });
```

- field reduction with `projection`(optional)

```c#
var reducedDocuments = CityRepository.Find(city => city.Population >= 1000,
                                           projection: builder => builder.Include(city => city.Population)
                                                                         .Include(city => city.Name));
var reducedDocumentAsync = await CityRepository.FindAsync(city => city.Population >= 1000, 
                                                          projection: builder => builder.Include(city => city.Population)
                                                                                        .Include(city => city.Name));
```



## **FindOne/FindOneAsync**

```c#
var firstOrDefaultDocument = CityRepository.FindOne(city => city.Population >= 1000);
var firstOrDefaultDocumentAsync = await CityRepository.FindOneAsync(city => city.Population >= 1000);
```



## **InsertOne/InsertOneAsync**

```c#
CityRepository.InsertOne(new City{Name = "New York"});
await CityRepository.InsertOneAsync(new City{Name = "New York"});
```



## **InsertMany/InsertManyAsync**

```c#
var cityList = new List<City>
{
	new City{}, 
	new City{Name = "New York"}, 
	new City{Population = 100}
};

CityRepository.InsertMany(cityList);
await CityRepository.InsertManyAsync(cityList);
```



## **ReplaceOne/ReplaceOneAsync**

```apl
var city = new City
{
	Name = "NewYork_America"
};

CityRepository.ReplaceOne(c => c.Name == "NewYork",
						  city);
await CityRepository.ReplaceOneAsync(c => c.Name == "NewYork",
									 city);
```

- with `upsert`(optional)

```c#
CityRepository.ReplaceOne(c => c.Name == "NewYork",
                          city,
                          isUpsert: true);
await CityRepository.ReplaceOneAsync(c => c.Name == "NewYork",
                                     city,
                                     isUpsert: true);
```



## **UpdateOne/UpdateOneAsync**

```c#
CityRepository.UpdateOne(city => city.Name == "New York",
                         builder => builder.Set(city => city.Name, "New York_America")
                       					   .Set(city => city.Population, 1000));
await CityRepository.UpdateOneAsync(city => city.Name == "New York",
                                    builder => builder.Set(city => city.Name, "New York_America")
                                    				  .Set(city => city.Population, 1000));
```

- with `upsert`(optional)

```c#
CityRepository.UpdateOne(city => city.Name == "New York",
                         builder => builder.Set(city => city.Name, "New York_America")
                       					   .Set(city => city.Population, 1000),
                         isUpsert: true);
await CityRepository.UpdateOneAsync(city => city.Name == "New York",
                                    builder => builder.Set(city => city.Name, "New York_America")
                                    				  .Set(city => city.Population, 1000),
                                   isUpsert: true);
```



## **UpdateMany/UpdateManyAsync**

```c#
CityRepository.UpdateMany(city => city.Name == "New York",
                          builder => builder.Set(city => city.Name, "New York_America")
                      					    .Set(city => city.Population, 1000));
await CityRepository.UpdateManyAsync(city => city.Name == "New York",
                                     builder => builder.Set(city => city.Name, "New York_America")
 				                                       .Set(city => city.Population, 1000));
```



## **DeleteOne/DeleteOneAsync**

```c#
CityRepository.DeleteOne(city => city.Name == "New York");
await CityRepository.DeleteOneAsync(city => city.Name == "New York");
```



## **DeleteMany/DeleteManyAsync**

```c#
CityRepository.DeleteMany(city => city.Name == "New York");
await CityRepository.DeleteManyAsync(city => city.Name == "New York");
```


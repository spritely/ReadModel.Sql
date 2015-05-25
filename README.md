[![Build status](https://ci.appveyor.com/api/projects/status/4v7eeja8dxlys2ao/branch/master?svg=true)](https://ci.appveyor.com/project/Spritely/readmodel-sql/branch/master)

# Spritely.ReadModel.Sql
Provides a mostly default implementation of a Spritely.Cqrs read model JSON database on top of SQL Server. Please visit https://github.com/spritely/Cqrs/ for more details on Spritely.Cqrs.

Spritely.ReadModel.Sql assumes you are developing with Command Query Responsibility Separation (CQRS). When using CQRS another common pattern is Event Sourcing whereby all the commands that have been executed are stored in database log. This log can be replayed to generate a snapshot of the data at any point in time. These snapshots are the respresentation of your model at that moment in time. Best practice is to store these snapshots so that future event source replays can continue from this point forward and avoid having to replay the entire event stream each time. Spritely.ReadModel.Sql provides most of what you need for creating, reading, updating, and deleting these snapshots in a SQL database. It is not designed to encapsulate the Event Source itself, just the reading and writing of snapshot data.

## How it works
Spritely.ReadModel.Sql snapshots are assumed to follow a database structure with columns:
- Id - primary key (can be any type)
- CreatedUtc - datetime the row was created
- UpdatedUtc - datetime the row was last updated
- Model - nvarchar(max) - the snapshot in a blob of json text
- (other columns can optionally be added, a future version of Spritely.ReadModel.Sql will provide built-in support for these extra columns)
 
Spritely.ReadModel.Sql doesn't try to create the database for you. It assumes you have one setup with the correct tables and columns already.

To get started you need a way to connect to this database. First setup a type:
```csharp
    public class MyReadModelDatabase : ReadModelDatabase<MyReadModelDatabase>
    {
        // You don't need any code in here
        // The main purpose of this class is to have a unique type so that a
        // dependency injection container can create one of these automatically
    }
    
    // Create one
    var myReadModelDatabase = new MyReadModelDatabase();
    
    // or (using SimpleInjector)
    var myReadModelDatabase = container.GetInstance<MyReadModelDatabase>();
```

SimpleInjector is our dependency injection (DI) container of choice and can be found here: https://simpleinjector.org/

You'll notice we didn't give it a connection string yet! To get one we recommend using Its.Configuration which is available at: https://github.com/jonsequitur/Its.Configuration

The code to setup SimpleInjector with Its.Configuration is easy:
```csharp
  container.RegisterSingle(Settings.Get<MyReadModelDatabase>);
```

Now you write a json file named MyReadModelDatabase such as:
```javascript
{
    "ConnectionSettings": {
        "Server": "myserver.mydomain.com",
        "Database": "myDatabase",
        "Credentials": {
            "User": "sa",
            "Password": "MySuperSecurePassword"
        }
    }
}
```

When you resolve your database type (MyReadModelDatabase) from your DI container (`container.GetInstance<MyReadModelDatabase>()`), it will ask Its.Configuration to deserialize an object from the JSON file by the same name (`Settings.Get<MyReadModelDatabase>()`). Its.Configuration will instantiate a MyReadModelDatabase and fill in the properties on the .NET object to match the JSON file. You can optionally fill in these properties yourself directly in C#. The class properties directly match the JSON file above so you can write `myReadModelDatabase.ConnectionSettings.Database = "myDatabase"` if you want.

Now you're ready to start querying the database via built-in queries. Spritely.ReadModel.Sql implements many common command and query handlers for you from Spritely.Cqrs (https://github.com/spritely/Cqrs/) as follows:

### GetAllQuery
```csharp
    var query = new GetAllQuery<MyModel>
    {
        ModelType = "MyModel" // This is the table name in the database
    };

    // Inject this into your class via a constructor parameter to make this code unit testable
    // As an IQueryHandler<GetAllQuery<MyModel>, IReadOnlyCollection<MyModel>>
    var queryHandler = new GetAllQueryHandler<MyReadModelDatabase, MyModel>(myReadModelDatabase);

    var results = queryHandler.Handle(query);

    // use results
```

### GetByIdQuery
```csharp
    var query = new GetByIdQuery<Guid, MyModel>
    {
        Id = someGuid,
        ModelType = "MyModel"
    };

    // Inject into constructor as an IQueryHandler<GetOneByQuery<MyModel>, MyModel>
    var queryHandler = new GetOneByQueryHandler<MyReadModelDatabase, MyModel>(myReadModelDatabase);

    var model = queryHandler.Handle(query);
    
    Console.WriteLine(model.AnyProperty);
```

Make the code a little cleaner and type safe for your callers by adding a class:
```csharp
    public class GetMyModelByIdQuery : GetByIdQuery<Guid, MyModel>
    {
        public GetMyModelByIdQuery()
        {
            this.ModelType = "MyModel";
        }
    }
```

Now the earlier code is rewritten as:
```csharp
    var query = new GetMyModelByIdQuery
    {
        Id = someGuid
    };

    var model = this.queryHandler.Handle(query);

    // The line above assumes queryHandler injected into constructor as IQueryHandler<GetMyModelByIdQuery, MyModel>
    // To register in your container use:
    // container.Register<IQueryHandler<GetMyModelByIdQuery, MyModel>, GetOneByQueryHandler<MyReadModelDatabase, MyModel>>();

    Console.WriteLine(model.AnyProperty);
```

### GetManyBy
```csharp
    public class GetManyMyModelByIdsInQuery : GetManyByQuery<MyModel>
    {
        public GetManyMyModelByIdsInQuery()
        {
            this.ModelType = "MyModel";
        }
        
        // The fact that this is IEnumerable<T> means it will generate a SQL query: in (...)
        // Name of property must match column name to be queried
        // Queries on SQL columns, not inside the Model object of the database
        public IEnumerable<Guid> Id { get; set; }
    }
    
    // Later...
    var query = new GetManyMyModelByIdsInQuery
    {
        Id = new [] { guid1, guid2 }
    };

    var model = this.queryHandler.Handle(query);
```

### AddOrUpdateCommand
```csharp
    public class AddOrUpdateMyModelCommand : AddOrUpdateCommand<MyModel>
    {
        public AddOrUpdateMyModelCommand()
        {
            this.ModelType = "MyModel";
        }
    }
    
    // Later...
    var addOrUpdateMyModelCommand = new AddOrUpdateMyModelCommand
    {
        Model = modelInstance
    };

    // Container setup:
    // container.Register<ICommandHandler<AddOrUpdateCommand<MyModel>>, AddOrUpdateCommandHandler<MyReadModelDatabase, MyModel>>();
    this.addOrUpdateMyModelCommandHandler.Handle(addOrUpdateMyModelCommand);
```

### Other commands
Hopefully you get the idea by now. There are also built-in commands: **DeleteAllCommand**, **DeleteByCommand**, and **DeleteByIdCommand**.

### Future
In a future version of Spritely.ReadModel.Sql we plan to add support for additional columns on the built in AddOrUpdateCommand. The Get queries are already extensible enough to support additional columns, but you'd need to write your own AddOrUpdateCommandHandlers to get two sides of the equation. Spritely.ReadModel.Sql doesn't try to model all query types. It just tries to hit the 90% case. When you need to go beyond this, step down to Spritely.Cqrs and write the SQL you need directly.

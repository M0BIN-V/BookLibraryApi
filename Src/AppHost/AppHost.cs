using Projects;

var builder = DistributedApplication.CreateBuilder(args);

var database = builder
    .AddSqlServer("sqlserver")
    .WithLifetime(ContainerLifetime.Persistent)
    .AddDatabase("BookLibraryDb");

builder.AddProject<BookLibraryApi>("api")
    .WithReference(database)
    .WaitFor(database);

builder.Build().Run();
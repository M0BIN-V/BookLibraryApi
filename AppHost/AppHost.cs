using Projects;

var builder = DistributedApplication.CreateBuilder(args);

var database = builder
    .AddSqlServer("sqlserver")
    .AddDatabase("BookLibraryDb");

builder.AddProject<BookLibraryApi>("api")
    .WithReference(database)
    .WaitFor(database);

builder.Build().Run();
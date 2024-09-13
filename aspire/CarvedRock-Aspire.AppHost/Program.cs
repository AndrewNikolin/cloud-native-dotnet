var builder = DistributedApplication.CreateBuilder(args);

var carvedrockdb = builder.AddPostgres("postgres")
                            .AddDatabase("CarvedRockPostgres");

var emailService = builder.AddSmtp4Dev("SmtpUri");  // custom extension

var identityserver = builder.AddProject<Projects.CarvedRock_Identity>("carvedrock-identity")
    .WithSharedLoggingLevels();

var identityEndpoint = identityserver.GetEndpoint("https");

var api = builder.AddProject<Projects.CarvedRock_Api>("carvedrock-api")
    .WithReference(carvedrockdb)
    .WithSharedLoggingLevels()
    .WithEnvironment("Auth__Authority", identityEndpoint);

builder.AddProject<Projects.CarvedRock_WebApp>("carvedrock-webapp")
    .WithReference(api)    
    .WithSharedLoggingLevels()
    .WithEnvironment("Auth__Authority", identityEndpoint)
    .WithReference(emailService);    

builder.Build().Run();

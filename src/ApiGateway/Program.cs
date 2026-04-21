var builder = WebApplication.CreateBuilder(args);

//Register Service Discovery and YARP
builder.Services.AddServiceDiscovery();
builder.Services.AddReverseProxy()
    .LoadFromConfig(builder.Configuration.GetSection("ReverseProxy"))
    .AddServiceDiscoveryDestinationResolver();

var app = builder.Build();

app.MapGet("/", () => "YARP API Gateway is running");
app.MapReverseProxy();

app.Run();

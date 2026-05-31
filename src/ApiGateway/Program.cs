using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);

//Configure authentication with JWT Bearer tokens issued by Keycloak
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.Authority = builder.Configuration["Authentication:Authority"]!;
        options.Audience = builder.Configuration["Authentication:Audience"];
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true, //Ensure the token was issued by the trusted Keycloak server
            ValidateAudience = false, //Audience validation is handled by individual services, so we disable it here
            ValidateLifetime = true, //Ensure the token has not expired
            ValidateIssuerSigningKey = true, //Ensure the token's signature is valid and was signed by the trusted Keycloak server
            ClockSkew = TimeSpan.Zero //Remove default clock skew to enforce exact token expiration times, improving security by preventing use of expired tokens for a grace period
        };
        if (builder.Environment.IsDevelopment())
            options.RequireHttpsMetadata = false;
    });
builder.Services.AddAuthorization();


//Register Service Discovery and YARP
builder.Services.AddServiceDiscovery();
builder.Services.AddReverseProxy()
    .LoadFromConfig(builder.Configuration.GetSection("ReverseProxy"))
    .AddServiceDiscoveryDestinationResolver();

var app = builder.Build();

app.UseAuthentication();
app.UseAuthorization();

app.MapGet("/", () => "YARP API Gateway is running");
app.MapReverseProxy();

app.Run();

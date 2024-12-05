using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using OrderPickingSystem.Context;
using OrderPickingSystem.Models.Enums;
using OrderPickingSystem.Services;
using OrderPickingSystem.Services.Interfaces;
using OrderPickingSystem.Services.Mappers;
using OrderPickingSystem.Services.OrderServices;

var builder = WebApplication.CreateBuilder(args);

// if (builder.Environment.IsDevelopment())
// {
//     // Use in-memory database for testing
//     builder.Services.AddDbContext<BiddingSystemContext>(options =>
//         options.UseInMemoryDatabase("TestDatabase"));
// }
// else
// {
builder.Services.AddDbContext<OrderPickingContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("order-picking-context") ??
                         throw new InvalidOperationException(
                             "Connection string 'order-picking-context' not found.")));
// }

builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IContainerService, ContainerService>();
builder.Services.AddScoped<IItemService, ItemService>();
builder.Services.AddScoped<IJwtService, JwtService>();
builder.Services.AddScoped<ILocationService, LocationService>();
builder.Services.AddScoped<IOrderService, OrderService>();
builder.Services.AddScoped<IPaletteService, PaletteService>();
builder.Services.AddScoped<IPickingOrderService, PickingOrderService>();
builder.Services.AddScoped<IPickService, PickService>();
builder.Services.AddScoped<IReachingOrderService, ReachingOrderService>();
builder.Services.AddScoped<IRelocatingOrderService, RelocatingOrderService>();
builder.Services.AddScoped<IUserContextService, UserContextService>();
builder.Services.AddScoped<IUserMapper, UserMapper>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IUserRoleMappingService, UserRoleMappingService>();
builder.Services.AddHttpContextAccessor();

builder.Services.AddAuthorization(options =>
    {
        options.AddPolicy("Picker", policy =>
            policy.RequireAssertion(context =>
                context.User.HasClaim(claim => claim is { Type: ClaimTypes.Role, Value: "Picker" })
            )
        );
        options.AddPolicy("Reacher", policy =>
            policy.RequireAssertion(context =>
                context.User.HasClaim(claim => claim is { Type: ClaimTypes.Role, Value: "Reacher" })
            )
        );
        options.AddPolicy("Admin", policy =>
            policy.RequireAssertion(context =>
                context.User.HasClaim(claim => claim is { Type: ClaimTypes.Role, Value: "Admin" })
            )
        );
        options.AddPolicy("Relocator", policy =>
            policy.RequireAssertion(context =>
                context.User.HasClaim(claim => claim is { Type: ClaimTypes.Role, Value: "Relocator" })
            )
        );
        options.AddPolicy("Troubleshooting", policy =>
            policy.RequireAssertion(context =>
                context.User.HasClaim(claim => claim is { Type: ClaimTypes.Role, Value: "Troubleshooting" })
            )
        );
        options.AddPolicy("PickerOrReacherOrRelocator", policy =>
            policy.RequireAssertion(context =>
                context.User.HasClaim(claim =>
                    claim is { Type: ClaimTypes.Role, Value: "Picker" }
                        or { Type: ClaimTypes.Role, Value: "Reacher" }
                        or { Type: ClaimTypes.Role, Value: "Relocator" }
                )
            )
        );
    }
);

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = "OrderPickingSystem",
        ValidAudience = "http://localhost:5076",
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(
            builder.Configuration.GetSection("JwtSettings:Token").Value!))
    };
});

builder.Services.AddControllers();

builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    // Optional: Set the session timeout period; 20 minutes is the default.
    options.IdleTimeout = TimeSpan.FromMinutes(20);
    options.Cookie.HttpOnly = true; // Helps prevent JavaScript access to the session cookie.
    options.Cookie.IsEssential = true; // Allows session cookie even if the user hasn't consented to cookies.
});

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo { Title = "Order Picking System", Version = "v1" });

    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });

    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    });
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    // app.UseDeveloperExceptionPage();
    // app.UseSwagger();
    // app.UseSwaggerUI(c =>
    // {
    //     c.SwaggerEndpoint("/swagger/v1/swagger.json", "Your API V1");
    //     c.RoutePrefix = string.Empty; // Set the root URL to redirect to Swagger UI
    // });

    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();
app.UseSession();

app.UseHttpsRedirection();

app.MapControllers();

app.Run();
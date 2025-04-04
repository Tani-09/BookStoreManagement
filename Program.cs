using AutoMapper;
using BookStoreManagement;
using BookStoreManagement.BLL.AutoMapper;
using BookStoreManagement.BLL.Services;
using BookStoreManagement.DAL.DBContext;
using BookStoreManagement.DAL.Repositories;
using BookStoreManagement.Middlewares;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;



var builder = WebApplication.CreateSlimBuilder(args);






//CONFIGURE JWT SETTINGS
builder.Services.Configure<JwtSettings>(builder.Configuration.GetSection("JwtSettings"));
builder.Services.AddScoped<AuthService>();


//CONFIGURE LOGGING
builder.Logging.ClearProviders(); //Clear default providers to add your custom ones
builder.Logging.AddConsole();     //Add console logging
builder.Logging.AddDebug();      //Optional: Add debug logging



//CONFIGURE JWT AUTHENTICATION
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{

    options.IncludeErrorDetails = true;
    var jwtSettings = builder.Configuration.GetSection("JwtSettings").Get<JwtSettings>();
    Console.WriteLine($"Issuer: {jwtSettings.Issuer}, Audience: {jwtSettings.Audience}, Key: {jwtSettings.Key}");


    if (jwtSettings == null || string.IsNullOrEmpty(jwtSettings.Issuer) ||
    string.IsNullOrEmpty(jwtSettings.Audience) || string.IsNullOrEmpty(jwtSettings.Key))
    {
        throw new InvalidOperationException("JWT settings are not configured properly.");
    }

    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = jwtSettings.Issuer,
        ValidAudience = jwtSettings.Audience,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.Key))
    };
});


//CONFIGURE DATABASE
builder.Services.AddDbContext<BookStoreDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));


//CONFIGURE AUTOMAPPER
builder.Services.AddAutoMapper(typeof(Program).Assembly); // Add this line




//CONFIGURE CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAllOrigins", builder =>
    {
        builder.AllowAnyOrigin()
               .AllowAnyMethod()
               .AllowAnyHeader();
    });
});




//REGISTER SERVICES AND REPOSITORIES
builder.Services.AddScoped<IRepositoryWrapper, RepositoryWrapper>();
builder.Services.AddScoped<IBookService, BookService>();
builder.Services.AddScoped<ICartItemService, CartItemService>();
builder.Services.AddScoped<ICategoryService, CategoryService>();
builder.Services.AddScoped<IOrderItemService, OrderItemService>();
builder.Services.AddScoped<IOrderService, OrderService>();
builder.Services.AddScoped<IRoleService, RoleService>();
builder.Services.AddScoped<IUserService, UserService>();


builder.Services.AddAuthorization();
builder.Services.AddControllers();


builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(


   c =>
   {
       c.SwaggerDoc("v1", new OpenApiInfo { Title = "BookStore API", Version = "v1" });

       // Add security definition for JWT
       c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
       {
           In = ParameterLocation.Header,
           Description = "Please enter token",
           Name = "Authorization",
           Type = SecuritySchemeType.ApiKey
       });

       c.AddSecurityRequirement(new OpenApiSecurityRequirement
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
            new string[] {}
        }
    });
   }




    );

/*builder.Services.ConfigureHttpJsonOptions(options =>
{
    options.SerializerOptions.TypeInfoResolverChain.Insert(0, AppJsonSerializerContext.Default);
});*/

builder.Services.ConfigureHttpJsonOptions(options =>
{
    options.SerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
});




var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI();
}



var todosApi = app.MapGroup("/todos");
todosApi.MapGet("/", () => new[] {
    new { Id = 1, Title = "Walk the dog" },
    new { Id = 2, Title = "Do the dishes" }
});

app.UseHttpsRedirection();

app.UseCors("AllowAllOrigins");

app.UseMiddleware<ExceptionMiddleware>();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();



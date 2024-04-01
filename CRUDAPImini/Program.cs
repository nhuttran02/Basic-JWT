using CRUDAPImini.AutoMapper;
using CRUDAPImini.Data;
using CRUDAPImini.Models.DTOs;
using CRUDAPImini.Repositories;
using CRUDAPImini.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<AppDBContext>(o => o.UseSqlite(builder.Configuration.GetConnectionString("Default")));
builder.Services.AddScoped<IProductRepo, ProductRepo>();
builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddAutoMapper(typeof(MapperProfile));
builder.Services.AddScoped<IAccountRepo, AccountRepo>();
builder.Services.AddScoped<IAccountService, AccountService>();
builder.Services.AddAuthorization();


builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]!))
        };
    });

builder.Services.AddSwaggerGen(swagger =>
{
    //This is to generate the Default UI of Swagger Documentation 
    swagger.SwaggerDoc("v1", new OpenApiInfo
    {
        Version = "v1",
        Title = "ASP.NET 8 Web API",
        Description = "Authentication"
    });

    //To enable authorization using swagger (JWT)
    swagger.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
    {
        Name = "authorization",
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header
    });
    swagger.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            }, Array.Empty<string>()
        }
    });
});


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseAuthentication();
app.UseAuthorization();
app.UseHttpsRedirection();


//Authentication
app.MapPost("/register", async (RegisterDTO register, IAccountService account) =>
{
    return Results.Ok(await account.Register(register));
}).AllowAnonymous();

app.MapPost("/login", async (LoginDTO login, IAccountService account) =>
{
    return Results.Ok(await account.Login(login));
}).AllowAnonymous();

app.MapGet("/GetProduct", async (IProductService productService) =>
{
    return Results.Ok(await productService.GetAll());
}).RequireAuthorization();

app.MapGet("/GetProduct/{id:int}", async (IProductService productService, int id) =>
{
    return Results.Ok(await productService.GetById(id));
}).RequireAuthorization();

app.MapPost("/AddProduct", async (AddRequestDTO request, IProductService productService) =>
{
    return Results.Ok(await productService.Add(request));
}).RequireAuthorization();

app.MapPut("/UpdateProduct", async (UpdateRequestDTO request, IProductService productService) =>
{
    return Results.Ok(await productService.Update(request));
}).RequireAuthorization();

app.MapDelete("/DeleteProduct/{id:int}", async (IProductService productService, int id) =>
{
    return Results.Ok(await productService.Delete(id));
});

app.Run();



using System.Text;
using API.Data;
using API.Entities;
using API.Middleware;
using API.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    var jwtSecurityScheme = new OpenApiSecurityScheme
    {
        BearerFormat = "JWT",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = JwtBearerDefaults.AuthenticationScheme,
        Description= "Put Bearer + your token in the box below",
        Reference= new OpenApiReference
        {
            Id = JwtBearerDefaults.AuthenticationScheme,
            Type = ReferenceType.SecurityScheme
        }
    };

    c.AddSecurityDefinition(jwtSecurityScheme.Reference.Id, jwtSecurityScheme);

    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            jwtSecurityScheme, Array.Empty<string>()
        }
    });
});



// Service for DB Connection
builder.Services.AddDbContext<StoreContext>(options =>
{
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection"));
});

/*
Kjo linjë kod shton middleware për politikat e CORS në shërbimin e ASP.NET Core,
duke lejuar kërkesat nga burime të ndryshme dhe duke e bërë aplikacionin të sigurt dhe funksional 
nëse ka nevojë për të marrë resurse nga burime të ndryshme.
*/
builder.Services.AddCors();
// implementimi i authit permes identity 
builder.Services.AddIdentityCore<User>(opt =>
{
    // kto jon mundsit per me shtu contraints ne identity configuration 
    opt.User.RequireUniqueEmail = true;
})
    .AddRoles<Role>()
    .AddEntityFrameworkStores<StoreContext>();
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(opt =>
    {
        opt.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = false,
            ValidateAudience = false,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JWTSettings:TokenKey"]))
        };
    });
builder.Services.AddAuthorization();
// service mbetet alive perderisa eshte tu u procesu httprequesti pasi t mbaron servisi asgjesohet poashtu
builder.Services.AddScoped<TokenService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
// the exception handling middleware for the application.
app.UseMiddleware<ExceptionMiddleware>();


if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c => 
    {
        c.ConfigObject.AdditionalItems.Add("persistAuthorization","true");
    });
}
// pjesa ku shtohet middleware per me leju kerkesa pi burimit localhost:3000
app.UseCors(opt =>
{                                      //kta e perdorum(allow credentials) per me lehu klientin me pass cookie prej api ne client side dhe anasjelltas
    opt.AllowAnyHeader().AllowAnyMethod().AllowCredentials().WithOrigins("http://localhost:3000");
});

// authentication e qesum para authorization se mas pari duhet me dit kush o per me autorizu 
app.UseAuthentication();
app.UseAuthorization();
//api e din se ku ko me dergu request kur vjen puna te nje API endpoint sepse e shton konfigurimin e rrugetimit
//per kontrollerat duke e perdor ket metod specifike..
app.MapControllers();
// pjesa ku fillohet menaxhimi nese ka databaz ose jo e poashtu ndonje pending migration...
var scope = app.Services.CreateScope();
var context = scope.ServiceProvider.GetRequiredService<StoreContext>();
var userManager = scope.ServiceProvider.GetRequiredService<UserManager<User>>();
var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();
try
{
    await context.Database.MigrateAsync();
    await DbInitializer.Initialize(context, userManager);
}
catch (Exception ex)
{
    logger.LogError(ex, "A problem occurred during migration");
}

app.Run();
